using NLog;

namespace SFA.DAS.Payments.Reference.Accounts.Processor
{
    public class ApiProcessor : IApiProcessor
    {
        private readonly ILogger _logger;
        private readonly IImportAccountsOrchestrator _importAccountsOrchestrator;

        public ApiProcessor(ILogger logger, IImportAccountsOrchestrator importAccountsOrchestrator)
        {
            _logger = logger;
            _importAccountsOrchestrator = importAccountsOrchestrator;
        }


        public void Process()
        {
            _logger.Info("Started Accounts API Processor.");
            _importAccountsOrchestrator.ImportAccounts();
            _logger.Info("Finished Accounts API Processor.");
        }
    }
}
