using UnityEditor;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Sprite))]
    public class SpritePropertyDrawer : PropertyDrawer
    {
        private const float TextureSize = 70;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent labelN)
        {
            return TextureSize;
        }


        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, prop);
            
                position.width = EditorGUIUtility.labelWidth;
                GUI.Label(position, prop.displayName);
 
                position.x += position.width;
                position.width = TextureSize;
                position.height = TextureSize;

                if (prop.hasMultipleDifferentValues)
                {
	                var tmpValue = EditorGUI.ObjectField(position, null, typeof(Sprite), false);
	                if (tmpValue != null)
	                {
		                prop.objectReferenceValue = tmpValue;
	                }
	                GUI.Label(position, "Multiple\nvalues");
                }
                else
                {
					prop.objectReferenceValue = EditorGUI.ObjectField(position, prop.objectReferenceValue, typeof(Sprite), false);
                }
 
            EditorGUI.EndProperty();
        }
    }
}