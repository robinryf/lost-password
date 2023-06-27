#region Disclaimer

// <copyright file="AppDomainExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extension and helper methods for <see cref="AppDomain" /> class.
    /// </summary>
    public static class AppDomainExtensions
    {
        /// <summary>
        /// Get a single type from the <see cref="AppDomain" />.
        /// </summary>
        public static Type GetType(this AppDomain domain, string typeName)
        {
            return GetType(domain, typeName, false);
        }

        /// <summary>
        /// Get a single type from the <see cref="AppDomain" />.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="typeName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static Type GetType(this AppDomain domain, string typeName, bool ignoreCase)
        {
            Assembly[] assemblies = domain.GetAssemblies();

            for (var i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];

                if (assembly == null)
                    continue;

                Type type = assembly.GetType(typeName, false, ignoreCase);

                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Get types that inherit from <paramref name="baseType" /> from the <see cref="AppDomain" />.
        /// Does not include the <paramref name="baseType" /> itself.
        /// </summary>
        /// <returns>Collection of found types.</returns>
        public static Type[] GetTypes(this AppDomain domain, Type baseType)
        {
            Assembly[] assemblies = domain.GetAssemblies();
            var types = new List<Type>();

            for (var i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];

                Type[] loadedTypes;
                try
                {
                    loadedTypes = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    loadedTypes = ex.Types;
                }
                types.AddRange(loadedTypes.Where(t => t != baseType && baseType.IsAssignableFrom(t)));
            }

            return types.ToArray();
        }
    }
}