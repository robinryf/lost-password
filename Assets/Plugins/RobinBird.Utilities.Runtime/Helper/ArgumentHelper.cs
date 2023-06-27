using System;

namespace RobinBird.Utilities.Runtime.Helper
{
	public static class ArgumentHelper
    {
        public static string GetCLIArgument(string key)
        {
			var args = Environment.GetCommandLineArgs();

            for (var i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                int nextIndex = i + 1;
                if (arg == key && args.Length > nextIndex)
                {
                    return args[nextIndex];
                }
            }

            return string.Empty;
        }

        public static bool TryGetCLIArgument(string key, out string value)
        {
            value = GetCLIArgument(key);
            return string.IsNullOrEmpty(value) == false;
        }
    }
}