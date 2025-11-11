using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellButton : AButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        EconomyManager.Instance.SellTower();
        EconomyManager.Instance.CloseMenu();
    }

}
