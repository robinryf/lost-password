namespace RobinBird.Utilities.Unity.Editor.Inspectors
{
    using Unity.Helper;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LockPrefab))]
    public class LockPrefabInspector : GenericInspector<LockPrefab>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Target.IsEditable() == false)
            {
                bool prev = GUI.enabled;
                GUI.enabled = true;
                EditorGUILayout.HelpBox(
                    "This Prefab is locked from Instance modifications. Use the Prefab Preview Mode to edit across all instances.",
                    MessageType.Warning);
                GUI.enabled = prev;
            }
        }

        private static Texture lockTexture;

        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            GUIContent lockContent = EditorGUIUtility.IconContent("d_P4_LockedRemote");
            if (lockContent != null)
            {
                lockTexture = lockContent.image;
            }
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGui;
        }

        private static void OnHierarchyWindowItemOnGui(int instanceId, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

            if (obj == null)
            {
                return;
            }

            var lockPrefab = obj.GetComponentInParent<LockPrefab>();

            if (lockPrefab == null)
            {
                return;
            }

            if (lockPrefab.IsEditable() == false)
            {
                var iconRect = new Rect(selectionRect);
                iconRect.xMin += iconRect.width - 16;
                GUI.DrawTexture(iconRect, lockTexture);
            }
        }
    }
}