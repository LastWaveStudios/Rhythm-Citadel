using Gameplay;
using Gameplay.World;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : Utilities.Singleton<InputReader>, Actions.IBattleActions, Actions.IBuildActions
    {
        public Actions actions;
        public Vector2 PointerPosition { get; private set; }
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
            actions = new Actions();
            actions.Disable();
            useMobileInput = Application.isMobilePlatform
                         || SystemInfo.deviceType == DeviceType.Handheld;
        }

        #region EnablersAndDisablers
        private void OnEnable()
        {
            if (actions == null)
            {
                actions = new Actions();

                actions.Battle.SetCallbacks(this);
                actions.Build.SetCallbacks(this);
            }
            actions.Battle.SetCallbacks(this);
            actions.Build.SetCallbacks(this);

            //
        }

        private void OnDisable()
        {
            actions.Battle.SetCallbacks(null);
            actions.Build.SetCallbacks(null);
            actions.Disable();
        }
        public void EnableBuildActions()
        {
            Debug.Log("BUILD habilitado");
            actions.Build.Enable();
            actions.Battle.Disable();
        }

        public void EnableBattleActions()
        {
            //Debug.Log("BATTLE habilitado");
            actions.Battle.Enable();
            actions.Build.Disable();
        }

        public void DisableAll()
        {
            actions.Disable();
        }

        #endregion

        public void OnPlaceTower(InputAction.CallbackContext context)
        {
            //Debug.Log("COLOCAR TORRES");
            if (context.phase == InputActionPhase.Started) onPlaceTower.Invoke();
        }
        public void OnChangeToBattlePhase(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) onChangeToBattlePhase.Invoke();
        }

        public void OnPointerPosition(InputAction.CallbackContext context)
        {
            PointerPosition =context.ReadValue<Vector2>();
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
            //Debug.Log("Disparo TAMBOR");
        }
        public void MobileGroup2()
        {
            onTapGroup.Invoke(1);
            // Debug.Log("Disparo PIANO");
        }
        public void MobileGroup5()
        {
            onTapGroup.Invoke(4);
            // Debug.Log("Disparo TROMPETA");
        }
        public void MobileGroup6()
        {
            onTapGroup.Invoke(5);
            // Debug.Log("Disparo VIOLIN");
        }

    }
}
