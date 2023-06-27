using RobinBird.Utilities.Unity.Configuration.Attributes;
using SemanticVersioning;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Configuration
{
    [CreateAssetMenu(fileName = nameof(BuildProperties), menuName = "Build/" + nameof(BuildProperties))]
    public class BuildProperties : ScriptableObject
    {
        public const string DefaultBuildPropertiesName = "BuildProperties";

        private static BuildProperties @default;
        
        public static BuildProperties Default
        {
            get
            {
                if (@default == null)
                {
                    @default = Resources.Load<BuildProperties>(DefaultBuildPropertiesName);
                    if (@default == null)
                    {
                        // At least fallback for no NRE
                        @default = CreateInstance<BuildProperties>();
                    }
                }
                return @default;
            }
        }

        /// <summary>
        /// Id of the google cloud project. (e.g. robinbird-secrets)
        /// </summary>
        public string GoogleCloudProjectId;

        /// <summary>
        /// Base id used for all other identifier (e.g. secrets)
        /// </summary>
        public string ProjectShortName;

        [BuildCommandLineArg("-commitHash")]
        public string CommitHash;

        /// <summary>
        /// Name of release if any context is given. Usually the branch name or just the release when it is a pure release
        /// </summary>
        [BuildCommandLineArg("-versionName")]
        public string VersionName;
        
        /// <summary>
        /// Backend Url which is considered "default" for this build. Fallback to it to have the vanilla setting intended for the build.
        /// Builds from branches will have this set to the branch backend. Master builds will point to the master backend, etc.
        /// </summary>
        [BuildCommandLineArg("-backendUrl")]
        public string BackendUrl;
        
        /// <summary>
        /// Special backend url only set during local builds from developer machines. This can be used to set the build to point towards a local running backends. Making debugging possible.
        /// It should be extracted from the user settings since it is different for all machines.
        /// </summary>
        [HideInInspector]
        public string LocalBackendUrl;

        /// <summary>
        /// Bundle Version in Apple format (e.g. 1.1.0). Must be 3 separate numbers with dots between them.
        /// </summary>
        [BuildCommandLineArg("-bundleVersion")]
        public string BundleVersion = "0.9.0";

        /// <summary>
        /// Id assigned automatically to every app store app. You can find it on the AppStoreConnect page under "App Information" then look for "Apple ID"
        /// E.g. 1587875747
        /// </summary>
        public string AppleAppStoreId;

        private Version bundleVersionSem;
        public Version BundleVersionSem
        {
	        get
	        {
		        if (bundleVersionSem == null)
		        {
			        bundleVersionSem = Version.Parse(BundleVersion);
		        }
		        return bundleVersionSem;
	        }
        }

        [BuildEnvironmentVariable("FIREBASE_APP_CHECK_DEBUG_TOKEN")]
        [BuildCommandLineArg("--firebase-app-check-debug-token")]
        [BuildPropertySecret]
        public string FirebaseAppCheckDebugToken;

        /// <summary>
        /// Actual number of the build from the CI environment.
        /// </summary>
        [BuildCommandLineArg("-buildNumber")]
        public int BuildNumber = 0;

        public string UnityProjectId = "";

        public int SteamAppId = 0;

        public AndroidStore AndroidStore;

        public string PrivacyPolicyUrl;

        public string TermsAndConditionsUrl;

        public string SupportEmailAddress;

        public string FeedbackEmailAddress;

        public string GetFullVersionName()
        {
	        return $"{BundleVersion} ({BuildNumber.ToString()}) [{CommitHash}] {VersionName}";
        }
    }

    public enum AndroidStore
    {
        GooglePlay,
        Nutaku,
    }
}