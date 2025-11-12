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
        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onQuarter += OnRhythmUpdate;
        }

        new void Start()
        {
            _health = 16;
            _damageType = DamageType.Melee;
            _damage = 2;
            _moveTime = 0.5f;
            _resistance = Resistance.None;
            _vinylDrop = 3;
        }

        protected override void OnRhythmUpdate()
        {
            StartCoroutine(MoveToNextTile(_moveTime, Utilities.EasingFunctions.EaseInBack));
        }

        protected override void Death()
        {
            _isActive = false;
            _rhythmManager.onQuarter -= OnRhythmUpdate;
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }
    } 
}

