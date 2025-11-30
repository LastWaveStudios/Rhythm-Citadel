using Gameplay;
using Gameplay.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.GameplayUI.Buttons
{
    public class SellButton : AButton
    {
        private TextMeshProUGUI _price;
        public override void OnPointerClick(PointerEventData eventData)
        {
            _economyManager.SellTower();
            _economyManager.CloseMenu();
        }

        protected override void TakeTextReferences()
        {
            TextMeshProUGUI[] texts = _towerInfo.GetComponentsInChildren<TextMeshProUGUI>();

            if (texts.Length >= 2) _price = texts[1];
            
        }

        protected override void UpdateTowerInfo()
        {
            ATower _activeTower = _economyManager.GetActiveTower();

            _price.text = ($"Price: {_activeTower.GetSellingPrice().ToString()}");
        }
    }
}
