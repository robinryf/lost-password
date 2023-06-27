#region Disclaimer

// <copyright file="Vector3Extensions.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Unity.Extensions
{
    using UnityEngine;

    /// <summary>
    /// Extension and helper methods for <see cref="Vector3" /> class.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Returns the length of the shortest Vector between <paramref name="point" /> and the line provided by
        /// <paramref name="lineStart" />
        /// and <paramref name="lineEnd" />
        /// </summary>
        public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
        }

        /// <summary>
        /// Returns the shortest Vector between <paramref name="point" /> and the line provided by <paramref name="lineStart" />
        /// and <paramref name="lineEnd" />
        /// </summary>
        public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 rhs = point - lineStart;
            Vector3 vector2 = lineEnd - lineStart;
            float magnitude = vector2.magnitude;
            Vector3 lhs = vector2;
            if (magnitude > 1E-06f)
            {
                lhs = (Vector3) (lhs / magnitude);
            }
            float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
            return (lineStart + ((Vector3) (lhs * num2)));
        }
    }
}