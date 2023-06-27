using RobinBird.Utilities.Runtime.Extensions;
using UnityEditor;

namespace RobinBird.Utilities.Unity.Editor.Helper
{
    public class ContextMenuHelpers
    {
#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("Assets/Open in text editor", false, 0)]
#endif
        public static void OpenInTextEditor()
        {
            var guids = Selection.assetGUIDs;
            if (guids.IsNullOrEmpty())
                return;
            
            EditorUtility.OpenWithDefaultApp(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("Assets/Copy GUID", false, 0)]
#endif
        public static void CopyGuid()
        {
            var guids = Selection.assetGUIDs;
            if (guids.IsNullOrEmpty())
                return;

            EditorGUIUtility.systemCopyBuffer = guids[0];
        }
    }
}