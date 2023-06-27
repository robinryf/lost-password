#region Disclaimer

// <copyright file="StringBuilderExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System.Text;

    /// <summary>
    /// Extension and helper methods for <see cref="StringBuilder" /> class.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Clears the contents of the string builder.
        /// </summary>
        /// <param name="value">
        /// The <see cref="StringBuilder" /> to clear.
        /// </param>
        /// <param name="resetBuffer">
        /// If true the buffer of the <see cref="StringBuilder" /> is freed and will get picked up by GC.
        /// </param>
        public static void Clear(this StringBuilder value, bool resetBuffer = false)
        {
            value.Length = 0;
            if (resetBuffer)
            {
                value.Capacity = 0;
            }
        }
    }
}