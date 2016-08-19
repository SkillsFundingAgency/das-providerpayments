using System;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions
{
    public class PeriodNotFoundException : NotFoundException
    {
        public PeriodNotFoundException()
            : base("The period requested does not exist")
        {
        }
    }
}