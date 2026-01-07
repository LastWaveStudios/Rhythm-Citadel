using AIBehaviourAPI.Fsm;
using UnityEngine;

namespace Gameplay.Enemies.Behaviours
{
    public class BaseMinionBehaviour : IPushDeathPerception
    {
        private AEnemy _enemy;
        private FSM _rootFSM;
        public BaseMinionBehaviour(AEnemy enemy)
        {
            _enemy = enemy;
            
            _rootFSM = new FSM("Base Minion Behaviour", false, 2, 2);

            NodeFSM deathNodeFsm = new NodeFSM("Death NodeFSM", () =>
            {
                _enemy.Death();
                _rootFSM.Finish(); // Exit State so finish the fsm
            });
            #region AliveFSM
            FSM aliveFSM = new FSM("Alive FSM", true, 2, 3);
            
            NodeFSM prepareMovementNodeFsm = new NodeFSM("Prepare Movement", _enemy.PrepareMovement);
            NodeFSM moveNodeFsm = new NodeFSM("Move", _enemy.Move);
            NodeFSM collapseNodeFsm = new NodeFSM("Collapse", _enemy.Collapse);
            
            TransitionFSM goExplodeTransition = new TransitionFSM(prepareMovementNodeFsm, deathNodeFsm, "prepareMovement to death (attack)", _enemy.IsOverSomething, _enemy.Attack);
            TransitionFSM finishMovementPreparationTransition = new TransitionFSM(prepareMovementNodeFsm, moveNodeFsm, "prepareMovement to move", _enemy.CanMove);
            TransitionFSM movementFinishTransition = new TransitionFSM(moveNodeFsm, prepareMovementNodeFsm, "move to prepareMovement", () => true, _enemy.RestartMovementPreparation);
            
            TransitionFSM startCollapseTransition = new TransitionFSM(prepareMovementNodeFsm, collapseNodeFsm, "starting to collapse", (_enemy.isCollapsing));
            TransitionFSM collapsedFinishedTransition = new TransitionFSM(collapseNodeFsm, prepareMovementNodeFsm, "move to prepareMovement", () => true, _enemy.RestartMovementPreparation);
            
            aliveFSM.RegisterNode(prepareMovementNodeFsm);
            aliveFSM.RegisterNode(moveNodeFsm);
            aliveFSM.RegisterNode(collapseNodeFsm);

            aliveFSM.RegisterTransition(goExplodeTransition);
            aliveFSM.RegisterTransition(finishMovementPreparationTransition);
            aliveFSM.RegisterTransition(movementFinishTransition);

            aliveFSM.RegisterTransition(startCollapseTransition);
            aliveFSM.RegisterTransition(collapsedFinishedTransition);
            
            aliveFSM.Init(prepareMovementNodeFsm);
            aliveFSM.Pause(); // Always must have on the internal FSM
            #endregion
            NodeHFSM aliveNode = new NodeHFSM("Alive node", _rootFSM, aliveFSM);
            
            TransitionFSM deathTransition = new TransitionFSM(aliveNode, deathNodeFsm, "alive to death", () => !_enemy.IsAlive);
            
            _rootFSM.RegisterNode(deathNodeFsm);
            _rootFSM.RegisterNode(aliveNode);
            
            _rootFSM.RegisterTransition(deathTransition);
            
            _rootFSM.Init(aliveNode);
        }

        public void UpdateBehaviour()
        {
            _rootFSM.Update();
        }

        public void PushDeath()
        {
            _rootFSM.Update();
        }
    }
}

