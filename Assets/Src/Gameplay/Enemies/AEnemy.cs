using Gameplay.World;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.Enemies
{
    public abstract class AEnemy: MonoBehaviour  //Para poder probarlo he quitado que sea una clase abstracta
    {
        [SerializeField]protected int _health;
        [SerializeField]protected float _moveTime = 0.5f;
        [SerializeField]protected int _damage;
        [SerializeField]protected int _damageType;
        protected int _path = 0;    //Valor del path al que accede
        protected int _index = 0;   //Numero del tile actual

        public void Attack() { }
        public void TakeDamage() { }
        protected abstract void OnRhythmUpdate();
    }
}

