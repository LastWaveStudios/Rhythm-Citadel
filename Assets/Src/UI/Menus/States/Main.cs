using UI.Menus.Navigation;
using UnityEditor;
using UnityEngine;

namespace UI.Menus.States
{
    internal class Main : AMenuState
    {
        public Main() : base("Menus/MainMenu") { }

        protected override void OnMenuNavigation(EMenuButton option)
        {
            switch (option)
            {
                case EMenuButton.Play:
                    MenuManager.Instance.SetState(new LevelSelector());
                    break;
                case EMenuButton.Options:
                    MenuManager.Instance.SetState(new Options());
                    break;
                case EMenuButton.Tutorial:
                    MenuManager.Instance.SetState(new Tutorial());
                    break;
                case EMenuButton.Credits:
                    MenuManager.Instance.SetState(new Credits());
                    break;
                case EMenuButton.Exit:
#if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;
                default:
                    Debug.LogError($"Main::OnMenuNavigation ERROR_UNKNOWN_OPTION: {option}");
                    return;
            }
        }
        public override void Update(float deltaTime) { }
    }
}
