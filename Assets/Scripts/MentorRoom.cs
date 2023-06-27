using System;
using RobinBird.Utilities.Unity.Extensions;
using Stateless;
using TMPro;
using UnityEngine;

namespace LostPassword
{
	/// <summary>
	/// Mentor that gives guidance to the player
	/// </summary>
	public class MentorRoom : MonoBehaviour, IInteractableObject, StateMachine<MentorRoom.MentorState, MentorRoom.Trigger, MentorRoom>.IStateMachineContext
	{
		[SerializeField]
		private GameObject interactHint;

		[SerializeField]
		private GameObject speechBubble;

		[SerializeField]
		private TextMeshProUGUI text;
		
		[SerializeField]
		private AnimationClip idleAnimation;
		
		[SerializeField]
		private Animation animator;

		private StateMachine<MentorState, Trigger, MentorRoom>.StateMachineHandle handle;


		public enum MentorState
		{
			Idle,
			Talking
		}

		public enum Trigger
		{
			Talk,
			FinishTalking
		}

		private void Awake()
		{
			var machine = new StateMachine<MentorState, Trigger, MentorRoom>();

			machine.Configure(MentorState.Idle)
				.OnEntry(c =>
				{
					c.animator.Play(idleAnimation);
					c.interactHint.SetActive(false);
					c.speechBubble.SetActive(false);
				})
				.OnExit(c => c.interactHint.SetActive(false))
				.Permit(Trigger.Talk, MentorState.Talking);

			machine.Configure(MentorState.Talking)
				.OnEntry(c =>
				{
					c.speechBubble.SetActive(true);
				})
				.OnExit(c =>
				{
					c.speechBubble.SetActive(false);
				})
				.Permit(Trigger.FinishTalking, MentorState.Idle);

			handle = machine.CreateHandle(this, MentorState.Idle);

		}

		public void OnInteractableChanged(bool isInteractable)
		{
			interactHint.SetActive(isInteractable);
		}

		public void OnInteract()
		{
			handle.Fire(Trigger.Talk);
		}

		public bool IsInteractable => CurrentState == MentorState.Idle;

		public MentorState CurrentState { get; set; }
	}
}