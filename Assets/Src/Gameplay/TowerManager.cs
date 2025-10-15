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

    #region Gestión de clicks
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


    // METODO A ARREGLAR
    Vector3Int getPositionClickedOrtographic()
    {
        Plane gridPlane = new Plane(Vector3.up, Vector3.zero);
        Ray rayFromCameraToPlane = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 worldPosition = Vector3.zero;

        if (gridPlane.Raycast(rayFromCameraToPlane, out float distance))
        {
            worldPosition = rayFromCameraToPlane.GetPoint(distance);
        }
        return tilemap.WorldToCell(worldPosition);
    }

    void InputHandler()
    {
        Vector3Int clickedCellPosition = getPositionClicked();
        TileBase selectedTile = tilemap.GetTile(clickedCellPosition);

        if (selectedTile == buildableTile)
        {
            SpawnTower(clickedCellPosition, tower);
            tilemap.SetTile(clickedCellPosition, unBuildableTile);
        }
        else if (selectedTile == unBuildableTile)
        {
            DestroyTower(clickedCellPosition);
            tilemap.SetTile(clickedCellPosition, buildableTile);
        }
        else
        {
            Debug.Log("Otro tile)");
        }

    }

    #endregion

    /// <summary>
    /// Instancia una torre en el tile que se le pase
    /// </summary>
    /// <param name="spawnPosition"> Coordenadas del tile </param>
    /// <param name="towerToSpawn"> Prefab de la torre a spawnear</param>
    void SpawnTower(Vector3Int spawnPosition, GameObject towerToSpawn)
    {
        Vector3 tileCenter = tilemap.GetCellCenterWorld(spawnPosition);
        UnityEngine.GameObject instantiatedTower = Instantiate(tower, tileCenter, Quaternion.identity);
        existingTowers.Add(spawnPosition, instantiatedTower);

    }

    /// <summary>
    /// Destruye la torre del tile que se le pase
    /// </summary>
    /// <param name="desrtoyPosition">Coordenadas del tile</param>
    void DestroyTower(Vector3Int desrtoyPosition)
    {

        existingTowers.TryGetValue(desrtoyPosition, out UnityEngine.GameObject towerToDestroy);
        Destroy(towerToDestroy);
        existingTowers.Remove(desrtoyPosition);

    }

}
