using Gameplay;
using UnityEngine;

using UnityEngine.EventSystems;
public class UpgradeButton : AButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        EconomyManager.Instance.UpdateTower();
        EconomyManager.Instance.CloseMenu();
    }
}
