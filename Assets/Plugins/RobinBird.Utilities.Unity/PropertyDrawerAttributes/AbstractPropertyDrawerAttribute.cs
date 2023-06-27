#region Disclaimer
// <copyright file="AbstractPropertyDrawer.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Unity.PropertyDrawerAttributes
{
    using UnityEngine;

    public abstract class AbstractPropertyDrawerAttribute : PropertyAttribute
    {
        public string Tooltip { get; private set; }

        protected AbstractPropertyDrawerAttribute(string tooltip)
        {
            Tooltip = tooltip;
        }
    }
}