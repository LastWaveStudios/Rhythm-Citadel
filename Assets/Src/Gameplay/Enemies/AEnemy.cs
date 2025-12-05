using Gameplay.RhythmSystem;
using Gameplay.World;
using System;
using System.Collections;
using Gameplay.Enemies.Common;
using UnityEngine;
using Utilities.ServiceLocator;


namespace Gameplay.Enemies
{
    public abstract class AEnemy : MonoBehaviour
    {
        #region --------------------------- Variables ---------------------------

        [SerializeField] protected EnemyDamageType _damageType;   //Melee, Range, Contact
        [SerializeField] protected DamageType _resistance;        //None, String, Percussion or Hybrid

        [SerializeField] protected float _moveTime = 0.5f;
        
        // Enemy stats that it gets from the scriptable objects

        protected EnemyStats _stats;

        protected int _health;
        protected int _damage;
        protected int _vinylDrop = 0;
        protected int _preparationBeats = 4;     // Beats that the enemy needs to prepare to move. Some enemies may change this value
        protected float _resistanceMultiplayer = 0.5f;

        [Header("Shield configuration")]
        [SerializeField] private int _shieldMaxStacks = 2;
        [SerializeField] private GameObject _shieldPrefab;
        [SerializeField] private float _radius;
        [SerializeField] private float _shieldScaleFactor;
        [SerializeField] private float _timeToRotate = 0.1f;
        [SerializeField] private float _angleToRotate = 30.0f; // In Degrees
        private ShieldWheelController _shieldWheelController;
        

        // Non editable variables
        protected int _path = 0;
        protected int _index = 0;           //Current Tile
        public bool IsAlive { get; protected set; } = true;    // If is death is not active
        protected int _currentPreparation = 0;

        protected int _currentShield = 0;

        public Action<AEnemy> onDeath = delegate { };

        // References
        protected WorldManager _worldManager;
        protected RhythmManager _rhythmManager;
        protected Dancer _dancer;

        #endregion
        

        #region ------------------------------ Starting methods ------------------------------
        public void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(TakeReferences);
            StartPosition();
            
            StartStats();

            //_shieldWheelController = GetComponentInChildren<ShieldWheelController>();
            //_shieldWheelController.Init(_radius, _shieldMaxStacks, _shieldScaleFactor, _timeToRotate, _angleToRotate, _shieldPrefab);
        }
        
        private void StartPosition()
        {
            transform.position = _worldManager.GetCellCenterWorld(_worldManager.GetTile(_path, _index));
        }

        private void StartStats()
        {
            Debug.Log("Trying to get stats");
            _stats = DifficultyManager.Instance.GetStats(this);

            _health = _stats.health;
            _damage = _stats.damage;
            _vinylDrop = _stats.vinylDrop;
            _preparationBeats = _stats.preparationBeats;
            _resistanceMultiplayer = _stats.reststanceMultiplayer;

            Debug.Log("I got " + _preparationBeats + " preparations beats");
        }

        private void TakeReferences()
        {
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();
            if (_worldManager == null)
            {
                Debug.LogError("AEnemy::TakeReferences: The WorldManager was null");
            }

            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("AEnemy::TakeReferences: The RhythmManager was null");
            }
            SubscribeToRhythm();
            _rhythmManager.onBeat += OnBeat;

            _dancer = ServiceLocatorSubsystem.Instance.GetService<Dancer>();
            if (_dancer == null)
            {
                Debug.LogError("AEnemy::TakeReferences: The Dancer was null");
            }

