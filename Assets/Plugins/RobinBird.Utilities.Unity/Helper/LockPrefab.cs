namespace RobinBird.Utilities.Unity.Helper
{
    using Extensions;
    using UnityEngine;

    /// <summary>
    /// Utility script that locks a prefab from being modified in instances. This forces the user to only modify this
    /// on the Prefab Asset itself
    /// </summary>
    [ExecuteInEditMode]
    public class LockPrefab : MonoBehaviour
    {
        private void Awake()
        {
            if (IsEditable() == false)
            {
                GameObject obj = gameObject;
                ApplyEditLock(obj);
                obj.ApplyChangeToAllChilds(ApplyEditLock);
            }
        }

        private static void ApplyEditLock(Object obj)
        {
            obj.hideFlags |= HideFlags.NotEditable;
        }

        private static void RemoveEditLock(Object obj)
        {
            obj.hideFlags &= ~HideFlags.NotEditable;
        }

        private void OnDestroy()
        {
            RemoveEditLock(gameObject);
            gameObject.ApplyChangeToAllChilds(RemoveEditLock);
        }

        public bool IsEditable()
        {
            // ReSharper disable once RedundantAssignment
            bool isPreviewScene = false;
            // ReSharper disable once RedundantAssignment
            bool isPartOfPrefabInstance = false;
#if UNITY_EDITOR && UNITY_2019_1_OR_NEWER
            isPreviewScene = UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(gameObject);
            isPartOfPrefabInstance = UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject);
#endif
            return isPreviewScene || isPartOfPrefabInstance == false;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Destroy the locks because we don't need them in the build or playmode
        /// </summary>
        [UnityEditor.Callbacks.PostProcessScene]
        public static void PostBuildAction()
        {
            LockPrefab[] prefabLocks = FindObjectsOfType<LockPrefab>();

            foreach (LockPrefab prefabLock in prefabLocks)
            {
                GameObject go = prefabLock.gameObject;
                prefabLock.Destroy();
                RemoveEditLock(go);
                go.ApplyChangeToAllChilds(RemoveEditLock);
            }
        }
#endif
    }
}