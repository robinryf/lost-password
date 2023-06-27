#region Disclaimer

// <copyright file="TypeExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

using System.Collections.Generic;

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Extension and helper methods for <see cref="Type" /> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns attribute from type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="type">The type to get the attribute from.</param>
        /// <param name="inherit">if true search types inheritance chain for attribute.</param>
        [CanBeNull]
        public static T GetCustomAttribute<T>(this Type type, bool inherit) where T : Attribute
        {
            return (T)type.GetCustomAttribute(typeof (T), inherit);
        }

        /// <summary>
        /// Returns attribute from type.
        /// </summary>
        /// <param name="type">The type to get the attribute from.</param>
        /// <param name="attributeType">The type of the attribute</param>
        /// <param name="inherit">if true search types inheritance chain for attribute.</param>
        [CanBeNull]
        public static Attribute GetCustomAttribute(this Type type, Type attributeType, bool inherit)
        {
            object[] attributes = type.GetCustomAttributes(attributeType, inherit);

            if (attributes.Length > 0)
            {
                return (Attribute)attributes[0];
            }
            return null;
        }

        /// <summary>
        /// Returns attributes of type <typeparamref name="T"/>
        /// </summary>
        public static T[] GetCustomAttributes<T>(this Type type, bool inherit)
        {
            var attributes = type.GetCustomAttributes(typeof (T), inherit);
            return attributes as T[];
        }

        /// <summary>
        /// Gets the <see cref="Type.FullName" /> of a type but without the generic declaration.
        /// (e.g. from System.Collections.List`1; to System.Collections.List )
        /// </summary>
        public static string FullNameWithoutGenericParameters(this Type type)
        {
            string name = type.FullName;
            return name.Split('`')[0];
        }

        /// <summary>
        /// Gets the <see cref="Type.Name" /> of a type but without the generic declaration.
        /// (e.g. from List`1; to List )
        /// </summary>
        public static string NameWithoutGenericParameters(this Type type)
        {
            string name = type.Name;
            return name.Split('`')[0];
        }

        public static string GlobalFullName(this Type type)
        {
            return "global::" + type.FullName;
        }
        
        public static bool IsStruct(this Type source) 
        {
            return source.IsValueType && !source.IsPrimitive && !source.IsEnum;
        }
        
        public static List<Type> GetInheritors(this Type source)
        {
	        List<Type> result = new List<Type>();
	        foreach (Type type in AppDomain.CurrentDomain.GetTypes(source))
	        {
		        if (type.IsClass && type.IsAbstract == false && type.IsSubclassOf(source))
		        {
			        result.Add(type);
		        }
	        }
	        return result;
        }
    }
}