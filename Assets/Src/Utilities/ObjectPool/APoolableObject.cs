using System;
using UnityEngine;

namespace Utilities.ObjectPool
{
    public abstract class APoolableObject : IPoolableObject
    {
        private bool _isActive;
        public bool IsActive()
        {
            return _isActive;
        }
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
