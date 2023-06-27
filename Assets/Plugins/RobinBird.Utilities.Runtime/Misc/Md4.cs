#region Disclaimer
// <copyright file="MD4.cs">
// Copyright (c) 2017 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Runtime.Misc
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Security.Cryptography;

   // Taken from http://www.superstarcoders.com/blogs/posts/md4-hash-algorithm-in-c-sharp.aspx
   // Probably not the best implementation of MD4, but it works.
   public class Md4 : HashAlgorithm
   {
      private uint a;
      private uint b;
      private uint c;
      private uint d;
      private readonly uint[] x;
      private int bytesProcessed;
    
      public Md4()
      {
         x = new uint[16];
    
         Reset();
      }

      private void Reset()
      {
         a = 0x67452301;
         b = 0xefcdab89;
         c = 0x98badcfe;
         d = 0x10325476;
    
         bytesProcessed = 0;
      }
    
      public override void Initialize()
      {
         Reset();
      }
    
      protected override void HashCore(byte[] array, int offset, int length)
      {
         ProcessMessage(Bytes(array, offset, length));
      }
    
      protected override byte[] HashFinal()
      {
         try
         {
            ProcessMessage(Padding());
    
            return new [] {a, b, c, d}.SelectMany(word => Bytes(word)).ToArray();
         }
         finally
         {
            Initialize();
         }
      }
    
      private void ProcessMessage(IEnumerable<byte> bytes)
      {
         foreach (byte b in bytes)
         {
            int c = bytesProcessed & 63;
            int i = c >> 2;
            int s = (c & 3) << 3;
      
            x[i] = (x[i] & ~((uint)255 << s)) | ((uint)b << s);
      
            if (c == 63)
            {
               Process16WordBlock();
            }
      
            bytesProcessed++;
         }
      }
    
      private static IEnumerable<byte> Bytes(byte[] bytes, int offset, int length)
      {
         for (int i = offset; i < length; i++)
         {
            yield return bytes[i];
         }
      }
    
      private IEnumerable<byte> Bytes(uint word)
      {
         yield return (byte)(word & 255);
         yield return (byte)((word >> 8) & 255);
         yield return (byte)((word >> 16) & 255);
         yield return (byte)((word >> 24) & 255);
      }
    
      private IEnumerable<byte> Repeat(byte value, int count)
      {
         for (int i = 0; i < count; i++)
         {
            yield return value;
         }
      }
    
      private IEnumerable<byte> Padding()
      {
         return Repeat(128, 1)
            .Concat(Repeat(0, ((bytesProcessed + 8) & 0x7fffffc0) + 55 - bytesProcessed))
            .Concat(Bytes((uint)bytesProcessed << 3))
            .Concat(Repeat(0, 4));
      }
    
      private void Process16WordBlock()
      {
         uint aa = a;
         uint bb = b;
         uint cc = c;
         uint dd = d;
    
         foreach (int k in new [] { 0, 4, 8, 12 })
         {
            aa = Round1Operation(aa, bb, cc, dd, x[k], 3);
            dd = Round1Operation(dd, aa, bb, cc, x[k + 1], 7);
            cc = Round1Operation(cc, dd, aa, bb, x[k + 2], 11);
            bb = Round1Operation(bb, cc, dd, aa, x[k + 3], 19);
         }
    
         foreach (int k in new [] { 0, 1, 2, 3 })
         {
            aa = Round2Operation(aa, bb, cc, dd, x[k], 3);
            dd = Round2Operation(dd, aa, bb, cc, x[k + 4], 5);
            cc = Round2Operation(cc, dd, aa, bb, x[k + 8], 9);
            bb = Round2Operation(bb, cc, dd, aa, x[k + 12], 13);
         }
    
         foreach (int k in new [] { 0, 2, 1, 3 })
         {
            aa = Round3Operation(aa, bb, cc, dd, x[k], 3);
            dd = Round3Operation(dd, aa, bb, cc, x[k + 8], 9);
            cc = Round3Operation(cc, dd, aa, bb, x[k + 4], 11);
            bb = Round3Operation(bb, cc, dd, aa, x[k + 12], 15);
         }
    
         unchecked
         {
            a += aa;
            b += bb;
            c += cc;
            d += dd;
         }
      }
    
      // ReSharper disable once InconsistentNaming
      private static uint ROL(uint value, int numberOfBits)
      {
         return (value << numberOfBits) | (value >> (32 - numberOfBits));
      }
    
      private static uint Round1Operation(uint a, uint b, uint c, uint d, uint xk, int s)
      {
         unchecked
         {
            return ROL(a + ((b & c) | (~b & d)) + xk, s);
         }
      }
    
      private static uint Round2Operation(uint a, uint b, uint c, uint d, uint xk, int s)
      {
         unchecked
         {
            return ROL(a + ((b & c) | (b & d) | (c & d)) + xk + 0x5a827999, s);
         }
      }
    
      private static uint Round3Operation(uint a, uint b, uint c, uint d, uint xk, int s)
      {
         unchecked
         {
            return ROL(a + (b ^ c ^ d) + xk + 0x6ed9eba1, s);
         }
      }
   }
}