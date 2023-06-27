#region Disclaimer
// <copyright file="GizmosHelper.cs">
// Copyright (c) 2017 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Unity.Helper
{
    using Runtime.Extensions;
    using UnityEngine;

    /// <summary>
    /// Helper methods for the <see cref="Gizmos"/> class.
    /// </summary>
    public static class GizmosHelper
    {
        /// <summary>
        /// Show a Gizmo representation for Collider2D shapes. Mainly uses the <see cref="UnityEditor.Handles"/> class.
        /// </summary>
        /// <param name="position">Position of the Gizmo.</param>
        /// <param name="collider">Collider to show Gizmo of.</param>
        public static void DrawCollider2DGizmo(Vector3 position, Collider2D collider)
        {
#if UNITY_EDITOR
            CircleCollider2D circleCollider2D;
            BoxCollider2D boxCollider2D;
            PolygonCollider2D polygonCollider2D;

            if (collider.TryCast(out circleCollider2D))
            {
                UnityEditor.Handles.DrawWireDisc(position, Vector3.back, circleCollider2D.radius);
            }
            else if (collider.TryCast(out boxCollider2D))
            {
                Vector3 halfSize = new Vector3(boxCollider2D.size.x * 0.5f, boxCollider2D.size.y * 0.5f);
                Rect rect = new Rect(position - halfSize, boxCollider2D.size);

                Vector3 p1 = new Vector3(rect.xMin, rect.yMin);
                Vector3 p2 = new Vector3(rect.xMax, rect.yMin);
                Vector3 p3 = new Vector3(rect.xMax, rect.yMax);
                Vector3 p4 = new Vector3(rect.xMin, rect.yMax);

                UnityEditor.Handles.DrawPolyLine(p1, p2, p3, p4, p1);
            }
            else if (collider.TryCast(out polygonCollider2D))
            {
                Vector3[] polyLine = new Vector3[polygonCollider2D.GetTotalPointCount() + 1];
                for (int i = 0; i < polygonCollider2D.GetTotalPointCount(); i++)
                {
                    Vector3 point = polygonCollider2D.points[i];
                    polyLine[i] = point + position;
                }
                polyLine[polyLine.Length-1] = polyLine[0];

                UnityEditor.Handles.DrawPolyLine(polyLine);
            }
#endif
        }
    }
}