#region Disclaimer
// <copyright file="LogLevel.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Runtime
{
    /// <summary>
    /// Specifies different log levels to prioritize logs.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Information about the program that helps understanding what is happening.
        /// </summary>
        Info = 100,

        /// <summary>
        /// Events that should not happen and can influence the product quality but do not
        /// interrupt execution.
        /// </summary>
        Warn = 200,

        /// <summary>
        /// Events that should not happen and can harm the execution and stability of the program.
        /// </summary>
        Error = 300,

        /// <summary>
        /// This log level does not output any logs.
        /// </summary>
        Disabled = int.MaxValue,
    }
}