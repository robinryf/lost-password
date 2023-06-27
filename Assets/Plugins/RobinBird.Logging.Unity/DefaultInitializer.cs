#region Disclaimer

// <copyright file="DefaultInitializer.cs">
// Copyright (c) 2019 - 2019 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Logging.Unity
{
    using Runtime;
    using Provider;
    using UnityEngine;
    
#if ROBIN_BIRD_EDITOR_UTILS
    [DefaultExecutionOrder(2000)]
    public static class DefaultInitializer
    {
        private static bool isInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnRuntimeMethodLoad()
        {
            if (isInitialized == false)
            {
                Log.FallbackProvider = new UnityLogProvider(LogLevel.Info);
                isInitialized = true;
            }
        }
    }
#endif
}