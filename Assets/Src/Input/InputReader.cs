using Gameplay;
using Gameplay.World;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : Utilities.Singleton<InputReader>, Actions.IBattleActions, Actions.IBuildActions
    {
        private Actions _actions;
        private bool useMobileInput = false;

        // The delegate { } are just for initialice them to something and ignore the null check on the invokes

        #region Events
        public Action<int> onTapGroup = delegate { };
        
        public Action onPlaceTower = delegate { };
        public Action onChangeToBattlePhase = delegate { };
        #endregion
        private void Awake()
        {
            base.Awake();
            _actions = new Actions();
            EnableBuildActions();
            useMobileInput = Application.isMobilePlatform
                         || SystemInfo.deviceType == DeviceType.Handheld;
        }

        #region EnablersAndDisablers
        private void OnEnable()
        {
            if (_actions == null)
            {
                _actions = new Actions();

                _actions.Battle.SetCallbacks(this);
                _actions.Build.SetCallbacks(this);
            }
            _actions.Battle.SetCallbacks(this);
            _actions.Build.SetCallbacks(this);

            //
        }

        private void OnDisable()
        {
            _actions.Battle.SetCallbacks(null);
            _actions.Build.SetCallbacks(null);
            _actions.Disable();
        }
        public void EnableBuildActions()
        {
            _actions.Build.Enable();
            _actions.Battle.Disable();
        }

        public void EnableBattleActions()
        {
            _actions.Battle.Enable();
            _actions.Build.Disable();
        }

        #endregion

        public void OnPlaceTower(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onPlaceTower.Invoke();
        }
        public void OnChangeToBattlePhase(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onChangeToBattlePhase.Invoke();
        }


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

        //MOBILE
        public void MobileGroup1()
        {
            onTapGroup.Invoke(0);
        }
        public void MobileGroup2()
        {
            onTapGroup.Invoke(1);
        }
        public void MobileGroup5()
        {
            onTapGroup.Invoke(4);
        }
        public void MobileGroup6()
        {
            onTapGroup.Invoke(5);
        }

    }
}
