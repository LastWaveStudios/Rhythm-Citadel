using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Utilities.ObjectPool
{
    public interface IPoolableObject
    {
        bool IsActive
        {
                get;
                set;
        }
        void Reset();
    }
}