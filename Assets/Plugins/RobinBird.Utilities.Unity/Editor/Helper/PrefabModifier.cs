namespace RobinBird.Utilities.Unity.Editor.Helper
{
    using System;
    using Logging.Runtime;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Used to instantiate a prefab in the scene so it can be edited freely. When this
    /// class is disposed the changes are saved back to the prefab.
    /// </summary>
    public class PrefabModifier : IDisposable
    {
        /// <summary>
        /// If false changes will not be saved to prefab. GameObject instance will be destroyed
        /// though.
        /// </summary>
        public bool SaveChanges = true;

        /// <summary>
        /// The instance that was created to modify the prefab. Use this and do whatever you like
        /// to change the prefab. Do not destroy this GameObject
        /// </summary>
        public readonly GameObject PrefabInstance;

        public PrefabModifier(GameObject prefab)
        {
            // create an instance of the prefab
            PrefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (PrefabInstance == null)
            {
                Log.Error("Could not create prefab to assign sprites to.");
            }
        }


        #region IDisposable Implementation

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (SaveChanges)
            {
                // apply the instance to the prefab
                Object sourcePrefab;
#if UNITY_2018_2_OR_NEWER
                sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(PrefabInstance);
#else
                sourcePrefab = PrefabUtility.GetPrefabParent(PrefabInstance);
#endif

	            var assetPath = AssetDatabase.GetAssetPath(sourcePrefab);

                if (string.IsNullOrEmpty(assetPath) == false)
                {
					PrefabUtility.SaveAsPrefabAssetAndConnect(PrefabInstance, assetPath, InteractionMode.AutomatedAction);
                }
            }

            // remove the instance from the scene
            UnityEngine.Object.DestroyImmediate(PrefabInstance);
        }

        #endregion
    }
}