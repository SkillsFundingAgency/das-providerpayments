using System;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        {
            
        }
    }
}