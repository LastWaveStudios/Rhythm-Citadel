using Gameplay.Enemies;
using Gameplay.World;
using Gameplay.Waves;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;
using Utilities.ServiceLocator;
using System.Collections;
using Gameplay.RhythmSystem;
using Gameplay.Towers.Bullets;

namespace Gameplay.Towers
{
    /// <summary>
    /// This class is for have the minimun that all towers will have regardless of anything and how they work togheter with the TowersGroup,
    /// that will give the perceptions to the AI that each tower will have
    /// </summary>
    public abstract class ATower : MonoBehaviour
    {
        [SerializeField] const float PRICEMULTIPLIER = 0.7f;
        [SerializeField] const int MAX_LEVEL = 4;

        [SerializeField] protected RhythmPattern _pattern;
        [SerializeField] protected DamageType _damageType; // String, Percussion, Hybrid
        [SerializeField] protected ABullet _bulletPrefab; // Must be one that have Bullet Component  
        
        [SerializeField] protected int _groupId;
        [SerializeField] protected int _damage;   //The damage the towers can do is between two values: minDamage and maxDamage
        [SerializeField] protected int _range;
        [SerializeField] protected int _price;
        
        [SerializeField] protected float _damageMultiplier = 1.15f;
        [SerializeField] protected float _timeForProjectile = 0.1f; // Time of projectile to reach the target
        [SerializeField] protected Vector3 _bulletOffset = new Vector3(0f, 1.2f, 0f);
        
        protected bool _isEnabled = true;
        protected int _level = 1;

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
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            if (_waveManager == null)
            {
                Debug.LogError("ATower::Init: The world manager is null");
                return;
            }
            PoolInit();
        }

        protected virtual void PoolInit()
        {
            _poolManager = new PoolManager();
            _poolManager.RegisterPool<Bullet>(new ObjectPool<Bullet>(_bulletPrefab.GetComponent<Bullet>()));
        }
        #endregion

        #region Getters and setters

        public int GetPrice()
        {
            return _price;
        }

        public int GetImprovePrice()
        {
            return (int)(_price * _level * PRICEMULTIPLIER);
        }
        public int GetSellingPrice()
        {
            return (int)Mathf.Round((float)(_price * PRICEMULTIPLIER * _level));
        }
        
        public int GetDamage()
        {
            return _damage;
        }

        public bool IsMaxLevel()
        {
            return _level == MAX_LEVEL;
        }

        public int GetGroup()
        {
            return _groupId;
        }

        public RhythmPattern GetPattern()
        {
            return _pattern;
        }
        
        public void SetTile(Vector3Int tile)
        {
            _positionInWorldCell = tile;
        }

        public int GetRange()
        {
            return _range;
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
        }
        public void Attack()   //call it when the user taps correctly
        {
            List<AEnemy> enemies = _waveManager.GetEnemiesList();
            if (enemies == null || enemies.Count == 0) return;

            List<AEnemy> objectives = focusType(enemies, _positionInWorldCell, _range);
            if (objectives == null || objectives.Count == 0 || objectives[0] == null) return;

            ABullet bullet = GetFromPool();

            Vector3 from = transform.position;
            bullet.Shot(from + _bulletOffset, objectives, _timeForProjectile, _poolManager, _damageType, GetDamage());
        }

        protected virtual ABullet GetFromPool()
        {
            return _poolManager.Get<Bullet>();
        }
        public void Improve()
        {
            _level++;
            if (_level % 2 == 0) _range += (int)_level / 2;
            _damage = (int)Math.Ceiling(_damage * _damageMultiplier);
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
