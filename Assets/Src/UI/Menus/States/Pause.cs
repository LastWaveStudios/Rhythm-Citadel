using UI.Menus.Navigation;
using UnityEditor;
using UnityEngine;

namespace UI.Menus.States
{
    internal class Pause : AMenuState
    {
        public Pause() : base("Menus/PauseMenu") { }

        protected override void OnMenuNavigation(EMenuButton option)
        {
            switch (option)
            {
                case EMenuButton.Resume:
                    // TODO: Maybe store the gameplay state for returning to that instance if we used for something useful
                    MenuManager.Instance.SetState(new Gameplay());
                    break;
                case EMenuButton.Options:
                    MenuManager.Instance.SetState(new Options());
                    break;
                case EMenuButton.Tutorial:
                    MenuManager.Instance.SetState(new Tutorial());
                    break;
                case EMenuButton.ReturnToMainMenu:
                    MenuManager.Instance.SetState(new Main());
                    break;
                default:
                    Debug.LogError($"Main::OnMenuNavigation ERROR_UNKNOWN_OPTION: {option}");
                    return;
            }
        }
        public override void Update(float deltaTime) { }
    }
}
