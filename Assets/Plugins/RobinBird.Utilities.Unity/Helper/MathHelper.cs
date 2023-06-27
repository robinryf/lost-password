#region Disclaimer

// <copyright file="MathHelper.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Unity.Helper
{
    using UnityEngine;

    public static class MathHelper
    {
        /// <summary>
        /// Ensure that the angle is within -180 to 180 range.
        /// </summary>
        [System.Diagnostics.DebuggerHidden]
        [System.Diagnostics.DebuggerStepThrough]
        public static float WrapAngle180(float angle)
        {
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            return angle;
        }

        /// <summary>
        /// Wrap i around the range defined by <paramref name="min"/> and <paramref name="max"/>. So the result will
        /// always be >= min and <= max. The offset will be taken into account.
        /// Example
        /// i: 4
        /// min: 0
        /// max: 2
        /// result: 1
        /// The rest the bigger i had was added to the min of the collection
        /// </summary>
        public static int WrapInt(int i, int min, int max)
        {
            int tempMin = min;
            min = 0;
            i -= tempMin;
            max -= tempMin;
            
            int range = max - min + 1;
            if (i < min)
            {
                return max + i % range + 1 + tempMin;
            }

            if (i > max)
            {
                return min + (i % range) + tempMin;
            }

            return i + tempMin;
        }

        /// <summary>
        /// Apply <see cref="Mathf.Max(float, float)"/> to both components of the parameters
        /// </summary>
        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, a.y));
        }

        /// <summary>
        /// Apply <see cref="Mathf.Clamp(float, float, float)"/> to both both components of vector
        /// </summary>
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
        }
    }
}