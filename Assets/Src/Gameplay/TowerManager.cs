using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase buildableTile;
    [SerializeField] private TileBase unBuildableTile;

    private Dictionary<Vector3Int, UnityEngine.GameObject> existingTowers = new Dictionary<Vector3Int, UnityEngine.GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InputHandler();
        }
    }

    // FUNCIONA CON LA CÁMARA CENITAL
    Vector3Int getPositionClicked()
    {
        Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickedPosition.z = 0;

        return tilemap.WorldToCell(clickedPosition);
    }

    void SpawnTower(Vector3Int spawnPosition, GameObject towerToSpawn)
    {
        TileBase selectedTile = tilemap.GetTile(spawnPosition);
        if (selectedTile == buildableTile)
        {
            Vector3 tileCenter = tilemap.GetCellCenterWorld(spawnPosition);
            UnityEngine.GameObject instantiatedTower = Instantiate(tower, tileCenter, Quaternion.identity);
            tilemap.SetTile(spawnPosition, unBuildableTile);
            existingTowers.Add(spawnPosition, instantiatedTower);
        }
        else
        {
            Debug.LogError("En este tile no se puede construir");
        }
    }

    void DestroyTower(Vector3Int desrtoyPosition)
    {
        TileBase selectedTile = tilemap.GetTile(desrtoyPosition);
        if (selectedTile == unBuildableTile)
        {
            existingTowers.TryGetValue(desrtoyPosition, out UnityEngine.GameObject towerToDestroy);
            Destroy(towerToDestroy);
            existingTowers.Remove(desrtoyPosition);
            tilemap.SetTile(desrtoyPosition, buildableTile);
        }
    }

    void InputHandler()
    {
        Vector3Int clickedCellPosition = getPositionClicked();
        TileBase selectedTile = tilemap.GetTile(clickedCellPosition);

        if (selectedTile == buildableTile)
        {
            SpawnTower(clickedCellPosition, tower);
        }
        else if (selectedTile == unBuildableTile)
        {
            DestroyTower(clickedCellPosition);
        }
        else
        {
            Debug.Log("Otro tile)");
        }


    }
}
