using System;

namespace RobinBird.Utilities.Unity.Legacy
{
    using UnityEngine;

    /// <summary>
    /// Representation of <see cref="Vector2"/> but with whole numbers
    /// </summary>
#if UNITY_2017_2_OR_NEWER
    [Obsolete("Use UnityEngine.Vector2Int since it comes with the engine", true)]
#endif
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        private readonly int privateX;
        private readonly int privateY;


        public int x { get { return privateX; } }
        public int y { get { return privateY; } }


        public Vector2Int(int x, int y)
        {
            privateX = x;
            privateY = y;
        }

        public static implicit operator Vector2(Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2Int operator+(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public static Vector2Int operator-(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        public static Vector2Int operator*(Vector2Int a, int b)
        {
            return new Vector2Int(a.x * b, a.y * b);
        }
        
        public static Vector2Int operator*(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }

        public static bool operator==(Vector2Int lhs, Vector2Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator!=(Vector2Int lhs, Vector2Int rhs)
        {
            return (lhs == rhs) == false;
        }

        public override bool Equals(object other)
        {
            return (other is Vector2Int) && Equals((Vector2Int)other);
        }

        public bool Equals(Vector2Int other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (privateX * 397) ^ privateY;
            }
        }
    }
}
