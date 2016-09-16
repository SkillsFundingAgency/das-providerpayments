using CS.Common.External.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.Common.Context
{
    public class ContextWrapper
    {
        public IExternalContext Context { get; }

        public ContextWrapper(IExternalContext context)
        {
            if (context == null)
            {
                throw new InvalidContextException(InvalidContextException.ContextNullMessage);
            }

            if (context.Properties == null || context.Properties.Count == 0)
            {
                throw new InvalidContextException(InvalidContextException.ContextNoPropertiesMessage);
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
