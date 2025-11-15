using Gameplay.Towers;
using Gameplay.Waves;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using Utilities.ServiceLocator;

namespace Gameplay.World
{
    public class EconomyManager : Utilities.ServiceLocator.AService
    {
        // Referencia al tilemap donde van a aparecer las torres, se puede asignar por editor o en el start
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _buildableTile;
        [SerializeField] private TileBase _unBuildableTile;
        [SerializeField] private GameObject _buildingMenu;
        [SerializeField] private GameObject _updateMenu;
        [SerializeField] private GameObject _vinylTextPrefab;

        [SerializeField] private int _vinyl = 0;
        private int _countExitMenu = 0;
        private TextMeshProUGUI _vinylText;
        private Dictionary<Vector3Int, UnityEngine.GameObject> _existingTowers = new Dictionary<Vector3Int, UnityEngine.GameObject>();
        private Vector3Int? _selectedTilePosition = null;

        #region Services references
        private WaveManager _waveManager;
        private TowersManager _towersManager;
        #endregion

        public override void Init()
        {
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            _waveManager.onEnemyDeath += AddVinyl;

            _towersManager = ServiceLocatorSubsystem.Instance.GetService<TowersManager>();
        }

        private void Start()
        {
            _vinylText = _vinylTextPrefab.GetComponent<TextMeshProUGUI>();
            _vinylText.text = _vinyl.ToString();
        }
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
                if (_countExitMenu >= 1) CloseMenu();
                else _countExitMenu++;

                return;

            }

            TileBase selectedTile = null;
            GetPositionClicked();
            if (_selectedTilePosition != null)
                selectedTile = _tilemap.GetTile(_selectedTilePosition.Value);

            if (selectedTile == _buildableTile)
                _buildingMenu.SetActive(true);
            else if (selectedTile == _unBuildableTile)
                _updateMenu.SetActive(true);
            else
                Debug.Log("Otro tile)");

        }

        public void CloseMenu()
        {
            _countExitMenu = 0;
            _selectedTilePosition = null;
            _buildingMenu.SetActive(false);
            _updateMenu.SetActive(false);
            return;

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

            ATower aTower = instantiatedTower.GetComponent<ATower>();
            aTower.SetTile(spawnPosition);
            _towersManager.AddTower(aTower, aTower.GetGroup());
        }

        /// <summary>
        /// Destruye la torre del tile que se le pase
        /// </summary>
        /// <param name="destroyPosition">Coordenadas del tile</param>
        int DestroyTower(Vector3Int destroyPosition)
        {
            _existingTowers.TryGetValue(destroyPosition, out UnityEngine.GameObject towerToDestroy);
            // TODO: Select the group base on something right now hardcoded for alpha test
            //if (towerToDestroy != null) TowersManager.Instance.RemoveTower(towerToDestroy.GetComponent<ATower>(), 4);
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
            _vinylText.text = _vinyl.ToString();
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
            return price <= _vinyl;
        }
        void SpendVinyl(int price)
        {
            _vinyl -= price;
            _vinylText.text = _vinyl.ToString();
        }
        #endregion

        private void OnDestroy()
        {
            _waveManager.onEnemyDeath -= AddVinyl;
        }
    }
}
