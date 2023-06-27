#if UNITY_UI_PACKAGE
using UnityEngine;
using UnityEngine.UI;

namespace RobinBird.Utilities.Unity.Helper
{
	
	/// <summary>
	/// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
	/// Useful for providing a raycast target without actually drawing anything.
	/// </summary>
	[RequireComponent(typeof(CanvasRenderer))]
	public class NonDrawingGraphic : Graphic
	{
		public override void SetMaterialDirty() { return; }
		public override void SetVerticesDirty() { return; }

		/// Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen; so here really just as a fail-safe.
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			return;
		}
	}
}
#endif