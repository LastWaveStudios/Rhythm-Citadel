using Gameplay.Towers;
using Gameplay.Waves;
using Input;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

        [SerializeField] private GameObject _buildingMenuPrefab;
        [SerializeField] private GameObject _updateMenuPrefab;
        [SerializeField] private GameObject _vinylTextPrefab;

        [SerializeField] private int _vinyl = 0;

        private Canvas _canvas;
        private GameObject _currentMenu;

        private int _countExitMenu = 0;
        private TextMeshProUGUI _vinylText;
        private Dictionary<Vector3Int, UnityEngine.GameObject> _existingTowers = new Dictionary<Vector3Int, UnityEngine.GameObject>();
        private Vector3Int? _selectedTilePosition = null;
        private Vector2? _selectedScreenPosition = null;

        #region Services references
        private WaveManager _waveManager;
        private TowersManager _towersManager;
        private WorldManager _worldManager;
        #endregion

        public override void Init()
        {
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            _towersManager = ServiceLocatorSubsystem.Instance.GetService<TowersManager>();
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();

            _waveManager.onEnemyDeath += AddVinyl;
        }

        private void Start()
        {
            _vinylText = _vinylTextPrefab.GetComponent<TextMeshProUGUI>();
            _vinylText.text = _vinyl.ToString();
            _canvas = FindFirstObjectByType<Canvas>();

            InputReader.Instance.onPlaceTower += InputHandler;
        }
        
        #region ClickMethods 

        public void InputHandler()
        {
            if (_currentMenu != null)
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
            {
                _currentMenu = Instantiate(_buildingMenuPrefab, _canvas.transform);
                //_currentMenu.GetComponent<RectTransform>().localPosition = _selectedScreenPosition.Value;
                if (_selectedScreenPosition.HasValue)
                {
                    var rect = _currentMenu.GetComponent<RectTransform>();
                    rect.anchoredPosition = _selectedScreenPosition.Value;   // mejor anchored que localPosition
                }
            }

            else if (selectedTile == _unBuildableTile)
            {
                _currentMenu = Instantiate(_updateMenuPrefab, _canvas.transform);
                _currentMenu.GetComponent<RectTransform>().localPosition = _selectedScreenPosition.Value;
            }
           
            else
                Debug.Log("Otro tile");

        }

        public void CloseMenu()
        {
            _countExitMenu = 0;
            _selectedTilePosition = null;
            Destroy(_currentMenu);

        }

        void GetPositionClicked() // Works only with top down camera
        {
            Vector3 mousePosition = Pointer.current.position.ReadValue();

            _selectedTilePosition = _tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(mousePosition));  // Getting the tile clicked - for the towers and the other stuff

            // This part gets the position clicked relative to the canvas, then we will copy this out value to the global one, so we can use it out of here
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                RectTransformUtility.WorldToScreenPoint(Camera.main, _tilemap.GetCellCenterWorld(_selectedTilePosition.Value)),
                _canvas.worldCamera,
                out Vector2 localPoint);

            _selectedScreenPosition = localPoint;
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
            SetOrderInLayer(aTower, spawnPosition);
            _towersManager.AddTower(aTower, aTower.GetGroup());

            // TODO: Not really god have it here
            _towersManager.SetPatternGroup(aTower.GetPattern(), aTower.GetGroup());
        }

        private void SetOrderInLayer(ATower tower, Vector3Int position)
        {
            if (_worldManager.IsPositionOnPathTileMap(position + Vector3Int.up))
            {
                tower.SetSortingInLayer(10);
            }
            else if (_worldManager.IsPositionOnPathTileMap(position + Vector3Int.down))
            {
                tower.SetSortingInLayer(-10);
            }
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
            int towerPrice = script.GetImprovePrice();
            if (CanBuy(towerPrice))
            {
                AddVinyl(-towerPrice);
                script.Improve();
            }
        }

        public ATower GetActiveTower()
        {
            _existingTowers.TryGetValue(_selectedTilePosition.Value, out UnityEngine.GameObject towerToImprove);
            ATower activeTower = towerToImprove.GetComponent<ATower>();
            return activeTower;
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

        public Vector3Int GetSelectedSite()
        {
           return _selectedTilePosition.Value;
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
                AddVinyl(-towerPrice);
                SpawnTower(_selectedTilePosition.Value, towerToBuy);
                ChangeTile(_selectedTilePosition.Value);
            }
            else Debug.Log("Eres pobre, no te la permites");
        }
        public void SellTower()
        {
            AddVinyl(DestroyTower(_selectedTilePosition.Value));
            ChangeTile(_selectedTilePosition.Value);
        }
        bool CanBuy(int price)
        {
            return price <= _vinyl;
        }

        #endregion

        private void OnDestroy()
        {
            _waveManager.onEnemyDeath -= AddVinyl;
            InputReader.Instance.onPlaceTower -= InputHandler;
        }
    }
}
