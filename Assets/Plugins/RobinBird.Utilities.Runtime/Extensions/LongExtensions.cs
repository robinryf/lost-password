#region Disclaimer

// <copyright file="LongExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime.Extensions
{
    /// <summary>
    /// Extensions and helper methods for <see cref="long" /> struct.
    /// </summary>
    public static class LongExtensions
    {
        /// <summary>
        /// Returns <see cref="long" /> created from two <see cref="int" />s.
        /// </summary>
        /// <param name="left">Left component of resulting long.</param>
        /// <param name="right">Right component of resulting long.</param>
        public static long CreateFromInts(int left, int right)
        {
            //implicit conversion of left to a long
            long res = left;

            //shift the bits creating an empty space on the right
            // ex: 0x0000CFFF becomes 0xCFFF0000
            res = (res << 32);

            //combine the bits on the right with the previous value
            // ex: 0xCFFF0000 | 0x0000ABCD becomes 0xCFFFABCD
            res = res | (uint) right; //uint first to prevent loss of signed bit

            //return the combined result
            return res;
        }

        /// <summary>
        /// Splits a variable of datatype <see cref="long" /> into two <see cref="int" />s.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void SplitToInts(long value, out int left, out int right)
        {
            left = (int) (value >> 32);
            right = (int) (value & 0xffffffffL);
        }
    }
}