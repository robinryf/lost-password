using System;
using System.IO;
using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
	/// <summary>
	/// Set custom key in Info.plist
	/// </summary>
	[CreateAssetMenu(fileName = nameof(SetInfoPlistValue), menuName = CreateAssetMenuItemName + nameof(SetInfoPlistValue))]
	public class SetInfoPlistValue : AbstractXcodeModificationBuildCallback
	{
		[SerializeField]
		private string key;

		[SerializeField]
		private ValueType valueType;

		[SerializeField]
		private bool boolValue;
		[SerializeField]
		private string stringValue;
		[SerializeField]
		private int integerValue;
		[SerializeField]
		private float floatValue;
		
		public override bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			return true;
		}

		public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
		{
			if (target != BuildTarget.iOS)
			{
				return;
			}

			SetPlistValue(buildPath, key, valueType, boolValue, stringValue, integerValue, floatValue);
		}
	}
}