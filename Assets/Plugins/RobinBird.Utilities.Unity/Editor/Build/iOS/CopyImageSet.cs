using System.IO;
using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using EditorTools = RobinBird.Utilities.Unity.Editor.Helper.EditorTools;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
	[CreateAssetMenu(fileName = nameof(CopyImageSet), menuName = CreateAssetMenuItemName + nameof(CopyImageSet))]
	public class CopyImageSet : AbstractXcodeModificationBuildCallback
	{
		private const string XcodeImagesCatalogName = "Images.xcassets";

		[SerializeField]
		private Object[] imageSetDirectories;
		
		public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			if (target != BuildTarget.iOS && target != BuildTarget.StandaloneOSX)
				return;
			if (imageSetDirectories == null)
				return;

			GetXcodeProjectPaths(target, buildPath, out string xcodePBXProjectPath, out string targetName, out PBXProject xcodeProject, out string buildDirectory, out string targetGuid);

			foreach (Object imageSetDirectory in imageSetDirectories)
			{
				string sourcePath = Path.Combine(EditorTools.GetProjectPath(),AssetDatabase.GetAssetPath(imageSetDirectory));
				string directoryName = new DirectoryInfo(sourcePath).Name;
				string targetPath = Path.Combine(buildDirectory, targetName, XcodeImagesCatalogName, directoryName);
				if (Directory.Exists(targetPath))
				{
					Directory.Delete(targetPath, true);
				}
				Log.Info($"Copying image set from: {sourcePath} to {targetPath}");
				FileUtil.CopyFileOrDirectory(sourcePath, targetPath);
				var targetDirectory = new DirectoryInfo(targetPath);

				foreach (FileInfo file in targetDirectory.GetFiles("*.meta"))
				{
					file.Delete();
				}
			}
		}
	}
}