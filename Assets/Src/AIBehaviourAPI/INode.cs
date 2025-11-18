using System.Collections.Generic;


namespace AIBehaviourAPI
{
    public interface INode
    {
        public System.Action Action { get; }
        
        public List<ITransition> InputTransitions { get; }
        
        public List<ITransition> OutputTransitions { get; }
        
        public string Name { get; }
    }
}