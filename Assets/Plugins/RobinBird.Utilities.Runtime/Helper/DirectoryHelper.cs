namespace RobinBird.Utilities.Runtime.Helper
{
    using System.IO;

    /// <summary>
    /// Helper methods for directories
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Copy one directory tree to another location.
        /// (e.g. C:/../MySourceDirectory to C:/../MyDestinationDirectory)
        /// </summary>
        public static void CopyDirectory(string sourcePath, string destinationPath)
        {
            if (destinationPath[destinationPath.Length - 1] != Path.DirectorySeparatorChar)
                destinationPath += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
            string[] files = Directory.GetFileSystemEntries(sourcePath);
            foreach (string element in files)
            {
                // Sub directories
                if (Directory.Exists(element))
                    CopyDirectory(element, destinationPath + Path.GetFileName(element));
                // Files in directory
                else
                    File.Copy(element, destinationPath + Path.GetFileName(element), true);
            }
        }
    }
}