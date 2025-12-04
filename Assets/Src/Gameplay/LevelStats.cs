using Gameplay.RhythmSystem;
using Gameplay.Waves;
using Gameplay.World;
using UnityEngine;
using Utilities.ServiceLocator;

namespace Gameplay
{
    public class LevelStats : Utilities.ServiceLocator.AService
    {
        // All the percentages of this class are between 0 and 1
        #region Public Properties
        public float WavePercentageSpawned => _waveManager.LastEnemySpawnedInCurrentWave / (float)_waveManager.GetEnemiesList().Count;
        public float WavePercentageKilled => _numberOfEnemiesKilledOnCurrentWave / (float)_waveManager.GetEnemiesList().Count;
        public float CurrentLevelPercentage => (float)_waveManager.CurrentWave / (float)_waveManager.NumberOfWaves;
        public float DancerHpPercentage => _dancer.Health / _dancer.MaxLife;
        public int CurrentMeasure => _rhythmManager?.MeasureCount ?? 0;
        #endregion
        
        #region Private variables
        private int _numberOfEnemiesKilledOnCurrentWave = 0;
        #endregion
        
        #region Services References
        private WaveManager _waveManager;
        private Dancer _dancer;
        private RhythmManager _rhythmManager;
        private GameplayManager _gameplayManager;
        #endregion
        public override void Init()
        {
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            if (_waveManager == null)
            {
                Debug.LogError("LevelStats::Init: No wave manager found");
                return;
            }
            
            _dancer = ServiceLocatorSubsystem.Instance.GetService<Dancer>();
            if (_dancer == null)
            {
                Debug.LogError("LevelStats::Init: No dancer found");
                return;
            }
            
            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("LevelStats::Init: No rhythm manager found");
                return;
            }
            
            _gameplayManager =  ServiceLocatorSubsystem.Instance.GetService<GameplayManager>();
            if (_gameplayManager == null)
            {
                Debug.LogError("LevelStats::Init: No gameplay manager found");
                return;
            }

            _gameplayManager.onFightStateStart += OnFightStateStart;
            _waveManager.onEnemyDeath += OnEnemyDeath;
        }

        private void OnFightStateStart()
        {
            _numberOfEnemiesKilledOnCurrentWave = 0;
        }

        private void OnEnemyDeath(int enemyDrop)
        {
            _numberOfEnemiesKilledOnCurrentWave++;
        }
    }
}