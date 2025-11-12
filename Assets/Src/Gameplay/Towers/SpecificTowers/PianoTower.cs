using Gameplay.Enemies;
using Gameplay.Towers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class PianoTower : ATower
    {
        private bool isEnabled = true;
        private SpriteRenderer spriteRenderer; //Prevent towers from overlapping incorrectly

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy; 
            _cost = 110; 
            _damageType = DamageType.Hybrid;    
            _minDamage = 6;
            _MaxDamage = 11;
            _range = 1;  
        }

        void LateUpdate()
        {
            //Lower is, higher appear
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }

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
            //StartCoroutine(Shoot());
            StartCoroutine(TestThing());
        }

        private IEnumerator TestThing()
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;

            float t = 0.0f;
            while (t < 0.25f)
            {
                t += Time.deltaTime;
                yield return null;
            }
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // TODO: Change to actual shoot, just for alpha test
        private IEnumerator Shoot()
        {
            float t = 0.0f;
            List<AEnemy> enemies = _waveManager.GetEnemiesList();
            if (enemies ==null || enemies.Count == 0)
            {
                yield return null;
            }
            List<AEnemy> objectives = focusType(enemies, _positionInWorldCell, _range);
            if (objectives == null || objectives.Count == 0)
            {
                yield return null;
            }
            AEnemy enemy = objectives[0];

            VisualAttack(enemy);
            //LLAMAR A FUNCION DE HACERSE DA�O DE LOS ENEMIGOS
            //DEJAMOS ESTO POR AHORA PARA SABER CUANDO SI SE DEBE DISPARAR Y SI ESTA SUCEDIENDO
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            sprite.color = Color.green;
            
            while (t < _timeForProjectile)
            {
                t += Time.deltaTime;
                yield return null;
            }

            if (isEnabled) sprite.color = Color.white;
        }

    }

}

