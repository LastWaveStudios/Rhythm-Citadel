

namespace AIBehaviourAPI.Exceptions
{
    public class StatusException : System.Exception
    {
        public ABehaviourEngine BehaviourEngine {  get; set; }

        public StatusException(ABehaviourEngine behaviourEngine)
        {
            BehaviourEngine = behaviourEngine;
        }

        public StatusException(ABehaviourEngine behaviourEngine, string message) : base(message)
        {
            BehaviourEngine = behaviourEngine;
        }
    }
}