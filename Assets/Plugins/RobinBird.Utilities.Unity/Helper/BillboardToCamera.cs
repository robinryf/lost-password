namespace RobinBird.Utilities.Unity.Helper
{
    using UnityEngine;

    public class BillboardToCamera : MonoBehaviour
    {
        private static Camera billboardCamera;

        private static Camera BillboardCamera
        {
            get
            {
                if (billboardCamera == null)
                {
                    billboardCamera = Camera.main;
                }
                return billboardCamera;
            }
        }

        private void LateUpdate()
        {
            if (BillboardCamera == null)
            {
                return;
            }
            
            Quaternion cameraRotation = BillboardCamera.transform.rotation;
            transform.LookAt(transform.position + cameraRotation * Vector3.forward,
                cameraRotation * Vector3.up);
        }
    }
}