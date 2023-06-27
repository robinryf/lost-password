namespace RobinBird.Utilities.Runtime.Math
{
    using System;

    [Serializable]
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

#if UNITY_2017_1_OR_NEWER
        public static implicit operator UnityEngine.Vector2(Vector2 vector)
        {
            return new UnityEngine.Vector2(vector.x, vector.y);
        }

        public static implicit operator Vector2(UnityEngine.Vector2 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static bool operator ==(UnityEngine.Vector2 left, Vector2 right)
        {
            return left == (UnityEngine.Vector2) right;
        }

        public static bool operator !=(UnityEngine.Vector2 left, Vector2 right)
        {
            return (left == right) == false;
        }
#endif

        public bool Equals(Vector2 other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x.ToString()}, {nameof(y)}: {y.ToString()}";
        }
    }
}