using Gameplay.Enemies;
using Gameplay.RhythmSystem;
using Gameplay.Waves;
using Gameplay.World;
using System.Collections;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.Enemies
{
    public class EighthNote : AEnemy
    {
        void Start()
        {
            RhythmManager.Instance.onEighth += OnRhythmUpdate;
            _health = 70;
            _damageType = DamageType.Melee;
            _damage = 18;
            _moveTime = 0.5f;
            _resistance = Resistance.String;
            _vinylDrop = 12;
        }

        protected override void OnRhythmUpdate()
        {
            StartCoroutine(MoveToNextTile(_moveTime, EaseInBack));
        }

        // Taken from https://easings.net/#easeInBack 
        private float EaseInBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1.0f;

            return c3 * t * t * t - c1 * t * t;
        }

        private void OnDestroy()
        {
            RhythmManager.Instance.onQuarter -= OnRhythmUpdate;
            InvokeDeath();
        }
    } 
}

