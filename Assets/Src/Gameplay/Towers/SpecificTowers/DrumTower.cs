using Gameplay.Towers.Bullets;
using UnityEngine;
using Utilities.ObjectPool;

namespace Gameplay.Towers.SpecificTowers
{
    public class DrumTower : ATower
    {

        private SpriteRenderer spriteRenderer;  //Prevent towers from overlapping incorrectly
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            focusType = FocusStrategies.AreaAttack;
        }

        protected override void PoolInit()
        {
            _poolManager = new PoolManager();
            _poolManager.RegisterPool<AreaBullet>(new ObjectPool<AreaBullet>(_bulletPrefab.GetComponent<AreaBullet>()));
        }

        protected override ABullet GetFromPool()
        {
            return _poolManager.Get<AreaBullet>();
        }
    }
}

