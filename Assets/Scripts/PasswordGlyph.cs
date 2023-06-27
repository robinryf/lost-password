using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using OnePassword;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;
using Stateless;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LostPassword
{
	/// <summary>
	/// Glyph to keep track of the password inside the 1Password.app and show the respective lock/unlock visuals
	/// </summary>
	public class PasswordGlyph : MonoBehaviour, StateMachine<PasswordGlyph.State, PasswordGlyph.Trigger, PasswordGlyph>.IStateMachineContext
	{
		public enum PasswordType
		{
			Random,
			Memorable,
			PinCode,
		}
		
		[SerializeField]
		private Color lockedGlyphColor;
		
		[SerializeField]
		private Color unlockedGlyphColor;

		[SerializeField]
		private Material lockedGlyphMaterial;
		
		[SerializeField]
		private Material unlockedGlyphMaterial;

		[SerializeField]
		private SpriteRenderer glyphRenderer;

		[SerializeField]
		private Light2D glyphLight;
		
		[SerializeField]
		private Light2D glyphLightOuter;

		[SerializeField]
		private OnePasswordUnityManager onePasswordUnityManager;

		[SerializeField]
		private PasswordType type;

		[SerializeField]
		private string passwordName;
		
		public State CurrentState { get; set; }

		private static StateMachine<State, Trigger, PasswordGlyph> _machine;
		private StateMachine<State, Trigger, PasswordGlyph>.StateMachineHandle handle;

		public enum State
		{
			Locked,
			Unlocked
		}

		public enum Trigger
		{
			Unlock,
			Lock
		}
		
		private void Start()
		{
			CreateMachine();

			handle = _machine.CreateHandle(this, State.Locked);

			StartCoroutine(CheckForPassword());
		}

		private readonly Regex randomRegex = new Regex(@".{8,128}");
		private readonly Regex memorableRegex = new Regex(@".*?[- .,_].*?[- .,_].*?[- .,_].*?[- .,_].*");
		private readonly Regex pinCodeRegex = new Regex(@"\d{3,128}");

		private IEnumerator CheckForPassword()
		{
			while (true)
			{
				bool shouldContinue = false;
				bool? isPasswordValid = null;
				onePasswordUnityManager.EnqueueCommand(manager =>
				{
					if (MainGame.CreatedVault == null)
					{
						return;
					}
					Item passwordItem = manager.GetItems(MainGame.CreatedVault).FirstOrDefault(item => item.Title == passwordName);

					if (passwordItem == null)
					{
						passwordItem = CreatePassword(manager);
					}

					var passwordDetails = manager.GetItem(passwordItem, MainGame.CreatedVault);

					Field passwordField = passwordDetails.Fields.First(field => field.Purpose == FieldPurpose.Password);

					Regex passwordRegex = null;
					switch (type)
					{
						case PasswordType.Random:
							passwordRegex = randomRegex;
							break;
						case PasswordType.Memorable:
							passwordRegex = memorableRegex;
							break;
						case PasswordType.PinCode:
							passwordRegex = pinCodeRegex;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					
					isPasswordValid = passwordRegex.IsMatch(passwordField.Value);
				}, () =>
				{
					if (isPasswordValid.HasValue)
					{
						Trigger trigger = isPasswordValid.Value ? Trigger.Unlock : Trigger.Lock;
						if (handle.CanFire(trigger))
						{
							handle.Fire(trigger);
						}
					}
					shouldContinue = true;
				});

				while (shouldContinue == false)
				{
					yield return null;
				}
				//yield return checkForPasswordInterval;
			}
		}

		private static void CreateMachine()
		{
			if (_machine != null)
				return;
			_machine = new StateMachine<State, Trigger, PasswordGlyph>();

			_machine.Configure(State.Locked)
				.OnEntry(c =>
				{
					c.glyphRenderer.material = c.lockedGlyphMaterial;
					c.glyphLight.color = c.lockedGlyphColor;
					c.glyphLightOuter.color = c.lockedGlyphColor;
				})
				.Permit(Trigger.Unlock, State.Unlocked);

			_machine.Configure(State.Unlocked)
				.OnEntry(c =>
				{
					c.glyphRenderer.material = c.unlockedGlyphMaterial;
					c.glyphLight.color = c.unlockedGlyphColor;
					c.glyphLightOuter.color = c.unlockedGlyphColor;
				})
				.Permit(Trigger.Lock, State.Locked);
		}

		public Item CreatePassword(OnePasswordManager manager)
		{
			if (MainGame.CreatedVault == null)
			{
				return null;
			}
			Template template = manager.GetTemplate(Category.Password);
			template.Title = passwordName;
			template.Fields.First(field => field.Purpose == FieldPurpose.Password).Value = "...";
			return manager.CreateItem(template, MainGame.CreatedVault);
		}
	}
}