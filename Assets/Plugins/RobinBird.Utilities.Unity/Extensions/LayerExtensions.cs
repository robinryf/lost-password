using UnityEngine;

namespace RobinBird.Utilities.Unity.Extensions
{
	public static class LayerExtensions
	{
		/// <summary>
		/// Extension method to check if a layer is in a layer mask
		/// </summary>
		public static bool Contains(this LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}
	}
}