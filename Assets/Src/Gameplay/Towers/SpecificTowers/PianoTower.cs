using Gameplay.Towers;
using System.Collections;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class PianoTower : ATower
    {

        // TODO: Just for alpha test
        private bool isEnabled = true;

        public override void Disable()
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            isEnabled = false;
            sprite.color = Color.red;
        }

        public override void Enable()
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            isEnabled = true;
            sprite.color = Color.white;
        }

        public override void OnRhythmHit()
        {
            StartCoroutine(Shoot());
        }

        // TODO: Change to actual shoot, just for alpha test
        private IEnumerator Shoot()
        {
            float t = 0.0f;

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            sprite.color = Color.green;

            while (t < _timeForProjectile)
            {
                t += Time.deltaTime;
                yield return null;
            }

            if (isEnabled) sprite.color = Color.white;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            base.Start();
        }
    }

}

