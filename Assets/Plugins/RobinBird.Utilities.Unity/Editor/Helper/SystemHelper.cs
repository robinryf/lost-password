using RobinBird.Logging.Runtime;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Helper
{
    public class SystemHelper
    {
        public static void OpenDirectoryInSystemExplorer(string path)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,{path}");
                    break;
                case RuntimePlatform.OSXEditor:
                    System.Diagnostics.Process.Start("open",$"\"{path}\"");
                    break;
                default:
                    Log.Error("Not supported platform.");
                    break;
            }
        }
    }
}