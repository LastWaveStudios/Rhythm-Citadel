using UnityEngine;

namespace Utilities.ObjectPool
{
    public interface IPoolManager
    {
        //Use: PoolManager.Release(obj) where obj implements APoolableObject
        public void Release(IPoolableObject obj);
        
        //Use: Get(typeof(Type)) where Type is of a type that implements APoolableObject or IPoolableObject
        public IPoolableObject Get(System.Type objectType);

        //Use: poolManager.RegisterPool(typeof(Type), new ObjectPool<Type>()) 
        //where Type is of a type that implements APoolableObject or IPoolableObject
        public void RegisterPool(System.Type type, IObjectPool pool);
    }

}
