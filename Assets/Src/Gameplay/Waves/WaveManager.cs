using Gameplay.Enemies;
using Gameplay.RhythmSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Waves
{
    public class WaveManager : Utilities.Subsystem<WaveManager>
    {
        public Action<int> onEnemyDeath = delegate { };

        private int _activeEnemies = 0;

        [SerializeField] private List<Wave> _waves;
        private List<AEnemy> _currentWaveEnemies;

        public int CurrentWave { get; private set; } = -1;
        public int LastEnemySpawnedInCurrentWave { get; private set; } = -1;


        private void Start()
        {
            _currentWaveEnemies = new List<AEnemy>();
        }

        public bool InitNextWave()
        {
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
                _currentWaveEnemies.Add(enemy);
            }
            LastEnemySpawnedInCurrentWave = -1;

            return true;
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
            if (CurrentWave < 0 || CurrentWave >= _waves.Count) return;

            RhythmManager.Instance.onSixteenth += OnSixteenth;
        }

        private void OnSixteenth()
        {
            while (LastEnemySpawnedInCurrentWave < _currentWaveEnemies.Count && LastEnemySpawnedInCurrentWave + 1 < _currentWaveEnemies.Count &&
                _waves[CurrentWave].enemiesToSpawn[LastEnemySpawnedInCurrentWave + 1].SixteenthOfSpawn == RhythmManager.Instance.SixteenthCountGlobal)
            {
                _currentWaveEnemies[++LastEnemySpawnedInCurrentWave].gameObject.SetActive(true);
                Debug.Log($"Active the {LastEnemySpawnedInCurrentWave} enemy");
            }
            if (LastEnemySpawnedInCurrentWave == _currentWaveEnemies.Count - 1)
            {
                RhythmManager.Instance.onSixteenth -= OnSixteenth;
            }
        }
    }
}