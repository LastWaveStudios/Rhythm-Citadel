using Gameplay.RhythmSystem;
using System;
using UnityEngine;


namespace Gameplay
{
    public enum GameplayState
    {
        None = 0,
        Build,
        Fight
    }

    public class GameplayManager : Utilities.Subsystem<GameplayManager>
    {
        [SerializeField] private GameplayState _currentState = GameplayState.Build;
        public GameplayState Currentstate { get { return _currentState; } }

        #region Init
        public bool IsInitialice { get; private set; } = false;
        public Action onInitialize = delegate { };
        #endregion

        #region events
        public Action onFightStateStart = delegate { };
        public Action onFightStateEnd = delegate { };
        public Action onBuildStateStart = delegate { };
        public Action onBuildStateEnd = delegate { };
        public Action<GameplayState> onPause = delegate { };
        public Action<GameplayState> onResume = delegate { };
        #endregion
        private void Start()
        {
            switch(this.Currentstate)
            {
                case GameplayState.Build:
                    BuildAction();
                    break;
                case GameplayState.Fight:
                    FightAction();
                    break;
            }
            IsInitialice = true;
        }

        public void OnEnable()
        {
            onResume.Invoke(Currentstate);
        }

        public void OnDisable()
        {
            onPause.Invoke(Currentstate);
        }

        private void ChangeBuildState()
        {
            if (Currentstate == GameplayState.Build) return;

            _currentState = GameplayState.Build;

            BuildAction();
        }

        private void BuildAction()
        {
            onBuildStateStart.Invoke();
            onFightStateEnd.Invoke();
            // TODO: Activate the InputMap

        }

        private void ChangeFightState()
        {
            if (Currentstate == GameplayState.Fight) return;

            _currentState = GameplayState.Fight;

            FightAction();
        }
        private void FightAction()
        {
            onFightStateStart.Invoke();
            onBuildStateEnd.Invoke();
            // TODO: Activate the map of Fight

        }
    }
}

