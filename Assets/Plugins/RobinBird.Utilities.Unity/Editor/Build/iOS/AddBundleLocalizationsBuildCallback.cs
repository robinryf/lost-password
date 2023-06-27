using System.Collections.Generic;
using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Localization;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
	/// <summary>
	/// Sets the localizations that are shown in the AppStore for this build
	/// </summary>
	[CreateAssetMenu(fileName = nameof(AddBundleLocalizationsBuildCallback), menuName = CreateAssetMenuItemName + nameof(AddBundleLocalizationsBuildCallback))]
	public class AddBundleLocalizationsBuildCallback : AbstractXcodeModificationBuildCallback
	{
		private const string PlistKeyValue = "CFBundleLocalizations";

		public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			if (target != BuildTarget.iOS)
			{
				return;
			}
			
#if UNITY_LOCALIZATION_PACKAGE
			// Get Localizations
			var locales = UnityEngine.Localization.LocalizationEditorSettings.GetLocales();
			CreateArray(buildPath, PlistKeyValue, array =>
			{
				foreach (UnityEngine.Localization.Locale locale in locales)
				{
					Log.Info($"Got code: {locale.Identifier.Code}");
					array.AddString(locale.Identifier.Code);
				}
			});
#endif
		}
	}
}