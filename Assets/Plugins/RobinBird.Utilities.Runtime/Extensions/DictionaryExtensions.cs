#region Disclaimer

// <copyright file="DictionaryExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Extension and helper methods for <see cref="Dictionary{TKey,TValue}" /> class.
    /// </summary>
    public static class DictionaryExtensions
    {
        private static StringBuilder printStringBuilder;

        /// <summary>
        /// Add item to dictionary or if the dictionary already contains the item, replace it.
        /// </summary>
        public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
        
        public static string ToStringKeyValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (printStringBuilder == null)
            {
                printStringBuilder = new StringBuilder();
            }

            printStringBuilder.Append("Count: ");
            printStringBuilder.Append(dictionary.Count);
            printStringBuilder.Append(" Type: ");
            printStringBuilder.AppendLine(dictionary.GetType().ToString());
            
            foreach (KeyValuePair<TKey,TValue> pair in dictionary)
            {
                printStringBuilder.Append("Key: ");
                printStringBuilder.Append(pair.Key.ToString());
                printStringBuilder.Append(" Value: ");
                if (pair.Value == null)
                    printStringBuilder.AppendLine();
                else
                    printStringBuilder.AppendLine(pair.Value.ToString());
            }

            string result = printStringBuilder.ToString();
            printStringBuilder.Clear();
            return result;
        }

        public static TValue GetOptional<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue fallback = default)
        {
            if (key == null)
                return fallback;
            TValue result;
            if (dict.TryGetValue(key, out result) == false)
            {
                result = fallback;
            }

            return result;
        }
    }
}