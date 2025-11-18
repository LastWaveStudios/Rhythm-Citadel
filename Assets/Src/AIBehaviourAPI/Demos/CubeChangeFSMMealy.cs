using System;
using AIBehaviourAPI.FSM;
using UnityEngine;

namespace BehaviourAPI.Demos
{
    public class CubeChangeFSMMealy : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private FSM _fsm;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _fsm = new FSM("Cube Change");
            
            Node redNode = new Node("Red");
            Node greenNode = new Node("Green");
            Node blueNode = new Node("Blue");
            
            Transition redToGreen = new Transition(redNode, greenNode, "redToGreen", () => UnityEngine.Input.GetKey(KeyCode.G), ChangeGreen);
            Transition greenToBlue = new Transition(greenNode, blueNode, "greenToBlue", () => UnityEngine.Input.GetKey(KeyCode.B), ChangeBlue);
            Transition blueToRed = new Transition(blueNode, redNode, "blueToRed", () => UnityEngine.Input.GetKey(KeyCode.R), ChangeRed);
            
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
            Debug.Log("Change Red");
            _spriteRenderer.color = Color.red;
        }

        private void ChangeGreen()
        {
            Debug.Log("Change Green");
            _spriteRenderer.color = Color.green;
        }

        private void ChangeBlue()
        {
            Debug.Log("Change Blue");
            _spriteRenderer.color = Color.blue;
        }
        #endregion
    }
}

