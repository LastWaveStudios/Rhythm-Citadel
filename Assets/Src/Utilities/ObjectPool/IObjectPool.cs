using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities.ObjectPool
{
    public interface IObjectPool
    {
        IPoolableObject Get();
        void Release(IPoolableObject obj);

    }

}

