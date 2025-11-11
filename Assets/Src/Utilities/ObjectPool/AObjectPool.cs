using System.Collections.Generic;
using UnityEngine;

namespace Utilities.ObjectPool
{
    public abstract class AObjectPool<T> : IObjectPool where T : MonoBehaviour, IPoolableObject
    {
        private readonly T _prefab;
        private Transform _parent;
        private List<T> _activeObjects = new List<T>();
        private Stack<T> _inactiveObjects = new Stack<T>();
        protected AObjectPool(T prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
        }
        public T Get()
        {
            T obj;
            if (_inactiveObjects.Count != 0)
            {
                obj = _inactiveObjects.Pop();
                obj.gameObject.SetActive(true);
            }
            else
                obj = Object.Instantiate(_prefab, _parent);

            _activeObjects.Add(obj);
            //obj.Activate() //Are we gonna have an activate method on each obj orrrrrrrrrrrrrr
            return obj;
        }
        IPoolableObject IObjectPool.Get() => Get();
        public void Release(T obj)
        {
            obj.Reset();
            obj.gameObject.SetActive(false);
            _activeObjects.Remove(obj);
            _inactiveObjects.Push(obj);
        }

        void IObjectPool.Release(IPoolableObject obj) => Release((T)obj);
    }

}
