using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus.Navigation
{
    public class MenuOption : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]private EMenuButton _button;

        public Action<EMenuButton> onOptionClicked;
        public void OnPointerClick(PointerEventData eventData)
        {
            onOptionClicked?.Invoke(_button);
        }
    }
}