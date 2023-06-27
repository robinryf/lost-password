#region Disclaimer

// <copyright file="PathHelper.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime.Helper
{
    using System;
    using System.IO;

    /// <summary>
    /// Helper class for path operations.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        /// <summary>
        /// Combines multiple path parts with the <see cref="Path.Combine" /> method.
        /// </summary>
        public static string Combine(params string[] pathParts)
        {
            if (pathParts == null || pathParts.Length == 0)
            {
                return null;
            }
            string currentPath = pathParts[0];
            for (var i = 1; i < pathParts.Length; i++)
            {
                currentPath = Path.Combine(currentPath, pathParts[i]);
            }
            return currentPath;
        }
    }
}