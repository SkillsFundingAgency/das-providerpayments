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
        protected LevyPaymentsProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Levy Payments Processor.");

            // Get earnings for account
            // Draw down levy for learners in account
            // Output amount paid with levy to db

            _logger.Info("Finished Levy Payments Processor.");
        }
    }
}
