using System;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
	public class DisableInRelease : MonoBehaviour
	{
		// TODO: Does not work with prefabs! Use a Asset Label "CheckOnBuild" and search for that during build to destroy any GameObjects in prefabs before the build
		private void Awake()
		{
			if (Debug.isDebugBuild == false)
			{
				gameObject.SetActive(false);
			}
		}
	}
}