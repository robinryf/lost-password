using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
	/// <summary>
	/// Used to setup multiple particle systems in a grid layout
	/// </summary>
	public class ParticleSystemGrid : MonoBehaviour
	{
		public ParticleSystem ParticleTemplate;

		public Vector2Int GridSize;

		public Vector2 CellSize;

		private void OnDrawGizmosSelected()
		{
			#if UNITY_EDITOR
			var rect = new Rect(transform.position - (Vector3)CellSize * 0.5f, GridSize * CellSize);
			UnityEditor.Handles.color = Color.magenta;
			UnityEditor.Handles.DrawWireCube(rect.center, rect.size);
			UnityEditor.Handles.color = Color.white;
			#endif
		}
	}
}