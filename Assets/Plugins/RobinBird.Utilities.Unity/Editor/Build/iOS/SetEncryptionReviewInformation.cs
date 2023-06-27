using System.IO;
using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
    [CreateAssetMenu(fileName = nameof(SetEncryptionReviewInformation), menuName = CreateAssetMenuItemName + nameof(SetEncryptionReviewInformation))]
    public class SetEncryptionReviewInformation : AbstractCIBuildCallbackHandler
    {
        [SerializeField]
        private bool disableEncryptionReviewInformation;
        
        public override bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            return true;
        }

        public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }
            if (disableEncryptionReviewInformation == false)
            {
                return;
            }
            
            string plistPath = Path.Combine(buildPath, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            Log.Info("Setting Firebase Cloud Messaging information to be disabled by default");
            plist.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}