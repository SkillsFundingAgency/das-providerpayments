using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Logging;

namespace SFA.DAS.Payments.DCFS
{
    public abstract class DcfsTask : IExternalTask
    {
        private readonly string _databaseSchema;

        protected DcfsTask(string databaseSchema)
        {
            _databaseSchema = databaseSchema;
        }

        public virtual void Execute(IExternalContext context)
        {
            var contextWrapper = GetContextWrapper(context);

            if (IsValidContext(contextWrapper))
            {
                SetupLogging(contextWrapper);

                Execute(contextWrapper);
            }
        }

        protected abstract void Execute(ContextWrapper context);

        protected virtual ContextWrapper GetContextWrapper(IExternalContext context)
        {
            return new ContextWrapper(context);
        }

        protected virtual bool IsValidContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesNoConnectionStringMessage);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesNoLogLevelMessage);
            }

            return true;
        }

        protected virtual void SetupLogging(ContextWrapper contextWrapper)
        {
            LoggingConfiguration.Configure(
                   contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString),
                   contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel),
                   _databaseSchema
               );
        }
    }
}
