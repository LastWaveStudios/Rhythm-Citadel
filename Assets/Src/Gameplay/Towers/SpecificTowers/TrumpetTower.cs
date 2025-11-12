using Gameplay.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class TrumpetTower : ATower
    {
        private List<AEnemy> enemies;
        private SpriteRenderer spriteRenderer; //Prevent towers from overlapping incorrectly

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy;
            _cost = 95; 
            _damageType = DamageType.Percussion;    
            _minDamage = 9;
            _MaxDamage = 17;
            _range = 1;  
        }

        void LateUpdate()
        {
            //Lower is, higher appear
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

