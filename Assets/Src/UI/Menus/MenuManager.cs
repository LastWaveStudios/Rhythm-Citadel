using UnityEngine;
using UI.Menus.States;
using UnityEngine.SceneManagement;


namespace UI.Menus
{
    // Context of the state pattern
    public class MenuManager : Utilities.Singleton<MenuManager>
    {
        private IMenuState _currentState;
        private IMenuState _previousState;

        private IMenuState _nextStateWhenChangingScene;

        private void Start()
        {
            _previousState = null;
            SetState(new States.Main());
        }

        public void SetState(IMenuState newState)
        {
            _previousState = _currentState;

            _currentState?.Exit();

            _currentState = newState;

            _currentState.Enter();
        }

        public IMenuState GetState() { return _currentState; }

        public IMenuState GetPreviousState() { return _previousState; }

        public void Update()
        {
            _currentState?.Update(Time.deltaTime);
        }

        public void ChangeSceneAndState(string sceneName, IMenuState stateToChangeWhenLoaded)
        {
            _nextStateWhenChangingScene = stateToChangeWhenLoaded;
            SceneManager.sceneLoaded += OnSceneChange;
            SceneManager.LoadScene(sceneName);
        }

        private void OnSceneChange(Scene scene,  LoadSceneMode mode)
        {
            SetState(_nextStateWhenChangingScene);
            _nextStateWhenChangingScene = null;
            SceneManager.sceneLoaded -= OnSceneChange;
        }
    }
}


