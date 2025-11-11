using UnityEngine;
using Utilities.ObjectPool;

public class ObjectPool<T> : AObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    public ObjectPool(T prefab, Transform parent = null) : base(prefab, parent) { }
}
