using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
    using UnityEngine.Serialization;

    /// <summary>
    /// Used by <see cref="UnityUI3DObjectHelper"/> to figure out which camera to use and where to place objects which
    /// should be used to capture a texture for UI textures
    /// </summary>
    public class UnityUI3DObjectCamera : MonoBehaviour
    {
        [FormerlySerializedAs("camera")]
        [SerializeField]
        private Camera objectCamera;

        [SerializeField]
        private Transform objectAnchor;
        
        public Camera ObjectCamera
        {
            get { return objectCamera; }
        }


        public Transform ObjectAnchor
        {
            get { return objectAnchor; }
        }
    }
}