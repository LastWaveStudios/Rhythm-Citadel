using UnityEngine;
using UnityEngine.InputSystem;
namespace Input
{
    public class PhaseManager : MonoBehaviour
    {
        private PlayerInput inputs;
        public Phase current { get; set; } = Phase.Build;

        /* private void Awake()
         {
             Set
         }*/
        public void SetPhase(Phase phase)
        {
            switch (phase)
            {
                case Phase.Build:
                    inputs.SwitchCurrentActionMap("Build");
                    break;
                case Phase.Battle:
                    inputs.SwitchCurrentActionMap("Battle");
                    break;
            }
            Debug.Log("Cambiar de fase");
        }
    }

    public enum Phase
    {
        Build,
        Battle
    }
}

