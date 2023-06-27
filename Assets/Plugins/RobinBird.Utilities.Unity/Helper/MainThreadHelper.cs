using System.Threading;

namespace RobinBird.Utilities.Unity.Helper
{
	using Logging.Runtime;
	using System;
    using System.Collections.Generic;

    public class MainThreadHelper
    {
        private static Thread unityThread;
        
        private static MainThreadHelper instance;

        private readonly Queue<Action> mainThreadActions = new Queue<Action>();

        public static MainThreadHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainThreadHelper();
                }

                return instance;
            }
        }

        /// <summary>
        /// Make sure this is called from the main/unity thread
        /// </summary>
        public static void Init()
        {
            unityThread = Thread.CurrentThread;
        }

        public static bool IsCurrentlyOnUnityThread()
        {
            if (unityThread == null)
            {
                Log.Error("To check for unity thread first call 'Init()' method.");
                return true;
            }
            return Thread.CurrentThread == unityThread;
        }

        public static void QueueOnMainThread(Action action)
        {
            Instance.mainThreadActions.Enqueue(action);
        }

        public void Update()
        {
            if (IsCurrentlyOnUnityThread() == false)
            {
                Log.Error("Only call this from Main/Unity thread!");
                return;
            }
            while (instance.mainThreadActions.Count > 0)
            {
                var action = instance.mainThreadActions.Dequeue();
                action();
            }
        }
    }
}