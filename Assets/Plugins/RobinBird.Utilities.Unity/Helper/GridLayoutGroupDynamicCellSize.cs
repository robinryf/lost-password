using System;
using UnityEngine;
using UnityEngine.UI;

namespace RobinBird.Utilities.Unity.Helper
{
	/// <summary>
	/// Sizes the grid cells dynamically based on the parent so they expand to match the <see cref="GridLayoutGroupDynamicCellSize.columns"/> value.
	/// </summary>
	public class GridLayoutGroupDynamicCellSize : MonoBehaviour
	{
		public int columns = 2;
		
		void Start ()
		{
			ApplyCellSize();
		}

		private void ApplyCellSize()
		{
			var rectTransform = gameObject.GetComponent<RectTransform>();
			var width = Mathf.Abs(rectTransform.rect.width);
			var gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			int gapCount = columns - 1;
			Vector2 spacing = gridLayoutGroup.spacing;
			spacing.x /= gapCount;
			spacing.y /= gapCount;
			Vector2 newSize = new Vector2(width / columns - spacing.x, width / columns - spacing.y);
			gridLayoutGroup.cellSize = newSize;
		}

		private void OnValidate()
		{
			ApplyCellSize();
		}
	}
}