// Modifications made by Luiz Wendt, Rob Tranquillo
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT

// Must be placed within a folder named "Editor"

using System;
using System.Collections.Generic;
using System.IO;
using RobinBird.Logging.Runtime;
using RobinBird.Utilities.Runtime.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RobinBird.Utilities.Unity.Editor.Inspectors
{
    /// <summary>
    /// Extends how ScriptableObject object references are displayed in the inspector
    /// Shows you all values under the object reference
    /// Also provides a button to create a new ScriptableObject if property is null.
    /// todo: enable custom editors for scriptable objects
    /// </summary>
#if ROBIN_BIRD_EDITOR_UTILS
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
#endif
    public class ExtendedScriptableObjectDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
	        return EditorGUIUtility.singleLineHeight;
            // float totalHeight = EditorGUIUtility.singleLineHeight;
            // if(property.objectReferenceValue == null){
            //     return totalHeight;
            // }
            // if (!IsThereAnyVisibileProperty(property))
            //     return totalHeight;
            // if (property.isExpanded)
            // {
            //     var data = property.objectReferenceValue as ScriptableObject;
            //     if (data == null) return EditorGUIUtility.singleLineHeight;
            //     SerializedObject serializedObject = new SerializedObject(data);
            //     SerializedProperty prop = serializedObject.GetIterator();
            //     if (prop.NextVisible(true))
            //     {
            //         do
            //         {
            //             if (prop.name == "m_Script") continue;
            //             var subProp = serializedObject.FindProperty(prop.name);
            //             float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
            //             totalHeight += height;
            //         }
            //         while (prop.NextVisible(false));
            //     }
            //     // Add a tiny bit of height if open for the background
            //     totalHeight += EditorGUIUtility.standardVerticalSpacing;
            // }
            // return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.objectReferenceValue != null)
            {
                // if (IsThereAnyVisibileProperty(property))
                // {
                //
                //     property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property.isExpanded, label, true);
                // }
                // else
                // {
                    EditorGUI.LabelField(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), label);
                    property.isExpanded = false;
                //}

                EditorGUI.PropertyField(new Rect(EditorGUIUtility.labelWidth + 14, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property, GUIContent.none, true);
                if (GUI.changed) property.serializedObject.ApplyModifiedProperties();
                if (property.objectReferenceValue == null) EditorGUIUtility.ExitGUI();

                // if (property.isExpanded)
                // {
                //     // Draw a background that shows us clearly which fields are part of the ScriptableObject
                //     GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");
                //
                //     EditorGUI.indentLevel++;
                //     var data = (ScriptableObject)property.objectReferenceValue;
                //     SerializedObject serializedObject = new SerializedObject(data);
                //
                //
                //     // Iterate over all the values and draw them
                //     SerializedProperty prop = serializedObject.GetIterator();
                //     float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                //     if (prop.NextVisible(true))
                //     {
                //         do
                //         {
                //             // Don't bother drawing the class file
                //             if (prop.name == "m_Script") continue;
                //             float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                //             EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), prop, true);
                //             y += height + EditorGUIUtility.standardVerticalSpacing;
                //         }
                //         while (prop.NextVisible(false));
                //     }
                //     if (GUI.changed)
                //         serializedObject.ApplyModifiedProperties();
                //
                //     EditorGUI.indentLevel--;
                // }
            }
            else
            {
                EditorGUI.ObjectField(new Rect(position.x, position.y, position.width - 44, EditorGUIUtility.singleLineHeight), property, label);
                
                bool shouldCreateAsset = GUI.Button(new Rect(position.x + position.width - 20*2, position.y, 20, EditorGUIUtility.singleLineHeight), new GUIContent("A", "Create standalone Asset"));
                bool shouldCreateSubAsset = GUI.Button(new Rect(position.x + position.width - 20 + 2, position.y, 20, EditorGUIUtility.singleLineHeight), new GUIContent("S", "Create nested SubAsset"));
                if (shouldCreateAsset || shouldCreateSubAsset)
                {
                    string selectedAssetPath = "Assets";
                    if (property.serializedObject.targetObject is MonoBehaviour)
                    {
                        MonoScript ms = MonoScript.FromMonoBehaviour((MonoBehaviour)property.serializedObject.targetObject);
                        selectedAssetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
                    }
                    else if (property.serializedObject.targetObject is ScriptableObject)
                    {
	                    var scriptableObjectPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
	                    selectedAssetPath = Path.GetDirectoryName(scriptableObjectPath);
                    }
                    Type type = fieldInfo.FieldType;
                    if (type.IsArray) type = type.GetElementType();
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) type = type.GetGenericArguments()[0];
                    if (shouldCreateSubAsset)
                    {
	                    CreateSubAsset(type, property, property.serializedObject.targetObject);
                    }
                    else
                    {
						CreateAssetWithSavePrompt(type, property, selectedAssetPath);
                    }
                }
            }
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        // Creates a new ScriptableObject via the default Save File panel
        private void CreateAssetWithSavePrompt(Type type, SerializedProperty property, string path)
        {
	        GetInheritType(type, finalType =>
	        {
	            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", "New" + finalType.Name + ".asset", "asset", "Enter a file name for the ScriptableObject.", path);
	            if (string.IsNullOrEmpty(path) == false)
	            {
		            ScriptableObject asset = ScriptableObject.CreateInstance(finalType);
		            AssetDatabase.CreateAsset(asset, path);
		            AssetDatabase.SaveAssets();
		            AssetDatabase.Refresh();
		            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		            EditorGUIUtility.PingObject(asset);
		            property.objectReferenceValue = asset;
		            property.serializedObject.ApplyModifiedProperties();
	            }
	        });
        }
        
        private void CreateSubAsset(Type type, SerializedProperty property, Object parentAsset)
        {
	        GetInheritType(type, finalType =>
	        {
		        ScriptableObject asset = ScriptableObject.CreateInstance(finalType);
		        asset.name = finalType.Name;
		        AssetDatabase.AddObjectToAsset(asset, parentAsset);
		        AssetDatabase.SaveAssets();
		        AssetDatabase.Refresh();
		        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parentAsset), ImportAssetOptions.ForceUpdate);
		        EditorGUIUtility.PingObject(asset);
		        property.objectReferenceValue = asset;
		        property.serializedObject.ApplyModifiedProperties();
	        });
        }

        private void GetInheritType(Type type, Action<Type> createWithType)
        {
	        if (type.IsAbstract == false)
	        {
		        createWithType(type);
		        return;
	        }

	        var inheritors = type.GetInheritors();
	        if (inheritors.IsNullOrEmpty())
	        {
		        Log.Error($"Could not create scriptable object because type is abstract and has no inheritors. ({type})");
		        return;
	        }

	        var genericMenu = new GenericMenu();
	        foreach (Type inheritor in inheritors)
	        {
		        var inheritNested = inheritor;
		        genericMenu.AddItem(new GUIContent(inheritor.Name), false, () =>
		        {
			        createWithType(inheritNested);
		        });
	        }
	        genericMenu.ShowAsContext();
        }

        // public bool IsThereAnyVisibileProperty(SerializedProperty property)
        // {
        //     var data = (ScriptableObject)property.objectReferenceValue;
        //     SerializedObject serializedObject = new SerializedObject(data);
        //
        //     SerializedProperty prop = serializedObject.GetIterator();
        //
        //     while (prop.NextVisible(true))
        //     {
        //         if (prop.name == "m_Script") continue;
        //         return true; //if theres any visible property other than m_script
        //     }
        //     return false;
        // }
    }
}