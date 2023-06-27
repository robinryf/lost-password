using System.IO;
using System.Reflection;
using RobinBird.Utilities.Runtime.Helper;
using RobinBird.Utilities.Unity.Configuration;
using RobinBird.Utilities.Unity.Configuration.Attributes;
using RobinBird.Utilities.Unity.Helper;
using UnityEditor.Build.Player;
using UnityEditor.Compilation;

namespace RobinBird.Utilities.Unity.Editor.Build
{
    using System;
    using System.Collections.Generic;
    using Helper;
    using Logging.Runtime;
    using Popups;
    using Runtime.Extensions;
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public class BuildHelper
    {
        private const string BuildPathCLIName = "-buildPath";
        private const string ExportProjectCLIName = "-exportProject";
        private static List<AbstractCIBuildCallbackHandler> buildHandlers;

#if ROBIN_BIRD_EDITOR_UTILS
        [MenuItem("File/CI Build/Android/Test")]
        public static void AndroidTest()
        {
            // We don't want to have a debug build here because the tests on device should be as close as possible
            // to the real thing. Also debug mode some times breaks automated tests because of permission popups
            ProxyCIBuild(BuildTarget.Android, BuildOptions.IncludeTestAssemblies);
        }

        [MenuItem("File/CI Build/Android/Debug")]
        public static void AndroidDebug()
        {
            ProxyCIBuild(BuildTarget.Android, BuildOptions.Development);
        }

        [MenuItem("File/CI Build/Android/Release")]
        public static void AndroidRelease()
        {
            ProxyCIBuild(BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("File/CI Build/Android/Compile Player Scripts")]
        public static void AndroidCompilePlayerScripts()
        {
	        var buildTargetGroup = BuildTargetGroup.Android;
	        var buildTarget = BuildTarget.Android;
	        CompilePlayerScripts(buildTarget, buildTargetGroup);
        }

        [MenuItem("File/CI Build/iOS/Test")]
        public static void IosTest()
        {
            // We don't want to have a debug build here because the tests on device should be as close as possible
            // to the real thing. Also debug mode some times breaks automated tests because of permission popups
            ProxyCIBuild(BuildTarget.iOS, BuildOptions.IncludeTestAssemblies);
        }

        [MenuItem("File/CI Build/iOS/Debug")]
        public static void IosDebug()
        {
            ProxyCIBuild(BuildTarget.iOS, BuildOptions.Development);
        }

        [MenuItem("File/CI Build/iOS/Release")]
        public static void IosRelease()
        {
            ProxyCIBuild(BuildTarget.iOS, BuildOptions.None);
        }

        [MenuItem("File/CI Build/iOS/Compile Player Scripts")]
        public static void IosCompilePlayerScripts()
        {
	        var buildTargetGroup = BuildTargetGroup.iOS;
	        var buildTarget = BuildTarget.iOS;
	        CompilePlayerScripts(buildTarget, buildTargetGroup);
        }

        [MenuItem("File/CI Build/Windows/Debug")]
        public static void WindowsDebug()
        {
            ProxyCIBuild(BuildTarget.StandaloneWindows, BuildOptions.Development);
        }
        
        [MenuItem("File/CI Build/Windows/Release")]
        public static void WindowsRelease()
        {
            ProxyCIBuild(BuildTarget.StandaloneWindows, BuildOptions.None);
        }
        
        [MenuItem("File/CI Build/Windows/Compile Player Scripts")]
        public static void WindowsCompilePlayerScripts()
        {
	        var buildTargetGroup = BuildTargetGroup.Standalone;
	        var buildTarget = BuildTarget.StandaloneWindows;
	        CompilePlayerScripts(buildTarget, buildTargetGroup);
        }

        [MenuItem("File/CI Build/macOS/Debug")]
        public static void MacOSDebug()
        {
            ProxyCIBuild(BuildTarget.StandaloneOSX, BuildOptions.Development);
        }

        [MenuItem("File/CI Build/macOS/Release")]
        public static void MacOSRelease()
        {
            ProxyCIBuild(BuildTarget.StandaloneOSX, BuildOptions.None);
        }
        
        [MenuItem("File/CI Build/macOS/Compile Player Scripts")]
        public static void MacOSCompilePlayerScripts()
        {
	        var buildTargetGroup = BuildTargetGroup.Standalone;
	        var buildTarget = BuildTarget.StandaloneOSX;
	        CompilePlayerScripts(buildTarget, buildTargetGroup);
        }
        
        [MenuItem("File/CI Build/WebGL/Debug")]
        public static void WebGLDebug()
        {
	        ProxyCIBuild(BuildTarget.WebGL, BuildOptions.None);
        }

        [MenuItem("File/CI Build/WebGL/Release")]
        public static void WebGLRelease()
        {
            ProxyCIBuild(BuildTarget.WebGL, BuildOptions.None);
        }
        
        [MenuItem("File/CI Build/WebGl/Compile Player Scripts")]
        public static void WebGLCompilePlayerScripts()
        {
	        var buildTargetGroup = BuildTargetGroup.WebGL;
	        var buildTarget = BuildTarget.WebGL;
	        CompilePlayerScripts(buildTarget, buildTargetGroup);
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
        }

        private static void BuildPlayerHandler(BuildPlayerOptions options)
        {
            Build(options.locationPathName, options.target, options.options);
        }
#endif

        private static void ProxyCIBuild(BuildTarget buildTarget, BuildOptions buildOptions)
        {
	        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
	        Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            var buildPath = ArgumentHelper.GetCLIArgument(BuildPathCLIName);
            
#if !UNITY_2020_2_OR_NEWER // It seems in 2020 we can't append to an empty directory
            // This will speed up builds and allow incremental builds
            buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;
#endif
            
            if (buildTarget == BuildTarget.StandaloneOSX)
            {
#if UNITY_STANDALONE_OSX
	            // Since we don't have any Apple Silicon hardware we build for intel only and rely on Rosetta for now
#if UNITY_2022_1_OR_NEWER
                UnityEditor.OSXStandalone.UserBuildSettings.architecture = UnityEditor.Build.OSArchitecture.x64ARM64;
#elif UNITY_2020_1_OR_NEWER
                UnityEditor.OSXStandalone.UserBuildSettings.architecture = UnityEditor.OSXStandalone.MacOSArchitecture.x64ARM64;
#endif
#endif
            }
            else if (buildTarget == BuildTarget.WebGL)
            {
	            PlayerSettings.SetAdditionalIl2CppArgs("--emit-null-checks --enable-array-bounds-check");
            }
            
            if (buildOptions.HasFlag(BuildOptions.Development))
            {
	            // For ci builds automatically set the il2cpp settings
#if UNITY_2021_OR_NEWER
	            EditorUserBuildSettings.il2CppCodeGeneration = buildOptions.HasFlag(BuildOptions.Development) ? Il2CppCodeGeneration.OptimizeSize : Il2CppCodeGeneration.OptimizeSpeed;
#endif
            }

            if (Application.isBatchMode == false && string.IsNullOrEmpty(buildPath))
                buildPath = EditorUtility.OpenFolderPanel("Enter BuildPath", "", "");

            var buildResult = Build(buildPath, buildTarget, buildOptions);
            if (buildResult == false)
            {
                Log.Error("Build failed. Check the logs.");
                if (Application.isBatchMode)
                    EditorApplication.Exit(1);
            }
            else
            {
                Log.Info($"Build succeeded at path: {buildPath}");
            }
        }

        private static bool Build(string buildPath, BuildTarget buildTarget, BuildOptions buildOptions)
        {
            if (string.IsNullOrEmpty(buildPath))
            {
                Log.Error($"Build path is empty. Please specify the {BuildPathCLIName} option.");
            }

            buildOptions |= BuildOptions.StrictMode;
            buildOptions &= ~BuildOptions.ShowBuiltPlayer;
            buildOptions |= BuildOptions.CompressWithLz4HC;

            Log.Info($"Building for {buildTarget} with Options: {buildOptions.ToString()}. Path: {buildPath}");

            // Build Hooks should have access to thread helper even if everything runs on main/unity thread
            MainThreadHelper.Init();
            
            InitBuildCallbacks();
            bool prebuildResult = PreBuild(buildTarget, buildPath, buildOptions);

            if (prebuildResult == false)
            {
                Log.Error("Aborting build because of pre-build error.");
                return false;
            }

            if (ArgumentHelper.TryGetCLIArgument(ExportProjectCLIName, out string _))
            {
#if UNITY_IOS || UNITY_STANDALONE_OSX
                UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject = true;
#endif
                EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
            }
            
            var buildProperties = SetupBuildProperties();

            if (buildProperties == null)
            {
	            Log.Error("Could not setup build Properties");
	            return false;
            }

            if (Application.isBatchMode)
            {
	            PlayerSettings.bundleVersion = buildProperties.BundleVersion;
	            PlayerSettings.Android.bundleVersionCode = buildProperties.BuildNumber;
	            PlayerSettings.iOS.buildNumber = buildProperties.BuildNumber.ToString();
            }

            var report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, buildTarget, buildOptions);

            Log.Info($"Build ended with result: {report.summary.result}");
            if (report.summary.result != BuildResult.Succeeded)
            {
                Log.Error($"Errors: {report.summary.totalErrors}, Warnings: {report.summary.totalWarnings}");
                return false;
            }
            PostBuild(report, buildTarget, buildPath, buildOptions);

            return true;
        }

        private static BuildProperties SetupBuildProperties()
        {
	        var guids = AssetDatabase.FindAssets($"t:{nameof(BuildProperties)}");

            if (guids.IsNullOrEmpty())
                return null;


            BuildProperties result = null;
            
            foreach (string guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                if (UnityPathHelper.IsInResourcesDirectory(path) == false)
                {
                    Log.Error($"Detected build properties which are not in Resources directory: {path}");
                    continue;
                }

                if (Path.GetFileNameWithoutExtension(path) != BuildProperties.DefaultBuildPropertiesName)
                {
                    Log.Error("Your BuildProperties are not named correctly and will not be found");
                }

                var buildProperties = AssetDatabase.LoadAssetAtPath<BuildProperties>(path);
                Log.Info($"Setting build properties at: {path}");
                SetBuildPropertiesFromBuildEnvironment(buildProperties);
                result = buildProperties;
            }

            return result;
        }

        private static void SetBuildPropertiesFromBuildEnvironment(BuildProperties buildProperties)
        {
	        bool IsSecret(FieldInfo field)
	        {
		        return field.GetCustomAttribute<BuildPropertySecretAttribute>() != null;
	        }
	        void SetFinalValue(FieldInfo field, string sourceValue, object finalValue)
	        {
		        bool isSecret = IsSecret(field);
		        object convertedValue = Convert.ChangeType(finalValue, field.FieldType);
		        Log.Info($"Setting build property {field.Name} from build environment via {sourceValue} to '{(isSecret ? "************" : convertedValue)}' (before conversion: '{(isSecret ? "************" : finalValue)}')");
		        field.SetValue(buildProperties, convertedValue);
	        }
	        
	        foreach (FieldInfo field in buildProperties.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
	        {
		        if (Application.isBatchMode && IsSecret(field) && field.GetValue(buildProperties) != null)
		        {
			        Log.Info($"Purging secret from field '{field.Name}' in BuildProperties");
			        field.SetValue(buildProperties, null);
		        }
		        foreach (var commandLineArg in field.GetCustomAttributes<BuildCommandLineArgAttribute>())
		        {
			        if (ArgumentHelper.TryGetCLIArgument(commandLineArg.CLIArgName, out string value))
			        {
				        SetFinalValue(field, commandLineArg.CLIArgName, value);
				        break;
			        }
		        }

		        foreach (var envVariable in field.GetCustomAttributes<BuildEnvironmentVariableAttribute>())
		        {
			        var envValue = Environment.GetEnvironmentVariable(envVariable.EnvVariableName);

			        if (string.IsNullOrEmpty(envValue) == false)
			        {
				        SetFinalValue(field, envVariable.EnvVariableName, envValue);
				        break;
			        }
		        }
	        }
	        EditorUtility.SetDirty(buildProperties);
        }

        private static bool PreBuild(BuildTarget target, string buildPath, BuildOptions buildOptions)
        {
            foreach (AbstractCIBuildCallbackHandler handler in buildHandlers)
            {
                if (handler.ShouldRun(target, buildOptions) == false)
                    continue;
                
                Log.Info($"Calling Pre-Build on {handler.name} ({handler.GetType()})");
                bool result = handler.OnPreBuild(target, buildOptions, buildPath);
                if (result == false)
                {
                    Log.Error($"Pre-Build Callback {handler.name} ({handler.GetType()}) had error. Aborting build");
                    return false;
                }
            }

            return true;
        }

        private static void PostBuild(BuildReport report, BuildTarget target, string buildPath,
            BuildOptions buildOptions)
        {
            Log.Info(
                $"BuildResult: {report.summary.result} errors: {report.summary.totalErrors.ToString()} path: {report.summary.outputPath}");

            foreach (AbstractCIBuildCallbackHandler handler in buildHandlers)
            {
                if (handler.ShouldRun(target, buildOptions) == false)
                    continue;
                
                Log.Info($"Calling Post-Build on {handler.name} ({handler.GetType()})");
                handler.OnPostBuild(report, target, buildOptions, buildPath);
            }
            Log.Info("Post-Build callbacks run successfully.");
        }

        private static void InitBuildCallbacks()
        {
            buildHandlers = new List<AbstractCIBuildCallbackHandler>();
            var buildHandlerTypes = AppDomain.CurrentDomain.GetTypes(typeof(AbstractCIBuildCallbackHandler));
            foreach (Type type in buildHandlerTypes)
            {
                var assetGuids = AssetDatabase.FindAssets($"t:{type.Name}");
                if (assetGuids.Length == 0)
                {
                    continue;
                }

                foreach (string assetGuid in assetGuids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                    var instance = AssetDatabase.LoadAssetAtPath(assetPath, type) as AbstractCIBuildCallbackHandler;
                    if (instance == null)
                    {
                        Log.Error($"Could not find build callback ({type}) at path: {assetPath}");
                    }
                    else
                    {
                        Log.Info($"Found callback handler of type {type} at path: {assetPath}");
                        buildHandlers.Add(instance);
                    }
                }
            }
        }
        
        /// <summary>
        /// Only compiles the player scripts with the selected build target.
        /// </summary>
        private static void CompilePlayerScripts(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup)
        {
	        Log.Info($"Compiling {buildTarget.ToString()} Player Scripts");
	        var result = PlayerBuildInterface.CompilePlayerScripts(
		        new ScriptCompilationSettings() {@group = buildTargetGroup, target = buildTarget},
		        $"{buildTarget.ToString()}PlayerScripts");
	        int exitCode;
	        if (result.assemblies.Count == 0)
	        {
		        Log.Error("Player Scripts Build failed");
		        exitCode = 1;
	        }
	        else
	        {
				Log.Info($"Compiled {result.assemblies.Count.ToString()} assemblies.");
				exitCode = 0;
	        }
			if (Application.isBatchMode)
			{
				EditorApplication.Exit(exitCode);
			}
        }
    }
}