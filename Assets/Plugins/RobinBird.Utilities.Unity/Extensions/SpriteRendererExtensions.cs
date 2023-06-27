using UnityEngine;

namespace RobinBird.Utilities.Unity.Extensions
{
	public static class SpriteRendererExtensions
	{
		public static void SetAlpha(this SpriteRenderer renderer, float alpha)
		{
			var col = renderer.color;
			col.a = alpha;
			renderer.color = col;
		}
	}
}