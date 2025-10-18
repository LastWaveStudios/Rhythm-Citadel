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