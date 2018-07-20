using NLog;

namespace SFA.DAS.Payments.Reference.Accounts.Processor
{
    public class ApiProcessor : IApiProcessor
    {
        private readonly ILogger _logger;
        private readonly IImportAccountsOrchestrator _importAccountsOrchestrator;
        private readonly IImportAccountLegalEntitiesOrchestrator _importAccountLegalEntitiesOrchestrator;

        public ApiProcessor(
            ILogger logger, 
            IImportAccountsOrchestrator importAccountsOrchestrator,
            IImportAccountLegalEntitiesOrchestrator importAccountLegalEntitiesOrchestrator)
        {
            _logger = logger;
            _importAccountsOrchestrator = importAccountsOrchestrator;
            _importAccountLegalEntitiesOrchestrator = importAccountLegalEntitiesOrchestrator;
        }


        public void Process()
        {
            _logger.Info("Started Accounts API Processor.");

            _importAccountsOrchestrator.ImportAccounts();
            //_importAccountLegalEntitiesOrchestrator.ImportAccountLegalEntities();

            _logger.Info("Finished Accounts API Processor.");
        }
    }
}
