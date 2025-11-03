using Gameplay.Enemies;
using Gameplay.RhythmSystem;
using Gameplay.World;
using System.Collections;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.Enemies
{
    public class QuarterNote : AEnemy
    {
        void Start()
        {
            RhythmManager.Instance.onQuarter += OnRhythmUpdate;
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
        }
    } 
}

