using System;
using System.Collections.Generic;
using System.Text;

namespace AIBehaviourAPI.FSM
{
    public class NodeFSM : INode
    {
        protected Action _action;
        protected List<ITransition> _inputTransitions;
        protected List<ITransition> _outputTransitions;
        protected string _name;
        
        public Action Action => _action;
        public List<ITransition> InputTransitions => _inputTransitions;
        public List<ITransition> OutputTransitions => _outputTransitions;
        public string Name => _name;
        public NodeFSM(string name, Action action = null)
        {
            _action = action;
            _name = name;
            _inputTransitions = new List<ITransition>();
            _outputTransitions = new List<ITransition>();
        }
        public override string ToString()
        {
            return $"N: {_name} IT: [{TransitionsListToString(_inputTransitions)}]; OT: [{TransitionsListToString(_outputTransitions)}]";
        }

        private string TransitionsListToString(List<ITransition> transitions)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < transitions.Count - 1; ++i)
            {
                builder.Append($"{transitions[i].Name}, ");
            }
            builder.Append($"{transitions[transitions.Count - 1].Name}");
            return builder.ToString();
        }
    }
}