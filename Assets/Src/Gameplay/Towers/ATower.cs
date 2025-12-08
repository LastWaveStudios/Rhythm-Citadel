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
        const float PRICEMULTIPLIER = 0.7f;
        const int MAX_LEVEL = 4;
        const int RANGEIMPROVEMENT = 1;

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

        [Header("ShootAnimation")]
        [SerializeField] protected float _timeOfAnimation = 0.5f;
        [SerializeField] protected float _scaleFactorForShoot = 0.9f;
        [SerializeField] protected float _scaleFactorForFail = 1.2f;
        
        private Vector3 _originScale;
        private Vector3 _offsetScale;
        
        
        protected bool _isEnabled = true;
        protected int _level = 1;

        protected IPoolManager _poolManager;
        protected Vector3Int _positionInWorldCell;

        [SerializeField]
        private List<Sprite> frames = new List<Sprite>();
        private int currentFrame = 0;
        private SpriteRenderer sprite;

        #region Services references
        protected WaveManager _waveManager;
        protected WorldManager _worldManager;
        protected RhythmManager _rhythmManager;
        #endregion

        public Func<List<AEnemy>, Vector3Int, int, List<AEnemy>> focusType;

        #region Starting methods
        public void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(Init);
            sprite = GetComponent<SpriteRenderer>();
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
            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("ATower::Init: The rhythm manager is null");
                return;
            }
            _rhythmManager.onBeat += StepOneFrame;
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
        
        public int GetLevel()
        {
            return _level;
        }

        public int GetDamage()
        {
            return _damage;
        }

        public int GetImprovedDamage()
        {
            return (int)Math.Ceiling(_damage * _damageMultiplier);
        }

        public int GetAttacksPerMeasure()
        {
            return _pattern.GetNotes();
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

        public int GetImprovedRange()
        {
            if (_level % 2 == 0) return (_range + RANGEIMPROVEMENT);
            else return _range;
        }
        #endregion

        #region Other methods
        public virtual void Disable() // call it when disable the tower (just for sound and animations)
        {
            //SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            _isEnabled = false;
            sprite.color = Color.red;
            _originScale = transform.localScale;
            _offsetScale = new Vector3(_originScale.x * _scaleFactorForFail, _originScale.y, _originScale.z);
        }
        public virtual void WhileDisabled(float t, float T)
        {
            sprite.color = Color.Lerp(Color.red, Color.white, T);
            // Scale animation, scale in x
            //transform.localScale = Vector3.Lerp(_originScale, _offsetScale, Utilities.EasingFunctions.NormalizeParabolaNotConvex(t));
            float sT = Mathf.Cos(10.0f * Mathf.PI * t + Mathf.PI) * 0.5f + 0.5f;
            transform.localScale = transform.localScale = Vector3.Lerp(_originScale, _offsetScale, sT);
        }
        public virtual void Enable() // call it when Enable the tower (just for sound and animations)
        {
            //SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            _isEnabled = true;
            sprite.color = Color.white;
            transform.localScale = _originScale;
        }
        public virtual void OnRhythmHit() // The callback when the user taps correctly, not callback of this type if not correct
        {
            Attack();
        }
        public void Attack()   //call it when the user taps correctly
        {
            // Do the animation for let the player know that the tap was correct
            StartCoroutine(ShootAnimationScale(Utilities.EasingFunctions.EaseOutQuart));
            
            // Logic of search enemies and shoot to them
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
            if (_level % 2 == 0) _range += RANGEIMPROVEMENT;
            _damage = (int)Math.Ceiling(_damage * _damageMultiplier);
            _level++;
        }

        public void StepOneFrame(bool isFirstBeat)
        {
            if (frames == null || frames.Count == 0)
            {
                Debug.LogError("ATower is missing animation frames");
                return;
            }

            currentFrame++;

            if (currentFrame >= frames.Count)
                currentFrame = 0;
            if (sprite != null)
            {
                sprite.sprite = frames[currentFrame];
            }
            
            Debug.Log("Current frame: " + currentFrame);
        }

        #endregion

        #region Corutines

        private IEnumerator ShootAnimationScale(Func<float, float> easingFunction = null)
        {
            Vector3 originScale = transform.localScale;
            Vector3 targetScale = new Vector3(originScale.x, originScale.y * _scaleFactorForShoot, originScale.z);
            
            float t = 0.0f;
            while (t < _timeOfAnimation)
            {
                float T;
                if (easingFunction == null) T = t / _timeOfAnimation;
                else T = easingFunction(t / _timeOfAnimation);

                float sT = Utilities.EasingFunctions.NormalizeParabolaNotConvex(T);
                
                transform.localScale = originScale * (1.0f - sT) + targetScale * sT;
                
                t += Time.deltaTime;
                yield return null;
            }
            
            transform.localScale = originScale;
        }
        #endregion

    }

}
