namespace RobinBird.Utilities.Unity.Legacy
{
    using System;

    /// <summary>
    /// Representation of <see cref="RectInt"/> but with whole numbers.
    /// </summary>
#if UNITY_2017_2_OR_NEWER
    [Obsolete("Use UnityEngine.RectInt since it comes with the engine", true)]
#endif
    public struct RectInt
    {
        private int privateX, privateY, privateWidth, privateHeight;

        public RectInt(int x, int y, int width, int height)
        {
            privateX = x;
            privateY = y;
            privateWidth = width;
            privateHeight = height;
        }
       
        public int x { get { return privateX; } }
        public int y { get { return privateY; } }

        public int width { get { return privateWidth; } }
        public int height { get { return privateHeight; } }

        public int xMin { get { return Math.Min(privateX, privateX + privateWidth); } set { int max = xMax; privateX = value; privateWidth = max - privateX; } }
        public int yMin { get { return Math.Min(privateY, privateY + privateHeight); } set { int max = yMax; privateY = value; privateHeight = max - privateY; } }
        public int xMax { get { return Math.Max(privateX, privateX + privateWidth); } set { privateWidth = value - privateX; } }
        public int yMax { get { return Math.Max(privateY, privateY + privateHeight); } set { privateHeight = value - privateY; } }

        public Vector2Int position { get { return new Vector2Int(privateX, privateY); } }
        public Vector2Int size { get { return new Vector2Int(privateWidth, privateHeight); } }
    }
}