using Gameplay.Towers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay
{


    public class EconomyManager : MonoBehaviour
    {
        // Referencia al tilemap donde van a aparecer las torres, se puede asignar por editor o en el start
        [SerializeField] private Tilemap _tilemap;
        private Dictionary<Vector3Int, UnityEngine.GameObject> existingTowers = new Dictionary<Vector3Int, UnityEngine.GameObject>();

        #region Gestión de clicks - Todo esto es borrable, esta para que funcione temporalmente

        [SerializeField] private GameObject tower;
        [SerializeField] private TileBase buildableTile;
        [SerializeField] private TileBase unBuildableTile;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputHandler();
            }
        }

        // FUNCIONA CON LA CÁMARA CENITAL
        Vector3Int GetPositionClicked()
        {
            Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickedPosition.z = 0;

            return _tilemap.WorldToCell(clickedPosition);
        }

        void InputHandler()
        {
            Vector3Int clickedCellPosition = GetPositionClicked();
            TileBase selectedTile = _tilemap.GetTile(clickedCellPosition);

            if (selectedTile == buildableTile)
            {
                SpawnTower(clickedCellPosition, tower);
                _tilemap.SetTile(clickedCellPosition, unBuildableTile);
            }
            else if (selectedTile == unBuildableTile)
            {
                DestroyTower(clickedCellPosition);
                _tilemap.SetTile(clickedCellPosition, buildableTile);
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
            Vector3 offset = new Vector3(0, _tilemap.cellSize.y / 2, 0);
            Vector3 tileCenter = _tilemap.GetCellCenterWorld(spawnPosition);
            UnityEngine.GameObject instantiatedTower = Instantiate(towerToSpawn, tileCenter - offset, Quaternion.identity);
            existingTowers.Add(spawnPosition, instantiatedTower);

            // TODO: Select the group base on something right now hardcoded for alpha test
            //TowersManager.Instance.AddTower(instantiatedTower.GetComponent<ATower>(), 4);
        }

        /// <summary>
        /// Destruye la torre del tile que se le pase
        /// </summary>
        /// <param name="destroyPosition">Coordenadas del tile</param>
        void DestroyTower(Vector3Int destroyPosition)
        {
            existingTowers.TryGetValue(destroyPosition, out UnityEngine.GameObject towerToDestroy);
            // TODO: Select the group base on something right now hardcoded for alpha test
            //if (towerToDestroy != null) TowersManager.Instance.RemoveTower(towerToDestroy.GetComponent<ATower>(), 4);
            Destroy(towerToDestroy);
            existingTowers.Remove(destroyPosition);

        }

    }
}
