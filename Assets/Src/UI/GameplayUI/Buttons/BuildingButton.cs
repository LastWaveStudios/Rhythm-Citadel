using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.GameplayUI.Buttons
{
    public class BuildingButton : AButton
    {
        [SerializeField] private GameObject _towerToBuild;
        public override void OnPointerClick(PointerEventData eventData)
        {
            _economyManager.TryBuyTower(_towerToBuild);
            _economyManager.CloseMenu();
        }

    }
}   
