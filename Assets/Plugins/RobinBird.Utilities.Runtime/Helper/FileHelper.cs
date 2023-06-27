using System.IO;

namespace RobinBird.Utilities.Runtime.Helper
{
	public static class FileHelper
	{
		public static bool IsSymbolic(string path)
		{
			FileInfo pathInfo = new FileInfo(path);
			return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
		}
	}
}