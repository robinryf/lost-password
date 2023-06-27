#region Disclaimer
// <copyright file="ILogProvider.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Runtime.Interfaces
{
    /// <summary>
    /// Base interface for log providers. <see cref="Log" /> can server multiple
    /// providers to pass log. The log providers then can redirect the log the
    /// an appropriate target (e.g. console, file, etc.).
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Specify what the minimum log level of this provider is
        /// </summary>
        LogLevel LogLevel { get; }
    }
}