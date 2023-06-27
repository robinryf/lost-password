#region Disclaimer
// <copyright file="EmptyLogProvider.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Runtime.Provider
{
    using Interfaces;

    /// <summary>
    /// Empty log provider so that <see cref="Log" /> has a <see cref="ILogProvider" /> to work with and does
    /// not have to check for null all the time.
    /// </summary>
    public class EmptyLogProvider : ILogProvider
    {
        public EmptyLogProvider()
        {
            LogLevel = LogLevel.Disabled;
        }

        #region ILogProvider Implementation
        public LogLevel LogLevel { get; private set; }
        #endregion
    }
}