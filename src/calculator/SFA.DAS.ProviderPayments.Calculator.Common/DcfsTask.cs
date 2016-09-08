using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calculator.Common.Context;

namespace SFA.DAS.ProviderPayments.Calculator.Common
{
    public abstract class DcfsTask : IExternalTask
    {
        public virtual void Execute(IExternalContext context)
        {
            var contextWrapper = GetContextWrapper(context);
            ValidateContext(contextWrapper);

            SetupLogging(contextWrapper);
        }

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
        }
    }
}
