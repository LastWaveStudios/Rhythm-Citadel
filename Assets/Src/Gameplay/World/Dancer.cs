using System;
using UnityEngine;
using Utilities.ServiceLocator;

namespace Gameplay.World
{
    public class Dancer : Utilities.ServiceLocator.AService
    {
        [SerializeField] private float _health = 100;
        public Action onDancerDeath;

        public override void Init()
        {
            
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            CheckDeath();
            Debug.Log("La vida actual es " + _health);
        }

        public void CheckDeath()
        {
            if (_health <= 0)
                onDancerDeath.Invoke();

        }
    }
}

