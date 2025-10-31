using Gameplay.Enemies;
using Gameplay.World;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemieManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    private List<GameObject> _enemiesList = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpawnEnemy(WorldManager.Instance.GetSpawnPoints()[0], _enemy);
        }
    }

    public void SpawnEnemy(Vector3 spawnPoint, GameObject enemyToSpawn)
    {
        UnityEngine.GameObject instantiatedEnemy = Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
        _enemiesList.Add(instantiatedEnemy);

    }

    public void SpawnEnemy(Vector3Int spawnTile, GameObject enemyToSpawn)
    {
        Tilemap tilemap = WorldManager.FindObjectOfType<Tilemap>();
        Vector3 spawnPoint = tilemap.GetCellCenterWorld(spawnTile);

        UnityEngine.GameObject instantiatedEnemy = Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
        _enemiesList.Add(instantiatedEnemy);

    }

    public List<GameObject> getEnemiesList()
    {
        return _enemiesList;
    }
}
