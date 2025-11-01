using Gameplay.Enemies;
using Gameplay.RhythmSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Waves
{
    public class WaveManager : MonoBehaviour
    {
        #region Singleton pattern without live between scenes
        public static WaveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Must have just one instance of WaveManager at the same time on the same scene");
                Destroy(this.gameObject);
                return;
            }
        }
        #endregion

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

        public void StartWave()
        {
            if (CurrentWave < 0 || CurrentWave >= _waves.Count) return;

            RhythmManager.Instance.onSixteenth += OnSixteenth;
            RhythmManager.Instance.StartRhythm();
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
                // Must wait to all the enemies death for change the phase and for start preparing the nextWave, but at least do not have more calls for nothing
            }
        }
    }
}