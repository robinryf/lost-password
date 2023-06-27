namespace RobinBird.Utilities.Unity.Editor.UnityUI
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Editor helper controls to transform Unity UI controls
    /// </summary>
    public static class UnityUITools
    {
#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("uGUI/Anchors to Corners %[")]
#endif
        private static void AnchorsToCorners()
        {
            Undo.RecordObjects(Selection.transforms, "Anchors to Corners");
            foreach (Transform transform in Selection.transforms)
            {
                var t = transform as RectTransform;
                var pt = Selection.activeTransform.parent as RectTransform;

                if (t == null || pt == null) return;

                var newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                    t.anchorMin.y + t.offsetMin.y / pt.rect.height);
                var newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                    t.anchorMax.y + t.offsetMax.y / pt.rect.height);

                t.anchorMin = newAnchorsMin;
                t.anchorMax = newAnchorsMax;
                t.offsetMin = t.offsetMax = new Vector2(0, 0);
            }
        }

#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("uGUI/Corners to Anchors %]")]
#endif
        private static void CornersToAnchors()
        {
            Undo.RecordObjects(Selection.transforms, "Corners to Anchors");
            foreach (Transform transform in Selection.transforms)
            {
                var t = transform as RectTransform;

                if (t == null) return;

                t.offsetMin = t.offsetMax = new Vector2(0, 0);
            }
        }

#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("uGUI/Mirror Horizontally Around Anchors %;")]
#endif
        private static void MirrorHorizontallyAnchors()
        {
            MirrorHorizontally(false);
        }

#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("uGUI/Mirror Horizontally Around Parent Center %:")]
#endif
        private static void MirrorHorizontallyParent()
        {
            MirrorHorizontally(true);
        }

        private static void MirrorHorizontally(bool mirrorAnchors)
        {
            Undo.RecordObjects(Selection.transforms, "Mirror Horizontally");
            foreach (Transform transform in Selection.transforms)
            {
                var t = transform as RectTransform;
                var pt = Selection.activeTransform.parent as RectTransform;

                if (t == null || pt == null) return;

                if (mirrorAnchors)
                {
                    Vector2 oldAnchorMin = t.anchorMin;
                    t.anchorMin = new Vector2(1 - t.anchorMax.x, t.anchorMin.y);
                    t.anchorMax = new Vector2(1 - oldAnchorMin.x, t.anchorMax.y);
                }

                Vector2 oldOffsetMin = t.offsetMin;
                t.offsetMin = new Vector2(-t.offsetMax.x, t.offsetMin.y);
                t.offsetMax = new Vector2(-oldOffsetMin.x, t.offsetMax.y);

                t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
            }
        }

#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("uGUI/Mirror Vertically Around Anchors %'")]
#endif
        private static void MirrorVerticallyAnchors()
        {
            MirrorVertically(false);
        }

#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("uGUI/Mirror Vertically Around Parent Center %\"")]
#endif
        private static void MirrorVerticallyParent()
        {
            MirrorVertically(true);
        }

        private static void MirrorVertically(bool mirrorAnchors)
        {
            Undo.RecordObjects(Selection.transforms, "Mirror Vertically");
            foreach (Transform transform in Selection.transforms)
            {
                var t = transform as RectTransform;
                var pt = Selection.activeTransform.parent as RectTransform;

                if (t == null || pt == null) return;

                if (mirrorAnchors)
                {
                    Vector2 oldAnchorMin = t.anchorMin;
                    t.anchorMin = new Vector2(t.anchorMin.x, 1 - t.anchorMax.y);
                    t.anchorMax = new Vector2(t.anchorMax.x, 1 - oldAnchorMin.y);
                }

                Vector2 oldOffsetMin = t.offsetMin;
                t.offsetMin = new Vector2(t.offsetMin.x, -t.offsetMax.y);
                t.offsetMax = new Vector2(t.offsetMax.x, -oldOffsetMin.y);

                t.localScale = new Vector3(t.localScale.x, -t.localScale.y, t.localScale.z);
            }
        }
    }
}