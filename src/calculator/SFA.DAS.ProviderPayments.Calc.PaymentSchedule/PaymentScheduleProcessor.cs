using MediatR;
using NLog;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule
{
    public class PaymentScheduleProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public PaymentScheduleProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        protected PaymentScheduleProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            /*
             * For each earning
             *    Calculate payments required for month
             *    Store payment requirements in transient db
             */
        }
    }
}
