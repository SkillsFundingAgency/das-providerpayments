using JetBrains.Annotations;
using NLog;

namespace SFA.DAS.ProviderPayments.Calc.Refunds
{
    public class RefundsProcessor
    {
        private readonly ILogger _logger;

        [UsedImplicitly]
        public RefundsProcessor(
            ILogger logger)
        {
            _logger = logger;
        }

        public virtual void Process()
        {
            _logger.Info("Started Refunds Processor.");
        }
    }
}
