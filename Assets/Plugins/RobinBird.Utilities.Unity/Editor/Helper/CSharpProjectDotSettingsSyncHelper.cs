using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using RobinBird.Logging.Runtime;
using RobinBird.Utilities.Runtime.Extensions;
using RobinBird.Utilities.Runtime.Helper;
using UnityEditor;
using UnityEditorInternal;

namespace RobinBird.Utilities.Unity.Editor.Helper
{
	/// <summary>
	/// Event names come from com.unity.ide.rider/Rider/Editor/ProjectGeneration/ProjectGeneration.cs
	/// </summary>
	public class CSharpProjectDotSettingsSyncHelper
	{
#if ROBIN_BIRD_EDITOR_UTILS
		[MenuItem("Tools/Developer/Sync symlinks of DotSettings")]
#endif
		public static void SyncDotSettingsSymlinks()
		{
			if (EditorUtility.DisplayDialog("Attention",
				"We will move all .DotSettings files for that we find Assembly Definition files next to the ADF or DELETE it if there is already a DotSettings in the repo.",
				"Ok", "Abort") == false)
			{
				Log.Info("Cancelled");
				return;
			}

			var nameRegex = new Regex("\"name\": \"(.*?)\",");
			var assemblyDefinitionGuids = AssetDatabase.FindAssets($"t:{nameof(AssemblyDefinitionAsset)}");
			var assemblyDefinitionNames = new List<AssemblyDefinitionAsset>();
			foreach (string guid in assemblyDefinitionGuids)
			{
				var asset = AssetDatabaseHelper.LoadAssetAtGuid<AssemblyDefinitionAsset>(guid);
				assemblyDefinitionNames.Add(asset);
			}
			var csProjFiles = Directory.GetFiles(EditorTools.GetProjectPath(), "*.csproj");

			var commandBuilder = new StringBuilder();
			
			foreach (var csProjFile in csProjFiles)
			{
				var dotSettingsFile = csProjFile + ".DotSettings";

				if (File.Exists(dotSettingsFile))
				{
					if (FileHelper.IsSymbolic(dotSettingsFile))
					{
						Log.Info($"Found symlink at {dotSettingsFile}");
						continue;
					}
				}

				var cleanCsName = Path.GetFileNameWithoutExtension(csProjFile);
				var assemblyDefinition = assemblyDefinitionNames.Find(adf =>
				{
					var match = nameRegex.Match(adf.text);
					if (match.Success == false)
						return false;

					return match.Groups[1].Value == cleanCsName;
				});

				if (assemblyDefinition == null)
				{
					continue;
				}

				string adfPath = AssetDatabase.GetAssetPath(assemblyDefinition);
				Log.Info($"Found connection between {adfPath} and {csProjFile}");

				var dotSettingsFileInUnityPath = adfPath.Replace(".asmdef", ".DotSettings");

				bool createLink = false;
				if (File.Exists(dotSettingsFileInUnityPath) == false)
				{
					if (File.Exists(dotSettingsFile))
					{
						Log.Info($"Import {dotSettingsFile} to {dotSettingsFileInUnityPath}");
						File.Move(dotSettingsFile, dotSettingsFileInUnityPath);
						createLink = true;
					}
				}
				else
				{
					// We already have one in source control. That has priority
					Log.Info($"Delete {dotSettingsFile}");
					File.Delete(dotSettingsFile);
					createLink = true;
				}

				if (createLink)
				{
					commandBuilder.AppendLine($"ln -s {dotSettingsFileInUnityPath} {dotSettingsFile} &&\\");
				}
			}

			var result = commandBuilder.ToString();
			if (string.IsNullOrEmpty(result))
			{
				Log.Info("All settings files already synced");
			}
			else
			{
				result = result.RemoveLast(" &&\\");
				Log.Info($"Execute this (copied to clipboard):\n{result}");
				EditorGUIUtility.systemCopyBuffer = result;
			}
		}
	}
}