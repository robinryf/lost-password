using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RobinBird.Utilities.Runtime.Extensions
{
    public static class QueueExtensions
    {
        private static StringBuilder printStringBuilder;
        
        public static string ToStringList<T>(this Queue<T> queue)
        {
            if (printStringBuilder == null)
            {
                printStringBuilder = new StringBuilder();
            }
            
            foreach (T item in queue)
            {
                printStringBuilder.AppendLine(item.ToString());
            }

            var result = printStringBuilder.ToString();
            printStringBuilder.Clear();
            return result;
        }
    }
}