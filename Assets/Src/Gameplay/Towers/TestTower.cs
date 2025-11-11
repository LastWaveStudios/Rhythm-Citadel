using UnityEngine;


namespace Gameplay.Towers
{
    public class TestTower : ATower
    {
        [SerializeField] private int _testDebug;

        public override void Disable()
        {
            Debug.Log($"Tower {_testDebug} is Disabled");
        }

        protected override void Initialize()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy;
            _cost = 70; //Coste que variara con las mejoras
            _damageType = DamageType.String;    //Tipo de ataque (instrumento)
            _minDamage = 4;
            _MaxDamage = 6;
            _range = 2;  //Tiles de alcance
        }

        public override void Enable()
        {
            Debug.Log($"Tower {_testDebug} is Enabled");
        }

        public override void OnRhythmHit()
        {
            Debug.Log($"Callback reach the tower {_testDebug}");
        }
    }
}