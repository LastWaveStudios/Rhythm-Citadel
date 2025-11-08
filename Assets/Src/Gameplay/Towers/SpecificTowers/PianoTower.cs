using Gameplay.Enemies;
using Gameplay.Towers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class PianoTower : ATower
    {

        // TODO: Just for alpha test
        private bool isEnabled = true;

        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy; //Solo dice que metodo va a usar al atacar. Guarda referencia al metodo
            _range = 5;
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
            StartCoroutine(Shoot());
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
            List<AEnemy> objectives = focusType(enemies, _myPosition, _range);
            if (objectives == null || objectives.Count == 0)
            {
                yield return null;
            }
            AEnemy enemy = objectives[0];

            VisualAttack(enemy);
            //LLAMAR A FUNCION DE HACERSE DAÑO DE LOS ENEMIGOS
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

