using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
	public static class PlayerPrefsHelper
	{
		public static bool GetBool(string key, bool defaultValue = false)
		{
			var intValue = PlayerPrefs.GetInt(key, defaultValue ? 0 : 1);
			return intValue == 0;
		}

		public static void SetBool(string key, bool value)
		{
			var intValue = value ? 0 : 1;
			PlayerPrefs.SetInt(key, intValue);
		}
	}
}