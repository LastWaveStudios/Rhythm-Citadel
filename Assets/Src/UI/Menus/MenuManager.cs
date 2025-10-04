using UnityEngine;
using UI.Menus.States;


namespace UI.Menus
{
    // Context of the state pattern
    public class MenuManager : Utilities.Singleton<MenuManager>
    {
        private IMenuState _currentState;
        private IMenuState _previousState;

        private void Start()
        {
            _previousState = null;
            SetState(new Main());
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
    }
}