            InitializeBehaviour();
        }

        private void OnBeat(bool isMeasure)
        {
            //_shieldWheelController.RotateShields();
        }

        protected abstract void SubscribeToRhythm();

        protected abstract void InitializeBehaviour();

        protected abstract void PushDeath();

        public void Init(int path)
        {
            _path = path;
            _index = 0;
        }
        #endregion

        #region ------------------------------ Getters and setters ------------------------------

        public int GetDrop()
        {
            return _vinylDrop;
        }

        // Method used in case the enemy attacks, then you get no reward
        public void SetDrop(int drop)
        {
            _vinylDrop = drop;
        }
        public Vector3Int GetTile()
        {
            return _worldManager.GetTile(_path, _index);
        }

        public int GetDistanceToObjective()
        {
            int pathTilesCount = _worldManager.GetTileCount(_path);
            return pathTilesCount - _index;
        }

        #region PerceptionsMethods
        public bool IsMovementPrepared()
        {
            return (_currentPreparation >= _preparationBeats);
        }
        public bool IsInTarget()
        {
            return _worldManager.GetNextTile(_path, _index) == _worldManager.GetLastTile(_path);
        }
        public void PrepareMovement()
        {
            _currentPreparation++;
        }

        public void RestartMovementPreparation()
        {
            _currentPreparation = 0;
        }
        #endregion

        #endregion

        #region ---------------------------- Other methods ----------------------------
        public virtual void Attack()
        {
            _dancer.TakeDamage(_damage);
        }

        public void TakeDamage(DamageType type, int damageToTake)
        {
            if (RemoveShield(1)) return;
            
            if (_resistance == type) _health = (int)Mathf.Round(_health - damageToTake * _resistanceMultiplayer);
            else _health -= damageToTake;
            if (_health <= 0 && this.IsAlive && this.isActiveAndEnabled) PushDeath();
        }

        // Return true if can remove the shield and 0 if not
        protected bool RemoveShield(int stacksToRemove)
        {
            if (_currentShield > 0)
            {
                _currentShield = Math.Max(0, _currentShield - stacksToRemove);
                
                _shieldWheelController.SetCurrentShields(_currentShield);
                
                return true;
            }

            return false;
        }

        public void Move()
        {
            StartCoroutine(MoveToNextTile(_moveTime, Utilities.EasingFunctions.EaseInBack));
        }

        public void GiveShield(int shieldStacks)
        {
            _currentShield = Math.Min(_currentShield + shieldStacks, _shieldMaxStacks);
            // TODO: Add the visual shield
            _shieldWheelController.SetCurrentShields(_currentShield);
            Debug.Log($"Called give shield on enemy: {name} with a shield value of {_currentShield}");
        }

        #endregion
        
        // Override in all the children classes, but this is a general behaviour that have it all of them, so call base.OnRhythmUpdate on his override
        protected abstract void OnRhythmUpdate();

        #region ------------------------------ Abstract Methods ------------------------------
        /// <summary>
        /// Must desubscribe to the delegate of his rhythm disable the gameObject and invoke the onDeath delegate
        /// </summary>
        public abstract void Death();

        #endregion

        #region ------------------------------ Corutines ------------------------------
        /// <summary>
        /// Moves the enemy to the next tile with one animation using the easing function of the parameter delegate or a lerp if is null
        /// </summary>
        protected virtual IEnumerator MoveToNextTile(double moveTime, Func<float, float> easingFunction = null)
        {
            Vector3Int nextTile = _worldManager.GetNextTile(_path, _index);
            _index++;

            Vector3 originPos = transform.position;
            Vector3 targetPos = _worldManager.GetCellCenterWorld(nextTile);

            Vector3 tileSize = _worldManager.GetTileSize();
            //Offset for enemy stacking
            float offsetFactor = 0.3f;
            targetPos += new Vector3(UnityEngine.Random.Range(-tileSize.x * offsetFactor, tileSize.x * offsetFactor),
                                     UnityEngine.Random.Range(-tileSize.y * offsetFactor, tileSize.y * offsetFactor),
                                     0f);

            float t = 0.0f;
            while (t <= _moveTime)
            {
                //transform.position = Vector3.Lerp(originPos, targetPos, EaseInBack(t / _moveTime));

                float T;
                if (easingFunction == null) T = t / _moveTime;
                else T = easingFunction(t / _moveTime);
                transform.position = originPos * (1 - T) + targetPos * T;
                t += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;   // Fix for center final positions
            yield return null;
        }
    }
    #endregion

    public enum EnemyDamageType
    {
        Melee,
        Range,
        Contact
    }

}

