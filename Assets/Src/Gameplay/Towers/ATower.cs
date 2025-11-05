using Gameplay.Enemies;
using Gameplay.World;
using System.Collections.Generic;
using UnityEngine;

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

        protected Vector3Int _myPosition;

        public delegate List<AEnemy> FocusDelegate(List<AEnemy> enemiesList, Vector3Int position, int range);
        public FocusDelegate focusType;

        public void Start()
        {
            _myPosition = WorldManager.Instance.GetCellFromWorldPos(transform.position);
        }
        public abstract void Disable(); // call it when disable the tower (just for sound and animations)
        public abstract void Enable(); // call it when Enable the tower (just for sound and animations)
        public abstract void OnRhythmHit(); // The callback when the user taps correctly, not callback of this type if not correct
    }

}
