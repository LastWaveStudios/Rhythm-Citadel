using Gameplay.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class TrumpetTower : ATower
    {
        private List<AEnemy> enemies;
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy;
            _cost = 95; //Coste que variara con las mejoras
            _damageType = DamageType.Percussion;    //Tipo de ataque (instrumento)
            _minDamage = 9;
            _MaxDamage = 17;
            _range = 1;  //Tiles de alcance
        }

        void LateUpdate()
        {
            //Cuanto más abajo, más arriba en el render
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }

        public override void Disable()
        {
            throw new System.NotImplementedException();
        }

        public override void Enable()
        {
            throw new System.NotImplementedException();
        }

        public override void OnRhythmHit()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

