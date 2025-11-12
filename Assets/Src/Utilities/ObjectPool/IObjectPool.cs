

namespace Utilities.ObjectPool
{
    public interface IObjectPool
    {
        IPoolableObject Get();
        void Release(IPoolableObject obj);

    }

}

