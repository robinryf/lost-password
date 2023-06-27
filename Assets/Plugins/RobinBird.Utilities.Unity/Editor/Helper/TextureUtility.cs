#region Disclaimer
// <copyright file="TextureUtility.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Unity.Editor.Helper
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
#if UNITY_2017_2_OR_NEWER
    using RectInt = UnityEngine.RectInt;
    using Vector2Int = UnityEngine.Vector2Int;
#else
    using RectInt = RobinBird.Utilities.Unity.Legacy.RectInt;
    using Vector2Int = RobinBird.Utilities.Unity.Legacy.Vector2Int;
#endif

    public static class TextureUtility
    {
        /// <summary>
        /// Returns the real texture size before Unity import processing.
        /// When restricting the <see cref="TextureImporterSettings.maxTextureSizeSet"/> Unity reduces the texture size.
        /// Since this method uses reflection it is possible that the Unity api has changed and this check can fail. Use the
        /// return value to check for this.
        /// </summary>
        /// <param name="texture">The texture to analyze.</param>
        /// <param name="width">The resulting width.</param>
        /// <param name="height">The resulting height.</param>
        /// <returns>False if the reflection method failed.</returns>
        public static bool GetRealTextureSize(Texture2D texture, out int width, out int height)
        {
            height = width = 0;
            if (texture == null)
            {
                return false;
            }

            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer == null)
            {
                return false;
            }

            object[] args = new object[2] {0, 0};
            MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);

            if (mi == null)
            {
                Debug.LogError("This Unity version is not supported. Please contact the plugin owner.");
                return false;
            }

            mi.Invoke(importer, args);

            width = (int) args[0];
            height = (int) args[1];

            return true;
        }

        /// <summary>
        /// The rect of the original texture file without modifications made by the Unity Texture importer.
        /// </summary>
        public static RectInt GetRealTextureRect(Texture2D texture)
        {
            int width;
            int height;
            if (GetRealTextureSize(texture, out width, out height) == false)
            {
                return new RectInt(0, 0, texture.width, texture.height);
            }
            return new RectInt(0, 0, width, height);
        }

        /// <summary>
        /// Get the modifications that Unity did to the texture on import in case there were size adjustments throught the
        /// 'maxSize' setting. This can happen if you have a 4k texture and set the max size to 2k. Then Unity downscales the
        /// texture. This scale represents the downscale of width and height.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Vector2 GetTextureImporterScale(Texture2D texture)
        {
            var nativeRect = GetRealTextureRect(texture);
            return new Vector2((float)texture.width / nativeRect.width, (float)texture.height / nativeRect.height);
        }
    }
}