using System.Collections.Generic;

namespace RobinBird.Utilities.Runtime.Helper
{
    using System;
    using System.Text;
    using Extensions;
    using Logging.Runtime;

    public static class ErrorTracking
    {
        public const string LogCategory = "Crashlytics";
        private static readonly StringBuilder exceptionBuilder = new StringBuilder();

        private static readonly Type crashlyticsLoggedExceptionType;

#if ROBIN_BIRD_FIREBASE
        private static bool isFirebaseInitialized;
#endif

        private class CachedException
        {
	        public string Name;
	        public string Description;
        }
        
        private static List<string> cachedLogContext = new List<string>();
        private static List<CachedException> cachedRecordExceptions = new List<CachedException>();

        public static void SetFirebaseInitialized()
        {
#if ROBIN_BIRD_FIREBASE
	        Log.Info($"Got Firebase Initialize. Playing recorded context count: {cachedLogContext.Count.ToString()} and exception count: {cachedRecordExceptions.Count.ToString()}", category: LogCategory);
	        isFirebaseInitialized = true;
	        foreach (string cachedLog in cachedLogContext)
	        {
		        LogCrashlyticsContext(cachedLog);
	        }

	        foreach (CachedException cachedException in cachedRecordExceptions)
	        {
		        RecordCrashlyticsException(cachedException.Name, cachedException.Description);
	        }
#endif
        }
        
#if UNITY_2020_1_OR_NEWER && ROBIN_BIRD_FIREBASE
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Reset()
        {
	        isFirebaseInitialized = false;
	        cachedLogContext = new List<string>();
	        cachedRecordExceptions = new List<CachedException>();
        }
#endif

        static ErrorTracking()
        {
            crashlyticsLoggedExceptionType = AppDomain.CurrentDomain.GetType("Firebase.Crashlytics.LoggedException");
        }
        
        /// <summary>
        /// Record an Exception that should not happen.
        /// </summary>
        public static void RecordException(string name, string description = null, Exception exception = null)
        {
            // exceptionBuilder.AppendLine(name);
            const string innerExceptionString = "--- INNER EXCEPTION ---";
            exceptionBuilder.AppendLine(description);
            if (exception != null)
            {
                exceptionBuilder.AppendLine(innerExceptionString);
                exceptionBuilder.AppendLine(exception.ToString());
            }
            var descriptionWithException = exceptionBuilder.ToString();
#if ROBIN_BIRD_FIREBASE
            if (isFirebaseInitialized)
            {
	            RecordCrashlyticsException(name, descriptionWithException);
            }
            else
            {
                Log.Warn($"Would log exception but Crashlytics or Firebase not initialized.\nCaching for delayed reporting.\nName: {name}\nDescription: {descriptionWithException}", category: LogCategory);
                cachedRecordExceptions.Add(new CachedException()
                {
	                Name = name,
	                Description = descriptionWithException,
                });
            }
#endif
#if ROBIN_BIRD_SENTRY
            if (UnityEngine.Application.isEditor == false)
            {
                Sentry.SentrySdk.CaptureMessage($"{name}\n{descriptionWithException}");
            }
#endif
#if NETCOREAPP
	        Sentry.SentrySdk.CaptureException(
		        new InvalidOperationException($"{name}\n{descriptionWithException}"));
#endif
	        
            Log.Error($"<b>{name}</b>\n{descriptionWithException}", category: LogCategory);
            exceptionBuilder.Clear();
        }

#if ROBIN_BIRD_FIREBASE
        private static void RecordCrashlyticsException(string name, string descriptionWithException)
        {
	        if (Firebase.Crashlytics.Crashlytics.IsCrashlyticsCollectionEnabled)
	        {
		        // Fake exception stacktrace so we have our own name at the top 
		        exceptionBuilder.Clear();
		        exceptionBuilder.Append("  at ");
		        exceptionBuilder.Append(name);
		        exceptionBuilder.Append(".CustomException");
		        exceptionBuilder.AppendLine(" ()");

		        // Skip the frame of this method
		        var stackTrace = new System.Diagnostics.StackTrace(1).ToString();
		        exceptionBuilder.Append(stackTrace);
		        var stackTraceString = exceptionBuilder.ToString();

		        var crashlyticsException = CreateCrashlyticsLoggedException(name, descriptionWithException, stackTraceString);
		        Firebase.Crashlytics.Crashlytics.LogException(crashlyticsException);
	        }
	        else
	        {
		        Log.Warn("Would log exception but Firebase collection is not enabled", category: LogCategory);
	        }
        }
#endif

        private static Exception CreateCrashlyticsLoggedException(string name, string message, string stackTrace)
        {
            return (Exception)Activator.CreateInstance(crashlyticsLoggedExceptionType, name, message, stackTrace);
        }

        /// <summary>
        /// Log certain events that appear together with recorded exceptions by <see cref="RecordException"/>. Can be helpful
        /// to figure out what happened before the exception. Like breadcrumbs leading up to the exception
        /// </summary>
        public static void LogContext(string message)
        {
#if ROBIN_BIRD_FIREBASE
            if (isFirebaseInitialized)
            {
	            LogCrashlyticsContext(message);
            }
            else
            {
                Log.Warn("Would log context but crashlytics not initialized", category: LogCategory);
		        cachedLogContext.Add(message);
            }
#endif
#if ROBIN_BIRD_SENTRY
            if (UnityEngine.Application.isEditor == false)
            {
                Sentry.SentrySdk.AddBreadcrumb(message);
            }
#endif
#if NETCOREAPP
	        Sentry.SentrySdk.AddBreadcrumb(message);
#endif
            Log.Info(message, category: LogCategory);
        }

#if ROBIN_BIRD_FIREBASE
        private static void LogCrashlyticsContext(string message)
        {
	        if (Firebase.Crashlytics.Crashlytics.IsCrashlyticsCollectionEnabled)
	        {
		        Firebase.Crashlytics.Crashlytics.Log(message);
	        }
	        else
	        {
		        Log.Warn("Would log context but Firebase collection is not enabled", category: LogCategory);
	        }
        }
#endif
    }
}