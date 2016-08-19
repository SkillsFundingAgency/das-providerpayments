using System;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions
{
    public class PageNotFoundException : NotFoundException
    {
        public PageNotFoundException()
            : base("The page requested does not exist")
        {
        }
    }
}