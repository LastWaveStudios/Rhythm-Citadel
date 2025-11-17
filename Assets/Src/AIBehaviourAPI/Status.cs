
using Unity.VisualScripting;

namespace AIBehaviourAPI
{
    public enum Status
    {
        None = 0,
        Running = 1,
        Succes = (1 << 1),
        Failure = (1 << 2),
        Paused = (1 << 3)
    }

    public enum StatusFlags
    {
        None = 0,
        Running = 1,
        Succes = (1 << 1),
        Failure = (1 << 2),
        Finished = Running | Failure,
        Paused = (1 << 3),
        Unfinished = Running | Paused,
        Active = Running | Paused | Succes | Failure
    }

    public static class StatusExtension
    {
        public static bool ANDBitWise(this Status status1, Status status2)
        {
            return ((int)status1 & (int)status2) != 0;
        }

        public static bool ANDBitWise(this Status status1, StatusFlags status2)
        {
            return ((int)status1 & (int)status2) != 0;
        }

        public static bool ANDBitWise(this StatusFlags status1, Status status2)
        {
            return ((int)status1 & (int)status2) != 0;
        }
    }
    

}
