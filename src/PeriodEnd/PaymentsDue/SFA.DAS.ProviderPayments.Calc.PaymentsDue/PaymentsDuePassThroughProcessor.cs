using MediatR;
using NLog;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDuePassThroughProcessor
    {
        private readonly ILogger _logger;

        public PaymentsDuePassThroughProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
        }

        protected PaymentsDuePassThroughProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Payments Due Pass Through Processor.");

            _logger.Info("Finished Payments Due Pass Through Processor.");
        }
    }
}