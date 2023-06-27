#region Disclaimer
// <copyright file="GuidHelper.cs">
// Copyright (c) 2017 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Runtime.Helper
{
    using System;
    using System.Text;

    public static class GuidHelper
    {
        public static Guid StringToGuid(string src)
        {
            byte[] stringbytes = Encoding.UTF8.GetBytes(src);
            byte[] hashedBytes = new System.Security.Cryptography
                    .SHA1CryptoServiceProvider()
                .ComputeHash(stringbytes);
            Array.Resize(ref hashedBytes, 16);
            return new Guid(hashedBytes);
        }
    }
}