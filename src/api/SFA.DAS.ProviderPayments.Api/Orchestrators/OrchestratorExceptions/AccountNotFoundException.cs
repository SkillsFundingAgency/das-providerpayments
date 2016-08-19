namespace SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions
{
    public class AccountNotFoundException : NotFoundException
    {
        public AccountNotFoundException()
            : base("The account requested does not exist")
        {
        }
    }
}