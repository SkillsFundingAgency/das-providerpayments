using CS.Common.External.Interfaces;
using NLog;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Context;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.DependencyResolution;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Exceptions;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Logging;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments
{
    public class LevyPaymentsTask : IExternalTask
    {
        private readonly IDependencyResolver _dependencyResolver;

        public LevyPaymentsTask()
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal LevyPaymentsTask(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public void Execute(IExternalContext context)
        {
            _dependencyResolver.Init(typeof(LevyPaymentsProcessor));

            var contextWrapper = new ContextWrapper(context);

            ValidateContext(contextWrapper);

            LoggingConfig.ConfigureLogging(
                contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)
            );

            var logger = _dependencyResolver.GetInstance<ILogger>();

            var processor = new LevyPaymentsProcessor(logger);

            processor.Process();
        }

        private void ValidateContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new LevyPaymentsInvalidContextException(LevyPaymentsExceptionMessages.ContextPropertiesNoConnectionString);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new LevyPaymentsInvalidContextException(LevyPaymentsExceptionMessages.ContextPropertiesNoLogLevel);
            }
        }
    }
}
