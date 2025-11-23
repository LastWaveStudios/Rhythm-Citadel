using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    internal class DificultySelector : AMenuState
    {
        public DificultySelector() : base("Menus/DificultySelector") { }

        protected override void OnMenuNavigation(EMenuButton option)
        {
            switch (option)
            {
                case EMenuButton.Play:
                    MenuManager.Instance.ChangeSceneAndState("BaseLevel", new Gameplay());
                    break;
                case EMenuButton.Back:
                    MenuManager.Instance.SetState(MenuManager.Instance.GetPreviousState());
                    break;
                default:
                    Debug.LogError($"Main::OnMenuNavigation ERROR_UNKNOWN_OPTION: {option}");
                    return;
            }
        }
        public override void Update(float deltaTime) { }
    }
}

