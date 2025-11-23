using Gameplay;
using Gameplay.Towers;
using Gameplay.World;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.ServiceLocator;

namespace UI.GameplayUI.Buttons
{
    public class BuildingButton : AButton
    {
        [SerializeField] private GameObject _towerToBuild;
        [SerializeField] private Color _hightlight;

        private WorldManager _worldManager;
        private int _buildingRange;
        private Vector3Int _buildingPosition;
        private List<Vector3Int> _affectedTiles;

        protected override void TakeReferences()
        {
            base.TakeReferences();
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();
            if (_economyManager == null)
            {
                Debug.LogError("AButton::TakeReferences: The WorldManager was null");
            }
            
        }

        private void OnEnable()
        {
            _buildingPosition = _economyManager.GetSelectedSite();
            ATower _aTower = _towerToBuild.GetComponent<ATower>();
            _buildingRange = _aTower.GetRange();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _affectedTiles = _worldManager.GetTilesInRange(_buildingPosition, _buildingRange);
            foreach (var tile in _affectedTiles) {_worldManager.Highlight(tile, _hightlight); }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            foreach (var tile in _affectedTiles) { _worldManager.ClearHightlight(tile); }
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            _economyManager.TryBuyTower(_towerToBuild);
            foreach (var tile in _affectedTiles) { _worldManager.ClearHightlight(tile); }
            _economyManager.CloseMenu();
        }

    }
}   
