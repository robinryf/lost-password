#region Disclaimer

// <copyright file="PivotHelper.cs">
// Copyright (c) 2018 - 2018 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Unity.Helper
{
    using System;
    using UnityEngine;

    public static class PivotHelper
    {
        /// <summary>
        /// Get the <see cref="Vector2"/> value for the pivot.
        /// </summary>
        /// <param name="alignment">The alignment to solve</param>
        /// <param name="customOffset">If the <paramref name="alignment"/> is set to <see cref="SpriteAlignment.Custom"/>
        /// this value will be returned.</param>
        public static Vector2 GetPivotValue(SpriteAlignment alignment, Vector2 customOffset)
        {
            switch (alignment)
            {
                case SpriteAlignment.Center:
                    return new Vector2(0.5f, 0.5f);
                case SpriteAlignment.TopLeft:
                    return new Vector2(0.0f, 1f);
                case SpriteAlignment.TopCenter:
                    return new Vector2(0.5f, 1f);
                case SpriteAlignment.TopRight:
                    return new Vector2(1f, 1f);
                case SpriteAlignment.LeftCenter:
                    return new Vector2(0.0f, 0.5f);
                case SpriteAlignment.RightCenter:
                    return new Vector2(1f, 0.5f);
                case SpriteAlignment.BottomLeft:
                    return new Vector2(0.0f, 0.0f);
                case SpriteAlignment.BottomCenter:
                    return new Vector2(0.5f, 0.0f);
                case SpriteAlignment.BottomRight:
                    return new Vector2(1f, 0.0f);
                case SpriteAlignment.Custom:
                    return customOffset;
                default:
                    return Vector2.zero;
            }
        }

        public static SpriteAlignment GetAlignmentFromValue(Vector2 pivotValue)
        {
            var values = (SpriteAlignment[])Enum.GetValues(typeof(SpriteAlignment));

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                if (GetPivotValue(value, pivotValue) == pivotValue)
                {
                    return value;
                }
            }
            return SpriteAlignment.Custom;
        }
    }
}