#if LOCALIZATION_PACKAGE

using System;
using System.Collections.Generic;
using System.Reflection;
using RobinBird.Logging.Runtime;
using RobinBird.Utilities.Runtime.Extensions;
using RobinBird.Utilities.Runtime.Helper;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RobinBird.Utilities.Unity.Extensions
{
	public static class LocalizedStringExtensions
	{
		/// <summary>
		/// Using the blocking thread method which is not recommended but catches missing strings
		/// </summary>
		public static string GetLocalizedStringSafe(this LocalizedString localizedString)
		{
			if (IsLocalizedStringInvalid(localizedString, out string localizedStringSafe))
			{
				return localizedStringSafe;
			}

			return localizedString.GetLocalizedString();
		}

		/// <summary>
		/// Helper function to handle missing or incomplete keys.
		/// Also handles errors while loading the string.
		/// </summary>
		public static void GetLocalizedStringAsyncSafe(this LocalizedString localizedString, Action<string> completeHandler)
		{
			if (IsLocalizedStringInvalid(localizedString, out string localizedStringSafe))
			{
				completeHandler(localizedStringSafe);
				return;
			}


			AsyncOperationHandle<string> loadOp = localizedString.GetLocalizedStringAsync();
			loadOp.Completed += handle =>
			{
				if (handle.Status == AsyncOperationStatus.Failed)
				{
					ErrorTracking.RecordException("FailedTranslationKeyLoading", $"String: {localizedString}", handle.OperationException);
					completeHandler(localizedStringSafe);
				}
				else
				{
					completeHandler(handle.Result);
				}
			};
		}

		private static FieldInfo cachedArgumentsField;

		public static void ApplyLocalizedStringSafeAsync(this LocalizedString localizedString, TextMeshProUGUI text, List<Object> arguments = null)
		{
			if (text.TryGetComponent(out LocalizeStringEvent component) == false)
			{
				component = text.gameObject.AddComponent<LocalizeStringEvent>();
				component.OnUpdateString.AddListener(eventText =>
				{
					text.text = eventText;
				});
			}

			if (arguments.IsNullOrEmpty() == false)
			{
				CheckMethodCache();
				cachedArgumentsField.SetValue(component, arguments);
			}
			
			component.StringReference = localizedString;
			component.RefreshString();
		}

		private static void CheckMethodCache()
		{
			if (cachedArgumentsField == null)
			{
				cachedArgumentsField = typeof(LocalizeStringEvent).GetField("m_FormatArguments",
					BindingFlags.Instance | BindingFlags.NonPublic);
			}
		}

		private static bool IsLocalizedStringInvalid(LocalizedString localizedString, out string localizedStringSafe)
		{
			if (localizedString == null || localizedString.TableReference.ReferenceType == TableReference.Type.Empty)
			{
				ErrorTracking.RecordException("MissingTranslationKey", "Detected empty localisation key. Check the stacktrace.");
				localizedStringSafe = "!KEY NOT SET";
				return true;
			}

			localizedStringSafe = string.Empty;
			return false;
		}
	}
}
#endif