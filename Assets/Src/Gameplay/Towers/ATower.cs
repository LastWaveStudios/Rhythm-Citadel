using Gameplay.Enemies;
using Gameplay.World;
using Gameplay.Waves;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;
using Utilities.ServiceLocator;
using System.Collections;

namespace Gameplay.Towers
{
    /// <summary>
    /// This class is for have the minimun that all towers will have regardless of anything and how they work togheter with the TowersGroup,
    /// that will give the perceptions to the AI that each tower will have
    /// </summary>
    public abstract class ATower : MonoBehaviour
    {
        [SerializeField] const float PRICEMULTIPLIER = 0.7f;
        [SerializeField] const int MAX_LEVEL = 2;

        [SerializeField] protected int _level;
        [SerializeField] protected DamageType _damageType; // String, Percussion, Hybrid
        [SerializeField] protected int _minDamage;   //The damage the towers can do is between two values: minDamage and maxDamage
        [SerializeField] protected int _MaxDamage;
        [SerializeField] protected int _range;
        [SerializeField] protected Bullet _bulletPrefab; // Must be one that have Bullet Component  
        [SerializeField] protected int _price;
        [SerializeField] protected float _timeForProjectile = 0.1f; // Time of projectile to reach the target
        protected bool _isEnabled = true;

        protected IPoolManager _poolManager;
        protected Vector3Int _positionInWorldCell;

        #region Services references
        protected WaveManager _waveManager;
        protected WorldManager _worldManager;
        #endregion

        public Func<List<AEnemy>, Vector3Int, int, List<AEnemy>> focusType;

        #region Starting methods
        public void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(Init);
        }

        private void Init()
        {
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();
            if (_worldManager == null)
            {
                Debug.LogError("ATower::Init: The world manager is null");
                return;
            }

            _poolManager = new PoolManager();
            _poolManager.RegisterPool<Bullet>(new ObjectPool<Bullet>(_bulletPrefab.GetComponent<Bullet>()));
        }
        #endregion

        #region Getters and setters

        public int GetPrice()
        {
            return _price;
        }

        public int GetSellingPrice()
        {
            return (int)Mathf.Round((float)(_price * PRICEMULTIPLIER * _level));
        }
        
        public int GetDamage()
        {
            if (_level == 1) return _minDamage;
            else return _MaxDamage;
        }

        public bool IsMaxLevel()
        {
            return _level == MAX_LEVEL;
        }
        #endregion

        #region Other methods
        public virtual void Disable() // call it when disable the tower (just for sound and animations)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            _isEnabled = false;
            sprite.color = Color.red;
        }
        public virtual void Enable() // call it when Enable the tower (just for sound and animations)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            _isEnabled = true;
            sprite.color = Color.white;
        }
        public virtual void OnRhythmHit() // The callback when the user taps correctly, not callback of this type if not correct
        {
            Attack();
            StartCoroutine(TestThing());
        }
        public void Attack()   //call it when the user taps correctly
        {
            List<AEnemy> enemies = _waveManager.GetEnemiesList();
            if (enemies == null || enemies.Count == 0) return;

            List<AEnemy> objectives = focusType(enemies, _positionInWorldCell, _range);
            if (objectives == null || objectives.Count == 0) return;

            Bullet bullet = _poolManager.Get<Bullet>();

            Vector3 from = transform.position;
            bullet.Shot(from, objectives, _timeForProjectile, _poolManager, _damageType, GetDamage());
        }
        public void Improve()
        {
            _level++;
            _price = (int)(PRICEMULTIPLIER * _price);
        }
        #endregion

        #region Corutines

        private IEnumerator TestThing()
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;

            float t = 0.0f;
            while (t < 0.25f)
            {
                t += Time.deltaTime;
                yield return null;
            }
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        #endregion

    }

}
