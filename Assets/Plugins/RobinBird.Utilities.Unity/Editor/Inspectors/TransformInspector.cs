#region Disclaimer

// <copyright file="TransformInspector.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Unity.Editor.Inspectors
{
    using System.Reflection;
    using Helper;
    using PropertyDrawers;
    using Unity.Helper;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Better inspector for <see cref="Transform" />. Uses <see cref="Vector3PropertyDrawer" /> so
    /// all vectors have copy, paste and reset support. Also this inspector expose the global position
    /// which is very useful.
    /// </summary>
#if ROBIN_BIRD_EDITOR_UTILS
    [CustomEditor(typeof (Transform))]
#endif
    [CanEditMultipleObjects]
    public class TransformInspector : GenericInspector<Transform>
    {
        private SerializedProperty mLocPos;
        private SerializedProperty mRot;
        private SerializedProperty mScale;

        protected override void OnEnable()
        {
            mLocPos = serializedObject.FindProperty("m_LocalPosition");
            mRot = serializedObject.FindProperty("m_LocalRotation");
            mScale = serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = 15f;

            serializedObject.Update();

            DrawLocalPosition();
            DrawGlobalPosition();
            DrawRotation();
            DrawScale();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLocalPosition()
        {
            GUILayout.BeginHorizontal();
            {
                var reset = false;
                switch (DrawButton("P", "Reset Local Position", IsResetPositionValid(Target), 20f))
                {
                    case ButtonClickedAction.ClickActive:
                        reset = true;
                        break;
                    case ButtonClickedAction.ContextClickActive:
                    case ButtonClickedAction.ContextClickInactive:
                        DisplayContextMenu(Target.GetType().GetProperty("localPosition"));
                        break;
                }

                EditorGUILayout.PropertyField(mLocPos.FindPropertyRelative("x"));
                EditorGUILayout.PropertyField(mLocPos.FindPropertyRelative("y"));
                EditorGUILayout.PropertyField(mLocPos.FindPropertyRelative("z"));

                if (reset) mLocPos.vector3Value = Vector3.zero;
            }
            GUILayout.EndHorizontal();
        }

        private void DrawGlobalPosition()
        {
            GUILayout.BeginHorizontal();
            {
                var reset = false;
                switch (DrawButton("G", "Reset Global Position", IsResetGlobalPositionValid(Target), 20f))
                {
                    case ButtonClickedAction.ClickActive:
                        reset = true;
                        break;
                    case ButtonClickedAction.ContextClickActive:
                    case ButtonClickedAction.ContextClickInactive:
                        DisplayContextMenu(Target.GetType().GetProperty("position"));
                        break;
                }

                Vector3 globalPosition;
                GUILayoutOption opt = GUILayout.MinWidth(30f);
                globalPosition.x = EditorGUILayout.FloatField("X", Target.position.x, opt);
                globalPosition.y = EditorGUILayout.FloatField("Y", Target.position.y, opt);
                globalPosition.z = EditorGUILayout.FloatField("Z", Target.position.z, opt);

                if (Target.position.Equals(globalPosition) == false)
                {
                    Undo.RecordObject(Target, "Change global position");
                    Target.position = globalPosition;
                }

                if (reset)
                {
                    Undo.RecordObject(Target, "Reset global position");
                    Target.position = Vector3.zero;
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawScale()
        {
            GUILayout.BeginHorizontal();
            {
                var reset = false;
                switch (DrawButton("S", "Reset Scale", IsResetScaleValid(Target), 20f))
                {
                    case ButtonClickedAction.ClickActive:
                        reset = true;
                        break;
                    case ButtonClickedAction.ContextClickActive:
                    case ButtonClickedAction.ContextClickInactive:
                        DisplayContextMenu(Target.GetType().GetProperty("localScale"));
                        break;
                }

                EditorGUILayout.PropertyField(mScale.FindPropertyRelative("x"));
                EditorGUILayout.PropertyField(mScale.FindPropertyRelative("y"));
                EditorGUILayout.PropertyField(mScale.FindPropertyRelative("z"));

                if (reset) mScale.vector3Value = Vector3.one;
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Context Menu that Allows Copying and Pasting of Vector3 Values
        /// </summary>
        /// <param name="propInfo"></param>
        private void DisplayContextMenu(PropertyInfo propInfo)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Copy"), false, ContextCopyCallback, propInfo);

            try
            {
                EditorTools.ParseVector3(EditorGUIUtility.systemCopyBuffer);
                menu.AddItem(new GUIContent("Paste"), false, ContextPasteCallback, propInfo);
            }
            catch
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }
            menu.ShowAsContext();
        }

        /// <summary>
        /// Callback if user Copys a Vector3
        /// </summary>
        /// <param name="propertyInfo"></param>
        private void ContextCopyCallback(object propertyInfo)
        {
            var propInfo = propertyInfo as PropertyInfo;
            var vec = (Vector3) propInfo.GetValue(target, null);

            EditorGUIUtility.systemCopyBuffer = "(" + vec.x + "," + vec.y + "," + vec.z + ")";
        }

        /// <summary>
        /// Callback if user Pastes a Vector3
        /// </summary>
        /// <param name="propertyInfo"></param>
        private void ContextPasteCallback(object propertyInfo)
        {
            string clipBoard = EditorGUIUtility.systemCopyBuffer;
            Vector3 vec = Vector3.zero;

            try
            {
                vec = EditorTools.ParseVector3(clipBoard);
            }
            catch
            {
                return;
            }

            var propInfo = propertyInfo as PropertyInfo;
            if (propInfo != null)
            {
                Undo.RecordObject(target, "Paste");
                propInfo.SetValue(target, vec, null);
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
                    else if (evt.button == 1) return ButtonClickedAction.ContextClickInactive;
                    else return ButtonClickedAction.NoClick;
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
        private static bool IsResetPositionValid(Transform targetTransform)
        {
            Vector3 v = targetTransform.localPosition;
            return (v.x != 0f || v.y != 0f || v.z != 0f);
        }

        /// <summary>
        /// Helper function that determines whether its worth it to show the rest position button.
        /// </summary>
        private static bool IsResetGlobalPositionValid(Transform targetTransform)
        {
            Vector3 v = targetTransform.position;
            return (v.x != 0f || v.y != 0f || v.z != 0f);
        }

        /// <summary>
        /// Helper function that determines whether its worth it to show the reset rotation button.
        /// </summary>
        private static bool IsResetRotationValid(Transform targetTransform)
        {
            Vector3 v = targetTransform.localEulerAngles;
            return (v.x != 0f || v.y != 0f || v.z != 0f);
        }

        /// <summary>
        /// Helper function that determines whether its worth it to show the reset scale button.
        /// </summary>
        private static bool IsResetScaleValid(Transform targetTransform)
        {
            Vector3 v = targetTransform.localScale;
            return (v.x != 1f || v.y != 1f || v.z != 1f);
        }

        /// <summary>
        /// Helper function that removes not-a-number values from the vector.
        /// </summary>
        private static Vector3 Validate(Vector3 vector)
        {
            vector.x = float.IsNaN(vector.x) ? 0f : vector.x;
            vector.y = float.IsNaN(vector.y) ? 0f : vector.y;
            vector.z = float.IsNaN(vector.z) ? 0f : vector.z;

            return vector;
        }

        private enum ButtonClickedAction
        {
            NoClick,
            ClickActive,
            ContextClickActive,
            ClickInactive,
            ContextClickInactive,
        }

        #region Rotation is ugly as hell... since there is no native support for quaternion property drawing

        private enum Axes : int
        {
            None = 0,
            X = 1,
            Y = 2,
            Z = 4,
            All = 7,
        }

        private Axes CheckDifference(Transform t, Vector3 original)
        {
            Vector3 next = t.localEulerAngles;

            var axes = Axes.None;

            if (Differs(next.x, original.x)) axes |= Axes.X;
            if (Differs(next.y, original.y)) axes |= Axes.Y;
            if (Differs(next.z, original.z)) axes |= Axes.Z;

            return axes;
        }

        private Axes CheckDifference(SerializedProperty property)
        {
            var axes = Axes.None;

            if (property.hasMultipleDifferentValues)
            {
                Vector3 original = property.quaternionValue.eulerAngles;

                foreach (Object obj in serializedObject.targetObjects)
                {
                    axes |= CheckDifference(obj as Transform, original);
                    if (axes == Axes.All) break;
                }
            }
            return axes;
        }

        /// <summary>
        /// Draw an editable float field.
        /// </summary>
        /// <param name="hidden">Whether to replace the value with a dash</param>
        /// <param name="greyedOut">Whether the value should be greyed out or not</param>
        private static bool FloatField(string name, ref float value, bool hidden, bool greyedOut, GUILayoutOption opt)
        {
            float newValue = value;
            GUI.changed = false;

            if (!hidden)
            {
                if (greyedOut)
                {
                    GUI.color = new Color(0.7f, 0.7f, 0.7f);
                    newValue = EditorGUILayout.FloatField(name, newValue, opt);
                    GUI.color = Color.white;
                }
                else
                {
                    newValue = EditorGUILayout.FloatField(name, newValue, opt);
                }
            }
            else if (greyedOut)
            {
                GUI.color = new Color(0.7f, 0.7f, 0.7f);
                float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
                GUI.color = Color.white;
            }
            else
            {
                float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
            }

            if (GUI.changed && Differs(newValue, value))
            {
                value = newValue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Because Mathf.Approximately is too sensitive.
        /// </summary>
        private static bool Differs(float a, float b)
        {
            return Mathf.Abs(a - b) > 0.0001f;
        }

        private void DrawRotation()
        {
            GUILayout.BeginHorizontal();
            {
                var reset = false;
                switch (DrawButton("R", "Reset Rotation", IsResetRotationValid(Target), 20f))
                {
                    case ButtonClickedAction.ClickActive:
                        reset = true;
                        break;
                    case ButtonClickedAction.ContextClickActive:
                    case ButtonClickedAction.ContextClickInactive:
                        DisplayContextMenu(Target.GetType().GetProperty("localEulerAngles"));
                        break;
                }

                Vector3 visible = (serializedObject.targetObject as Transform).localEulerAngles;

                visible.x = MathHelper.WrapAngle180(visible.x);
                visible.y = MathHelper.WrapAngle180(visible.y);
                visible.z = MathHelper.WrapAngle180(visible.z);

                Axes changed = CheckDifference(mRot);
                var altered = Axes.None;

                GUILayoutOption opt = GUILayout.MinWidth(30f);

                if (FloatField("X", ref visible.x, (changed & Axes.X) != 0, false, opt)) altered |= Axes.X;
                if (FloatField("Y", ref visible.y, (changed & Axes.Y) != 0, false, opt)) altered |= Axes.Y;
                if (FloatField("Z", ref visible.z, (changed & Axes.Z) != 0, false, opt)) altered |= Axes.Z;

                if (reset)
                {
                    mRot.quaternionValue = Quaternion.identity;
                }
                else if (altered != Axes.None)
                {
                    Undo.RecordObjects(serializedObject.targetObjects, "Change Rotation");

                    foreach (Object obj in serializedObject.targetObjects)
                    {
                        var t = obj as Transform;
                        Vector3 v = t.localEulerAngles;

                        if ((altered & Axes.X) != 0) v.x = visible.x;
                        if ((altered & Axes.Y) != 0) v.y = visible.y;
                        if ((altered & Axes.Z) != 0) v.z = visible.z;

                        t.localEulerAngles = v;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        #endregion
    }
}