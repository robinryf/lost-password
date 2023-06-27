using System.IO;
using RobinBird.Logging.Runtime;
using RobinBird.Utilities.Runtime.Helper;
using RobinBird.Utilities.Unity.Editor.Helper;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.Android
{
	[CreateAssetMenu(fileName = nameof(MakeGradleProjectPathRelative), menuName = CreateAssetMenuItemName + nameof(MakeGradleProjectPathRelative))]
	public class MakeGradleProjectPathRelative : AbstractCIBuildCallbackHandler
	{
		public override bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			return true;
		}

		public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			if (target != BuildTarget.Android)
			{
				Log.Warn("This post process step is only suited for Android");
				return;
			}
			var unityLibraryGradleFilePath = Path.Combine(buildPath, "unityLibrary", "build.gradle");

			if (File.Exists(unityLibraryGradleFilePath) == false)
			{
				Log.Error($"Could not find gradle build file for unityLibrary at: {unityLibraryGradleFilePath}");
			}
			else
			{
				var text = File.ReadAllText(unityLibraryGradleFilePath);

				var projectDir = EditorTools.GetProjectPath();
				var relativePath = PathHelper.MakeRelativePath(buildPath, projectDir);
				Log.Info($"Extracted relative path from build to project: {relativePath}");

				// Adding one more directory up because gradle is operating from within the buildPath directory
				text = text.Replace($"\"file:///{projectDir}", $"\"file:///${{project.rootDir}}/../{relativePath}");
				
				File.WriteAllText(unityLibraryGradleFilePath, text);
			}
		}
	}
}