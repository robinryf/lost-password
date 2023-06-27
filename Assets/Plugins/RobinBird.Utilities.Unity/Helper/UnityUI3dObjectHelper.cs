#region Disclaimer
// <copyright file="UnityUI3dObjectHelper.cs">
// Copyright (c) 2019 - 2019 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Utilities.Unity.Helper
{
    using System.Collections.Generic;
    using Extensions;
    using Logging.Runtime;
    using UnityEngine;

    /// <summary>
    /// Helper class to render 3d objects inside UI elements using Render Textures
    /// This script uses the <see cref="GameObject.layer"/> property to control which object is rendered
    /// This way only one camera is needed for multiple UI objects.
    /// </summary>
    public class UnityUI3DObjectHelper
    {
        private const string UIObjectRenderLayerName = "UIObjectRender";
        private const string UIObjectRenderIgnoreLayerName = "UIObjectRenderIgnore";
        private readonly int textureWidth;
        private readonly int textureHeight;

        private struct UIObjectData
        {
            public RenderTexture RenderTexture;

            public GameObject RenderObject;
        }
        
        private readonly Dictionary<GameObject, UIObjectData> objectCache = new Dictionary<GameObject, UIObjectData>();
        private readonly int uiObjectRenderLayer;
        private readonly int uiObjectRenderIgnoreLayer;
        private readonly UnityUI3DObjectCamera cameraInstance;
        
        /// <summary>
        /// Create as many helpers as you like for different UI Mediators
        /// </summary>
        /// <param name="cameraPrefab">Prefab with the camera which should be used to capture the 3d object.</param>
        /// <param name="textureWidth">Width of the render texture captured.</param>
        /// <param name="textureHeight">Height of the render texture captured.</param>
        public UnityUI3DObjectHelper(UnityUI3DObjectCamera cameraPrefab, int textureWidth, int textureHeight)
        {
            cameraInstance = Object.Instantiate(cameraPrefab);
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            uiObjectRenderLayer = LayerMask.NameToLayer(UIObjectRenderLayerName);
            uiObjectRenderIgnoreLayer = LayerMask.NameToLayer(UIObjectRenderIgnoreLayerName);

            if (uiObjectRenderLayer == -1 || uiObjectRenderIgnoreLayer == -1)
            {
                Log.Error("Please make sure you have the layers: " + UIObjectRenderLayerName + ", " + UIObjectRenderIgnoreLayerName
                          + " defined in Project Settings");
            }

            // The camera rendering is only invoked manually and should not draw to screen if no render texture
            // is assigned
            cameraInstance.ObjectCamera.enabled = false;
        }

        public RenderTexture SetupRenderTexture(GameObject objectToRender)
        {
            UIObjectData data;

            if (objectCache.TryGetValue(objectToRender, out data) == false)
            {
                data = new UIObjectData();
                data.RenderTexture = new RenderTexture(textureWidth, textureHeight, 32);
                
                objectToRender.layer = uiObjectRenderIgnoreLayer;
                objectToRender.transform.SetParent(cameraInstance.ObjectAnchor, false);
                data.RenderObject = objectToRender;
                
                objectCache.Add(objectToRender, data);
            }
            
            return data.RenderTexture;
        }

        public void UpdateAllRenderTextures()
        {
            foreach (KeyValuePair<GameObject,UIObjectData> pair in objectCache)
            {
                UpdateRenderTexture(pair.Value);
            }
        }

        public void UpdateRenderTexture(GameObject objectToRender)
        {
            UpdateRenderTexture(objectCache[objectToRender]);
        }

        public void Destroy()
        {
            ReleaseRenderTextures();
            if (cameraInstance != null)
            {
                Object.Destroy(cameraInstance.gameObject);
            }
        }

        public void ReleaseRenderTextures()
        {
            foreach (KeyValuePair<GameObject,UIObjectData> pair in objectCache)
            {
                UIObjectData data = pair.Value;
                data.RenderTexture.Release();
            }
        }

        private void UpdateRenderTexture(UIObjectData data)
        {
            cameraInstance.ObjectCamera.targetTexture = data.RenderTexture;
            data.RenderObject.SetLayerInChilds(uiObjectRenderLayer);
            cameraInstance.ObjectCamera.Render();
            cameraInstance.ObjectCamera.targetTexture = null;
            data.RenderObject.SetLayerInChilds(uiObjectRenderIgnoreLayer);
        }
    }
}