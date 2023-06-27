using System.IO;
using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEditor.OSXStandalone;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
    [CreateAssetMenu(fileName = nameof(SetCodeSigningEntitlements), menuName = CreateAssetMenuItemName + nameof(SetCodeSigningEntitlements))]
    public class SetCodeSigningEntitlements : AbstractXcodeModificationBuildCallback
    {
        [SerializeField]
        private DefaultAsset developmentEntitlements;
        
        [SerializeField]
        private DefaultAsset releaseEntitlements;

        public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            if (target != BuildTarget.iOS && target != BuildTarget.StandaloneOSX)
                return;

            if (target == BuildTarget.StandaloneOSX && UserBuildSettings.createXcodeProject == false)
                return;
            
            GetXcodeProjectPaths(target, buildPath, out string xcodePBXProjectPath, out string targetName,
                out PBXProject xcodeProject, out string buildDirectory, out string targetGuid);

            DefaultAsset file = Debug.isDebugBuild ? developmentEntitlements : releaseEntitlements;

            if (file == null)
            {
                Log.Warn("Skipping to add entitlements because no file specified");
                return;
            }
            
            string entitlementsFilePath = AssetDatabase.GetAssetPath(file);
            
            string entitlementsFileName = Path.GetFileName(entitlementsFilePath);
            string xcodeProjectRelativePath = Path.Combine(targetName, entitlementsFileName);
            
            string xcodeEntitlementsDestination = Path.Combine(buildDirectory, xcodeProjectRelativePath);

            if (File.Exists(xcodeEntitlementsDestination))
            {
                File.Delete(xcodeEntitlementsDestination);
            }
            
            FileUtil.CopyFileOrDirectory(entitlementsFilePath, xcodeEntitlementsDestination);

            xcodeProject.AddFile(xcodeProjectRelativePath, entitlementsFileName);
            xcodeProject.AddBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", xcodeProjectRelativePath);

            xcodeProject.WriteToFile(xcodePBXProjectPath);
        }
    }
}