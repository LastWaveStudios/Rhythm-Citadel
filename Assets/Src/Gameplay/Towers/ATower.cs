using Gameplay.Enemies;
using Gameplay.World;
using Gameplay.Waves;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;
using Utilities.ServiceLocator;

namespace Gameplay.Towers
{
    /// <summary>
    /// This class is for have the minimun that all towers will have regardless of anything and how they work togheter with the TowersGroup,
    /// that will give the perceptions to the AI that each tower will have
    /// </summary>
    public abstract class ATower : MonoBehaviour
    {
        protected int _level;
        protected int _cost;
        protected DamageType _damageType; // String, Percussion, Hybrid
        protected int _minDamage;   //The damage the towers can do is between two values: minDamage and maxDamage
        protected int _MaxDamage;
        protected int _range;
        [SerializeField] protected Bullet _bulletPrefab; // Must be one that have Bullet Component  
        
        [SerializeField] protected double _timeForProjectile = 0.1; // Time of projectile to reach the target
        protected IPoolManager _poolManager;
        protected WaveManager _waveManager;

        [SerializeField]protected int _price = 0;
        protected Vector3Int _positionInWorldCell;

        #region Services references
        protected WorldManager _worldManager;
        #endregion

        public Func<List<AEnemy>, Vector3Int, int, List<AEnemy>> focusType;

        public int GetPrice()
        {
            return _price;
        }

        // BALANCEAR EL SELLING PRICE
        public int GetSellingPrice()
        {
            return (int)Mathf.Round((float)(_price * 0.7));
        }

        public void Improve()
        {
            // TO DO
            // DEBERIA AUMENTAR UNA STAT O ALGO ASI, 
            // DEBERIA AUMENTAR EL PRECIO - PORQUE ES LO QUE CUESTA LA MEJORA
            // A LO MEJOR UNA VARIABLE QUE GUARDE EL NIVEL
        }
        public void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(Init);
        }

        private void Init()
        {
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();
            if (_worldManager == null )
            {
                Debug.LogError("ATower::Init: The world manager is null");
                return;
            }

            _poolManager = new PoolManager();
            _poolManager.RegisterPool<Bullet>(new ObjectPool<Bullet>(_bulletPrefab.GetComponent<Bullet>()));
        }

        public abstract void Disable(); // call it when disable the tower (just for sound and animations)
        public abstract void Enable(); // call it when Enable the tower (just for sound and animations)
        public abstract void OnRhythmHit(); // The callback when the user taps correctly, not callback of this type if not correct

        public void VisualAttack(AEnemy enemy)   //call it when the user taps correctly
        {
            Debug.Log("Estamos en VISUAL ATACK");
            Bullet bullet =_poolManager.Get<Bullet>();

            Vector3 from = transform.position;
            bullet.Shot(from, enemy, 5f, _poolManager);
        }
    }

    public enum DamageType
    {
        String,
        Percussion,
        Hybrid
    }
}
