using System;
using UnityEngine;
using Utilities.ServiceLocator;

namespace Gameplay.World
{
    public class Dancer : Utilities.ServiceLocator.AService
    {
        [SerializeField] private float _health = 100;
        public Action onDancerDeath;
        private SpriteRenderer _spriteRender;
        [SerializeField] private Sprite[] _sprites;

        public override void Init()
        {
            
        }
        private void Awake()
        {
            _spriteRender = GetComponent<SpriteRenderer>();
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            CheckDeath();
            Debug.Log("La vida actual es " + _health);
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

        public void CheckDeath()
        {
            if (_health <= 0)
                onDancerDeath.Invoke();

        }
    }
}

