namespace RobinBird.Utilities.Unity.Helper
{
    using UnityEngine;

    /// <summary>
    /// Can be used to start Coroutines. Very usefull when starting
    /// a Coroutine from an non <see cref="MonoBehaviour" /> object.
    /// </summary>
    public class RoutineDispatcher : MonoBehaviour
    {
        private static RoutineDispatcher _instance;
        private static GameObject _singletonGameObject;
        private static object _singletonLock = new object();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void DomainReset()
        {
            _instance = null;
            _singletonGameObject = null;
        }

        public static RoutineDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    Init();
                }
                return _instance;
            }
        }

        private static void Init()
        {
            if (_instance != null)
            {
                return;
            }

            lock (_singletonLock)
            {
                if (_instance != null)
                {
                    return;
                }

                _singletonGameObject = new GameObject();
                DontDestroyOnLoad(_singletonGameObject);
                _instance = _singletonGameObject.AddComponent<RoutineDispatcher>();
                _singletonGameObject.name = "RoutineDispatcher";
            }
        }
    }
}