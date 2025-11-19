using System;

namespace AIBehaviourAPI.FSM
{
    public class TransitionFSM : ITransition
    {
        private INode _originNode;
        private INode _destinationNode;
        private Action _action;
        private Func<bool> _condition;
        private string _name;
        
        public INode OriginNode => _originNode;
        public INode DestinationNode => _destinationNode;
        public Action Action => _action;
        public Func<bool> Condition => _condition;
        public string Name => _name;
        
        public TransitionFSM(INode originNode, INode destinationNode, string name, Func<bool> condition, Action action = null)
        {
            _originNode = originNode;
            _destinationNode = destinationNode;
            _name = name;
            _condition = condition;
            _action = action;
        }

        public override string ToString()
        {
            return $"O: {_originNode.Name} -> N: {_name} -> D: {_destinationNode.Name}";
        }
    }
}