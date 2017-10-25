using NLog;

namespace SFA.DAS.Payments.Reference.Commitments
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
            _logger.Info("Started Commitments Pass Through Processor.");

            _logger.Info("Finished Commitments Pass Through Processor.");
        }
    }
}