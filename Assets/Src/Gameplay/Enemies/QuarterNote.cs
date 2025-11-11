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
    public class QuarterNote : AEnemy
    {
        void OnEnable()
        {
            RhythmManager.Instance.onQuarter += OnRhythmUpdate;
        }

        protected override void OnRhythmUpdate()
        {
            StartCoroutine(MoveToNextTile(_moveTime, Utilities.EasingFunctions.EaseInBack));
        }

        protected override void Death()
        {
            RhythmManager.Instance.onQuarter -= OnRhythmUpdate;
            onDeath.Invoke(this);
        }
    } 
}

