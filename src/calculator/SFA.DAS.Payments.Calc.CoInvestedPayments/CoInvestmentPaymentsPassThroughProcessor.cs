using NLog;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestmentPaymentsPassThroughProcessor
    {
        private readonly ILogger _logger;

        public CoInvestmentPaymentsPassThroughProcessor(ILogger logger)
        {
            _logger = logger;
        }
        protected CoInvestmentPaymentsPassThroughProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Co Invested Payments Processor.");
            _logger.Info("Finished Co Invested Payments Processor.");
        }
    }
}