using UnityEngine;

namespace Gameplay.World
{
    public class Dancer : MonoBehaviour
    {
        private float _health = 100;
        public static Dancer Instance { get; private set; }
        private SpriteRenderer _spriteRender;
        [SerializeField] private Sprite[] _sprites;

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
            _spriteRender = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            if (_health < 25)
            {
                _spriteRender.sprite = _sprites[0];
            }
            else if (_health < 50)
            {
                _spriteRender.sprite = _sprites[1];
            }
            else if (_health < 75)
            {
                _spriteRender.sprite = _sprites[2];
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

