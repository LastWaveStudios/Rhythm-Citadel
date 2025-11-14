using Gameplay.Enemies;
using Gameplay.RhythmSystem;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.ServiceLocator;

namespace Gameplay.Waves
{
    public class WaveManager : Utilities.ServiceLocator.AService
    {
        public Action<int> onEnemyDeath = delegate { };

        private int _activeEnemies = 0;

        [SerializeField] private List<Wave> _waves;
        private List<AEnemy> _currentWaveEnemies;

        public int CurrentWave { get; private set; } = -1;
        public int LastEnemySpawnedInCurrentWave { get; private set; } = -1;
        public bool AllEnemiesDeadInCurrentWave { get; private set; } = false;
        private int _numberOfEnemiesDeadInCurrentWave = 0;

        private RhythmManager _rhythmManager;

        public override void Init()
        {
            _currentWaveEnemies = new List<AEnemy>();
            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
        }

        public bool InitNextWave()
        {
            _numberOfEnemiesDeadInCurrentWave = 0;
            AllEnemiesDeadInCurrentWave = false;
            CurrentWave++;
            if (CurrentWave < 0 || CurrentWave >= _waves.Count) return false;

            if (CurrentWave - 1 >= 0)
            {
                _waves[CurrentWave - 1].enemiesToSpawn.Clear();
                _waves[CurrentWave - 1].enemiesToSpawn.Capacity = 0; // Clear the memory of completed waves for do not have that metadata of memory (is not a lot but is)
            }

            _waves[CurrentWave].Init();
            _currentWaveEnemies.Clear();
            _currentWaveEnemies.Capacity = _waves[CurrentWave].enemiesToSpawn.Count;

            _activeEnemies = _currentWaveEnemies.Capacity; //Count how many have to die

            foreach (EnemyToSpawnData enemyData in _waves[CurrentWave].enemiesToSpawn)
            {
                AEnemy enemy = GameObject.Instantiate(enemyData.enemyPrefab).GetComponent<AEnemy>();
                enemy.Init(enemyData.idSpawnpoint); // Same as path
                enemy.gameObject.SetActive(false);
                enemy.onDeath += OnEnemyDead;
                _currentWaveEnemies.Add(enemy);
            }
            LastEnemySpawnedInCurrentWave = -1;

            return true;
        }

        private void OnEnemyDead(AEnemy enemy)
        {
            _numberOfEnemiesDeadInCurrentWave++;
            if (_numberOfEnemiesDeadInCurrentWave == _currentWaveEnemies.Count)
            {
                AllEnemiesDeadInCurrentWave = true;

            }
            onEnemyDeath.Invoke(enemy.GetDrop());
        }

        public AEnemy GetEnemy(int index)
        {
            if (index < 0 || index >= _currentWaveEnemies.Count) return null;

            return _currentWaveEnemies[index];
        }

        public List<AEnemy> GetEnemiesList()
        {
            return _currentWaveEnemies;
        }

        public void StartWave()
        {
            if (CurrentWave < 0 || CurrentWave >= _waves.Count)
                Victory();
                //Here

                _rhythmManager.onSixteenth += OnSixteenth;
        }

        private void OnSixteenth()
        {
            while (LastEnemySpawnedInCurrentWave < _currentWaveEnemies.Count && LastEnemySpawnedInCurrentWave + 1 < _currentWaveEnemies.Count &&
                _waves[CurrentWave].enemiesToSpawn[LastEnemySpawnedInCurrentWave + 1].SixteenthOfSpawn == _rhythmManager.SixteenthCountGlobal)
            {
                _currentWaveEnemies[++LastEnemySpawnedInCurrentWave].gameObject.SetActive(true);
                Debug.Log($"Active the {LastEnemySpawnedInCurrentWave} enemy");
            }
            if (LastEnemySpawnedInCurrentWave == _currentWaveEnemies.Count - 1)
            {
                _rhythmManager.onSixteenth -= OnSixteenth;
            }
        }

        private void Victory()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}