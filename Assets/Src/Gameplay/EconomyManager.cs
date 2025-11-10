using Gameplay.Towers;
using Gameplay.Waves;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace Gameplay
{


    public class EconomyManager : Utilities.Subsystem<EconomyManager>
    {
        private void Start()
        {
            GameplayManager.Instance.onEnemyDeath += AddVinyl;
        }

        // Referencia al tilemap donde van a aparecer las torres, se puede asignar por editor o en el start
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _buildableTile;
        [SerializeField] private TileBase _unBuildableTile;
        [SerializeField] private GameObject _buildingMenu;
        [SerializeField] private GameObject _updateMenu;
        [SerializeField] private int _vinyl = 0;

        private Dictionary<Vector3Int, UnityEngine.GameObject> _existingTowers = new Dictionary<Vector3Int, UnityEngine.GameObject>();
        private Vector3Int? _selectedTilePosition = null;

        #region ClickMethods 

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputHandler();
            }
        }

        void InputHandler()
        {
            if (_selectedTilePosition != null)
            {
                _selectedTilePosition = null;
                _buildingMenu.SetActive(false);
                _updateMenu.SetActive(false);
                return;
            }

            TileBase selectedTile = null;
            GetPositionClicked();
            if (_selectedTilePosition != null)
                selectedTile = _tilemap.GetTile(_selectedTilePosition.Value);

            //Vector3Int clickedCellPosition = GetPositionClicked();

            if (selectedTile == _buildableTile)
                _buildingMenu.SetActive(true);
            else if (selectedTile == _unBuildableTile)
                _updateMenu.SetActive(true);
            else
                Debug.Log("Otro tile)");

        }

        #endregion

        #region Tower methods

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
            _existingTowers.Add(spawnPosition, instantiatedTower);

            // TODO: Select the group base on something right now hardcoded for alpha test
            TowersManager.Instance.AddTower(instantiatedTower.GetComponent<ATower>(), 4);
        }

        /// <summary>
        /// Destruye la torre del tile que se le pase
        /// </summary>
        /// <param name="destroyPosition">Coordenadas del tile</param>
        int DestroyTower(Vector3Int destroyPosition)
        {
            _existingTowers.TryGetValue(destroyPosition, out UnityEngine.GameObject towerToDestroy);
            // TODO: Select the group base on something right now hardcoded for alpha test
            if (towerToDestroy != null) TowersManager.Instance.RemoveTower(towerToDestroy.GetComponent<ATower>(), 4);
            int sellingPrice = towerToDestroy.GetComponent<ATower>().GetSellingPrice();
            Destroy(towerToDestroy);
            _existingTowers.Remove(destroyPosition);

            return sellingPrice;
        }

        public void UpdateTower()
        {
            _existingTowers.TryGetValue(_selectedTilePosition.Value, out UnityEngine.GameObject towerToImprove);
            ATower script = towerToImprove.GetComponent<ATower>();
            int towerPrice = script.GetPrice();
            if (CanBuy(towerPrice))
            {
                script.Improve();
            }
        }
        #endregion

        #region Tile methods
        void ChangeTile(Vector3Int clickedCellPosition)
        {
            if (_tilemap.GetTile(clickedCellPosition) == _unBuildableTile)
                _tilemap.SetTile(clickedCellPosition, _buildableTile);
            else
                _tilemap.SetTile(clickedCellPosition, _unBuildableTile);
        }

        void GetPositionClicked() // FUNCIONA CON LA C�MARA CENITAL
        {
            Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickedPosition.z = 0;

            _selectedTilePosition = _tilemap.WorldToCell(clickedPosition);
        }

        #endregion

        #region Economy methods
        void AddVinyl(int vinyl)
        {
            _vinyl += vinyl;
        }
        public void TryBuyTower(GameObject towerToBuy)
        {
            ATower script = towerToBuy.GetComponent<ATower>();
            int towerPrice = script.GetPrice();
            if (CanBuy(towerPrice))
            {
                SpendVinyl(towerPrice);
                SpawnTower(_selectedTilePosition.Value, towerToBuy);
                ChangeTile(_selectedTilePosition.Value);
            }
            else Debug.Log("Eres pobre, no te la permites");
        }
        public void SellTower()
        {
            _vinyl += DestroyTower(_selectedTilePosition.Value);
            ChangeTile(_selectedTilePosition.Value);

        }
        bool CanBuy(int price)
        {
            return price <= _vinyl && GameplayManager.Instance.InBuildState();
        }
        void SpendVinyl(int price)
        {
            _vinyl -= price;
        }

        #endregion
    }
}
