#region Disclaimer

// <copyright file="Log.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

using System.Diagnostics;

namespace RobinBird.Logging.Runtime
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using JetBrains.Annotations;
    using Provider;

    /// <summary>
    /// Logging utility to log program messages to <see cref="ILogProvider" />s. Providers
    /// handle the logic where the message is received or displayed (e.g. Console, File, etc.)
    /// </summary>
    public abstract class Log
    {
        private const string ConditionalDefine = "DEBUG";
        private const string UnityTestConditionalDefine = "UNITY_INCLUDE_TESTS";
        private static readonly List<ILogProvider> Provider = new List<ILogProvider>();
        private static ILogProvider fallbackProvider;

        public static int ProviderCount
        {
            get { return Provider.Count; }
        }

        public static ILogProvider FallbackProvider
        {
            get
            {
                if (fallbackProvider == null)
                {
                    fallbackProvider = new DotnetLogProvider();
                }
                return fallbackProvider;
            }
            set { fallbackProvider = value; }
        }

        /// <summary>
        /// Add provider to pass logs to it.
        /// </summary>
        public static void AddProvider([NotNull] ILogProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider", "Added providers should not be null.");
            }
            Provider.Add(provider);
            var statusReceiver = provider as ILogProviderStatusReceiver;
            if (statusReceiver != null)
            {
                statusReceiver.OnEnable();
            }
        }

        /// <summary>
        /// Remove provider from Log so no logs are passed to the provider.
        /// </summary>
        public static void RemoveProvider([NotNull] ILogProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider", "Cannot remove null provider.");
            }
            Provider.Remove(provider);
            var statusReceiver = provider as ILogProviderStatusReceiver;
            if (statusReceiver != null)
            {
                statusReceiver.OnDisable();
            }
        }
        
        public static void RemoveAllProviders()
        {
            foreach (ILogProvider logProvider in Provider)
            {
                RemoveProvider(logProvider);
            }
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Info" />
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Optional context that is passed to the <see cref="ILogProvider" />.</param>
        /// <param name="category">Specify if this log should belong to some grouping or combined category</param>
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void Info([NotNull] string message, [CanBeNull] object context = null, string category = "")
        {
            LogFormatContext(LogLevel.Info, message, null, context, category);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Info" /> with format information.
        /// </summary>
        /// <param name="format">The message including formatting tags.</param>
        /// <param name="args">The objects to fill the formatting tags.</param>
        [StringFormatMethod("format")]
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void InfoFormat([NotNull] string format, params object[] args)
        {
            LogFormatContext(LogLevel.Info, format, args, null);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Info" /> with format information.
        /// </summary>
        /// <param name="context">Optional context that is passed to the <see cref="ILogProvider" />.</param>
        /// <param name="format">The message including formating tags.</param>
        /// <param name="args">The objects to fill the formatting tags.</param>
        [StringFormatMethod("format")]
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void InfoFormatContext([CanBeNull] object context, [NotNull] string format, params object[] args)
        {
            LogFormatContext(LogLevel.Info, format, args, context);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Warn" />
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Optional context that is passed to the <see cref="ILogProvider" />.</param>
        /// <param name="category">Specify if this log should belong to some grouping or combined category</param>
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void Warn([NotNull] string message, [CanBeNull] object context = null, string category = null)
        {
            LogFormatContext(LogLevel.Warn, message, null, context, category);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Warn" /> with format information.
        /// </summary>
        /// <param name="format">The message including formating tags.</param>
        /// <param name="args">The objects to fill the formatting tags.</param>
        [StringFormatMethod("format")]
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void WarnFormat([NotNull] string format, params object[] args)
        {
            LogFormatContext(LogLevel.Warn, format, args, null);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Warn" /> with format information.
        /// </summary>
        /// <param name="context">Optional context that is passed to the <see cref="ILogProvider" />.</param>
        /// <param name="format">The message including formating tags.</param>
        /// <param name="args">The objects to fill the formatting tags.</param>
        [StringFormatMethod("format")]
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void WarnFormatContext([CanBeNull] object context, [NotNull] string format, params object[] args)
        {
            LogFormatContext(LogLevel.Warn, format, args, context);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Error" />
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="context">Optional context that is passed to the <see cref="ILogProvider" />.</param>
        /// <param name="category">Specify if this log should belong to some grouping or combined category</param>
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void Error([NotNull] string message, [CanBeNull] object context = null, string category = null)
        {
            LogFormatContext(LogLevel.Error, message, null, context, category);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Error" /> with format information.
        /// </summary>
        /// <param name="format">The message including formating tags.</param>
        /// <param name="args">The objects to fill the formatting tags.</param>
        [StringFormatMethod("format")]
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void ErrorFormat([NotNull] string format, params object[] args)
        {
            LogFormatContext(LogLevel.Error, format, args, null);
        }

        /// <summary>
        /// Log to <see cref="ILogProvider" />s with log level <see cref="LogLevel.Error" /> with format information.
        /// </summary>
        /// <param name="context">Optional context that is passed to the <see cref="ILogProvider" />.</param>
        /// <param name="format">The message including formating tags.</param>
        /// <param name="args">The objects to fill the formatting tags.</param>
        [StringFormatMethod("format")]
        [Conditional(ConditionalDefine)]
        [Conditional(UnityTestConditionalDefine)]
        public static void ErrorFormatContext(object context, string format, params object[] args)
        {
            LogFormatContext(LogLevel.Error, format, args, context);
        }

        /// <summary>
        /// Main method to output logs.
        /// </summary>
        private static void LogFormatContext(LogLevel logLevel, [NotNull] string format, object[] args,
            [CanBeNull] object context, string category = null)
        {
            string transcodedLog = null;
            if (ProviderCount != 0)
            {
                for (var i = 0; i < Provider.Count; i++)
                {
                    ILogProvider provider = Provider[i];
                    TranscodedLog(provider, logLevel, context, format, args, category, ref transcodedLog);
                }
            }
            else
            {
                TranscodedLog(FallbackProvider, logLevel, context, format, args, category, ref transcodedLog);
            }
        }

        private static void TranscodedLog(ILogProvider provider, LogLevel logLevel, object context, string format, [CanBeNull] object[] args,
            string category, ref string transcodedLog)
        {
            if (logLevel < provider.LogLevel)
            {
                // This provider does not want to log at this log level.
                return;
            }

            // The provider wants format information
            var transcodedProvider = provider as ILogTranscodedProvider;

            if (transcodedProvider != null)
            {
                // Pass transcoded log
                if (transcodedLog == null)
                {
                    // Lazy loading generation of format string
                    transcodedLog = (args == null || args.Length == 0) ? format : string.Format(format, args);
                }

                switch (logLevel)
                {
                    case LogLevel.Info:
                        transcodedProvider.Info(context, transcodedLog, category);
                        break;
                    case LogLevel.Warn:
                        transcodedProvider.Warn(context, transcodedLog, category);
                        break;
                    case LogLevel.Error:
                        transcodedProvider.Error(context, transcodedLog, category);
                        break;
                }
            }
            else
            {
                var formatProvider = provider as ILogFormatProvider;

                if (formatProvider != null)
                {
                    // Pass log with format information
                    switch (logLevel)
                    {
                        case LogLevel.Info:
                            formatProvider.InfoFormat(context, format, args, category);
                            break;
                        case LogLevel.Warn:
                            formatProvider.WarnFormat(context, format, args, category);
                            break;
                        case LogLevel.Error:
                            formatProvider.ErrorFormat(context, format, args, category);
                            break;
                    }
                }
            }
        }
    }
}