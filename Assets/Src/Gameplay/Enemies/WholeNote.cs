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
    public class WholerNote : AEnemy
    {
        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onWhole += OnRhythmUpdate;
        }

        public override void Death()
        {
            _isAlive = false;
            _rhythmManager.onWhole -= OnRhythmUpdate;
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
    } 
}

