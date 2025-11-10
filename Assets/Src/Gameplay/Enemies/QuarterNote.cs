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
        void Start()
        {
            _vinylDrop = 3;
            RhythmManager.Instance.onQuarter += OnRhythmUpdate;
            _damage = 10;
        }

        protected override void OnRhythmUpdate()
        {
            StartCoroutine(MoveToNextTile(_moveTime, Utilities.EasingFunctions.EaseInBack));
        }

        private void OnDestroy()
        {
            RhythmManager.Instance.onQuarter -= OnRhythmUpdate;
            InvokeDeath();
        }
    } 
}

