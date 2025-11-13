
using Gameplay;
using Gameplay.RhythmSystem;
using Gameplay.Waves;
using System;
using UI.Menus.Navigation;
using UnityEditor;
using UnityEngine;
using Utilities.ServiceLocator;

namespace UI.Menus.States
{
    internal class Gameplay : IMenuState
    {
        private GameplayManager _gameplayManager;

        public Gameplay() { }

        public void Enter()
        {
            if (_gameplayManager == null)
            {
                ServiceLocatorSubsystem.SubscribeToInitialice(Init);
                return;
            }
            ResumeGameplay();
        }

        private void Init()
        {
            _gameplayManager = ServiceLocatorSubsystem.Instance.GetService<GameplayManager>();
            if (_gameplayManager == null )
            {
                Debug.LogError("Gameplay::Init: The GameplayManager is null");
                return;
            }
            ResumeGameplay();
        }

        private void ResumeGameplay()
        {
            if (_gameplayManager == null) return;

            _gameplayManager.Resume();
        }

        public void Exit()
        {
            StopGameplay();
        }

        private void StopGameplay()
        {
            if (_gameplayManager == null) return;

            _gameplayManager.Pause();
        }

        public void Update(float deltaTime)
        {
            // TODO: Change to input system if we finally use it
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuManager.Instance.SetState(new Pause());
            }
        }
    }
}