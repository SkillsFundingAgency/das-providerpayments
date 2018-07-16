using NLog;

namespace SFA.DAS.Payments.Reference.Accounts.Processor
{
    public class ApiProcessor : IApiProcessor
    {
        private readonly ILogger _logger;
        private readonly ICopyAccountsOrchestrator _copyAccountsOrchestrator;

        public ApiProcessor(ILogger logger, ICopyAccountsOrchestrator copyAccountsOrchestrator)
        {
            _logger = logger;
            _copyAccountsOrchestrator = copyAccountsOrchestrator;
        }


        public void Process()
        {
            _logger.Info("Started Accounts API Processor.");
            _copyAccountsOrchestrator.ImportAccounts();
            _logger.Info("Finished Accounts API Processor.");
        }
    }
}
