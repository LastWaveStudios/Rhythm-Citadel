using Gameplay.Enemies;
using Gameplay.RhythmSystem;
using Gameplay.Waves;
using Gameplay.World;
using System.Collections;
using Gameplay.Enemies.Behaviours;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.Enemies
{
    public class HalfNote : AEnemy
    {
        BaseEnemyBehaviour _behaviour;
        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onHalf += OnRhythmUpdate;
        }

        protected override void InitializeBehaviour()
        {
            _behaviour = new BaseEnemyBehaviour(this);
        }

        protected override void PushDeath()
        {
            IsAlive = false;
            _behaviour.PushDeath();
        }

        protected override void OnRhythmUpdate()
        {
            _behaviour.UpdateBehaviour();
        }

        public override void Death()
        {
            DeSubscribeToRhythmParent();
            _rhythmManager.onHalf -= OnRhythmUpdate;
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
    } 
}

