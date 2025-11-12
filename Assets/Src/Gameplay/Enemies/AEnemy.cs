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
        protected int _health;
        protected DamageType _damageType;   //Melee, Range, Contact
        protected int _damage;
        protected float _moveTime = 0.5f;
        protected Resistance _resistance;   //None, String, Percussion or Hybrid
        protected int _vinylDrop = 0;

        protected int _path = 0;    
        protected int _index = 0;   //Current Tile
        protected bool _isActive = false; // If is death is not active
        public Action<AEnemy> onDeath = delegate {  };

        protected WorldManager _worldManager;
        protected RhythmManager _rhythmManager;


        private void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(TakeReferences);
        }

        private void TakeReferences()
        {
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();
            if (_worldManager == null )
            {
                Debug.LogError("AEnemy::TakeReferences: The WorldManager was null");
            }

            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if ( _rhythmManager == null )
            {
                Debug.LogError("AEnemy::TakeReferences: The RhythmManager was null");
            }
            SubscribeToRhythm();
        }

        protected abstract void SubscribeToRhythm();


        public void Init(int path)
        {
            _path = path;
            _index = 0;
        }

        public void Attack() {
            Debug.Log("En metodo atacar");

            Dancer.Instance.TakeDamage(_damage);
        }
        public void TakeDamage() { }

        public int GetDrop()
        {
            return _vinylDrop;
        }
        protected abstract void OnRhythmUpdate();

        /// <summary>
        /// Must desubscribe to the delegate of his rhythm disable the gameObject and invoke the onDeath delegate
        /// </summary>
        protected abstract void Death();

        public Vector3Int GetTile()
        {
            return _worldManager.GetTile(_path, _index);
        }

        public int GetDistanceToObjective()
        {
            int pathTilesCount = _worldManager.GetTileCount(_path);
            return pathTilesCount - _index;
        }
        /// <summary>
        /// Moves the enemy to the next tile with one animation using the easing function of the parameter delegate or a lerp if is null
        /// </summary>
        protected virtual IEnumerator MoveToNextTile(double moveTime, Func<float, float> easingFunction = null)
        {
            Vector3Int nextTile = _worldManager.GetNextTile(_path, _index);
            _index++;
            
            // TODO: Change the finalTile value that is not the last tile of the path xd
            Vector3Int finalTile = new Vector3Int(0, 0, 1);
            if (nextTile == finalTile)
            {
                Death();
                yield break;
            }
            
            Vector3 originPos = transform.position;
            Vector3 targetPos = _worldManager.GetCellCenterWorld(nextTile);
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

    public enum DamageType
    {
        Melee,
        Range,
        Contact
    }

    public enum Resistance
    {
        None,
        String,
        Percussion,
        Hybrid
    }
}

