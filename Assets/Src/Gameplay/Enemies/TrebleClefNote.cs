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
            _rhythmManager.onMeasure -= OnRhythmUpdate;
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
    } 
}

