namespace LostPassword
{
	/// <summary>
	/// Can be implemented by <see cref="MonoBehaviour"/> with a Trigger Collider so it can be interacted with
	/// from <see cref="CharacterController2D"/>
	/// </summary>
	public interface IInteractableObject
	{
		public void OnInteractableChanged(bool isInteractable);
		public void OnInteract();
		public bool IsInteractable { get; }
	}
}