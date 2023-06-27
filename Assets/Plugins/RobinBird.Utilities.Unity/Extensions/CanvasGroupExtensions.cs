using UnityEngine;

namespace RobinBird.Utilities.Unity.Extensions
{
	public static class CanvasGroupExtensions
	{
		public static void Show(this CanvasGroup group)
		{
			group.alpha = 1;
			group.blocksRaycasts = true;
			group.interactable = true;
		}
		
		public static void Hide(this CanvasGroup group)
		{
			group.alpha = 0;
			group.blocksRaycasts = false;
			group.interactable = false;
		}

		public static void SetVisibility(this CanvasGroup group, bool isVisible)
		{
			if (isVisible)
				group.Show();
			else
				group.Hide();
		}
	}
}