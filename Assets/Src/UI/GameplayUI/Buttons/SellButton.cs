using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.GameplayUI.Buttons
{
    public class SellButton : AButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _economyManager.SellTower();
            _economyManager.CloseMenu();
        }

    }
}
