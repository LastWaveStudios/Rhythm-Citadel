using Gameplay.Enemies;
using Gameplay.World;
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
        [SerializeField]protected int _damageType; // TODO: Change for enum with the actual DamageType, or even for one value that can contains partial damageTypes
        [SerializeField]protected int _range;  //Nº de tiles de alcance
        [SerializeField]protected float _damage;
        [SerializeField]protected double _timeForProjectile = 0.1; // Time of projectile to reach the target
        protected IPoolManager _poolManager;

        protected Vector3Int _myPosition;

        public delegate List<AEnemy> FocusDelegate(List<AEnemy> enemiesList, Vector3Int position, int range);
        public FocusDelegate focusType;

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
        }
        public abstract void Disable(); // call it when disable the tower (just for sound and animations)
        public abstract void Enable(); // call it when Enable the tower (just for sound and animations)
        public abstract void OnRhythmHit(); // The callback when the user taps correctly, not callback of this type if not correct
        
        //public void VisualAttack(AEnemy enemy)   //call it when the user taps correctly
        public void VisualAttack()  //Debe ser public para q se lea desde los diferentes tipos de torretas
        {
            Debug.Log("Estamos en VISUAL ATACK");
            var pool =_poolManager.Get(typeof(Bullets));    //Devuelve IPoolableObjects
            Bullets bullet =(Bullets)pool;

            Vector3 from = transform.position;
            // Vector3 to = enemy.transform.position; SE COMENTA HASTA QUE ESTEMOS EN LA FASE 2
            Vector3 to = new Vector3(5f, 3f, 0f);
            bullet.Shot(from, to, 5f, _poolManager);
        }
    }

}
