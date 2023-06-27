using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
    /// <summary>
    /// Component to identify object during play mode test
    /// </summary>
    public class TestId : MonoBehaviour
    {
        [SerializeField]
        private string id;

        public string Id
        {
	        get => id;
	        set => id = value;
        }
    }
}