using Gameplay.RhythmSystem;
using Gameplay.World;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities.ObjectPool;
using Utilities.ServiceLocator;


namespace Gameplay.Enemies
{
    public abstract class AEnemy : MonoBehaviour
    {
        [SerializeField] const float RESISTANCE_MULTIPLAYER = 0.5f;

        [SerializeField] protected int _health;
        [SerializeField] protected EnemyDamageType _damageType;   //Melee, Range, Contact
        [SerializeField] protected int _damage;
        [SerializeField] protected float _moveTime = 0.5f;
        [SerializeField] protected DamageType _resistance;   //None, String, Percussion or Hybrid
        [SerializeField] protected int _vinylDrop = 0;

        protected int _path = 0;    
        protected int _index = 0;   //Current Tile
        protected bool _isAlive = false; // If is death is not active
        public Action<AEnemy> onDeath = delegate {  };

        protected WorldManager _worldManager;
        protected RhythmManager _rhythmManager;
        protected Dancer _dancer;


        #region Starting methods
        public void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(TakeReferences);
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
            
            _dancer = ServiceLocatorSubsystem.Instance.GetService<Dancer>();
            if (_rhythmManager == null)
            {
                Debug.LogError("AEnemy::TakeReferences: The Dancer was null");
            }
        }

        protected abstract void SubscribeToRhythm();

        public void Init(int path)
        {
            _path = path;
            _index = 0;
        }
        #endregion

        #region Getters and setters

        public int GetDrop()
        {
            return _vinylDrop;
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
        #endregion

        #region Other methods
        public void Attack() {
            Debug.Log("En metodo atacar");

            _dancer.TakeDamage(_damage);
        }
        public void TakeDamage(DamageType type, int damageToTake) 
        {
            if (_resistance == type) _health = (int)Mathf.Round(_health - damageToTake * RESISTANCE_MULTIPLAYER);
            else _health -= damageToTake;
        }
        #endregion

        #region Abstract Methods
        protected abstract void OnRhythmUpdate();

        /// <summary>
        /// Must desubscribe to the delegate of his rhythm disable the gameObject and invoke the onDeath delegate
        /// </summary>
        protected abstract void Death();

        #endregion

        /// <summary>
        /// Moves the enemy to the next tile with one animation using the easing function of the parameter delegate or a lerp if is null
        /// </summary>
        protected virtual IEnumerator MoveToNextTile(double moveTime, Func<float, float> easingFunction = null)
        {
            Vector3Int nextTile = _worldManager.GetNextTile(_path, _index);
            _index++;
            
            // Cambiar para q sea el WorldManager del q coja el utimo Tile. Ya he creado la variable ahora relacionemosla
            Vector3Int finalTile = _worldManager.GetLastTile(_path);
            if (nextTile == finalTile)
            {
                Debug.Log("Estamos en el Tile final");
                Death();
                yield break;
            }
            
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

    public enum EnemyDamageType
    {
        Melee,
        Range,
        Contact
    }

}

