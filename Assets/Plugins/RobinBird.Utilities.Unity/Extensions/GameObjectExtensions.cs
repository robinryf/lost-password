#region Disclaimer

// <copyright file="GameObjectExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

using RobinBird.Utilities.Unity.Helper;

namespace RobinBird.Utilities.Unity.Extensions
{
    using System;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    ///     Extension and helper methods for <see cref="GameObject" /> class.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        ///     Create and add new <see cref="GameObject" /> to transform of <paramref name="obj" />.
        /// </summary>
        /// <param name="obj"><see cref="GameObject" /> that is the parent.</param>
        /// <param name="name">The name of the new <see cref="GameObject" />.</param>
        /// <returns>The created <see cref="GameObject" />.</returns>
        public static GameObject AddChild(this GameObject obj, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(obj.transform);
            child.transform.Reset();
            return child;
        }

        /// <summary>
        ///     Create and add new <see cref="GameObject" /> to transform of <paramref name="obj" />.
        /// </summary>
        /// <param name="obj"><see cref="GameObject" /> that is the parent.</param>
        /// <param name="prefab">Prefab that is used to create the new child.</param>
        /// <param name="name">The name of the new <see cref="GameObject" />.</param>
        /// <param name="resetTransform">Pass true if the transform should be reset to default vaules. <see cref="Reset" /></param>
        /// <returns>The created <see cref="GameObject" />.</returns>
        public static GameObject AddChild(this GameObject obj, GameObject prefab, string name,
            bool resetTransform = true)
        {
            GameObject newGameObject = AddChild(obj, prefab, resetTransform);
            newGameObject.name = name;
            return newGameObject;
        }

        /// <summary>
        ///     Create and add new <see cref="GameObject" /> to transform of <paramref name="obj" />.
        /// </summary>
        /// <param name="obj"><see cref="GameObject" /> that is the parent.</param>
        /// <param name="prefab">Prefab that is used to create the new child.</param>
        /// <param name="resetTransform">
        ///     Pass true if the transform should be reset to default vaules.
        ///     <see cref="TransformExtensions.Reset" />
        /// </param>
        /// <returns>The created <see cref="GameObject" />.</returns>
        public static GameObject AddChild(this GameObject obj, GameObject prefab, bool resetTransform = true)
        {
            GameObject child = Object.Instantiate(prefab);
            child.transform.SetParent(obj.transform);
            if (resetTransform)
            {
                child.transform.Reset();
            }

            return child;
        }


        /// <summary>
        ///     Get or Add an component. This is usefull to get an component from a gameobject regardless if it already exists
        ///     or hast to be created.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="obj"><see cref="GameObject" /> to get or add the component from.</param>
        /// <returns>The get or added component.</returns>
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var comp = obj.GetComponent<T>();
            if (comp != null)
            {
                return comp;
            }

            return obj.AddComponent<T>();
        }
        
        public static Component GetOrAddComponent(this GameObject obj, Type type)
        {
            var comp = obj.GetComponent(type);
            if (comp != null)
            {
                return comp;
            }

            return obj.AddComponent(type);
        }
        
        public static bool HasComponent<T>(this GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }
        
        public static bool HasComponent(this GameObject obj, Type type)
        {
            return obj.GetComponent(type) != null;
        }

        public static void Destroy(this GameObject obj, bool isAllowingAssets = false)
        {
            if (Application.isPlaying == false)
            {
                Object.DestroyImmediate(obj, isAllowingAssets);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        public static void SetLayerInChilds(this GameObject obj, string layerName)
        {
            SetLayerInChilds(obj, LayerMask.NameToLayer(layerName));
        }

        public static void SetLayerInChilds(this GameObject obj, int layer)
        {
            Transform[] transforms = obj.GetComponentsInChildren<Transform>();

            foreach (Transform transform in transforms)
            {
                transform.gameObject.layer = layer;
            }
        }

        /// <summary>
        /// Recursively steps through all childs and gives you the chance to do something to the <see cref="GameObject"/>
        /// Will not apply change to <paramref name="rootGameObject"/>
        /// </summary>
        /// <param name="rootGameObject">The root object from where you want to start.</param>
        /// <param name="action">The action delegate you want to perform on every child</param>
        public static void ApplyChangeToAllChilds(this GameObject rootGameObject, Action<GameObject> action)
        {
            for (var i = 0; i < rootGameObject.transform.childCount; i++)
            {
                Transform child = rootGameObject.transform.GetChild(i);
                var gameObject = child.gameObject;
                action(gameObject);
                ApplyChangeToAllChilds(gameObject, action);
            }
        }

        public static bool IsPrefab(this GameObject gameObject)
        {
            if (Application.isEditor && Application.isPlaying == false)
                throw new InvalidOperationException("Method only allowed in playmode");
 
            return gameObject.scene.buildIndex < 0 && gameObject.scene.name != "DontDestroyOnLoad";// && gameObject.scene.rootCount == 0;
        }

        public static void RemoveComponentIfExists<T>(this GameObject gameObject) where T : Component
        {
            gameObject.GetComponent<T>()?.Destroy();
        }
        
        public static void RemoveComponentsIfExists<T>(this GameObject gameObject) where T : Component
        {
            var components = gameObject.GetComponents<T>();
            foreach (T component in components)
            {
                component.Destroy();
            }
        }

        public static bool IsInLayer(this GameObject gameObject, LayerMask layer)
        {
	        return layer.Contains(gameObject.layer);
        }
    }
}