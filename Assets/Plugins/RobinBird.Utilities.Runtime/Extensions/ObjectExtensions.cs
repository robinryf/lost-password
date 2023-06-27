namespace RobinBird.Utilities.Runtime.Extensions
{
    /// <summary>
    /// Extensions for <see cref="object" /> struct.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Try to cast an object to desired type. Out variable is != null on success.
        /// </summary>
        /// <typeparam name="T">Type you want to cast to.</typeparam>
        /// <param name="obj">The object that should be casted.</param>
        /// <param name="result">Result of the cast.</param>
        public static bool TryCast<T>(this object obj, out T result) where T : class
        {
            result = obj as T;
            return result != null;
        }
    }
}