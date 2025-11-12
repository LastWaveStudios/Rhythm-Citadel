using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellButton : AButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        _economyManager.SellTower();
        _economyManager.CloseMenu();
    }

}
