using UnityEngine;

namespace Gameplay.World
{
    public class Dancer : MonoBehaviour
    {
        private float _health = 100;
        public static Dancer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            Debug.Log("La vida actual es " + _health);
        }

        public bool CheckDeath()
        {
            return _health <= 0;
        }
    }
}

