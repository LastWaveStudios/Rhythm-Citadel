using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.GameplayUI.Buttons
{
    public class UpgradeButton : AButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _economyManager.UpdateTower();
            _economyManager.CloseMenu();
        }
    }
}