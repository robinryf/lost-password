using System;
using Object = UnityEngine.Object;

namespace RobinBird.Utilities.Unity.Editor.Inspectors
{
    using Editor = UnityEditor.Editor;

    public abstract class GenericInspector<T> : Editor where T : Object
    {
        protected T Target
        {
            get { return (T) target; }
        }

        protected T[] Targets
        {
            get { return Array.ConvertAll(targets, t => (T) t); }
        }

        protected virtual void OnEnable()
        {
        }
    }
}