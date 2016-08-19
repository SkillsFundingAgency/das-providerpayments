using System;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
            : base("The account requested does not exist")
        {
        }
    }
}