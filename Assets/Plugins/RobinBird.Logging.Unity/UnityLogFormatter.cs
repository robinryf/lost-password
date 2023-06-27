#region Disclaimer
// <copyright file="UnityLogFormatter.cs">
// Copyright (c) 2019 - 2019 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Unity
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public static class UnityLogFormatter
    {
        private static readonly StringBuilder exceptionMessageBuilder = new StringBuilder();
        private static readonly Regex stackTraceRegex = new Regex(@"at (?<Method>.*) \((?<Arguments>.*)\) \[(?<Hash>.*)\] in (?<Path>.*):(?<Line>\d+)");

        public static string FormatNestedException(Exception ex, StringBuilder builder = null)
        {
	        var sb = builder ?? exceptionMessageBuilder; 
	        
            sb.Append("<color=red>Exception:");
			sb.Append(ex.GetType().Name);
			sb.Append("</color><b>");
            sb.Append(ex.Message);
            sb.Append("</b>\n");
            StacktraceWithLinks(ex.StackTrace, sb);
            if (ex.InnerException != null)
            {
				sb.Append(FormatNestedException(ex.InnerException));
            }
            sb.Append("\n<color=red>-----------</color>\n");

            string result = sb.ToString();
            if (builder == null)
            {
	            // Only clear if we are the root method
				sb.Clear();
            }
            return result;
        }
        
        /// <summary>
        /// Converts a Exception stacktrace in a way to show hyperlinks in the Unity Editor Console window
        /// </summary>
        public static void StacktraceWithLinks(string stacktrace, StringBuilder builder)
        {
            if (Application.isEditor == false)
            {
                // No need for this when not in Editor
                builder.Append(stacktrace);
                return;
            }
            var matches = stackTraceRegex.Matches(stacktrace);

            foreach (Match match in matches)
            {
                string path = match.Groups["Path"].Value;
                path = ConvertExceptionPath(path);
                
                builder.Append(match.Groups["Method"]);
                builder.Append("(");
                builder.Append(match.Groups["Arguments"]);
                builder.Append(") (at ");
                builder.Append("<a href=\"" + path + "\" line=\"" + match.Groups["Line"] + "\">");
                builder.Append(path);
                builder.Append(":");
                builder.Append(match.Groups["Line"]);
                builder.Append("</a>");
                builder.AppendLine(")");
            }
        }

        private static string ConvertExceptionPath(string path)
        {
            path = path.Replace(@"\\", "/").Replace(@"\", "/");
            int assetsIndex = path.IndexOf("/Assets/", StringComparison.InvariantCulture);
            // +1 for the leading Slash
            return path.Remove(0, assetsIndex + 1);
        }
    }
}