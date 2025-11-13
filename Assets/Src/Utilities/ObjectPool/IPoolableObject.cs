
namespace Utilities.ObjectPool
{
    public interface IPoolableObject
    {
        public bool IsActive();
        void Reset();
    }
}