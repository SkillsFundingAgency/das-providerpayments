using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments
{
    public partial class TransfersProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ContextWrapper _context;

        public TransfersProcessor(ILogger logger, IMediator mediator, ContextWrapper context)
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }

        protected TransfersProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Transfers Processor.");

            // Get a list of accounts with transfers

            // In parallel, process each sending employer

            _logger.Info("Finished Transfers Processor.");
        }

        TransferPaymentSet ProcessSendingAccount(DasAccount account)
        {
            var result = new TransferPaymentSet();
            


            return result;
        }
    }
}
