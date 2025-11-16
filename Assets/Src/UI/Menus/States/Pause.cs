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
                    //TODO: Change to the name of MainMenuScene
                    MenuManager.Instance.ChangeSceneAndState("MainMenu", new Main());
                    break;
                default:
                    Debug.LogError($"Main::OnMenuNavigation ERROR_UNKNOWN_OPTION: {option}");
                    return;
            }
        }
        public override void Update(float deltaTime)
        {
            // TODO: Change to input system if we finally use it
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                MenuManager.Instance.SetState(new Gameplay());
            }
        }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 0.0f;
        }

        public override void Exit()
        {
            base.Exit();
            Time.timeScale = 1.0f;
        }


    }
}
