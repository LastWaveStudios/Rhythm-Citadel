using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class DrumTower : ATower
    {

        private SpriteRenderer spriteRenderer;  //Necesario para que las torretas no se superpongan de mala manera
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.AreaAttack;
            _cost = 130; //Coste que variara con las mejoras
            _damageType = DamageType.Percussion;    //Tipo de ataque (instrumento)
            _minDamage = 8; //Se divide en dos porque el daño seera un numero aleatorio entre estos dos
            _MaxDamage = 15;
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

