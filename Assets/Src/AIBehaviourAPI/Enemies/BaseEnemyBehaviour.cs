using System;
using System.Collections;
using AIBehaviourAPI.FSM;
using Gameplay.Enemies;
using Gameplay.World;
using UnityEngine;

namespace BehaviourAPI.Enemies
{
    public class BaseEnemyBehaviour : MonoBehaviour
    {
        private AEnemy _aEnemy;

        private FSM _fsm;
        private bool _moveFinished;

        private NodeFSM _movementPreparation;
        private NodeFSM _movement;
        private NodeFSM _dying;
        private NodeFSM _attacking;
        private void Start()
        {
            _aEnemy = GetComponent<AEnemy>();
            _aEnemy.onDeath += HandleDeath;

            _aEnemy.OnBeatEvent += OnBeatUpdate;
            _fsm = new FSM("Base Enemy Behaviour", false, 3, 3);

            _movementPreparation = new NodeFSM("prepare", Prepare);
            _movement = new NodeFSM("move", OnMove);
            _dying = new NodeFSM("death", OnDeath);
            _attacking = new NodeFSM("attack", Attack);

            TransitionFSM startMovement = new TransitionFSM(_movementPreparation, _movement, "start movement", transitionToMovement, ()=> Debug.Log("start movement"));
            TransitionFSM startAttack = new TransitionFSM(_movementPreparation, _attacking, "start movement", transitionToAttack, ()=> Debug.Log("start attack"));
            TransitionFSM startPreparation = new TransitionFSM(_movement, _movementPreparation, "start preparation", () => true, () => _aEnemy.ResetPreparation());


            _fsm.RegisterNode(_movementPreparation);
            _fsm.RegisterNode(_movement);
            _fsm.RegisterNode(_dying);
            _fsm.RegisterNode(_attacking);
            _fsm.RegisterTransition(startMovement);
            _fsm.RegisterTransition(startPreparation);
            _fsm.RegisterTransition(startAttack);

            _fsm.Init(_movementPreparation);
        }

        private void OnBeatUpdate(AEnemy enemy)
        {
            _fsm.Update();
        }

        private void HandleDeath(AEnemy enemy)
        {
            _fsm.ChangeState(_dying);
        }

        private void Prepare()
        {
            _aEnemy.AddPreparation();
        }

        private void OnMove()
        {
            _aEnemy.Move();
        }

        private void OnDeath() 
        {
            _aEnemy.Death();
        }

        private void Attack() 
        {
            _aEnemy.SetDrop(0);
            _aEnemy.Attack();
            _aEnemy.Death();
        }
        private bool transitionToMovement()
        {
            return _aEnemy.isPrepared() && !_aEnemy.isInLastTile();
        }
        private bool transitionToAttack()
        {
            return _aEnemy.isPrepared() && _aEnemy.isInLastTile();
        }

    }
}

