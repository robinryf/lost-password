#region Disclaimer

// <copyright file="InvalidDataException.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime.Exceptions
{
    using System;

    /// <summary>
    /// Exception to raise on invalid data.
    /// </summary>
    public class InvalidDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.InvalidDataException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidDataException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.InvalidDataException" /> class with a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the
        /// <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles
        /// the inner exception.
        /// </param>
        public InvalidDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}