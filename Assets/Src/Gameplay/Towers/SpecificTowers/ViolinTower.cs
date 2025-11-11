using Gameplay.Towers;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class ViolinTower : ATower
    {
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy;
            _cost = 70; //Coste que variara con las mejoras
            _damageType = DamageType.String;    //Tipo de ataque (instrumento)
            _minDamage=4;
            _MaxDamage=6;
            _range = 2;  //Tiles de alcance

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

