using RobinBird.Utilities.Runtime.Extensions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using RobinBird.Logging.Runtime;


namespace RobinBird.Utilities.Unity.Editor.Build
{
    public abstract class AbstractCIBuildCallbackHandler : ScriptableObject
    {
        protected const string CreateAssetMenuItemName = "BuildCallbacks/";

        [Header("Base")]
        [SerializeField]
        private bool isEnabled = true;

        [Header("Condition")]
        [SerializeField]
        private BuildTarget[] buildTargets;
        
        [SerializeField]
        [Tooltip("These DEFINES have to be set for the callback to run. Leave empty to always pass this check.")]
        private string[] runDefines;
        
        [FormerlySerializedAs("overrideDefines")]
        [SerializeField]
        [Tooltip("These DEFINES, if set will block the callback to run. Leave empty to always pass this check.")]
        private string[] blockDefines;
        
        [SerializeField]
        [Tooltip("These DEFINES have to be set for the callback to run. Leave empty to always pass this check.")]
        private BuildOptions[] runBuildOptions;
        
        [SerializeField]
        [Tooltip("These options, if set will block the callback to run. Leave empty to always pass this check.")]
        private BuildOptions[] blockBuildOptions;

        [Header("Properties")]
        [SerializeField]
        [Multiline]
        [Tooltip("Write down for what this post processor is needed")]
        private string notes;

        public bool ShouldRun(BuildTarget target, BuildOptions buildOptions)
        {
            if (isEnabled == false)
                return false;

            if (IsBuildTargetValid(target) == false)
            {
                Log.Info($"Ignoring {name} because build target is invalid.");
                return false;
            }

            bool? runDefinesSet = AreDefinesSet(runDefines);
            if (runDefinesSet.HasValue && runDefinesSet.Value == false)
            {
                Log.Info($"Ignoring {name} because run defines are not set.");
                return false;
            }

            bool? blockDefinesSet = AreDefinesSet(blockDefines);
            if (blockDefinesSet.HasValue && blockDefinesSet.Value)
            {
                Log.Info($"Ignoring {name} because block defines are set.");
                return false;
            }

            bool? runBuildOptionsSet = AreBuildOptionsValid(buildOptions, runBuildOptions);
            if (runBuildOptionsSet.HasValue && runBuildOptionsSet.Value == false)
            {
                Log.Info($"Ignoring {name} because run options are not set.");
                return false;
            }

            bool? blockBuildOptionsSet = AreBuildOptionsValid(buildOptions, blockBuildOptions);
            if (blockBuildOptionsSet.HasValue && blockBuildOptionsSet.Value)
            {
                Log.Info($"Ignoring {name} because block options are set.");
                return false;
            }

            return true;
        }

        private bool IsBuildTargetValid(BuildTarget target)
        {
            if (buildTargets.IsNullOrEmpty())
                return true;

            foreach (BuildTarget buildTarget in buildTargets)
            {
                if (buildTarget == target)
                    return true;
            }

            return false;
        }

        private bool? AreDefinesSet(string[] checkDefines)
        {
            if (checkDefines == null || checkDefines.Length == 0)
                return null;
            
            var defines = EditorUserBuildSettings.activeScriptCompilationDefines;

            foreach (string overrideDefine in checkDefines)
            {
                if (defines.Contains(overrideDefine) == false)
                    return false;
            }

            return true;
        }

        private bool? AreBuildOptionsValid(BuildOptions buildOptions, BuildOptions[] checkOptions)
        {
            if (checkOptions.IsNullOrEmpty())
                return null;
            
            foreach (BuildOptions buildOption in checkOptions)
            {
                if (buildOptions.HasFlag(buildOption) == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets called before the build
        /// </summary>
        /// <returns>False when the build should be stopped (e.g. because of an error)</returns>
        public abstract bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath);

        /// <summary>
        /// Gets called after the build is finished
        /// </summary>
        public abstract void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath);
    }
}
