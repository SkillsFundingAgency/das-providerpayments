using NLog;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestedPaymentsPassThroughProcessor
    {
        private readonly ILogger _logger;

        public CoInvestedPaymentsPassThroughProcessor(ILogger logger)
        {
            _logger = logger;
        }
        protected CoInvestedPaymentsPassThroughProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Co Invested Pass-Through Payments Processor.");
            _logger.Info("Finished Co Invested Pass-Through Payments Processor.");
        }
    }
}