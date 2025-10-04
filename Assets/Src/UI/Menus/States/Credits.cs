using UI.Menus.Navigation;
using UnityEngine;

namespace UI.Menus.States
{
    internal class Credits : AMenuState
    {
        public Credits() : base("Menus/CreditsMenu") { }

        protected override void OnMenuNavigation(EMenuButton option)
        {
            switch (option)
            {
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
