#region Disclaimer

// <copyright file="Vector4Int.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

// ReSharper disable InconsistentNaming
namespace RobinBird.Utilities.Runtime.Math
{
    using System;

    [Serializable]
    public struct Vector4Int
    {
        /// <summary>
        ///     <para>Shorthand for writing Vector4Int (0, 0, 0, 0).</para>
        /// </summary>
        public static readonly Vector4Int zero = new Vector4Int(0, 0, 0, 0);
        
        /// <summary>
        ///     <para>Shorthand for writing Vector4Int (1, 1, 1, 1).</para>
        /// </summary>
        public static readonly Vector4Int one = new Vector4Int(1, 1, 1, 1);

        public int x;
        public int y;
        public int z;
        public int w;


        public Vector4Int(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector4Int index addressed: {0}!", (object) index));
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector4Int index addressed: {0}!", (object) index));
                }
            }
        }

        /// <summary>
        ///     <para>Set x, y and z components of an existing Vector4Int.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Set(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        ///     <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector4Int Min(Vector4Int lhs, Vector4Int rhs)
        {
            return new Vector4Int(Math.Min(lhs.x,
                    rhs.x),
                Math.Min(lhs.y,
                    rhs.y),
                Math.Min(lhs.z,
                    rhs.z),
                Math.Min(lhs.w,
                    rhs.w));
        }

        /// <summary>
        ///     <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector4Int Max(Vector4Int lhs, Vector4Int rhs)
        {
            return new Vector4Int(Math.Max(lhs.x,
                    rhs.x),
                Math.Max(lhs.y,
                    rhs.y),
                Math.Max(lhs.z,
                    rhs.z),
                Math.Max(lhs.w,
                    rhs.w));
        }

        /// <summary>
        ///     <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static Vector4Int Scale(Vector4Int a, Vector4Int b)
        {
            return new Vector4Int(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        /// <summary>
        ///     <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector4Int scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
            w *= scale.w;
        }

        /// <summary>
        ///     <para>Clamps the Vector4Int to the bounds given by min and max.</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Clamp(Vector4Int min, Vector4Int max)
        {
            x = Math.Max(min.x, x);
            x = Math.Min(max.x, x);
            y = Math.Max(min.y, y);
            y = Math.Min(max.y, y);
            z = Math.Max(min.z, z);
            z = Math.Min(max.z, z);
            w = Math.Max(min.w, w);
            w = Math.Min(max.w, w);
        }

#if UNITY_2017_1_OR_NEWER
        public static implicit operator UnityEngine.Vector4(Vector4Int v)
        {
            return new UnityEngine.Vector4((float) v.x, (float) v.y, (float) v.z, v.w);
        }

        /// <summary>
        ///     <para>Converts a  Vector3 to a Vector4Int by doing a Floor to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static Vector4Int FloorToInt(UnityEngine.Vector4 v)
        {
            return new Vector4Int((int)Math.Floor(v.x),
                (int)Math.Floor(v.y),
                (int)Math.Floor(v.z),
                (int)Math.Floor(v.w));
        }

        /// <summary>
        ///     <para>Converts a  Vector3 to a Vector4Int by doing a Ceiling to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static Vector4Int CeilToInt(UnityEngine.Vector4 v)
        {
            return new Vector4Int((int)Math.Ceiling(v.x),
                (int)Math.Ceiling(v.y),
                (int)Math.Ceiling(v.z),
                (int)Math.Ceiling(v.w));
        }

        /// <summary>
        ///     <para>Converts a  Vector3 to a Vector4Int by doing a Round to each value.</para>
        /// </summary>
        /// <param name="v"></param>
        public static Vector4Int RoundToInt(UnityEngine.Vector4 v)
        {
            return new Vector4Int((int)Math.Round(v.x),
                (int)Math.Round(v.y),
                (int)Math.Round(v.z),
                (int)Math.Round(v.w));
        }
#endif

        public static Vector4Int operator +(Vector4Int a, Vector4Int b)
        {
            return new Vector4Int(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Vector4Int operator -(Vector4Int a, Vector4Int b)
        {
            return new Vector4Int(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Vector4Int operator *(Vector4Int a, Vector4Int b)
        {
            return new Vector4Int(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        public static Vector4Int operator *(Vector4Int a, int b)
        {
            return new Vector4Int(a.x * b, a.y * b, a.z * b, a.w * b);
        }

        public static bool operator ==(Vector4Int lhs, Vector4Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(Vector4Int lhs, Vector4Int rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///     <para>Returns true if the objects are equal.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            if (!(other is Vector4Int))
                return false;
            return this == (Vector4Int) other;
        }

        /// <summary>
        ///     <para>Gets the hash code for the Vector4Int.</para>
        /// </summary>
        /// <returns>
        ///     <para>The hash code of the Vector4Int.</para>
        /// </returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2 ^ w.GetHashCode() >> 1;
        }

        /// <summary>
        ///     <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", (object) x, (object) y, (object) z, w);
        }

        /// <summary>
        ///     <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2}, {3})", (object) x.ToString(format), (object) y.ToString(format), (object) z.ToString(format), w.ToString(format));
        }
    }
}