using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.Waves
{
    [System.Serializable]
    public struct EnemiesBucket
    {
        public GameObject enemyPrefab;
        public int numberOfEnemies;
        public uint SixteenthOfSpawn;
        public int idSpawnpoint;
        public uint spawnDelayBetweenEnemiesOnSixteenths;

        public EnemiesBucket(GameObject enemyPrefab, int idSpawnpoint, uint SixteenthOfSpawn, int numberOfEnemies = 1, uint spawnDelayBetweenEnemiesOnSixteenths = 1)
        {
            this.enemyPrefab = enemyPrefab;
            this.numberOfEnemies = numberOfEnemies;
            this.SixteenthOfSpawn = SixteenthOfSpawn;
            this.idSpawnpoint = idSpawnpoint;
            this.spawnDelayBetweenEnemiesOnSixteenths = spawnDelayBetweenEnemiesOnSixteenths;
        }
    }

    public struct EnemyToSpawnData
    {
        public GameObject enemyPrefab;
        public uint SixteenthOfSpawn;
        public int idSpawnpoint;

        public EnemyToSpawnData(GameObject enemyPrefab, uint SixteenthOfSpawn, int idSpawnpoint)
        {
            this.enemyPrefab = enemyPrefab;
            this.SixteenthOfSpawn = SixteenthOfSpawn;
            this.idSpawnpoint = idSpawnpoint;
        }
    }

    [CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
    public class Wave : ScriptableObject
    {
        [SerializeField] private List<EnemiesBucket> bucketsToSpawn;

        [SerializeField] public List<EnemyToSpawnData> enemiesToSpawn;

        // Fills the enemiesToSpawn list with the data of buckets the enemiesToSpawn list is sorted in ascending order using the measureOfSpawn
        public void Init()
        {
            enemiesToSpawn = new List<EnemyToSpawnData>();
            enemiesToSpawn.Capacity = bucketsToSpawn.Count; // At least allocate all the known instances that at minimun we will have at once because it is super fast to know

            foreach (EnemiesBucket bucket in bucketsToSpawn)
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
            for (int i = enemiesToSpawn.Count - 1; i == 0; i--)
            {
                if (enemiesToSpawn[i].SixteenthOfSpawn <= enemyData.SixteenthOfSpawn)
                {
                    enemiesToSpawn.Insert(i + 1, enemyData);
                    break;
                }
            }
        }
    }
}

