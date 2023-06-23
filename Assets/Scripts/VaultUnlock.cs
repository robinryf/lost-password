using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LostPassword;
using OnePassword;
using OnePassword.Vaults;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class VaultUnlock : MonoBehaviour, IInteractableObject
{
	private static void CreateVault()
	{
		var manager = new OnePasswordManager("/usr/local/bin", "op", true, true);

		var vaults = manager.GetVaults();

		foreach (Vault vault in vaults)
		{
			Debug.Log($"Vault: {vault.Name}");
		}

		manager.CreateVault("LostPassword", "Vault created by gamification tutorial to 1Password",
			VaultIcon.RoundDoor);
	}

	public void OnInteract()
	{
		Debug.Log("Interact with VaultUnlock");
		Task.Run(CreateVault);
	}
}