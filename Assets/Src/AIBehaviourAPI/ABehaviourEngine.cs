

using System;
using AIBehaviourAPI.Exceptions;

namespace AIBehaviourAPI
{
    public abstract class ABehaviourEngine
    {
        public Status Status { get; protected set; } = Status.None;

        public Action<Status> OnStatusChange { get; set; } = delegate { };


        public bool IsPaused { get {  return Status == Status.Paused; } }

        public bool IsExecuting { get { return StatusFlags.Unfinished.ANDBitWise(Status); } }


        public void Start()
        {
            if (Status != Status.None)
                throw new StatusException(this, "The initial status must be None");

            Status = Status.Running;
            OnStarted();
        }

        public void Stop()
        {
            if (Status == Status.None)
                throw new StatusException(this, "The status is None");

            Status = Status.None;
            OnStopped();
        }

        public void Update()
        {
            if (Status != Status.Running) return;
            OnUpdate();
        }

        public void Pause()
        {
            if (Status == Status.Running)
            {
                Status = Status.Paused;
                OnPaused();
            }
        }


        public void Unpause()
        {
            if (Status == Status.Paused)
            {
                Status = Status.Running;
                OnUnpaused();
            }
        }

        public void Finish(Status executionResult)
        {
            if (executionResult == Status.None || executionResult.ANDBitWise(Status.Running))
                throw new StatusException(this, $"The result of the execution can not be {executionResult}");

            Status = executionResult;
        }


        protected abstract void OnStarted();
        protected abstract void OnStopped();
        protected abstract void OnUpdate();
        protected abstract void OnPaused();
        protected abstract void OnUnpaused();
    }
}