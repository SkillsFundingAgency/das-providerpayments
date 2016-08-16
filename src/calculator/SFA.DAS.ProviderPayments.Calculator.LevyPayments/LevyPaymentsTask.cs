using CS.Common.External.Interfaces;
using NLog;
using SFA.DAS.ProviderPayments.Calculator.Domain.DependencyResolution;
using SFA.DAS.ProviderPayments.Calculator.Infrastructure.DcContext;
using SFA.DAS.ProviderPayments.Calculator.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calculator.Infrastructure.Logging;
using System;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments
{
    public class LevyPaymentsTask : IExternalTask
    {
        private readonly IDependencyResolver _dependencyResolver;
        private DcContextWrapper _contextWrapper;

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
            _contextWrapper = new DcContextWrapper(context);

            ValidateContext(_contextWrapper);

            LoggingConfig.ConfigureLogging(
                _contextWrapper.GetPropertyValue(DcContextPropertyKeys.TransientDatabaseConnectionString),
                _contextWrapper.GetPropertyValue(DcContextPropertyKeys.LogLevel)
            );

            var logger = _dependencyResolver.GetInstance<ILogger>();

            var processor = new LevyPaymentsProcessor(logger);

            processor.Process();
        }

        private void ValidateContext(DcContextWrapper contextWrapper)
        {
            if (contextWrapper.GetPropertyValue(DcContextPropertyKeys.TransientDatabaseConnectionString) == null)
            {
                throw new ArgumentNullException(DcContextPropertyKeys.TransientDatabaseConnectionString);
            }

            if (contextWrapper.GetPropertyValue(DcContextPropertyKeys.LogLevel) == null)
            {
                throw new ArgumentNullException(DcContextPropertyKeys.LogLevel);
            }
        }
    }
}
