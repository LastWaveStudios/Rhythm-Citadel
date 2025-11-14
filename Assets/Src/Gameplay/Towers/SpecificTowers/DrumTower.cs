using UnityEngine;

namespace Gameplay.Towers.SpecificTowers
{
    public class DrumTower : ATower
    {

        private SpriteRenderer spriteRenderer;  //Prevent towers from overlapping incorrectly
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            focusType = FocusStrategies.AreaAttack;
        }
        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.AreaAttack;
            _damageType = DamageType.Percussion;    
            _minDamage = 8; 
            _MaxDamage = 15;
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

