using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.ProviderPayments.Calc.Transfers
{
    public class TransfersProcessor
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

            

            _logger.Info("Finished Transfers Processor.");
        }
    }
}
