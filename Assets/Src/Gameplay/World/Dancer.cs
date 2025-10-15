using UnityEngine;

namespace Gameplay.World
{
    public class Dancer : MonoBehaviour
    {
        private float _health = 100;

        public void TakeDamage(float damage)
        {
            _health -= damage;
        }

        public bool CheckDeath()
        {
            return _health <= 0;
        }
    }
}

