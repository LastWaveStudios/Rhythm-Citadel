using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    internal class LevelSelector : AMenuState
    {
        public LevelSelector() : base("Menus/LevelSelectorMenu") { }

        protected override void OnMenuNavigation(EMenuButton option)
        {
            switch (option)
            {
                case EMenuButton.Play:
                    // TODO: Add the change scenes to the actual level selected and all that logic
                    MenuManager.Instance.SetState(new Gameplay());
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
