using System.Runtime.CompilerServices;

namespace RobinBird.Utilities.Runtime.Math
{
    using System;

    [Serializable]
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 zero => new Vector3(0, 0, 0);

        public float sqrMagnitude =>
	        (float) ((double) x * (double) x + (double) y * (double) y +
	                 (double) z * (double) z);
        
        public Vector3 normalized
        {
	        [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vector3.Normalize(this);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Normalize(Vector3 value)
        {
	        float num = Magnitude(value);
	        return (double) num > 9.999999747378752E-06 ? value / num : Vector3.zero;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Magnitude(Vector3 vector) => (float) Math.Sqrt((double) vector.x * (double) vector.x + (double) vector.y * (double) vector.y + (double) vector.z * (double) vector.z);

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator *(Vector3 a, float d) => new Vector3(a.x * d, a.y * d, a.z * d);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 a, float d) => new Vector3(a.x / d, a.y / d, a.z / d);

#if UNITY_2017_1_OR_NEWER
	    
	    public static Vector3 operator +(Vector3 a, UnityEngine.Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	    public static Vector3 operator -(Vector3 a, UnityEngine.Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	    public static Vector3 operator +(UnityEngine.Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	    public static Vector3 operator -(UnityEngine.Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	    
        public static implicit operator UnityEngine.Vector3(Vector3 vector)
        {
            return new UnityEngine.Vector3(vector.x, vector.y, vector.z);
        }

        public static implicit operator UnityEngine.Vector2(Vector3 vector)
        {
            return new UnityEngine.Vector2(vector.x, vector.y);
        }

        public static implicit operator Vector3(UnityEngine.Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
        
        public static implicit operator Vector3(UnityEngine.Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        public static bool operator ==(UnityEngine.Vector3 left, Vector3 right)
        {
            return left == (UnityEngine.Vector3) right;
        }

        public static bool operator !=(UnityEngine.Vector3 left, Vector3 right)
        {
            return (left == right) == false;
        }
#endif

        public bool Equals(Vector3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x.ToString()}, {nameof(y)}: {y.ToString()}, {nameof(z)}: {z.ToString()}";
        }
    }
}