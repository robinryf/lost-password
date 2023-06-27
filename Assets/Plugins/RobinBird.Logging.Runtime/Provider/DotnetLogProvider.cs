#region Disclaimer
// <copyright file="DotnetLogProvider.cs">
// Copyright (c) 2019 - 2019 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Logging.Runtime.Provider
{
    using System;
    using Interfaces;

    public class DotnetLogProvider : ILogFormatProvider
    {
        public LogLevel LogLevel
        {
            get { return LogLevel.Info; }
        }
        public void InfoFormat(object context, string format, object[] args, string category)
        {
            Console.WriteLine(format, args);
        }

        public void WarnFormat(object context, string format, object[] args, string category)
        {
            Console.WriteLine(format, args);
        }

        public void ErrorFormat(object context, string format, object[] args, string category)
        {
            Console.WriteLine(format, args);
        }
    }
}