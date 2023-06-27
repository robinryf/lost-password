#region Disclaimer

// <copyright file="XmlNodeExtensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime.Extensions
{
    using System.Xml;

    /// <summary>
    /// Extension and helper methods for <see cref="XmlNode" /> class.
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Returns true if the <see cref="XmlNode" /> has an attribute with a name that equals <paramref name="name" />.
        /// </summary>
        public static bool HasAttribute(this XmlNode node, string name)
        {
            return node.Attributes != null && node.Attributes[name] != null;
        }

        /// <summary>
        /// Returns true if the <see cref="XmlNode" /> has an attribute with a name that equals <paramref name="localName" />
        /// and the namespace equals <paramref name="namespaceURI" />.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static bool HasAttribute(this XmlNode node, string localName, string namespaceURI)
        {
            return node.Attributes != null && node.Attributes[localName, namespaceURI] != null;
        }
    }
}