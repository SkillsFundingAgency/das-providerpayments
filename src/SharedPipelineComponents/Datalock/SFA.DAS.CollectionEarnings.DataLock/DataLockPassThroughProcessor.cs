using NLog;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockPassThroughProcessor
    {
        private readonly ILogger _logger;

        public DataLockPassThroughProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public void Process()
        {
            _logger.Info("Started Data Lock Processor.");
            _logger.Info("Finished Data Lock Processor.");
        }
    }
}