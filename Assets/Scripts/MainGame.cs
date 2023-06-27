using System;
using System.Collections;
using System.Linq;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;
using RobinBird.Utilities.Unity.Helper;
using UnityEngine;
using UnityEngine.Playables;

namespace LostPassword
{
	/// <summary>
	/// Central point of game that keeps track of state across the whole game
	/// </summary>
	public class MainGame : MonoBehaviour
	{
		public static IVault CreatedVault { get; set; }

		[SerializeField]
		private GameObject[] solvedPuzzleObjects;

		[SerializeField]
		private GameObject lostPassword;

		[SerializeField]
		private PlayableDirector passwordReveal;

		[SerializeField]
		private GameObject winText;

		[SerializeField]
		private OnePasswordUnityManager onePasswordUnityManager;
		
		private void Awake()
		{
			MainThreadHelper.Init();
		}

		private void Update()
		{
			MainThreadHelper.Instance.Update();
		}

		#if UNITY_EDITOR
		[UnityEditor.MenuItem("CONTEXT/MainGame/Test Reveal")]
		public static void TestReveal()
		{
			var mainGame = FindObjectOfType<MainGame>();
			mainGame.RevealLostPassword();
		}
		#endif

		public void RevealLostPassword()
		{
			StartCoroutine(RevealCoroutine());
		}

		private IEnumerator RevealCoroutine()
		{
			foreach (GameObject solvedPuzzleObject in solvedPuzzleObjects)
			{
				yield return new WaitForSeconds(0.7f);
				solvedPuzzleObject.SetActive(false);
			}
			
			passwordReveal.Play();

			yield return new WaitForSeconds(1.5f);

			lostPassword.SetActive(true);
			
			onePasswordUnityManager.EnqueueCommand(manager =>
			{
				Template template = manager.GetTemplate(Category.Password);
				template.Title = "LOST PASSWORD";
				var passwordField = template.Fields.First(field => field.Purpose == FieldPurpose.Password);

				passwordField.Value = "Thank you for playing! ;)";

				manager.CreateItem(template, CreatedVault);
			}, () =>
			{
				
			});

			yield return new WaitForSeconds(3f);
			
			winText.SetActive(true);
		}

		public const string VaultName = "LostPassword";
		public const string Glyph1Name = "Glyph1";
		public const string Glyph2Name = "Glyph2";
		public const string Glyph3Name = "Glyph3";
	}
}