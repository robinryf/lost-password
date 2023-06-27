using UnityEngine;

namespace RobinBird.Utilities.Unity.Extensions
{
	public static class AnimationExtensions
	{
		public static void Play(this Animation animation, AnimationClip clip)
		{
			animation.Play(clip.name);
		}
	}
}