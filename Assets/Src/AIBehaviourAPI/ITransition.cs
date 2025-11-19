namespace AIBehaviourAPI
{
    public interface ITransition
    {
        public INode OriginNode { get; }
        
        public INode DestinationNode { get; }
        
        public System.Action Action { get; }
        
        public System.Func<bool> Condition { get; }
        public string Name { get; }
    }
}