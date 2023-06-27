using System.IO;
using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
    [CreateAssetMenu(fileName = nameof(FirebaseCloudMessageDisableAtStartup), menuName = CreateAssetMenuItemName + nameof(FirebaseCloudMessageDisableAtStartup))]
    public class FirebaseCloudMessageDisableAtStartup : AbstractCIBuildCallbackHandler
    {
        [SerializeField]
        private bool disableFirebaseCloudMessaging;
        
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
            if (disableFirebaseCloudMessaging == false)
            {
                return;
            }
            
            string plistPath = Path.Combine(buildPath, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            Log.Info("Setting Firebase Cloud Messaging information to be disabled by default");
            plist.root.SetBoolean("FirebaseMessagingAutoInitEnabled", false);

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}