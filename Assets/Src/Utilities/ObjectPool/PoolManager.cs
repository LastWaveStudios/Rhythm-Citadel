using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Gameplay.World;
namespace Utilities.ObjectPool
{
    public class PoolManager: MonoBehaviour, IPoolManager
    {
        private Dictionary<System.Type, IObjectPool> _pools = new();
        [SerializeField] private Bullets bulletPrefab;

        void Awake()
        {
            var bulletPool = new MBPool<Bullets>(bulletPrefab, 20, transform);
            RegisterPool(typeof(Bullets), bulletPool);
        }

        public void Release(IPoolableObject obj)
        {
            var objectType = obj.GetType();
            if (_pools.TryGetValue(objectType, out var pool))
                pool.Release(obj);
            else
                Debug.LogError($"No registered pool for object type {objectType.Name}");
        }

        public IPoolableObject Get(System.Type objectType)
        {
            if (_pools.TryGetValue(objectType, out var pool))
            {
                return pool.Get();
            }
            Debug.LogError($"No registered pool for object type {objectType.Name}");
            return null;
        }

        //To register -> poolManager.RegisterPool(typeof(Type), new ObjectPool<Type>());
        public void RegisterPool(System.Type type, IObjectPool pool)
        {
            if (!typeof(IPoolableObject).IsAssignableFrom(type))
            {
                //This should never happen, just remember to make pools of objects that implement IPoolableObject
                Debug.LogError($"Type {type.Name} of attempted pool object does not implement IPoolableObject");
                return;
            }

            _pools[type] = pool;
        }
        
    }
 
}
