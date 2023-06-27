#region Disclaimer

// <copyright file="Vector3PropertyDrawer.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Unity.Editor.PropertyDrawers
{
    using Helper;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A better inspector control for <see cref="Vector3" />. Has copy and paste
    /// support. Also the values can be reset.
    /// </summary>
#if ROBIN_BIRD_EDITOR_UTILS
    //[CustomPropertyDrawer(typeof (Vector3))]
#endif
    public class Vector3PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel(property.displayName);

                    EditorGUIUtility.labelWidth = 15f;

                    var reset = false;
                    switch (DrawButton("R", "Reset", IsResetVectorValid(property.vector3Value), 20f))
                    {
                        case ButtonClickedAction.ClickActive:
                            reset = true;
                            break;
                        case ButtonClickedAction.ContextClickActive:
                        case ButtonClickedAction.ContextClickInactive:
                            DisplayContextMenu(property);
                            break;
                    }

                    EditorGUILayout.PropertyField(property.FindPropertyRelative("x"));
                    EditorGUILayout.PropertyField(property.FindPropertyRelative("y"));
                    EditorGUILayout.PropertyField(property.FindPropertyRelative("z"));

                    if (reset) property.vector3Value = Vector3.zero;
                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }

        /// <summary>
        /// Context Menu that Allows Copying and Pasting of Vector3 Values
        /// </summary>
        private void DisplayContextMenu(SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Copy"), false, ContextCopyCallback, property);

            try
            {
                EditorTools.ParseVector3(EditorGUIUtility.systemCopyBuffer);
                menu.AddItem(new GUIContent("Paste"), false, ContextPasteCallback, property);
            }
            catch
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }
            menu.ShowAsContext();
        }

        /// <summary>
        /// Callback if user copies a Vector3
        /// </summary>
        /// <param name="property"></param>
        private void ContextCopyCallback(object property)
        {
            var serializedProperty = property as SerializedProperty;
            Vector3 vec = serializedProperty.vector3Value;

            EditorGUIUtility.systemCopyBuffer = "(" + vec.x + "," + vec.y + "," + vec.z + ")";
        }

        /// <summary>
        /// Callback if user Pastes a Vector3
        /// </summary>
        /// <param name="property"></param>
        private void ContextPasteCallback(object property)
        {
            string clipBoard = EditorGUIUtility.systemCopyBuffer;
            Vector3 vec;

            try
            {
                vec = EditorTools.ParseVector3(clipBoard);
            }
            catch
            {
                return;
            }

            var propInfo = property as SerializedProperty;
            if (propInfo != null)
            {
                propInfo.vector3Value = vec;
                propInfo.serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// Helper function that draws a button in an enabled or disabled state.
        /// </summary>
        private static ButtonClickedAction DrawButton(string title, string tooltip, bool enabled, float width)
        {
            Event evt = Event.current;
            if (enabled)
            {
                // Draw a regular button
                bool clicked = GUILayout.Button(new GUIContent(title, tooltip), GUILayout.Width(width));

                if (clicked)
                {
                    if (evt.button == 0) return ButtonClickedAction.ClickActive;
                    else if (evt.button == 1) return ButtonClickedAction.ContextClickActive;
                    else return ButtonClickedAction.NoClick;
                }
                else
                {
                    return ButtonClickedAction.NoClick;
                }
            }
            else
            {
                // Button should be disabled -- draw it darkened and ignore its return value
                Color color = GUI.color;
                GUI.color = new Color(1f, 1f, 1f, 0.25f);
                bool clicked = GUILayout.Button(new GUIContent(title, tooltip), GUILayout.Width(width));
                GUI.color = color;

                if (clicked)
                {
                    if (evt.button == 0) return ButtonClickedAction.ClickInactive;
                    if (evt.button == 1) return ButtonClickedAction.ContextClickInactive;
                    return ButtonClickedAction.NoClick;
                }
                else
                {
                    return ButtonClickedAction.NoClick;
                }
            }
        }


        /// <summary>
        /// Helper function that determines whether its worth it to show the reset position button.
        /// </summary>
        private static bool IsResetVectorValid(Vector3 vector3)
        {
            return (vector3.x != 0f || vector3.y != 0f || vector3.z != 0f);
        }

        private enum ButtonClickedAction
        {
            NoClick,
            ClickActive,
            ContextClickActive,
            ClickInactive,
            ContextClickInactive,
        }
    }
}