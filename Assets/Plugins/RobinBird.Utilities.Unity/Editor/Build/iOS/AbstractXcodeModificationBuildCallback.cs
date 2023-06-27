using System;
using System.IO;
using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
    public abstract class AbstractXcodeModificationBuildCallback : AbstractCIBuildCallbackHandler
    {
	    public enum ValueType
	    {
		    Bool,
		    String,
		    Integer,
		    Float,
	    }
	    
        public override bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            return true;
        }

        /// <summary>
        /// Get common Xcode paths and properties
        /// </summary>
        /// <param name="xcodePBXProjectPath">Path to the internal .pbxproj file</param>
        /// <param name="targetName">Name of the build target. (e.g. "Unity-iPhone")</param>
        /// <param name="xcodeProject">The xcode project. Use it to add build files and other stuff.</param>
        /// <param name="buildDirectory">Path to the directory in which the .xcodeproj file lives</param>
        /// <param name="targetGuid">The guid of the main build target from <paramref name="targetName"/></param>
        protected static void GetXcodeProjectPaths(BuildTarget target, string buildPath, out string xcodePBXProjectPath,
            out string targetName, out PBXProject xcodeProject, out string buildDirectory, out string targetGuid)
        {
            switch (target)
            {
                case BuildTarget.iOS:
                    targetName = "Unity-iPhone";
                    xcodePBXProjectPath = PBXProject.GetPBXProjectPath(buildPath);
                    break;
                case BuildTarget.StandaloneOSX:
                    // Target is the name of the directory where it is built to
                    targetName = PlayerSettings.productName;
                    string xcodeProjName = new DirectoryInfo(buildPath).Name;
                    xcodePBXProjectPath = Path.Combine(buildPath, $"{xcodeProjName}.xcodeproj", "project.pbxproj");
                    break;
                default:

                    xcodePBXProjectPath = default;
                    targetName = default;
                    xcodeProject = default;
                    buildDirectory = default;
                    targetGuid = default;
                    return;
            }


            xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodePBXProjectPath);
            var xcodePBXFileInfo = new FileInfo(xcodePBXProjectPath);
            buildDirectory = xcodePBXFileInfo.Directory.Parent.FullName;

            targetGuid = string.Empty;
            switch (target)
            {
                case BuildTarget.iOS:
                    targetGuid =
                        xcodeProject.GetUnityMainTargetGuid(); // Can't use 'TargetGuidByName' because Unity throws an error...
                    break;
                case BuildTarget.StandaloneOSX:
                    targetGuid = xcodeProject.TargetGuidByName(targetName);
                    break;
            }
        }
        
        protected static void SetPlistValue(string buildPath, string key, ValueType valueType, bool boolValue, string stringValue, int integerValue, float floatValue)
        {
	        string plistPath = Path.Combine(buildPath, "Info.plist");
	        PlistDocument plist = new PlistDocument();
	        plist.ReadFromFile(plistPath);

	        object value;
	        switch (valueType)
	        {
		        case ValueType.Bool:
			        value = boolValue;
			        plist.root.SetBoolean(key, boolValue);
			        break;
		        case ValueType.String:
			        value = stringValue;
			        plist.root.SetString(key, stringValue);
			        break;
		        case ValueType.Integer:
			        value = integerValue;
			        plist.root.SetInteger(key, integerValue);
			        break;
		        case ValueType.Float:
			        value = floatValue;
			        plist.root.SetReal(key, floatValue);
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }

	        Log.Info($"Set '{key}' with value: {value}");


	        File.WriteAllText(plistPath, plist.WriteToString());
        }

        protected static void CreateArray(string buildPath, string key, Action<PlistElementArray> fillArray)
        {
	        string plistPath = Path.Combine(buildPath, "Info.plist");
	        PlistDocument plist = new PlistDocument();
	        plist.ReadFromFile(plistPath);

	        var array = plist.root.CreateArray(key);
	        fillArray(array);
	        File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}