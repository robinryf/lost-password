#region Disclaimer
// <copyright file="ILogTranscodedProvider.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Runtime.Interfaces
{
    using JetBrains.Annotations;

    /// <summary>
    /// Log provider that receives logs. This is the standard log provider.
    /// </summary>
    public interface ILogTranscodedProvider : ILogProvider
    {
        void Info([CanBeNull] object context, [NotNull] string message, string category);
        void Warn([CanBeNull] object context, [NotNull] string message, string category);
        void Error([CanBeNull] object context, [NotNull] string message, string category);
    }
}