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
    public class TrebleClef : AEnemy
    {
        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onMeasure += OnRhythmUpdate; //CHANGE
        }

        protected override void OnRhythmUpdate()
        {
            StartCoroutine(MoveToNextTile(_moveTime, Utilities.EasingFunctions.EaseInBack));
        }

        protected override void Death()
        {
            _isAlive = false;
            _rhythmManager.onMeasure -= OnRhythmUpdate;
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
    } 
}

