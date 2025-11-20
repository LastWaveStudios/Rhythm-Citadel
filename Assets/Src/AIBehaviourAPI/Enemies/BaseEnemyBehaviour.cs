using System;
using AIBehaviourAPI.FSM;
using Gameplay.Enemies;
using UnityEngine;

namespace BehaviourAPI.Enemies
{
    public class BaseEnemyBehaviour : MonoBehaviour
    {
        private AEnemy _aEnemy;

        private FSM _fsm;

        private void Start()
        {
            _aEnemy = GetComponent<AEnemy>();

            _fsm = new FSM("Base Enemy Behaviour", false, 3, 3);

            NodeFSM movementPreparation = new NodeFSM("movementPreparation", _aEnemy.AddPreparation);
            NodeFSM movement = new NodeFSM("movement");
            NodeFSM dying = new NodeFSM("dying", _aEnemy.Attack);

            TransitionFSM startMovement = new TransitionFSM(movementPreparation, movement, "start movement", _aEnemy.isPrepared, ChangeToMovement);
            TransitionFSM startPreparation = new TransitionFSM(movement, movementPreparation, "start preparation", () => true);

            TransitionFSM redToGreen = new TransitionFSM(redNodeFsm, greenNodeFsm, "redToGreen", () => UnityEngine.Input.GetKey(KeyCode.G), ChangeGreen);
            TransitionFSM greenToBlue = new TransitionFSM(greenNodeFsm, blueNodeFsm, "greenToBlue", () => UnityEngine.Input.GetKey(KeyCode.B), ChangeBlue);
            TransitionFSM blueToRed = new TransitionFSM(blueNodeFsm, redNodeFsm, "blueToRed", () => UnityEngine.Input.GetKey(KeyCode.R), ChangeRed);

            _fsm.RegisterNode(redNodeFsm);
            _fsm.RegisterNode(greenNodeFsm);
            _fsm.RegisterNode(blueNodeFsm);
            _fsm.RegisterTransition(redToGreen);
            _fsm.RegisterTransition(greenToBlue);
            _fsm.RegisterTransition(blueToRed);

            _fsm.Init(redNodeFsm);
        }

        private void Update()
        {
            _fsm.Update();
        }

        #region Functions for agent

        private void ChangeToMovement()
        {

        }
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

