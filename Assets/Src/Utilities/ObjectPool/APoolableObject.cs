using UnityEngine;

namespace Utilities.ObjectPool
{
    public abstract class APoolableObject : MonoBehaviour, IPoolableObject
    {
        private bool _isActive;
        public bool IsActive()
        {
            return _isActive;
        }
        public abstract void Reset();
    }
}
