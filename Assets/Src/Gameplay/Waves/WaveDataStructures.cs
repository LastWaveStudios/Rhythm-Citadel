using UnityEngine;

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
        public int idSpawnpoint; // This is the same id that the correspondingPathHas

        public EnemyToSpawnData(GameObject enemyPrefab, uint SixteenthOfSpawn, int idSpawnpoint)
        {
            this.enemyPrefab = enemyPrefab;
            this.SixteenthOfSpawn = SixteenthOfSpawn;
            this.idSpawnpoint = idSpawnpoint;
        }
    }
}

