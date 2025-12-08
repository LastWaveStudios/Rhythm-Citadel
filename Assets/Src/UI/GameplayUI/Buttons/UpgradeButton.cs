using Gameplay;
using Gameplay.Towers;
using Gameplay.World;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.ServiceLocator;

namespace UI.GameplayUI.Buttons
{
    public class UpgradeButton : AButton
    {
        [SerializeField] private GameObject _upgradeIcon;
        [SerializeField] private GameObject _blockedIcon;

        private TextMeshProUGUI _level;
        private TextMeshProUGUI _damage;
        private TextMeshProUGUI _price;

        private List<Vector3Int> _tilesInRange;

        private WorldManager _worldManager;

        private bool _canUpdate = true;
        private bool _highlightActive = false;
        private int _improvedRange;

        #region ------------------- Starting methods -------------------

        protected override void Start()
        {
            base.Start();
            ATower _activeTower = _economyManager.GetActiveTower();
            _improvedRange = _activeTower.GetImprovedRange();
            if (_activeTower.IsMaxLevel()) DisableUpdate();
        }

        protected override void TakeReferences()
        {
            base.TakeReferences();
            _worldManager = ServiceLocatorSubsystem.Instance.GetService<WorldManager>();
            if (_economyManager == null)
            {
                Debug.LogError("AButton::TakeReferences: The WorldManager was null");
            }

        }

        protected override void TakeTextReferences()
        {
            TextMeshProUGUI[] texts = _towerInfo.GetComponentsInChildren<TextMeshProUGUI>();

            if (texts.Length >= 4)
            {
                _level = texts[1];
                _damage = texts[2];
                _price = texts[3];
            }
        }

        #endregion

        #region ------------------- Pointer methods -------------------

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            HighlightTiles();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            ClearHighlight();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_canUpdate) _economyManager.UpdateTower();
            ClearHighlight();
            _economyManager.CloseMenu();
        }
        #endregion

        public void DisableUpdate()
        {
            _canUpdate = false;
            _upgradeIcon.SetActive(false);
            _blockedIcon.SetActive(true);
        }

        protected override void UpdateTowerInfo()
        {
            ATower _activeTower = _economyManager.GetActiveTower();

            _level.text = ($"Level: {_activeTower.GetLevel().ToString()}");
            _damage.text = ($"Damage per hit: {_activeTower.GetDamage().ToString()}");

            if (_canUpdate)
            {
                _damage.text += ($" ->  {_activeTower.GetImprovedDamage().ToString()}");
                _price.text = ($"Price: {_activeTower.GetImprovePrice().ToString()}");
            }
            else _price.text = ("MAX level reached");

        }

        private void HighlightTiles()
        {
            _highlightActive = true;
            Vector3Int _buildingPosition = _economyManager.GetSelectedSite();
            _tilesInRange = _worldManager.GetTilesInRange(_buildingPosition, _improvedRange);
            foreach (var tile in _tilesInRange) { _worldManager.Highlight(tile); }
        }

        private void ClearHighlight()
        {
            foreach (var tile in _tilesInRange) { _worldManager.ClearHighlight(tile); }
            _highlightActive = false;
        }
        private void OnDestroy()
        {
            if (_highlightActive) ClearHighlight();
        }
    }
}