using Gameplay.RhythmSystem;
using Gameplay.Waves;
using Input;
using System;
using UnityEngine;
using Utilities.ServiceLocator;



namespace Gameplay
{
    public enum GameplayState
    {
        None = 0,
        Build,
        Fight,
        FinishRhythm
    }

    public class GameplayManager : Utilities.ServiceLocator.AService
    {
        [SerializeField] private GameplayState _currentState = GameplayState.Build;
        public GameplayState Currentstate { get { return _currentState; } }

        #region events
        public Action onFightStateStart = delegate { };
        public Action onFightStateEnd = delegate { };
        public Action onFinishRhythmStateStart = delegate { };
        public Action onFinishRhythmStateEnd = delegate { };
        public Action onBuildStateStart = delegate { };
        public Action onBuildStateEnd = delegate { };
        public Action<GameplayState> onPause = delegate { };
        public Action<GameplayState> onResume = delegate { };
        #endregion

        private WaveManager _waveManager;
        private RhythmManager _rhythmManager;

        public override void Init()
        {
            Debug.Log("Init del GamplayManager");
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            if ( _waveManager == null )
            {
                Debug.LogError("GameplayManager::Init: The WaveManager is null");
                return;
            }
            _waveManager.onEnemyDeath += OnEnemyDeath;

            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("GameplayManager::Init: The RhythmManager is null");
                return;
            }
            _rhythmManager.onEndRhythm += OnRhythmEnd;

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
                ChangeFinishRhythmState();
            }
        }

        public void Resume()
        {
            Time.timeScale = 1.0f;
            onResume.Invoke(Currentstate);
        }

        public void Pause()
        {
            Time.timeScale = 0.0f;
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
            onFinishRhythmStateEnd.Invoke();
            onBuildStateStart.Invoke();
            

            _waveManager.InitNextWave();

            InputReader.Instance.EnableBuildActions();  //ESTO SI ESTA BIEN

        }

        // Must be called by the user with the button, so this GameplayManager must subscribe to that button
        private void ChangeFightState()
        {
            if (Currentstate == GameplayState.Fight) return;

            _currentState = GameplayState.Fight;

            FightAction();
        }
        public void FightAction()
        {
            _waveManager.StartWave();

            onBuildStateEnd.Invoke();
            onFightStateStart.Invoke();
            InputReader.Instance.EnableBattleActions();

        }

        private void ChangeFinishRhythmState()
        {
            if (Currentstate == GameplayState.FinishRhythm) return;

            _currentState = GameplayState.FinishRhythm;

            FinishRhythmAction();
        }

        private void FinishRhythmAction()
        {
            onFightStateEnd.Invoke();
            onFinishRhythmStateStart.Invoke();
        }

        private void OnRhythmEnd()
        {
            ChangeBuildState();
        }
    }
}

