using System.Text;

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for <see cref="List{T}" /> struct.
    /// </summary>
    public static class ListExtensions
    {
        private static StringBuilder printStringBuilder;

        /// <summary>
        /// Resize an list to target count. If to many elements are in list they will be removed from the end.
        /// </summary>
        /// <typeparam name="T">Type of the list.</typeparam>
        /// <param name="list">List to resize.</param>
        /// <param name="targetCount">The target count to reach.</param>
        /// <param name="createNewFunc">A function to create a new list member if necessary.</param>
        public static void Resize<T>(this List<T> list, int targetCount, Func<T> createNewFunc)
        {
            Resize(list, targetCount, createNewFunc, listToRemove => { listToRemove.RemoveAt(listToRemove.Count - 1); });
        }

        /// <summary>
        /// Resize an list to target count. If to many elements are in list they will be removed from the end.
        /// </summary>
        /// <typeparam name="T">Type of the list.</typeparam>
        /// <param name="list">List to resize.</param>
        /// <param name="targetCount">The target count to reach.</param>
        /// <param name="createNewFunc">A function to create a new list member if necessary.</param>
        /// <param name="removeItemAction">
        /// A action to let the consumer of this api decide which and how many items
        /// should be removed when list ist to big
        /// </param>
        public static void Resize<T>(this List<T> list, int targetCount, Func<T> createNewFunc, Action<List<T>> removeItemAction)
        {
            while (list.Count != targetCount)
            {
                int delta = targetCount - list.Count;
                if (delta > 0)
                {
                    T newItem = createNewFunc != null ? createNewFunc() : default;
                    list.Add(newItem);
                }
                else
                {
                    removeItemAction?.Invoke(list);
                }
            }
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }
        
        public static string ToValueString<T>(this List<T> list)
        {
	        if (list == null)
	        {
		        return "null";
	        }
	        if (printStringBuilder == null)
            {
                printStringBuilder = new StringBuilder();
            }

            printStringBuilder.Append("Count: ");
            printStringBuilder.AppendLine(list.Count.ToString());

            for (var i = 0; i < list.Count; i++)
            {
                T value = list[i];
                printStringBuilder.Append("[");
                printStringBuilder.Append(i.ToString());
                printStringBuilder.Append("] ");
                printStringBuilder.AppendLine(value.ToString());
            }

            string result = printStringBuilder.ToString();
            printStringBuilder.Clear();
            return result;
        }
    }
}