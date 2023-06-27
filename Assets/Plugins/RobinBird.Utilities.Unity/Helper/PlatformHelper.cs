using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
	public static class PlatformHelper
	{
		public static bool IsIOSEditorOrRuntime()
		{
			#if UNITY_EDITOR
			if (Application.isEditor)
			{
				return UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS;
			}
			#endif
			return Application.platform == RuntimePlatform.IPhonePlayer;
		}
	}
}