using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
	[CreateAssetMenu(fileName = nameof(AddXcodeFrameworkBuildCallback), menuName = CreateAssetMenuItemName + nameof(AddXcodeFrameworkBuildCallback))]
	public class AddXcodeFrameworkBuildCallback : AbstractXcodeModificationBuildCallback
	{
		[SerializeField]
		[Tooltip("Without extension. Example: 'GameKit'")]
		private string frameworkName;

		[SerializeField]
		private bool weak;
		
		public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			GetXcodeProjectPaths(target, buildPath, out string xcodePBXProjectPath, out string targetName,
				out PBXProject xcodeProject, out string buildDirectory, out string targetGuid);

			var unityTarget = xcodeProject.GetUnityFrameworkTargetGuid();
			xcodeProject.AddFrameworkToProject(unityTarget, frameworkName + ".framework", weak);
			
			xcodeProject.WriteToFile(xcodePBXProjectPath);
		}
	}
}