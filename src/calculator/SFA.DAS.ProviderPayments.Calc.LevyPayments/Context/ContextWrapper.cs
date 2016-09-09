using CS.Common.External.Interfaces;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Exceptions;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Context
{
    public class ContextWrapper
    {
        public IExternalContext Context { get; }

        public ContextWrapper(IExternalContext context)
        {
            if (context == null)
            {
                throw new LevyPaymentsInvalidContextException(LevyPaymentsExceptionMessages.ContextNull);
            }

            if (context.Properties == null || context.Properties.Count == 0)
            {
                throw new LevyPaymentsInvalidContextException(LevyPaymentsExceptionMessages.ContextNoProperties);
            }

            Context = context;
        }

        public string GetPropertyValue(string key, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            if (!Context.Properties.ContainsKey(key))
            {
                return defaultValue;
            }

            return Context.Properties[key];
        }
    }
}
