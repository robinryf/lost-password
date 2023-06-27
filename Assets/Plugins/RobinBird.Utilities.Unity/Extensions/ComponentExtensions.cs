#region Disclaimer
// <copyright file="ComponentExtensions.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

using System;

namespace RobinBird.Utilities.Unity.Extensions
{
    using UnityEngine;

    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this Component obj) where T : Component
        {
            T comp = obj.GetComponent<T>();
            if (comp != null)
                return comp;
            return obj.gameObject.AddComponent<T>();
        }

        public static void Destroy(this Component component)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(component);
            }
            else
            {
                Object.DestroyImmediate(component);
            }
        }
        
        public static bool HasComponent<T>(this Component obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }
        
        public static bool HasComponent(this Component obj, Type type)
        {
            return obj.GetComponent(type) != null;
        }
    }
}