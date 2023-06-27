#region Disclaimer
// <copyright file="LayerPropertyDrawer.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Unity.Editor.PropertyDrawers
{
    using PropertyDrawerAttributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(LayerPropertyDrawerAttribute))]
    public class LayerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var att = (LayerPropertyDrawerAttribute)attribute;
            var oldTooltip = label.tooltip;
            if (string.IsNullOrEmpty(att.Tooltip) == false)
            {
                label.tooltip = att.Tooltip;
            }
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
            label.tooltip = oldTooltip;
        }
    }
}