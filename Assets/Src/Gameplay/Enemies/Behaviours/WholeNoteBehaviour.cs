using AIBehaviourAPI.Bt;
using AIBehaviourAPI.Bt.Composites;
using AIBehaviourAPI.Bt.Decorators;

namespace Gameplay.Enemies.Behaviours
{
    public class WholeNoteBehaviour : IPushDeathPerception
    {
        private WholeNote _enemy;
        private BT _bt;

        public WholeNoteBehaviour(WholeNote enemy)
        {
            _enemy = enemy;

            _bt = new BT("Whole note BT", 13, _enemy.actionDisplayTextLevel1);

            LeafNode attackLN = new LeafNode("Attack Leaf Node", _enemy.Attack, true);
            ConditionsAndActionsLeafNode moveCALN = new ConditionsAndActionsLeafNode("Move Leaf Node", () => !_enemy.IsInTarget(), () =>
                {
                    _enemy.Move();
                    _enemy.RestartMovementPreparation();
                }, true);
            LeafNode isPreparedForMoveLN = new LeafNode("Is Prepared for move Leaf Node", _enemy.IsMovementPrepared);
            
            LeafNode prepareMovementLN = new LeafNode("Prepare Movement Leaf Node", _enemy.PrepareMovement, true);
            LeafNode shieldLN = new LeafNode("Shield Leaf Node", _enemy.GiveShieldToNearbyAllies, true);
            
            ConditionsAndActionsLeafNode isAliveCALN = new ConditionsAndActionsLeafNode("Is Alive Leaf Node", () => !_enemy.IsAlive, _enemy.Death, true);
            InverseNode inverseIsAliveNode = new InverseNode("Inverse Is Alive Node", isAliveCALN);
            
            SelectorNode moveOrAttackSN = new SelectorNode("Move or Attack Selector Node", new ANodeBT[] { moveCALN, attackLN }, SelectorMode.Order);
            SequenceNode moveOrAttackSQN = new SequenceNode("Move or attack Sequence Node", new ANodeBT[] { isPreparedForMoveLN, moveOrAttackSN });
            
            SequenceNode prepareMoveAndShieldSQN = new SequenceNode("Prepare Move and Shield",  new ANodeBT[] { prepareMovementLN, shieldLN });
            
            SelectorNode aliveBranchSN = new SelectorNode("Alive Branch Selector Node", new ANodeBT[] { moveOrAttackSQN,  prepareMoveAndShieldSQN }, SelectorMode.Order);
            
            SequenceNode mainSequence = new SequenceNode("Main Sequence Node", new ANodeBT[] { inverseIsAliveNode, aliveBranchSN });
            
            LoopUntilFailNode mainLoopNode = new LoopUntilFailNode("Main Loop Node", mainSequence);
            
            _bt.RegisterNode(attackLN);
            _bt.RegisterNode(moveCALN);
            _bt.RegisterNode(isPreparedForMoveLN);
            
            _bt.RegisterNode(prepareMovementLN);
            _bt.RegisterNode(shieldLN);
            
            _bt.RegisterNode(isAliveCALN);
            _bt.RegisterNode(inverseIsAliveNode);
            
            _bt.RegisterNode(moveOrAttackSN);
            _bt.RegisterNode(moveOrAttackSQN);
            
            _bt.RegisterNode(prepareMoveAndShieldSQN);
            
            _bt.RegisterNode(aliveBranchSN);
            
            _bt.RegisterNode(mainSequence);
            
            _bt.RegisterNode(mainLoopNode);
            
            _bt.Init(mainLoopNode);
        }

        public void UpdateBehaviour()
        {
            _bt.Update();
        }

        public void PushDeath()
        {
            _bt.Update(); // Go To death Node and Action it
        }
    }
}

