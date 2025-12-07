using Gameplay.RhythmSystem;
using Gameplay.Waves;
using Gameplay.World;
using Input;
using System;
using UI.Menus;
using UI.Menus.States;
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
        [SerializeField] GameObject TowersButton;
        private bool useMobileInput = false;
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
        private Dancer _dancer;

        public override void Init()
        {

            useMobileInput = Application.isMobilePlatform
                         || SystemInfo.deviceType == DeviceType.Handheld;
            TowersButton.SetActive(false);
            _waveManager = ServiceLocatorSubsystem.Instance.GetService<WaveManager>();
            if (_waveManager == null)
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

            
            _dancer = ServiceLocatorSubsystem.Instance.GetService<Dancer>();
            if (_dancer == null)
            {
                Debug.LogError("GameplayManager::Init: Dancer is null");
                return;
            }
            _dancer.onDancerDeath += Defeat;

            InputReader.Instance.onChangeToBattlePhase += ChangeFightState;


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
            onFinishRhythmStateEnd.Invoke();
            onBuildStateStart.Invoke();

            _waveManager.InitNextWave();

            InputReader.Instance.EnableBuildActions();

        }

        // Must be called by the user with the button, so this GameplayManager must subscribe to that button
        public void ChangeFightState()
        {
            if (Currentstate == GameplayState.Fight) return;

            _currentState = GameplayState.Fight;

            FightAction();
        }
        private void FightAction()
        {
            _waveManager.StartWave();
            onBuildStateEnd.Invoke();
            
            onFightStateStart.Invoke();
            if (useMobileInput)
            {
                TowersButton.SetActive(true);
            }
            //#if UNITY_EDITOR
                //TowersButton.SetActive(true);
           // #endif
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
            Debug.Log("hi");
            if (_waveManager.nextWaveExists())
            {
                Victory();
            }
            ChangeBuildState();
        }

        public void Victory()
        {

            MenuManager.Instance.SetState(new UI.Menus.States.Victory());

        }

        public void Defeat()
        {

            MenuManager.Instance.SetState(new UI.Menus.States.Defeat());
                
        }

        private void OnDestroy()
        {
            InputReader.Instance.onChangeToBattlePhase -= ChangeFightState;
            _waveManager.onEnemyDeath -= OnEnemyDeath;
            _rhythmManager.onEndRhythm -= OnRhythmEnd;
            _dancer.onDancerDeath -= Defeat;
        }
    }
}

