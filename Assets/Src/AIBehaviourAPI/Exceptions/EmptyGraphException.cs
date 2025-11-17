

namespace AIBehaviourAPI.Exceptions
{
    public class EmptyGraphException : System.Exception
    {
        public ABehaviourGraph graph;

        public EmptyGraphException(ABehaviourGraph graph)
        {
            this.graph = graph;
        }

        public EmptyGraphException(ABehaviourGraph graph, string message) : base(message)
        {
            this.graph = graph;
        }
    }
}