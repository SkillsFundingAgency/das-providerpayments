using NLog;

namespace SFA.DAS.Payments.Reference.Accounts
{
    public class PassThroughProcessor
    {
        private readonly ILogger _logger;

        public PassThroughProcessor(ILogger logger)
        {
            _logger = logger;
        }
        protected PassThroughProcessor()
        {
            // For mocking
        }

        public virtual void Process()
        {
            _logger.Info("Started Accounts Pass Through Processor.");

            _logger.Info("Finished Accounts Pass Through Processor.");
        }

    }
}
