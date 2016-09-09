using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.Common.Logging;

namespace SFA.DAS.ProviderPayments.Calc.Common
{
    public abstract class DcfsTask : IExternalTask
    {
        public virtual void Execute(IExternalContext context)
        {
            var contextWrapper = GetContextWrapper(context);
            ValidateContext(contextWrapper);

            SetupLogging(contextWrapper);

            Execute(contextWrapper);
        }

        protected abstract void Execute(ContextWrapper context);

        protected virtual ContextWrapper GetContextWrapper(IExternalContext context)
        {
            return new ContextWrapper(context);
        }
        protected virtual void ValidateContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesNoConnectionStringMessage);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesNoLogLevelMessage);
            }
        }

        protected virtual void SetupLogging(ContextWrapper contextWrapper)
        {
            LoggingConfig.ConfigureLogging(
                   contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                   contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)
               );
        }
    }
}
