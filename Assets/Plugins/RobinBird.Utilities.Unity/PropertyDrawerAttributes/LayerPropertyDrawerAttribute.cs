#region Disclaimer
// <copyright file="LayerPropertyDrawerAttribute.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Unity.PropertyDrawerAttributes
{
    using UnityEngine;

    /// <summary>
    /// This attribute specifies for an <see cref="int"/> that it should be presented as
    /// Unity <see cref="GameObject.layer"/> editor. 
    /// </summary>
    public class LayerPropertyDrawerAttribute : AbstractPropertyDrawerAttribute
    {
        public LayerPropertyDrawerAttribute(string tooltip) : base(tooltip)
        {
        }
    }
}