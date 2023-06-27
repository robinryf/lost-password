namespace RobinBird.Utilities.Unity.Editor.Helper
{
    using System;
    using System.Collections.Generic;
    using Logging.Runtime;
    using UnityEditor;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Helper methods for <see cref="AssetDatabase"/>
    /// </summary>
    public static class AssetDatabaseHelper
    {
        /// <summary>
        /// Get a list of assets which reside under a given directory
        /// </summary>
        /// <param name="directoryPath">Path of directory in which the assets should be. Should start with 'Assets'. (e.g. Assets/MyGame ). Leave empty for whole project.</param>
        /// <param name="assetSearchFilter">Optional filter for assets in form of <see cref="AssetDatabase.FindAssets(string)"/> (e.g. 't:Prefab' assetName )</param>
        /// <param name="verifyFunc">Optional function to pass to verify some property on the asset to keep it in the results</param>
        /// <returns>A list of all assets which have been found</returns>
        public static List<T> GetAssetsInDirectory<T>(string directoryPath, string assetSearchFilter = null, Func<T, bool> verifyFunc = null) where T : Object
        {
            var paths = AssetDatabase.FindAssets(assetSearchFilter);

            var results = new List<T>();

            foreach (string assetGuid in paths)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                
                if (string.IsNullOrEmpty(directoryPath) || assetPath.StartsWith(directoryPath))
                {
                    Log.Info("Found path: " + assetPath);

                    var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                    if (asset == null)
                    {
                        continue;
                    }

                    if (verifyFunc != null)
                    {
                        if (verifyFunc(asset) == false)
                        {
                            continue;
                        }
                    }
                    
                    results.Add(asset);
                }
            }

            return results;
        }

        public static string GetAssetGuid(Object asset)
        {
            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
        }

        public static T LoadAssetAtGuid<T>(string guid) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
        }
    }
}