using System;

namespace ProviderPayments.TestStack.Core.ExecutionStatus
{
    public abstract class StatusWatcherBase
    {
        public virtual void ExecutionStarted(TaskDescriptor[] tasks)
        {
        }
        public virtual void TaskStarted(string taskId)
        {
        }
        public virtual void TaskCompleted(string taskId, Exception error)
        {
        }
        public virtual void ExecutionCompleted(Exception error)
        {
        }
    }
}
