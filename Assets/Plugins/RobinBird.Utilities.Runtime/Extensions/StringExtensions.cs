using System;
using System.Text;

namespace RobinBird.Utilities.Runtime.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Make first letter upper case
        /// </summary>
        public static string ToFirstUpperChar(this string value)
        {
            var firstChar = value[0].ToString();
            firstChar = firstChar.ToUpperInvariant();
            value = firstChar + value.Remove(0, 1);
            return value;
        }
        
        /// <summary>
        /// Make first letter lower case
        /// </summary>
        public static string ToFirstLowerChar(this string value)
        {
            var firstChar = value[0].ToString();
            firstChar = firstChar.ToLowerInvariant();
            value = firstChar + value.Remove(0, 1);
            return value;
        }

        public static string ReplaceAt(this string value, int index, char newChar)
        {
            value = value.Insert(index, newChar.ToString());
            value = value.Remove(index + 1, 1);
            return value;
        }

        /// <summary>
        /// Removes last occurrence of <paramref name="removeString"/> from the string
        /// </summary>
        public static string RemoveLast(this string value, string removeString)
        {
	        var index = value.LastIndexOf(removeString, StringComparison.Ordinal);

	        if (index >= 0)
	        {
		        return value.Remove(index, removeString.Length);
	        }

	        return value;
        }
        
        public static string ToSnakeCase(this string text)
        {
	        if(text == null) {
		        throw new ArgumentNullException(nameof(text));
	        }
	        if(text.Length < 2) {
		        return text;
	        }
	        var sb = new StringBuilder();
	        sb.Append(char.ToLowerInvariant(text[0]));
	        for(int i = 1; i < text.Length; ++i) {
		        char c = text[i];
		        if(char.IsUpper(c)) {
			        sb.Append('_');
			        sb.Append(char.ToLowerInvariant(c));
		        } else {
			        sb.Append(c);
		        }
	        }
	        return sb.ToString();
        }
    }
}