using UI.Menus.Navigation;
using UnityEditor;
using UnityEngine;


namespace UI.Menus.States
{
    internal class Victory : AMenuState
    {

        public Victory() : base("Menus/Victory") { }

        protected override void OnMenuNavigation(EMenuButton option)
        {
            switch (option)
            {
                case EMenuButton.Play:
                    MenuManager.Instance.ChangeSceneAndState("BaseLevel", new Gameplay());
                    break;
                case EMenuButton.ReturnToMainMenu:
                    MenuManager.Instance.ChangeSceneAndState("MainMenu", new Main());
                    break;
                default:
                    Debug.LogError($"Victory::OnMenuNavigation ERROR_UNKNOWN_OPTION: {option}");
                    return;
            }
        }
        public override void Update(float deltaTime) { }

    }
}