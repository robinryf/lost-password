#region Disclaimer
// <copyright file="PropertyInfoExtensions.cs">
// Copyright (c) 2019 - 2019 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;

    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Returns attribute from type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="propertyInfo">The type to get the attribute from.</param>
        /// <param name="inherit">if true search types inheritance chain for attribute.</param>
        [CanBeNull]
        public static T GetCustomAttribute<T>(this PropertyInfo propertyInfo, bool inherit) where T : Attribute
        {
            return (T)propertyInfo.GetCustomAttribute(typeof (T), inherit);
        }

        /// <summary>
        /// Returns attribute from type.
        /// </summary>
        /// <param name="propertyInfo">The type to get the attribute from.</param>
        /// <param name="attributeType">The type of the attribute</param>
        /// <param name="inherit">if true search types inheritance chain for attribute.</param>
        [CanBeNull]
        public static Attribute GetCustomAttribute(this PropertyInfo propertyInfo, Type attributeType, bool inherit)
        {
            object[] attributes = propertyInfo.GetCustomAttributes(attributeType, inherit);

            if (attributes.Length > 0)
            {
                return (Attribute)attributes[0];
            }
            return null;
        }

        /// <summary>
        /// Returns attributes of type <typeparamref name="T"/>
        /// </summary>
        public static T[] GetCustomAttributes<T>(this PropertyInfo propertyInfo, bool inherit)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof (T), inherit);
            return attributes as T[];
        }
    }
}