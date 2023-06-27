namespace RobinBird.Utilities.Runtime.Helper
{
    using System;
    using System.Collections.Generic;

    public struct DisposableDictionary<TKey, TValue> : IDisposable
    {
        private Dictionary<TKey, TValue> Dictionary { get; set; }

        public DisposableDictionary(Dictionary<TKey, TValue> dictionary) : this()
        {
            Dictionary = dictionary;
        }
        
        public void Dispose()
        {
            Dictionary.Clear();
        }

        public void Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
        }
        
        public static implicit operator Dictionary<TKey, TValue> (DisposableDictionary<TKey, TValue> disposableDictionary)
        {
            return disposableDictionary.Dictionary;
        }
    }
}