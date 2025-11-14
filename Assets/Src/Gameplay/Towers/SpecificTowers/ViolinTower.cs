using Gameplay.Towers;
using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class ViolinTower : ATower
    {
        private SpriteRenderer spriteRenderer; //Prevent towers from overlapping incorrectly

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            focusType = FocusStrategies.FirstEnemy;
        }

        new void Start()
        {
            base.Start();
             
            _damageType = DamageType.String;    
            _minDamage=4;
            _MaxDamage=6;
            _range = 2;  

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

