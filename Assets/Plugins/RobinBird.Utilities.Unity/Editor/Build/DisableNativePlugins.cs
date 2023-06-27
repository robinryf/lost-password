using RobinBird.Logging.Runtime;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build
{
    [CreateAssetMenu(fileName = nameof(DisableNativePlugins), menuName = CreateAssetMenuItemName + nameof(DisableNativePlugins))]
    public class DisableNativePlugins : AbstractCIBuildCallbackHandler
    {
        [SerializeField]
        private Object[] nativePlugins;
        
        public override bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            SetBuildCallback(NotIncludedBuildCallback);
            return true;
        }

        public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            // Restore default state in case this does not get called on next build
            SetBuildCallback(IncludedBuildCallback);
        }

        private void SetBuildCallback(PluginImporter.IncludeInBuildDelegate includeInBuildDelegate)
        {
            foreach (Object nativePlugin in nativePlugins)
            {
                string pluginPath = AssetDatabase.GetAssetPath(nativePlugin);
                var importer = AssetImporter.GetAtPath(pluginPath) as PluginImporter;

                if (importer == null)
                {
                    Log.Warn($"Could not load plugin: {nativePlugin} at path: {pluginPath}");
                    continue;
                }

                Log.Info($"Will ignore {pluginPath}");
                importer.SetIncludeInBuildDelegate(includeInBuildDelegate);
            }
        }

        private bool NotIncludedBuildCallback(string pluginPath)
        {
            return false;
        }
        
        private bool IncludedBuildCallback(string pluginPath)
        {
            return true;
        }
    }
}