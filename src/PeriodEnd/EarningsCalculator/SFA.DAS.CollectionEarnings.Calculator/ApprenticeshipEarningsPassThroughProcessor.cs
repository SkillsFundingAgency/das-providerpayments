using MediatR;
using NLog;

namespace SFA.DAS.CollectionEarnings.Calculator
{
    public class ApprenticeshipEarningsPassThroughProcessor
    {
        private readonly ILogger _logger;

        public ApprenticeshipEarningsPassThroughProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
        }

        protected ApprenticeshipEarningsPassThroughProcessor()
        {
        }

        public virtual void Process()
        {
            _logger.Info("Started Apprenticeship Earnings pass through Processor.");
            _logger.Info("Finished Apprenticeship Earnings pass through Processor.");
        }
    }
}