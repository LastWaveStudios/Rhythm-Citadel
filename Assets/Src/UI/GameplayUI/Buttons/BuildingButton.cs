using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingButton : AButton
{
    [SerializeField] private GameObject _towerToBuild;
    public override void OnPointerClick(PointerEventData eventData)
    {
        EconomyManager.Instance.TryBuyTower(_towerToBuild);
        EconomyManager.Instance.CloseMenu();
    }

}
