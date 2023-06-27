using UnityEngine;

namespace RobinBird.Utilities.Unity.Helper
{
    /// <summary>
    /// Normally <see cref="Camera.orthographicSize"/> is orientated from the screen height. This script makes it possible
    /// to orientate from the screen width
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode] // Required to change the cam size when GameView resolution changes
    public class CameraOrthographicWidth : MonoBehaviour
    {
        /// <summary>
        /// In Unity world units
        /// </summary>
        public float TargetWidth = 5;
        
        private void SetOrthoSize()
        {
            var cam = GetComponent<Camera>();
            var targetHeight = TargetWidth * 0.5f * (1/cam.aspect);
            cam.orthographicSize = targetHeight;
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            SetOrthoSize();
        }
#endif

        private void Awake()
        {
            SetOrthoSize();
        }

        private void OnValidate()
        {
            SetOrthoSize();
        }
    }
}