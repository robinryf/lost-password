#region Disclaimer
// <copyright file="EditorToolsLayout.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Utilities.Unity.Editor.Helper
{
    using System;
    using Unity.Helper;
    using UnityEditor;
    using UnityEngine;

    public static class EditorToolsLayout
    {
        private static readonly GUIContent[] pivotAlignmentOptions =
        {
            new GUIContent("Center"), 
            new GUIContent("Top Left"),
            new GUIContent("Top"),
            new GUIContent("Top Right"),
            new GUIContent("Left"),
            new GUIContent("Right"),
            new GUIContent("Bottom Left"),
            new GUIContent("Bottom"),
            new GUIContent("Bottom Right"),
            new GUIContent("Custom")
        };

        private static readonly GUIContent customPivotGuiContent = new GUIContent("Custom Pivot");
        private static readonly GUIContent pivotGuiContent = new GUIContent("Pivot");

        /// <summary>
        /// GUILayout version.
        /// Displays a text field with a button to open a file dialog. The path of the chosen file will be returned.
        /// </summary>
        /// <param name="content">Content of the label infront of the control</param>
        /// <param name="path">The currently store path.</param>
        /// <param name="relative">Should a relative path be created otherwise absolute path is used.</param>
        /// <param name="directory">In which directory should the file dialog start.</param>
        /// <param name="extension">The extension of the required file. Without a dot! "txt" not ".txt"</param>
        /// <returns>The newly selected path if user changed it.</returns>
        public static string FileField(GUIContent content, string path, bool relative = false, string directory = ".", string extension = "*")
        {
            return EditorTools.FileField(EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight), content, path, relative, directory, extension);
        }

        public static string DirectoryPathField(GUIContent content, string path, bool relative = false)
        {
            return EditorTools.DirectoryPathField(EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight), content, path, relative);
        }

        /// <summary>
        /// Draws a field that takes and returns an asset path of an project item inside Unity project. Ther user can drag any
        /// project item onto the supplied field and this control will return the relative path to the project directory.
        /// </summary>
        /// <param name="content">Prefix label content for the displayed control.</param>
        /// <param name="path">The path of the currently selected asset.</param>
        /// <returns>Path relative to the Unity project directory of the asset.</returns>
        public static string ProjectItemPathField(GUIContent content, string path)
        {
            return EditorTools.ProjectItemPathField(EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight), content, path);
        }

        /// <summary>
        /// Draw pivot control with custom offset support. Uses <see cref="SpriteAlignment"/> for options.
        /// </summary>
        /// <param name="currentPivot">The current pivot.</param>
        /// <returns>The modified pivot if user has changed ui.</returns>
        public static Vector2 DrawPivotControl(Vector2 currentPivot)
        {
            var alignment = PivotHelper.GetAlignmentFromValue(currentPivot);
            EditorGUI.BeginChangeCheck();
            var num = (SpriteAlignment)EditorGUILayout.Popup(pivotGuiContent, (int)alignment, pivotAlignmentOptions);
            if (EditorGUI.EndChangeCheck())
            {
                currentPivot = PivotHelper.GetPivotValue(num, currentPivot);
            }
            EditorGUI.BeginChangeCheck();
            currentPivot = EditorGUILayout.Vector2Field(customPivotGuiContent, currentPivot);
            if (EditorGUI.EndChangeCheck())
            {
                currentPivot = new Vector2(Mathf.Clamp(currentPivot.x, 0, 1), Mathf.Clamp(currentPivot.y, 0, 1));
            }
            return currentPivot;
        }
        
        /// <summary>
        /// Draw pivot control with custom offset support. Uses <see cref="SpriteAlignment"/> for options.
        /// </summary>
        /// <param name="pivotProperty">The current pivot property</param>
        /// <returns>True when values have changed.</returns>
        public static bool DrawPivotControlProperty(SerializedProperty pivotProperty)
        {
            bool hasChanged = false;
            SpriteAlignment alignment;
            if (pivotProperty.hasMultipleDifferentValues)
            {
                alignment = SpriteAlignment.Custom;
            }
            else
            {
                alignment = PivotHelper.GetAlignmentFromValue(pivotProperty.vector2Value);
            }
            EditorGUI.BeginChangeCheck();
            var num = (SpriteAlignment)EditorGUILayout.Popup(pivotGuiContent, (int)alignment, pivotAlignmentOptions);
            if (EditorGUI.EndChangeCheck())
            {
                pivotProperty.vector2Value = PivotHelper.GetPivotValue(num, pivotProperty.vector2Value);
                hasChanged = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(pivotProperty, customPivotGuiContent);
            if (EditorGUI.EndChangeCheck())
            {
                pivotProperty.vector2Value = new Vector2(Mathf.Clamp(pivotProperty.vector2Value.x, 0, 1), Mathf.Clamp(pivotProperty.vector2Value.y, 0, 1));
                hasChanged = true;
            }
            return hasChanged;
        }

        public static void DrawSortingLayer(Renderer renderer)
        {
            var sortingLayerNames = new string[SortingLayer.layers.Length];

            for (int i = 0; i < SortingLayer.layers.Length; i++)
            {
                var layer = SortingLayer.layers[i];
                sortingLayerNames[i] = layer.name;
            }

            // Look up the layer name using the current layer ID
            string oldName = SortingLayer.IDToName(renderer.sortingLayerID);

            // Use the name to look up our array index into the names list
            int oldLayerIndex = Array.IndexOf(sortingLayerNames, oldName);

            // Show the popup for the names
            int newLayerIndex = EditorGUILayout.Popup("Sorting Layer", oldLayerIndex, sortingLayerNames);

            // If the index changes, look up the ID for the new index to store as the new ID
            if (newLayerIndex != oldLayerIndex) {
                Undo.RecordObject(renderer, "Edit Sorting Layer");
                renderer.sortingLayerID = SortingLayer.NameToID(sortingLayerNames[newLayerIndex]);
                EditorUtility.SetDirty(renderer);
            }

            // Expose the manual sorting order
            int newSortingLayerOrder = EditorGUILayout.IntField("Order in Layer", renderer.sortingOrder);
            if (newSortingLayerOrder != renderer.sortingOrder) {
                Undo.RecordObject(renderer, "Edit Sorting Order");
                renderer.sortingOrder = newSortingLayerOrder;
                EditorUtility.SetDirty(renderer);
            }
        }
    }
}