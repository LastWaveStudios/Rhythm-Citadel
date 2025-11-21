using AIBehaviourAPI.FSM;

namespace Gameplay.Enemies.Behaviours
{
    public class BaseEnemyBehaviour : IPushDeathPerception
    {
        private AEnemy _enemy;
        private FSM _rootFSM;

        public BaseEnemyBehaviour(AEnemy enemy)
        {
            _enemy = enemy;
            
            _rootFSM = new FSM("Base Enemy Behaviour", false, 2, 2);

            NodeFSM deathNode = new NodeFSM("Death Node", () =>
            {
                _rootFSM.Finish(); // Exit State so finish the fsm
                _enemy.Death();
            });
            #region AliveFSM
            FSM aliveFSM = new FSM("Alive FSM", true, 2, 3);
            
            NodeFSM prepareMovementNode = new NodeFSM("Prepare Movement", _enemy.PrepareMovement);
            NodeFSM moveNode = new NodeFSM("Move", _enemy.Move);
            
            TransitionFSM finishMovementPreparationTransition = new TransitionFSM(prepareMovementNode, moveNode, "prepareMovement to move", _enemy.IsMovementPrepared);
            TransitionFSM movementFinishTransition = new TransitionFSM(moveNode, prepareMovementNode, "move to prepareMovement", () => true, _enemy.RestartMovementPreparation);
            TransitionFSM goAttackTransition = new TransitionFSM(prepareMovementNode, deathNode, "prepareMovement to death (attack)", _enemy.IsInTarget, _enemy.Attack);
            
            aliveFSM.RegisterNode(prepareMovementNode);
            aliveFSM.RegisterNode(moveNode);

            aliveFSM.RegisterTransition(finishMovementPreparationTransition);
            aliveFSM.RegisterTransition(movementFinishTransition);
            aliveFSM.RegisterTransition(goAttackTransition);
            
            aliveFSM.Init(prepareMovementNode);
            aliveFSM.Pause(); // Always must have on the internal FSM
            #endregion
            NodeHFSM aliveNode = new NodeHFSM("Alive node", _rootFSM, aliveFSM);
            
            TransitionFSM deathTransition = new TransitionFSM(aliveNode, deathNode, "alive to death", () => !_enemy.IsAlive);
            
            _rootFSM.RegisterNode(deathNode);
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
            _rootFSM.Update(); // Go to Death state
            _rootFSM.Update(); // Immediate second Update for do the action of Death because in this case we want to have it immediate
        }
    }
}

