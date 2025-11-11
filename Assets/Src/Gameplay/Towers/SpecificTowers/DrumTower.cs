using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class DrumTower : ATower
    {
        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy;
            _cost = 130; //Coste que variara con las mejoras
            _damageType = DamageType.Percussion;    //Tipo de ataque (instrumento)
            _attackType = AttackType.Area;    //Ataque en area o individual
            _damage = 0; //Quiero ponerle un random que elija entre dos valores
            _range = 2;  //Tiles de alcance
        }
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void LateUpdate()
        {
            // Cuanto más abajo (Y menor), más arriba en el render
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

