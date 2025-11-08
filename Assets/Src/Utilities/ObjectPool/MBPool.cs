using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;

public class MBPool<T> : IObjectPool where T : MonoBehaviour, IPoolableObject
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Stack<T> _stack = new();

    public MBPool(T prefab, int warmup = 0, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < warmup; i++)
        {
            var inst = Object.Instantiate(_prefab, _parent);
            inst.gameObject.SetActive(false);
            _stack.Push(inst);
        }
    }

    public IPoolableObject Get()
    {
        T obj = _stack.Count > 0 ? _stack.Pop() : Object.Instantiate(_prefab, _parent);
        obj.IsActive = true;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(IPoolableObject obj)
    {
        var t = (T)obj;
        t.Reset();                
        t.IsActive = false;
        t.gameObject.SetActive(false);
        _stack.Push(t);
    }
}

