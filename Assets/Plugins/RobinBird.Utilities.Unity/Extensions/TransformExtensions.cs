namespace RobinBird.Utilities.Unity.Extensions
{
    using UnityEngine;

    /// <summary>
    /// Extension and helper methods for <see cref="GameObject" /> class.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Create and add new <see cref="GameObject" /> to transform of <paramref name="transform" />.
        /// </summary>
        /// <param name="transform"><see cref="Transform" /> that is the parent.</param>
        /// <param name="name">The name of the new <see cref="GameObject" />.</param>
        /// <returns>The created <see cref="GameObject" />.</returns>
        public static GameObject AddChild(this Transform transform, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(transform.transform);
            child.transform.Reset();
            return child;
        }

        /// <summary>
        /// Create and add new <see cref="GameObject" /> to transform of <paramref name="transform" />.
        /// </summary>
        /// <param name="transform"><see cref="Transform" /> that is the parent.</param>
        /// <param name="prefab">Prefab that is used to create the new child.</param>
        /// <param name="name">The name of the new <see cref="GameObject" />.</param>
        /// <param name="resetTransform">Pass true if the transform should be reset to default values. <see cref="Reset" /></param>
        /// <returns>The created <see cref="GameObject" />.</returns>
        public static GameObject AddChild(this Transform transform, GameObject prefab, string name, bool resetTransform = true)
        {
            GameObject newGameObject = AddChild(transform, prefab, resetTransform);
            newGameObject.name = name;
            return newGameObject;
        }

        /// <summary>
        /// Create and add new <see cref="GameObject" /> to transform of <paramref name="transform" />.
        /// </summary>
        /// <param name="transform"><see cref="Transform" /> that is the parent.</param>
        /// <param name="prefab">Prefab that is used to create the new child.</param>
        /// <param name="resetTransform">Pass true if the transform should be reset to default values. <see cref="Reset" /></param>
        /// <returns>The created <see cref="GameObject" />.</returns>
        public static GameObject AddChild(this Transform transform, GameObject prefab, bool resetTransform = true)
        {
            GameObject child = Object.Instantiate(prefab);
            child.transform.SetParent(transform);
            if (resetTransform)
            {
                child.transform.Reset();
            }
            return child;
        }

        public static void DestroyChilds(this Transform transform)
        {
            int transformChildCount = transform.childCount;
            for (int i = transformChildCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);

                if (Application.isPlaying)
                {
                    Object.Destroy(child.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Reset position, rotation, scale of the transform to default values.
        /// </summary>
        /// <param name="transform">The transform to reset.</param>
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Get or Add an component. This is usefull to get an component from a gameobject regardless if it already exists
        /// or hast to be created.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="obj"><see cref="Transform" /> of the <see cref="GameObject" /> to get or add the component from.</param>
        /// <returns>The get or added component.</returns>
        public static T GetOrAddComponent<T>(this Transform obj) where T : Component
        {
            var comp = obj.GetComponent<T>();
            if (comp != null)
                return comp;
            return obj.gameObject.AddComponent<T>();
        }
    }
}