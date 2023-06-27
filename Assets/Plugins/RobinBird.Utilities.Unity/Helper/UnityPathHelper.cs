#region Disclaimer

// <copyright file="UnityPathHelper.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Unity.Helper
{
    using System.IO;
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>
    /// Utility class for Unity path operations.
    /// </summary>
    public static class UnityPathHelper
    {
        /// <summary>
        /// Name of the streaming assets directory of Unity that holds data which can be accessed by the player and is not
        /// compressed into an assets file.
        /// </summary>
        public const string StreamingAssetDirectoryName = "StreamingAssets";

        private const string UnityResourcesDirectoryName = "Resources";

        /// <summary>
        /// Convert a path to a Unity path. Mainly converts backward slashes to forward slashes
        /// </summary>
        public static string ConvertToUnityPath([CanBeNull] string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    path = path.Replace(@"\\", "/");
                    return path.Replace(@"\", "/");
            }
            return path;
        }

        /// <summary>
        /// Converts path to Windows path. Mainly converts forward slashes to backward slashes.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConvertToWindowsPath([CanBeNull] string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            path = path.Replace("/", @"\\");
            return path;
        }

        /// <summary>
        /// Path combine method similar to <see cref="Path.Combine" /> but this only returns
        /// Unity style paths with forward slashes.
        /// </summary>
        /// <param name="path1">First part of path.</param>
        /// <param name="path2">Second part of path.</param>
        /// <returns>Unity style combined path.</returns>
        public static string Combine(string path1, string path2)
        {
            return ConvertToUnityPath(Path.Combine(path1, path2));
        }

        public static bool IsInResourcesDirectory(string path)
        {
            return path.Contains(UnityResourcesDirectoryName);
        }
    }
}