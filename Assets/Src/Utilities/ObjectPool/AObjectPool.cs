using System.Collections.Generic;
using UnityEngine;

namespace Utilities.ObjectPool
{
    //Watch for the IPoolableObject, new(), the new() means all IPoolableObjects need to have an empty constructor
    //This is so the pool can generate a new object if no object is available
    //Just make sure all objects that implement IPoolableObject have a "Object"() constructor with no parameters
    public abstract class AObjectPool<T> : IObjectPool where T : IPoolableObject, new()
    {
        private List<T> _activeObjects = new List<T>();
        private Stack<T> _inactiveObjects = new Stack<T>();
        public T Get()
        {
            T obj;
            if (_inactiveObjects.Count != 0)
                obj = _inactiveObjects.Pop();
            else
                obj = new T();

            _activeObjects.Add(obj);
            //obj.Activate() //Are we gonna have an activate method on each obj orrrrrrrrrrrrrr
            return obj;
        }
        IPoolableObject IObjectPool.Get() => Get();
        public void Release(T obj)
        {
            obj.Reset();
            _activeObjects.Remove(obj);
            _inactiveObjects.Push(obj);
        }
        void IObjectPool.Release(IPoolableObject obj) => Release((T)obj);
    }

}
