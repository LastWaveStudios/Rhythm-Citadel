using Gameplay.World;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.Enemies
{
    public abstract class AEnemy : MonoBehaviour  //Para poder probarlo he quitado que sea una clase abstracta
    {
        protected int _health;
        protected DamageType _damageType;
        protected int _damage;
        protected float _moveTime = 0.5f;
        protected Resistance _resistance;
        protected int _vinylDrop = 0;

        protected int _path = 0;    //Valor del path al que accede
        protected int _index = 0;   //Numero del tile actual
        protected bool _isActive = false; // If is death is not active

        public void SetActive(bool isActive) { _isActive = isActive; }
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

        public Vector3Int GetTile()
        {
            return WorldManager.Instance.GetTile(_path, _index);
        }

        public int GetDistanceToObjective()
        {
            int pathTilesCount = WorldManager.Instance.GetTileCount(_path);
            return pathTilesCount - _index;
        }
        /// <summary>
        /// Moves the enemy to the next tile with one animation using the easing function of the parameter delegate or a lerp if is null
        /// </summary>
        protected virtual IEnumerator MoveToNextTile(double moveTime, Func<float, float> easingFunction = null)
        {
            Vector3Int nextTile = WorldManager.Instance.GetNextTile(_path, _index);
            _index++;

            Vector3Int finalTile = new Vector3Int(0, 0, 1);
            if (nextTile == finalTile)
            {
                Destroy(gameObject);
                yield break;
            }

            Vector3 originPos = transform.position;
            Vector3 targetPos = WorldManager.Instance.GetCellCenterWorld(nextTile);
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
        }

        protected void InvokeDeath()
        {
            GameplayManager.Instance.onEnemyDeath.Invoke(_vinylDrop);
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

