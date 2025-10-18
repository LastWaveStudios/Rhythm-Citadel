using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class InputReader : Utilities.Singleton<InputReader>, Actions.ITowersActions
    {
        private Actions _actions;

        // The delegate { } are just for initialice them to something and ignore the null check on the invokes

        #region TowersMap
        public Action<int> onTapGroup = delegate { };
        #endregion
    

        #region EnablersAndDisablers
        private void OnEnable()
        {
            if (_actions == null)
            {
                _actions = new Actions();

                _actions.Towers.SetCallbacks(this);
            }

            EnableTowers();
        }

        private void OnDisable()
        {
            DisableAll();
        }

        private void DisableAll()
        {
            _actions.Towers.Disable();
        }

        public void EnableTowers()
        {
            DisableAll();
            _actions.Towers.Enable();
        }
        #endregion

        #region TowersMap
        public void OnGroup1(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onTapGroup.Invoke(0);
        }

        public void OnGroup2(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onTapGroup.Invoke(1);

        }

        public void OnGroup3(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onTapGroup.Invoke(2);
        }

        public void OnGroup4(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onTapGroup.Invoke(3);
        }

        public void OnGroup5(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onTapGroup.Invoke(4);
        }

        public void OnGroup6(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onTapGroup.Invoke(5);
        }
        #endregion
    }
}
