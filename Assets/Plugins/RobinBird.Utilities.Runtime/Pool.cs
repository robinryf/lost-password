#region Disclaimer

// <copyright file="TalosPool.cs">
// Copyright (c) 2016 - 2017 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>

#endregion

namespace RobinBird.Utilities.Runtime
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pool to cache a collection of objects of type <typeparamref name="T" />. The developer
    /// can get or add objects to the pool. You must supply a create method so the pool can
    /// create new objects if required.
    /// </summary>
    /// <typeparam name="T">Type of the objects to pool</typeparam>
    public class Pool<T>
    {
        private readonly Stack<T> stack;
        private readonly Func<object,T> createFunc;
        private readonly Action<T, object> resetAction;
        private readonly object referenceObject;

        /// <summary>
        /// Constructor for pool
        /// </summary>
        /// <param name="createFunc">Method to create pooled object.</param>
        /// <param name="resetAction">Method to call when object is returned to pool and has to be resetted.</param>
        public Pool(Func<object,T> createFunc, Action<T,object> resetAction)
        {
            if (createFunc == null)
            {
                throw new ArgumentNullException("createFunc", "Create method cannot be null");
            }
            this.createFunc = createFunc;
            this.resetAction = resetAction;

            stack = new Stack<T>();
        }
        
        public Pool(Func<T> createFunc, Action<T> resetAction) : 
            this(o => createFunc(), (pooledObject, context) => resetAction(pooledObject))
        {
        }

        /// <summary>
        /// Constructor for pool
        /// </summary>
        /// <param name="referenceObject">Object that is passed with create and reset callbacks to use. Can be handy for creation on parent object.</param>
        /// <param name="createFunc">Method to create pooled object.</param>
        /// <param name="resetAction">Method to call when object is returned to pool and has to be resetted.</param>
        public Pool(Func<object, T> createFunc, Action<T, object> resetAction, object referenceObject)
            : this(createFunc, resetAction)
        {
            this.referenceObject = referenceObject;
        }

        public int PoolSize
        {
            get { return stack.Count; }
        }

        /// <summary>
        /// Add item to the pool.
        /// </summary>
        public virtual void AddToPool(T item)
        {
            if (item == null)
            {
                return;
            }
            if (resetAction != null)
            {
                resetAction(item, referenceObject);
            }
            stack.Push(item);
        }

        /// <summary>
        /// Returns a item from the pool.
        /// If the pool is empty a new item will be created.
        /// </summary>
        public virtual T GetFromPool()
        {
            if (stack.Count == 0)
            {
                return createFunc(referenceObject);
            }

            return stack.Pop();
        }
    }
}