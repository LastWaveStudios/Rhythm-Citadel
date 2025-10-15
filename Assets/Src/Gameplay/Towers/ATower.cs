using UnityEngine;

namespace Gameplay.Towers
{
    /// <summary>
    /// This class is for have the minimun that all towers will have regardless of anything and how they work togheter with the TowersGroup,
    /// that will give the perceptions to the AI that each tower will have
    /// </summary>
    public abstract class ATower : MonoBehaviour
    {
        protected int _damageType; // TODO: Change for enum with the actual DamageType, or even for one value that can contains partial damageTypes
        protected int _range;  //Nº de tiles de alcance
        protected float _damage;
        protected double _timeForProjectile; // Time of projectile to reach the target

        public abstract void Disable(); // call it when disable the tower (just for sound and animations)
        public abstract void Enable(); // call it when Enable the tower (just for sound and animations)
        public abstract void OnRhythmHit(); // The callback when the user taps correctly, not callback of this type if not correct
    }

}
