namespace RobinBird.Utilities.Unity.Editor.Popups
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class TextInputPopup : EditorWindow
    {
        private Action<string> confirmAction;
        private Action cancelAction;
        private string messageText;
        private string inputText;


        public static void Init(string messageText, Action<string> confirmAction, Action cancelAction)
        {
            TextInputPopup window = CreateInstance<TextInputPopup>();
            window.messageText = messageText;
            window.cancelAction = cancelAction;
            window.confirmAction = confirmAction;
            window.position = new Rect(Screen.width * 0.5f, y: Screen.height * 0.5f, 250, 150);
            window.ShowPopup();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField(messageText, EditorStyles.wordWrappedLabel);
            inputText = EditorGUILayout.TextField(inputText);
            GUILayout.Space(70);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Ok"))
                {
                    Close();
                    confirmAction?.Invoke(inputText);
                }

                if (GUILayout.Button("Cancel"))
                {
                    Close();
                    cancelAction?.Invoke();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}