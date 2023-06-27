namespace RobinBird.Utilities.Unity.Editor.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using JetBrains.Annotations;
    using Logging.Runtime;
    using Runtime.Helper;
    using Unity.Helper;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;
    
    public struct GuiSwitch
    {
        public object Data { get; set; }

        public bool Active { get; set; }

        public void Activate()
        {
            Set(true);
        }
        
        public void Activate(object data)
        {
            Data = data;
            Set(true);
        }

        public void Deactivate()
        {
            Set(false);
        }

        public void Set(bool active)
        {
            Active = active;
        }

        public bool Use()
        {
            if (Active == false)
            {
                return false;
            }
            Deactivate();
            return true;
        }
    }

    public struct GuiLever<T>
    {
        public T Active { get; set; }
        public T Pending { get; set; }

        public bool CanSwitch()
        {
            return EqualityComparer<T>.Default.Equals(Pending, Active) == false;
        }

        public bool TrySwitch()
        {
            if (CanSwitch())
            {
                Switch();
                return true;
            }
            return false;
        }

        public void Switch()
        {
            Active = Pending;
        }
    }

    /// <summary>
    /// Editor tools that provide convenient methods for editor controls.
    /// </summary>
    public class EditorTools
    {
        public const string ProjectSettingsDirectoryName = "ProjectSettings";
        public const string LibraryDirectoryName = "Library";
        public const string UserSettingsDirectoryName = "UserSettings";

        public const string RuntimeAssemblyName = "Assembly-CSharp.dll";
        public const string EditorAssemblyName = "Assembly-CSharp-Editor.dll";
        public const string PluginAssemblyName = "Assembly-CSharp-firstpass.dll";
        public const string EditorPluginAssemblyName = "Assembly-CSharp-Editor-firstpass.dll";

        /// <summary>
        /// Cached project path because that won't change without exeting Unity
        /// </summary>
        private static string _cachedProjectPath;

        /// <summary>
        /// Get path to the Unity project directory that contains the Assets and ProjectSettings directory.
        /// </summary>
        public static string GetProjectPath()
        {
            if (_cachedProjectPath != null)
            {
                return _cachedProjectPath;
            }

            var dataPath = new DirectoryInfo(Application.dataPath);

            if (dataPath.Parent == null)
            {
                throw new InvalidOperationException("Something is really messed up with Unity! Could not find Project Directory");
            }

            // remove Asset Folder
            _cachedProjectPath = UnityPathHelper.ConvertToUnityPath(dataPath.Parent.FullName);
            return _cachedProjectPath;
        }

        /// <summary>
        /// Returns path to the Unity project settings usally located next to the Assets directory.
        /// </summary>
        /// <returns></returns>
        public static string GetProjectSettingsPath()
        {
            string projectFolder = GetProjectPath();
            return Path.Combine(projectFolder, ProjectSettingsDirectoryName);
        }

        /// <summary>
        /// Returns path to the Library directory next to the Unity Assets directory.
        /// </summary>
        public static string GetLibraryPath()
        {
            string projectFolder = GetProjectPath();
            return Path.Combine(projectFolder, LibraryDirectoryName);
        }

        /// <summary>
        /// Returns path to the user settings path. Usally a directory next to the Assets directory.
        /// </summary>
        public static string GetUserSettingsPath()
        {
            string projectFolder = GetProjectPath();
            return Path.Combine(projectFolder, UserSettingsDirectoryName);
        }

        /// <summary>
        /// Removes the project path. So makes "C:\Projects\UnityProject\Assets/MyDir/Myasset.png" to "Assets/MyDir/Myasset.png". Slash direction
        /// does not matter
        /// </summary>
        /// <param name="absolutePathToAsset">Absolute path to asset.</param>
        /// <returns>Path relative to Unity project.</returns>
        public static string RemoveProjectPath(string absolutePathToAsset)
        {
            // Add one for the slash between the paths
            return absolutePathToAsset.Remove(0, GetProjectPath().Length + 1);
        }

        // TODO: Make non-layout version and move to EditorToolsLayout
        /// <summary>
        /// Draws a field that takes and returns a GUID of an project item inside Unity. The user can drag any project item
        /// onto the supplied field and this control will return the guid.
        /// </summary>
        /// <param name="content">Content for the displayed control.</param>
        /// <param name="guid">The current guid. Can be null or empty</param>
        /// <returns>The new guid if other object is referenced. The old guid matching <see cref="guid" /> otherwise.</returns>
        public static string ProjectItemGuidField(GUIContent content, [CanBeNull] string guid)
        {
            Object obj = null;

            if (string.IsNullOrEmpty(guid) == false)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                obj = AssetDatabase.LoadMainAssetAtPath(path);
            }

            Object tmpObj = EditorGUILayout.ObjectField(content, obj, typeof (Object), false);

            if (tmpObj != obj)
            {
                if (tmpObj == null)
                {
                    return string.Empty;
                }
                // New Object assigned
                return GetGuid(tmpObj);
            }
            return guid;
        }

        /// <summary>
        /// Draws a field that takes and returns an asset path of an project item inside Unity project. The user can drag any
        /// project item onto the supplied field and this control will return the relative path to the project directory.
        /// </summary>
        /// <param name="rect">Area to draw the control.</param>
        /// <param name="content">Prefix label content for the displayed control.</param>
        /// <param name="path">The path of the currently selected asset.</param>
        /// <returns>Path relative to the Unity project directory of the asset.</returns>
        public static string ProjectItemPathField(Rect rect, GUIContent content, string path)
        {
            Object obj = null;

            if (string.IsNullOrEmpty(path) == false)
            {
                obj = AssetDatabase.LoadMainAssetAtPath(path);
            }

            Object tmpObj = EditorGUI.ObjectField(rect, content, obj, typeof (Object), false);

            if (tmpObj != obj)
            {
                if (tmpObj == null)
                {
                    return string.Empty;
                }
                // New Object assigned
                return AssetDatabase.GetAssetPath(tmpObj);
            }
            return path;
        }

        /// <summary>
        /// Displays a text field with a button to open a file dialog. The path of the chosen file will be returned.
        /// </summary>
        /// <param name="rect">Where the control should be drawn.</param>
        /// <param name="label">Label infront of the control</param>
        /// <param name="path">The currently store path.</param>
        /// <param name="relative">Should a relative path be created otherwise absolute path is used.</param>
        /// <param name="directory">In which directory should the file dialog start.</param>
        /// <param name="extension">The extension of the required file. Without a dot! "txt" not ".txt"</param>
        /// <returns>The newly selected path if user changed it.</returns>
        public static string FileField(Rect rect, string label, string path, bool relative = false, string directory = ".", string extension = "*")
        {
            return FileField(rect, new GUIContent(label), path, relative, directory, extension);
        }

        /// <summary>
        /// Displays a text field with a button to open a file dialog. The path of the chosen file will be returned.
        /// </summary>
        /// <param name="rect">Where the control should be drawn.</param>
        /// <param name="content">Content of the label infront of the control</param>
        /// <param name="path">The currently store path.</param>
        /// <param name="relative">Should a relative path be created otherwise absolute path is used.</param>
        /// <param name="directory">In which directory should the file dialog start.</param>
        /// <param name="extension">The extension of the required file. Without a dot! "txt" not ".txt"</param>
        /// <returns>The newly selected path if user changed it.</returns>
        public static string FileField(Rect rect, GUIContent content, string path, bool relative = false, string directory = ".", string extension = "*")
        {
            const int selectFileButtonWidth = 25;
            const int padding = 2;
            var textFieldRect = new Rect(rect);
            textFieldRect.xMax -= (selectFileButtonWidth + padding);

            path = EditorGUI.TextField(textFieldRect, content, path);

            if (DragAndDropArea(textFieldRect))
            {
                string tempPath = DragAndDrop.paths[0];
                if (File.Exists(tempPath))
                {
                    path = tempPath;
                }
            }

            var buttonRect = new Rect(rect);
            buttonRect.xMin = textFieldRect.xMax + padding;

            if (GUI.Button(buttonRect, "..."))
            {
                path = EditorUtility.OpenFilePanel("Choose File ...", directory, extension);
                if (relative && string.IsNullOrEmpty(path) == false)
                {
                    path = PathHelper.MakeRelativePath(Application.dataPath, path);
                }
            }

            return UnityPathHelper.ConvertToUnityPath(path);
        }

        /// <summary>
        /// Displays an Object Field that receives and returns a Folder inside the Unity Context
        /// </summary>
        /// <param name="label"></param>
        /// <param name="folder"></param>
        /// <param name="quiet"></param>
        /// <returns></returns>
        public static Object DirectoryReferenceField(string label, Object folder, bool quiet = false)
        {
            return DirectoryReferenceField(new GUIContent(label), folder, quiet);
        }

        /// <summary>
        /// Displays an Object Field that receives and returns a Folder inside the Unity Context
        /// </summary>
        /// <param name="content"></param>
        /// <param name="folder"></param>
        /// <param name="quiet"></param>
        /// <returns></returns>
        public static Object DirectoryReferenceField(GUIContent content, Object folder, bool quiet = false)
        {
            Object tmpFolder = EditorGUILayout.ObjectField(content, folder, typeof (Object), true);

            if (folder != tmpFolder)
            {
                if (tmpFolder != null)
                {
                    string tmpFolderPath = AssetDatabase.GetAssetPath(tmpFolder);

                    if (Directory.Exists(tmpFolderPath) == false)
                    {
                        if (quiet == false)
                        {
                            Log.Warn("Only Directories are allowed.");
                        }
                    }
                    else
                    {
                        return tmpFolder;
                    }
                }
                else
                {
                    return null;
                }
            }
            return folder;
        }

        public static string DirectoryGuidField(string label, string guid, bool quiet = false)
        {
            return DirectoryGuidField(new GUIContent(label), guid, quiet);
        }

        public static string DirectoryGuidField(GUIContent content, string guid, bool quiet = false)
        {
            string projectItemGuid = ProjectItemGuidField(content, guid);

            if (string.IsNullOrEmpty(projectItemGuid))
            {
                return projectItemGuid;
            }

            string path = AssetDatabase.GUIDToAssetPath(projectItemGuid);

            if (Directory.Exists(path) == false)
            {
                if (quiet == false)
                {
                    Log.Warn("Only Directories are allowed.");
                }
                return string.Empty;
            }
            return projectItemGuid;
        }


        public static string DirectoryPathField(Rect rect, string label, string path, bool relative = false)
        {
            return DirectoryPathField(rect, new GUIContent(label), path, relative);
        }

        public static string DirectoryPathField(Rect rect, GUIContent content, string path, bool relative = false)
        {
            const int selectDirectoryButtonWidth = 25;
            const int padding = 2;
            var textFieldRect = new Rect(rect);
            textFieldRect.xMax -= (selectDirectoryButtonWidth + padding);

            path = EditorGUI.TextField(textFieldRect, content, path);

            if (DragAndDropArea(textFieldRect))
            {
                string tempPath = DragAndDrop.paths[0];
                if (Directory.Exists(tempPath))
                {
                    path = tempPath;
                }
            }

            var buttonRect = new Rect(rect);
            buttonRect.xMin = textFieldRect.xMax + padding;

            if (GUI.Button(buttonRect, "..."))
            {
                path = EditorUtility.OpenFolderPanel("Chose Directory ...", path, string.Empty);
                if (relative && string.IsNullOrEmpty(path) == false)
                {
                    path = PathHelper.MakeRelativePath(Application.dataPath, path);
                }
            }

            return UnityPathHelper.ConvertToUnityPath(path);
        }

        /// <summary>
        /// Defines an area where objects can be dragged onto. Use <see cref="DragAndDrop" /> to retrieve drop results.
        /// </summary>
        /// <param name="area">Area where an object can be dropped.</param>
        /// <param name="visualMode">Visual cursor change when object is dragged over area.</param>
        /// <returns>True when a drag was performed.</returns>
        public static bool DragAndDropArea(Rect area, DragAndDropVisualMode visualMode = DragAndDropVisualMode.Link)
        {
            // Drag and drop area
            Event ev = Event.current;

            switch (ev.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (area.Contains(ev.mousePosition) == false)
                    {
                        break;
                    }

                    if (DragAndDrop.paths == null || DragAndDrop.paths.Length > 1)
                    {
                        // We don't accept multiple paths.
                        break;
                    }
                    DragAndDrop.visualMode = visualMode;

                    if (ev.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        return true;
                    }

                    break;
            }
            return false;
        }

        /// <summary>
        /// Draws a separator that can be draged with the cursor. Returns the offset the cursor draged the separator.
        /// </summary>
        /// <param name="contentRect">The rect the separator should be drawn in.</param>
        /// <param name="cursor">The cursor type displayed when hovering the separator.</param>
        /// <param name="window">Pass the editor window so the control can repaint if separator changes.</param>
        public static Vector2 DrawSeparator(Rect contentRect, MouseCursor cursor, EditorWindow window = null)
        {
            GUI.DrawTexture(contentRect, EditorGUIUtility.whiteTexture);
            EditorGUIUtility.AddCursorRect(contentRect, cursor);

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            var state = (SeparatorInfo) GUIUtility.GetStateObject(typeof (SeparatorInfo), controlId);

            Event ev = Event.current;
            EventType type = ev.GetTypeForControl(controlId);

            if (state.Dragging == false && type == EventType.MouseDown && contentRect.Contains(ev.mousePosition))
            {
                state.Dragging = true;
            }
            if (state.Dragging && type == EventType.MouseUp)
            {
                state.Dragging = false;
            }

            if (state.Dragging && type == EventType.MouseDrag)
            {
                if (window != null)
                {
                    window.Repaint();
                }
                return ev.delta;
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Returns the GUID of the given object.
        /// </summary>
        /// <param name="obj">Unity object to retrieve GUID from.</param>
        public static string GetGuid(Object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
        }

        /// <summary>
        /// Parse a Vector3 from a string. String format is the same as the 'ToString()' Function of the Vector3 Struct
        /// '(x,y,z)'
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3 ParseVector3(string s)
        {
            s = s.TrimStart('(');
            s = s.TrimEnd(')');

            string[] xyz = s.Split(',');

            if (xyz.Length != 3) throw new InvalidDataException(string.Format("The input string {0} could not be recognized as Vector3", s));

            return new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
        }

        /// <summary>
        /// Creates and returns a 2D texture. Do not use to often because it is a heavy operation.
        /// </summary>
        public static Texture2D MakeTexture(int width, int height, Color col)
        {
            var pix = new Color[width * height];

            for (var i = 0; i < pix.Length; i++)
                pix[i] = col;

            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        /// <summary>
        /// Info container for the separator control.
        /// </summary>
        public class SeparatorInfo
        {
            /// <summary>
            /// Holds the state of the separator by dragging.
            /// </summary>
            public bool Dragging;
        }
    }
}