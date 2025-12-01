using Gameplay;
using Gameplay.Towers;
using Gameplay.World;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.ServiceLocator;

namespace UI.GameplayUI.Buttons
{
    public class BuildingButton : AButton
    {
        [SerializeField] private GameObject _towerToBuild;

        private TextMeshProUGUI _title;
        private TextMeshProUGUI _damage;
        private TextMeshProUGUI _hitsPerBeat;
        private TextMeshProUGUI _price;

        private WorldManager _worldManager;

        private Vector3Int _buildingPosition;
        private List<Vector3Int> _tilesInRange;

        private int _buildingRange;

        #region ------------------- Starting methods -------------------

        protected override void Start()
        {
            base.Start();
            _buildingPosition = _economyManager.GetSelectedSite();
            ATower _aTower = _towerToBuild.GetComponent<ATower>();
            _buildingRange = _aTower.GetRange();
        }
        
        protected override void TakeReferences()
        {
            base.TakeReferences();
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();
            if (_economyManager == null) Debug.LogError("AButton::TakeReferences: The WorldManager was null");
        }
        
        protected override void TakeTextReferences()
        {
            TextMeshProUGUI[] texts = _towerInfo.GetComponentsInChildren<TextMeshProUGUI>();

            if (texts.Length >= 4)
            {
                _title = texts[0];
                _damage = texts[1];
                _hitsPerBeat = texts[2];
                _price = texts[3];
            }
        }

        #endregion

        #region ------------------- Pointer methods -------------------
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _tilesInRange = _worldManager.GetTilesInRange(_buildingPosition, _buildingRange);
            foreach (var tile in _tilesInRange) { _worldManager.Highlight(tile); }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            foreach (var tile in _tilesInRange) { _worldManager.ClearHighlight(tile); }
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            _economyManager.TryBuyTower(_towerToBuild);
            foreach (var tile in _tilesInRange) { _worldManager.ClearHighlight(tile); }
            _economyManager.CloseMenu();
        }

        #endregion
        
        #region ------------------- Other methods -------------------

        protected override void UpdateTowerInfo()
        {
            ATower _aTower = _towerToBuild.GetComponent<ATower>();
            _title.text = name;
            _damage.text = ($"Damage per hit: {_aTower.GetDamage().ToString()}");
            _hitsPerBeat.text = ($"Hits: {_aTower.GetAttacksPerMeasure().ToString()}");
            _price.text = ($"Price: {_aTower.GetPrice().ToString()}");
        }
        
        #endregion
    }
}
