using UnityEngine;
using UI.Menus.Navigation;

namespace UI.Menus.States
{
    internal abstract class AMenuState : IMenuState
    {
        protected string _menuName;
        protected GameObject _menu;
        protected Canvas _canvas;

        public AMenuState(string menuName)
        {
            _menuName = menuName;
        }

        public virtual void Enter()
        {
            _canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
            GameObject menuPrefab = (GameObject)Resources.Load(_menuName);
            _menu = GameObject.Instantiate(menuPrefab, _canvas.transform);

            _menu.GetComponentInChildren<MenuOptionsGroup>().onMenuNavigation += OnMenuNavigation;
        }

        protected abstract void OnMenuNavigation(EMenuButton option);

        public virtual void Exit()
        {
            _menu.GetComponentInChildren<MenuOptionsGroup>().onMenuNavigation -= OnMenuNavigation;
            GameObject.Destroy(_menu);
        }

        public abstract void Update(float deltaTime);
    }
}
