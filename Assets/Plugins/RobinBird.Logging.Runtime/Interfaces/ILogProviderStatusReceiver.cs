#region Disclaimer
// <copyright file="ILogProviderStatusReceiver.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Runtime.Interfaces
{
    /// <summary>
    /// <see cref="ILogProvider" /> can implement this interface to know when the logging status
    /// changes.
    /// </summary>
    public interface ILogProviderStatusReceiver
    {
        /// <summary>
        /// Gets called when <see cref="Log" /> sends logs to this provider.
        /// </summary>
        void OnEnable();

        /// <summary>
        /// Gets called when <see cref="Log" /> stops sending logs to this provider.
        /// </summary>
        void OnDisable();
    }
}