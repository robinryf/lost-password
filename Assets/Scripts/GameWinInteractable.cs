using UnityEngine;

namespace LostPassword
{
	/// <summary>
	/// Check the game win condition and reports back to <see cref="MainGame"/>
	/// </summary>
	public class GameWinInteractable : MonoBehaviour, IInteractableObject
	{
		[SerializeField]
		private MainGame mainGame;

		[SerializeField]
		private GameObject interactHint;

		[SerializeField]
		private PasswordGlyph[] Glyphs;

		private bool hasAlreadyBeenActivated;
		
		public void OnInteractableChanged(bool isInteractable)
		{
			if (hasAlreadyBeenActivated == false)
			{
				interactHint.SetActive(true);
			}
		}

		public void OnInteract()
		{
			interactHint.SetActive(false);
			hasAlreadyBeenActivated = true;
			mainGame.RevealLostPassword();
		}

		public bool IsInteractable
		{
			get
			{
				bool isInteractable = true;
				foreach (PasswordGlyph glyph in Glyphs)
				{
					if (glyph.CurrentState != PasswordGlyph.State.Unlocked)
					{
						isInteractable = false;
						break;
					}
				}

				return this.hasAlreadyBeenActivated == false && isInteractable;
			}
		}
	}
}