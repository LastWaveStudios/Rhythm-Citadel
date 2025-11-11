using Gameplay.Enemies;
using Gameplay.World;
using Gameplay.Waves;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;

namespace Gameplay.Towers
{
    /// <summary>
    /// This class is for have the minimun that all towers will have regardless of anything and how they work togheter with the TowersGroup,
    /// that will give the perceptions to the AI that each tower will have
    /// </summary>
    public abstract class ATower : MonoBehaviour
    {
        [SerializeField]protected DamageType _damageType; // TODO: Change for enum with the actual DamageType, or even for one value that can contains partial damageTypes
        [SerializeField]protected int _range;  //N� de tiles de alcance
        [SerializeField]protected float _damage;
        [SerializeField]protected double _timeForProjectile = 0.1; // Time of projectile to reach the target
        protected IPoolManager _poolManager;
        //protected EnemieManager _enemieManager;
        protected WaveManager _waveManager;

        [SerializeField]protected int _price = 0;
        protected Vector3Int _myPosition;

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
            _myPosition = WorldManager.Instance.GetCellFromWorldPos(transform.position);
            _poolManager=FindAnyObjectByType<PoolManager>();
            if (_poolManager == null)   //BORRAR AL TERMINAR
            {
                Debug.Log("No se encontro el PoolManager"); //Se encuentra siempre el poolManager, asi q bien
            }
            else
            {
                Debug.Log("SI se encontro el PoolManager");
            }
            _waveManager = FindAnyObjectByType<WaveManager>();
            if (_poolManager == null)   //BORRAR AL TERMINAR
            {
                Debug.Log("No se encontro el ENEMIEManager"); //Se encuentra siempre el poolManager, asi q bien
            }
            else
            {
                Debug.Log("SI se encontro el ENEMIEManager");
            }
        }
        public abstract void Disable(); // call it when disable the tower (just for sound and animations)
        public abstract void Enable(); // call it when Enable the tower (just for sound and animations)
        public abstract void OnRhythmHit(); // The callback when the user taps correctly, not callback of this type if not correct
        
        public void VisualAttack(AEnemy enemy)   //call it when the user taps correctly
        {
            Debug.Log("Estamos en VISUAL ATACK");
            var pool =_poolManager.Get(typeof(Bullets));    //Devuelve IPoolableObjects
            Bullets bullet =(Bullets)pool;

            Vector3 from = transform.position;
            //Vector3 to = enemy.transform.position; 
            //Vector3 to = new Vector3(5f, 3f, 0f); solo para comprogbar que se dispara
            bullet.Shot(from, enemy, 5f, _poolManager);
        }
    }

}
