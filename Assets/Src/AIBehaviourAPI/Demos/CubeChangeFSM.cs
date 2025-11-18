using System;
using AIBehaviourAPI.FSM;
using UnityEngine;

namespace BehaviourAPI.Demos
{
    public class CubeChangeFSM : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private FSM _fsm;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _fsm = new FSM("Cube Change");
            
            Node redNode = new Node("Red", ChangeRed);
            Node greenNode = new Node("Green", ChangeGreen);
            Node blueNode = new Node("Blue",  ChangeBlue);
            
            Transition redToGreen = new Transition(redNode, greenNode, "redToGreen", () => UnityEngine.Input.GetKey(KeyCode.G), () => Debug.Log("Red -> Green"));
            Transition greenToBlue = new Transition(greenNode, blueNode, "greenToBlue", () => UnityEngine.Input.GetKey(KeyCode.B), () => Debug.Log("Green -> Blue"));
            Transition blueToRed = new Transition(blueNode, redNode, "blueToRed", () => UnityEngine.Input.GetKey(KeyCode.R), () => Debug.Log("Blue -> Red"));
            
            _fsm.RegisterNode(redNode);
            _fsm.RegisterNode(greenNode);
            _fsm.RegisterNode(blueNode);
            _fsm.RegisterTransition(redToGreen);
            _fsm.RegisterTransition(greenToBlue);
            _fsm.RegisterTransition(blueToRed);
            
            _fsm.Init(redNode);
        }

        private void Update()
        {
            _fsm.Update();
        }

        #region Functions for agent

        private void ChangeRed()
        {
            _spriteRenderer.color = Color.red;
        }

        private void ChangeGreen()
        {
            _spriteRenderer.color = Color.green;
        }

        private void ChangeBlue()
        {
            _spriteRenderer.color = Color.blue;
        }
        #endregion
    }
}

