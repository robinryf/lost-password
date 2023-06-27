using System.Collections;
using RobinBird.Utilities.Unity.Extensions;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
	/// <summary>
	/// Lets you specify on a rect transform to extend to the screen edges regardless of the safe area
	/// </summary>
	public class ExtendToScreenEdges : MonoBehaviour
	{
		public bool Top = true;
		public bool Bottom = true;
		public bool Right = true;
		public bool Left = true;
		
		private IEnumerator Start()
		{
			var canvas = GetComponentInParent<Canvas>();
			while (canvas.enabled == false)
			{
				yield return null;
			}
			ApplyScreenEdgeExtend();
		}

		[ContextMenu("ApplyScreenEdgeExtend")]
		public void ApplyScreenEdgeExtend()
		{
			var rectTransform = GetComponent<RectTransform>();

			if (rectTransform == null || rectTransform.parent == null)
			{
				return;
			}

			Canvas rootCanvas = rectTransform.GetRootCanvas();
			var rootRect = rootCanvas.GetComponent<RectTransform>();

			var worldCorners = new Vector3[4];
			rootRect.GetWorldCorners(worldCorners);

			for (int i = 0; i < worldCorners.Length; i++)
			{
				worldCorners[i] = rectTransform.InverseTransformPoint(worldCorners[i]);
			}

			var parentRect = rectTransform.parent.GetComponent<RectTransform>();
			
			if (Top)
				rectTransform.SetTop(parentRect.rect.yMax - worldCorners[1].y);
			if (Bottom)
				rectTransform.SetBottom(worldCorners[0].y - parentRect.rect.yMin);
			if (Right)
				rectTransform.SetRight(parentRect.rect.xMax - worldCorners[3].x);
			if (Left)
				rectTransform.SetLeft(worldCorners[0].x - parentRect.rect.xMin);
		}
	}
}