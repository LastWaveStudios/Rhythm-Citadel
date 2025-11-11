using Gameplay.RhythmSystem;
using Gameplay.Waves;
using System;
using UnityEngine;
using Utilities.ServiceLocator;


namespace Gameplay
{
    public enum GameplayState
    {
        None = 0,
        Build,
        Fight
    }

    public class GameplayManager : Utilities.ServiceLocator.AService
    {
        [SerializeField] private GameplayState _currentState = GameplayState.Build;
        public GameplayState Currentstate { get { return _currentState; } }

        #region events
        public Action onFightStateStart = delegate { };
        public Action onFightStateEnd = delegate { };
        public Action onBuildStateStart = delegate { };
        public Action onBuildStateEnd = delegate { };
        public Action<GameplayState> onPause = delegate { };
        public Action<GameplayState> onResume = delegate { };
        #endregion

        private WaveManager _waveManager;

        public override void Init()
        {
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            if ( _waveManager == null )
            {
                Debug.LogError("GameplayManager::Init: The WaveManager is null");
                return;
            }
            _waveManager.onEnemyDeath += OnEnemyDeath;

            switch (this.Currentstate)
            {
                case GameplayState.Build:
                    BuildAction();
                    break;
                case GameplayState.Fight:
                    _waveManager.InitNextWave();
                    FightAction();
                    break;
            }
        }

        private void OnEnemyDeath(int vinyls)
        {
            if (_waveManager.AllEnemiesDeadInCurrentWave)
            {
                ChangeBuildState();
            }
        }

        public void Resume()
        {
            onResume.Invoke(Currentstate);
        }

        public void Pause()
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

            _waveManager.InitNextWave();

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
            _waveManager.StartWave();

            onFightStateStart.Invoke();
            onBuildStateEnd.Invoke();
            // TODO: Activate the map of Fight

        }

        
    }
}

