#region Disclaimer
// <copyright file="UnityLogProvider.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Unity.Provider
{
    using Runtime;
    using Runtime.Interfaces;
    using UnityEngine;

    /// <summary>
    /// Logs to Unity console in Editor and to Log file in built player.
    /// Log locations can be found at: https://docs.unity3d.com/Manual/LogFiles.html
    /// </summary>
    public class UnityLogProvider : ILogFormatProvider
    {
        public UnityLogProvider(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

        #region ILogFormatProvider Implementation
        public void InfoFormat(object context, string format, object[] args, string category)
        {
            format = AddToConsoleFilter(format, category);
            if (context == null && (args == null || args.Length == 0))
            {
                Debug.Log(format);
            }
            else if (context != null && (args == null || args.Length == 0))
            {
                Debug.Log(format, (Object)context);
            }
            else
            {
                Debug.LogFormat((Object) context, format, args);
            }
        }

        public void WarnFormat(object context, string format, object[] args, string category)
        {
            format = AddToConsoleFilter(format, category);
            if (context == null && (args == null || args.Length == 0))
            {
                Debug.LogWarning(format);
            }
            else if (context != null && args.Length == 0)
            {
                Debug.LogWarning(format, (Object)context);
            }
            else
            {
                Debug.LogWarningFormat((Object) context, format, args);
            }
        }

        public void ErrorFormat(object context, string format, object[] args, string category)
        {
            if (string.IsNullOrEmpty(category) == false)
            {
                // For errors we just want to add the category and let them be as visible as possible
                format = $"[{category}] {format}";
            }
            if (context == null && (args == null || args.Length == 0))
            {
                Debug.LogError(format);
            }
            else if (context != null && (args == null || args.Length == 0))
            {
                Debug.LogError(format, (Object)context);
            }
            else
            {
                Debug.LogErrorFormat((Object) context, format, args);
            }
        }

        private static string AddToConsoleFilter(string log, string category)
        {
            if (Application.isEditor && string.IsNullOrEmpty(category) == false)
            {
                // Format for EditorConsole Pro to put log into specific category
                return $"[{category}] {log}\nCPAPI:{{\"cmd\":\"Filter\" \"name\":\"{category}\"}}";
            }

            return log;
        }
        #endregion

        #region ILogProvider Implementation
        public LogLevel LogLevel { get; set; }
        #endregion
    }
}