using UnityEngine;
using UnityEngine.InputSystem;
namespace Input
{
    public class PhaseManager : MonoBehaviour
    {
        public Phase current { get; set; } = Phase.Build;
        public void SetPhase(Phase phase)
        {
            switch (phase)
            {
                case Phase.Build:
                    InputReader.Instance.EnableBuildActions();
                    break;
                case Phase.Battle:
                    InputReader.Instance.EnableBattleActions();
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

