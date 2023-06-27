using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UserBuildSettings = UnityEditor.OSXStandalone.UserBuildSettings;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
    [Serializable]
    public class XcodeBuildPropertyEntry
    {
        public string Name;
        public string Value;
    }
    
    [CreateAssetMenu(fileName = nameof(SetXcodeBuildProperties), menuName = CreateAssetMenuItemName + nameof(SetXcodeBuildProperties))]
    public class SetXcodeBuildProperties : AbstractXcodeModificationBuildCallback
    {
        [SerializeField]
        private XcodeBuildPropertyEntry[] entries;
        
        public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            if (target != BuildTarget.iOS && target != BuildTarget.StandaloneOSX)
                return;
            
            if (target == BuildTarget.StandaloneOSX && UserBuildSettings.createXcodeProject == false)
                return;

            if (entries.Length == 0)
                return;

            GetXcodeProjectPaths(target, buildPath, out string xcodePBXProjectPath, out string targetName,
                out PBXProject xcodeProject, out string buildDirectory, out string targetGuid);

            
            foreach (XcodeBuildPropertyEntry entry in entries)
            {
                xcodeProject.AddBuildProperty(targetGuid, entry.Name, entry.Value);
            }
            
            xcodeProject.WriteToFile(xcodePBXProjectPath);
        }
    }
}