#if UNITY_UI_PACKAGE
using System;
using RobinBird.Utilities.Unity.Helper;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace RobinBird.Utilities.Unity.Editor.Inspectors
{

	[CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
	public class NonDrawingGraphicInspector : GraphicEditor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(m_Script, Array.Empty<GUILayoutOption>());
			EditorGUI.EndDisabledGroup();
			// skipping AppearanceControlsGUI
			RaycastControlsGUI();
			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif