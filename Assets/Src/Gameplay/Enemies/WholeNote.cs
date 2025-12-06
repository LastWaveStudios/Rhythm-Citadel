using Gameplay.Waves;
using Gameplay.Enemies.Behaviours;
using Gameplay.RhythmSystem;
using UnityEngine;
using Utilities.ServiceLocator;


namespace Gameplay.Enemies
{
    public class WholeNote : AEnemy
    {
        [SerializeField] private int _shieldRange = 3;
        [SerializeField] private int _shieldStacksPerActivation = 1;
        
        private WaveManager _waveManager;

        private WholeNoteBehaviour _behaviour;
        
        protected override void SubscribeToRhythm()
        {
            _rhythmManager.onWhole += OnRhythmUpdate;
            _timeOfNote =  _rhythmManager.GetTimeOfANote(NoteDuration.Whole) / 1000.0f;
        }

        protected override void InitializeBehaviour()
        {
            // A little tricky but this method right now is called after the Init so the services are initialize and I can take the Wave Manager without problems
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            if (_waveManager == null)
            {
                Debug.LogError("WholeNote::InitializeBehaviour: _waveManager is null");
                return;
            }

            _behaviour = new WholeNoteBehaviour(this);
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
            _rhythmManager.onWhole -= OnRhythmUpdate;
            onDeath.Invoke(this);
            gameObject.SetActive(false);
        }

        public override void Attack()
        {
            base.Attack();
            PushDeath();
        }

        public void GiveShieldToNearbyAllies()
        {
            Vector3Int tilePosition = GetTile();

            for (int i = 0; i <= _waveManager.LastEnemySpawnedInCurrentWave; ++i)
            {
                AEnemy ally = _waveManager.GetEnemy(i);
                if (Utilities.Distances.ManhattanDistance(ally.GetTile(), tilePosition) <=
                    _shieldRange)
                {
                    ally.GiveShield(_shieldStacksPerActivation);
                }
            }
        }
    } 
}

