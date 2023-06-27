using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build
{
	[CreateAssetMenu(fileName = nameof(DeleteAssetBeforeBuild), menuName = CreateAssetMenuItemName + nameof(DeleteAssetBeforeBuild))]
	public class DeleteAssetBeforeBuild : AbstractCIBuildCallbackHandler
	{
		public Object[] AssetsToDelete;
		
		public override bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			foreach (Object assetToDelete in AssetsToDelete)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(assetToDelete));
			}
			return true;
		}

		public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
		}
	}
}