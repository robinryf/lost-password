using RobinBird.Utilities.Unity.Extensions;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
	/// <summary>
	/// Can be placed on objects that you want in the scene/prefab but not included in the build
	/// </summary>
	public class DestroyInBuild : MonoBehaviour
	{
#if UNITY_EDITOR
		// TODO: Does not work with prefabs! Use a Asset Label "CheckOnBuild" and search for that during build to destroy any GameObjects in prefabs before the build
		[UnityEditor.Callbacks.PostProcessScene]
		public static void PostProcessScene()
		{
			var destroyInBuild = FindObjectsOfType<DestroyInBuild>();
			if (destroyInBuild == null)
				return;
			foreach (DestroyInBuild obj in destroyInBuild)
			{
				obj.gameObject.Destroy();
			}
		}
#endif
	}
}