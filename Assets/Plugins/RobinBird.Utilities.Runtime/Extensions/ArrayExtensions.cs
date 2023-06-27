#region Disclaimer

// <copyright file="ArrayExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

using System.Collections.Generic;

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System;

    /// <summary>
    /// Extension and helper methods for <see cref="AppDomain" /> class.
    /// </summary>
    public static class ArrayExtensions
    {
        public static T[] Remove<T>(this T[] array, T itemToRemove)
        {
            var result = new List<T>(array);
            result.Remove(itemToRemove);
            return result.ToArray();
        }

        public static T[] Append<T>(this T[] array, T itemToAdd)
        {
            return Append(array, new[] {itemToAdd});
        }

        /// <summary>
        /// Appends <see cref="arrays" /> to array.
        /// </summary>
        /// <returns>Combined array.</returns>
        public static T[] Append<T>(this T[] array, params T[][] arrays)
        {
            int finalArrayLength = array.Length;
            for (var i = 0; i < arrays.Length; i++)
            {
                T[] a = arrays[i];
                if (a == null || a.Length == 0)
                {
                    continue;
                }
                finalArrayLength += a.Length;
            }

            var final = new T[finalArrayLength];

            Array.Copy(array, 0, final, 0, array.Length);
            int nextIndex = array.Length;

            for (var i = 0; i < arrays.Length; i++)
            {
                T[] a = arrays[i];
                if (a == null || a.Length == 0)
                {
                    continue;
                }
                Array.Copy(a, 0, final, nextIndex, a.Length);
            }

            return final;
        }
        
        public static T[] Prepend<T>(this T[] array, T itemToAdd)
        {
            return Prepend(array, new[] {itemToAdd});
        }

        public static T[] Prepend<T>(this T[] array, params T[][] arrays)
        {
            int finalArrayLength = array.Length;
            for (var i = 0; i < arrays.Length; i++)
            {
                T[] a = arrays[i];
                if (a == null || a.Length == 0)
                {
                    continue;
                }
                finalArrayLength += a.Length;
            }

            var final = new T[finalArrayLength];

            Array.Copy(array, 0, final, finalArrayLength - array.Length, array.Length);
            int nextIndex = finalArrayLength - array.Length;

            for (var i = 0; i < arrays.Length; i++)
            {
                T[] a = arrays[i];
                if (a == null || a.Length == 0)
                {
                    continue;
                }
                Array.Copy(a, 0, final, finalArrayLength - nextIndex - a.Length, a.Length);
                nextIndex = finalArrayLength - nextIndex - a.Length;
            }

            return final;
        }

        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
    }
}