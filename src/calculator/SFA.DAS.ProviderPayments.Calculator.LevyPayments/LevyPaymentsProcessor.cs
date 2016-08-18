using NLog;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments
{
    public class LevyPaymentsProcessor
    {
        private readonly ILogger _logger;

        public LevyPaymentsProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public void Process()
        {
            _logger.Info("Started Levy Payments Processor.");
            _logger.Info("Finished Levy Payments Processor.");
        }
    }
}
