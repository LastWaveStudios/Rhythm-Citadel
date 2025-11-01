using Gameplay.Enemies;
using System.Collections.Generic;

namespace Gameplay.Towers.SpecificTowers
{
    public class TrumpetTower : ATower
    {
        private List<AEnemy> enemies;
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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        new void Start()
        {
            base.Start();
            focusType = FocusStrategies.FirstEnemy;
            _range = 5;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

