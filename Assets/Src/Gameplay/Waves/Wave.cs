using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.Waves
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
    public class Wave : ScriptableObject
    {
        [SerializeField] private List<EnemiesBucket> _bucketsToSpawn;

        [SerializeField] public List<EnemyToSpawnData> enemiesToSpawn;

        // Fills the enemiesToSpawn list with the data of buckets the enemiesToSpawn list is sorted in ascending order using the measureOfSpawn
        public void Init()
        {
            enemiesToSpawn = new List<EnemyToSpawnData>();
            // for reduce the number of allocations to just one big allocation
            int nEnemies = 0;
            foreach (EnemiesBucket bucket in _bucketsToSpawn)
            {
                nEnemies += bucket.numberOfEnemies;
            }
            enemiesToSpawn.Capacity = nEnemies;

            foreach (EnemiesBucket bucket in _bucketsToSpawn)
            {
                uint SixteenthOfSpawn = bucket.SixteenthOfSpawn;
                for (int i = 0; i < bucket.numberOfEnemies; ++i)
                {
                    PushEnemyToSpawnData(new EnemyToSpawnData(bucket.enemyPrefab, SixteenthOfSpawn, bucket.idSpawnpoint));
                    SixteenthOfSpawn += bucket.spawnDelayBetweenEnemiesOnSixteenths;
                }
            }
        }

        private void PushEnemyToSpawnData(EnemyToSpawnData enemyData)
        {
            if (enemiesToSpawn.Count == 0)
            {
                enemiesToSpawn.Add(enemyData);
                return;
            }
            for (int i = enemiesToSpawn.Count - 1; i >= 0; i--)
            {
                if (enemiesToSpawn[i].SixteenthOfSpawn <= enemyData.SixteenthOfSpawn)
                {
                    enemiesToSpawn.Insert(i + 1, enemyData);
                    return;
                }
            }
        }
    }
}