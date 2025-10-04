using System;
using UnityEngine;

namespace UI.Menus.Navigation
{
    public class MenuOptionsGroup : MonoBehaviour
    {
        public Action<EMenuButton> onMenuNavigation;

        void Start ()
        {
            foreach(MenuOption option in GetComponentsInChildren<MenuOption>())
            {
                option.onOptionClicked += OnOptionClicked;
            }
        }

        private void OnOptionClicked(EMenuButton button)
        {
            onMenuNavigation?.Invoke(button);
        }
    }
}