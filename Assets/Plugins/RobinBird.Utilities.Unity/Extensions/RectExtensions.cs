#region Disclaimer
// <copyright file="RechtExtensions.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion
namespace RobinBird.Utilities.Unity.Extensions
{
    using UnityEngine;

    public static class RectExtensions
    {
        public static Rect Shrink(this Rect rect, float shrinkValue)
        {
            return MoveBorders(rect, new Vector2(-shrinkValue, -shrinkValue));
        }

        public static Rect Grow(this Rect rect, float growValue)
        {
            return MoveBorders(rect, new Vector2(growValue, growValue));
        }

        private static Rect MoveBorders(Rect rect, Vector2 value)
        {
            return new Rect(rect.x - value.x, rect.y - value.y, rect.width + value.x * 2, rect.height + value.y * 2);
        }
    }
}