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
    public class QuarterRestNote : AEnemy
    {
        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onQuarter += OnRhythmUpdate;
        }

        protected override void InitializeBehaviour()
        {
            //TODO: Behaviour
            throw new System.NotImplementedException();
        }

        protected override void PushDeath()
        {
            //TODO: Behaviour
            throw new System.NotImplementedException();
        }

        protected override void OnRhythmUpdate()
        {
            //TODO: Behaviour
            throw new System.NotImplementedException();
        }

        public override void Death()
        {
            IsAlive = false;
            _rhythmManager.onQuarter -= OnRhythmUpdate;
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
    } 
}

