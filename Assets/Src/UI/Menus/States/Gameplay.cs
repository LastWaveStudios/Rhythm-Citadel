
using UnityEditor;
using UnityEngine;

namespace UI.Menus.States
{
    internal class Gameplay : IMenuState
    {
        public void Enter()
        {
            
        }

        public void Exit()
        {
            
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