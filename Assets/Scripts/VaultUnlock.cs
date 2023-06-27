using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LostPassword;
using OnePassword;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;
using RobinBird.Utilities.Unity.Helper;
using Stateless;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using OnePasswordManager = OnePassword.OnePasswordManager;

/// <summary>
/// Entry point into the riddle room
/// Set up the vault and initial passwords for the riddle.
/// </summary>
public class VaultUnlock : MonoBehaviour, IInteractableObject, StateMachine<VaultUnlock.VaultState, VaultUnlock.Trigger, VaultUnlock>.IStateMachineContext
{
	[SerializeField]
	private OnePasswordUnityManager onePasswordUnityManager;

	[SerializeField]
	private Animation unlockAnimation;

	[SerializeField]
	private AnimationClip unlockAnimationClip;
	[SerializeField]
	private AnimationClip unlockedAnimationClip;

	[SerializeField]
	private GameObject interactHint;

	private static StateMachine<VaultState, Trigger, VaultUnlock> _stateMachine;
	private StateMachine<VaultState, Trigger, VaultUnlock>.StateMachineHandle handle;

	public enum VaultState
	{
		Locked,
		Unlocking,
		Unlocked,
	}

	private enum Trigger
	{
		StartUnlock,
		FinishUnlock,
		Lock
	}
	
	private void Awake()
	{
		OnInteractableChanged(false);
		CreateStateMachine();

		handle = _stateMachine.CreateHandle(this, VaultState.Locked);
	}

	private static void CreateStateMachine()
	{
		void SetVaultState(StateMachine<VaultState, Trigger, VaultUnlock>.Transition transition, bool isActive)
		{
			transition.Context.gameObject.SetActive(isActive);
		}

		if (_stateMachine != null)
			return;
		_stateMachine = new StateMachine<VaultState, Trigger, VaultUnlock>();
		
		// TODO: Make transitions easier without changing state
		// TODO: Update method for states
		// TODO: Make nice animator control to play animation automatically for state (and transitions)

		_stateMachine.Configure(VaultState.Locked)
			.OnEntry((transition) => SetVaultState(transition, true))
			.Permit(Trigger.StartUnlock, VaultState.Unlocking);

		_stateMachine.Configure(VaultState.Unlocking)
			.OnEntry(context =>
			{
				context.interactHint.SetActive(false);
				context.unlockAnimation.Play(context.unlockAnimationClip.name);
			})
			.Permit(Trigger.FinishUnlock, VaultState.Unlocked);

		_stateMachine.Configure(VaultState.Unlocked)
			.OnEntry(context =>
			{
				context.unlockAnimation.Play(context.unlockedAnimationClip.name);
			})
			.Permit(Trigger.Lock, VaultState.Locked);
	}

	public void OnInteractableChanged(bool isInteractable)
	{
		interactHint.SetActive(isInteractable);
	}

	public void OnInteract()
	{
		handle.Fire(Trigger.StartUnlock);
		onePasswordUnityManager.EnqueueCommand(manager =>
		{
			var vaults = manager.GetVaults();

			IVault gameVault = null;
			foreach (IVault vault in vaults)
			{
				Debug.Log($"Vault: {vault.Name}");
				if (vault.Name == MainGame.VaultName)
				{
					gameVault = vault;
					break;
				}
			}

			if (ReferenceEquals(gameVault, null))
			{
				gameVault = manager.CreateVault(MainGame.VaultName, "Vault created by gamification tutorial to 1Password",
					VaultIcon.RoundDoor);


				var passwordGlyphs = FindObjectsOfType<PasswordGlyph>();

				foreach (PasswordGlyph passwordGlyph in passwordGlyphs)
				{
					passwordGlyph.CreatePassword(manager);
				}

			}
			MainGame.CreatedVault = gameVault;
		}, () =>
		{
			handle.Fire(Trigger.FinishUnlock);
		});
	}

	public bool IsInteractable => CurrentState == VaultState.Locked;

	public VaultState CurrentState { get; set; }
}