
using Gameplay.RhythmSystem;
using Gameplay.Waves;
using UI.Menus.Navigation;
using UnityEditor;
using UnityEngine;

namespace UI.Menus.States
{
    internal class Gameplay : AMenuState
    {
        public Gameplay() : base("Menus/GameplayMenu") { }

        protected override void OnMenuNavigation(EMenuButton option)
        {
            switch (option)
            {
                case EMenuButton.StartWave:
                    RhythmManager.Instance.UseMeasure(WaveManager.Instance._pattern.signature, WaveManager.Instance.BPM);
                    WaveManager.Instance.InitNextWave();
                    WaveManager.Instance.StartWave();
                    break;
                default:
                    Debug.LogError($"Main::OnMenuNavigation ERROR_UNKNOWN_OPTION: {option}");
                    return;
            }
        }

        public override void Update(float deltaTime)
        {
            // TODO: Change to input system if we finally use it
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuManager.Instance.SetState(new Pause());
            }
        }
    }
}