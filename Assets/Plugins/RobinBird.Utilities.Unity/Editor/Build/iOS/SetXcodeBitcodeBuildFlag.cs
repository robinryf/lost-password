using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
	[CreateAssetMenu(fileName = nameof(SetXcodeBitcodeBuildFlag), menuName = CreateAssetMenuItemName + nameof(SetXcodeBitcodeBuildFlag))]
	public class SetXcodeBitcodeBuildFlag : AbstractXcodeModificationBuildCallback
	{
		[SerializeField]
		private bool enableBitcode;
		
		public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			if (target != BuildTarget.iOS)
			{
				Log.Info($"Not running {nameof(SetXcodeBitcodeBuildFlag)} on platform: {target}");
			}
			GetXcodeProjectPaths(target, buildPath, out string xcodePBXProjectPath, out string targetName,
				out PBXProject xcodeProject, out string buildDirectory, out string targetGuid);

			string value = enableBitcode ? "YES" : "NO";
			
			//Disabling Bitcode on all targets

			//Main
			var xCodeTarget = xcodeProject.GetUnityMainTargetGuid();
			xcodeProject.SetBuildProperty(xCodeTarget, "ENABLE_BITCODE", value);


			//Unity Tests
			xCodeTarget = xcodeProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
			xcodeProject.SetBuildProperty(xCodeTarget, "ENABLE_BITCODE", value);


			//Unity Framework
			xCodeTarget = xcodeProject.GetUnityFrameworkTargetGuid();
			xcodeProject.SetBuildProperty(xCodeTarget, "ENABLE_BITCODE", value);

            
			xcodeProject.WriteToFile(xcodePBXProjectPath);
		}
	}
}