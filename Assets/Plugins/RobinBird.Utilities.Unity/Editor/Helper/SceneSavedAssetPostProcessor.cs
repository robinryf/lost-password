#region Disclaimer
// <copyright file="SceneSavedAssetPostProcessor.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Utilities.Unity.Editor.Helper
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEditor;
    using UnityEngine.SceneManagement;

    [UsedImplicitly]
    public class SceneSavedAssetPostProcessor : AssetModificationProcessor
    {
        public delegate void SceneSavedDelegate(Scene scene);

        public delegate void ScenesSavedDelegate(Scene[] scenes);

        /// <summary>
        /// Callback for when one scene are saved in the editor.
        /// Gets called multiple times if multiple scenes are saved.
        /// </summary>
        public static SceneSavedDelegate SceneSaved;

        /// <summary>
        /// Callback for when scenes are saved in the editor.
        /// </summary>
        public static ScenesSavedDelegate ScenesSaved;

        /// <summary>
        /// Called by Unity <see cref="AssetModificationProcessor" />
        /// </summary>
        [UsedImplicitly]
        public static string[] OnWillSaveAssets(string[] paths)
        {
            // Get the name of the scene to save.
            var scenes = new List<Scene>();

            for (var i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                if (path.EndsWith(".unity"))
                {
                    scenes.Add(SceneManager.GetSceneByPath(path));
                }
            }

            if (ScenesSaved != null)
            {
                ScenesSaved(scenes.ToArray());
            }

            for (var i = 0; i < scenes.Count; i++)
            {
                Scene scene = scenes[i];
                // A Scene has been saved
                if (SceneSaved != null)
                {
                    SceneSaved(scene);
                }
            }

            return paths;
        }
    }
}