#region Disclaimer
// <copyright file="UnityDllHelper.cs">
// Copyright (c) 2017 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Unity.Helper
{
   using System;
   using System.Security.Cryptography;
   using Runtime.Misc;

   /// <summary>
   /// Helper class to ease the use of DLLs inside the Unity Assets directory
   /// </summary>
   public static class UnityDllHelper
    {
        /// <summary>
        /// Unity saves a fileID for every referencable class inside the DLL. This fileID is created from the
        /// Types namespace + type name. This method returns the fileId based on given type information
        /// </summary>
        /// <param name="namespace">The type namespace to calcualte the fileID from.</param>
        /// <param name="typeName">The type name to calcualte the fileID from.</param>
        public static int ConvertDllTypeToFileId(string @namespace, string typeName)
       {
           string toBeHashed = "s\0\0\0" + @namespace + typeName;
 
           using (HashAlgorithm hash = new Md4())
           {
               byte[] hashed = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toBeHashed));
 
               int result = 0;
 
               for(int i = 3; i >= 0; --i)
               {
                   result <<= 8;
                   result |= hashed[i];
               }
 
               return result;
           }
       }
       
        /// <summary>
        /// Unity saves a fileID for every referencable class inside the DLL. This fileID is created from the
        /// Types namespace + type name. This method returns the fileId based on given type information
        /// </summary>
        /// <param name="t">The type to calcualte the fileID from.</param>
       public static int ConvertDllTypeToFileId(Type t)
       {
           return ConvertDllTypeToFileId(t.Namespace, t.Name);
       }
    }
}