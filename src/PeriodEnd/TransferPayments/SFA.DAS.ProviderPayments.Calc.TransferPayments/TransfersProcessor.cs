using System.Linq;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments
{
    public partial class TransfersProcessor
    {
        private readonly ILogger _logger;
        private readonly IAmATransferRepository _transferRepository;
        private readonly IProcessLevyTransfers _levyTransferProcessor;
        private readonly ContextWrapper _context;

        public TransfersProcessor(
            ILogger logger, 
            ContextWrapper context, 
            IAmATransferRepository transferRepository, 
            IProcessLevyTransfers levyTransferProcessor)
        {
            _logger = logger;
            
            _context = context;
            _transferRepository = transferRepository;
            _levyTransferProcessor = levyTransferProcessor;
        }

        public TransfersProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Transfers Processor.");

            // Get a list of payments with transfers
            var payments = _transferRepository.RequiredTransferPayments();
            var groupedPayments = payments.GroupBy(x => x.TransferSendingEmployerAccountId);

            // In parallel, process each sending employer
            _logger.Info("Processing transfers");
            Parallel.ForEach(groupedPayments, x =>
            {
                var results = _levyTransferProcessor.ProcessSendingAccount(x.Key, x);
                _logger.Info($"Saving transfer payment sets for sending account {x.Key}");
                _transferRepository.AddTransfers(results);
                _logger.Info($"Finished transfer processing for account {x.Key}");
            });

            _logger.Info("Finished Transfers Processor.");
        }
    }
}
