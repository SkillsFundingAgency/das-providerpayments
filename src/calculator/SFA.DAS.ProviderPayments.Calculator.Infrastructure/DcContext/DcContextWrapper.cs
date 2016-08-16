using CS.Common.External.Interfaces;

namespace SFA.DAS.ProviderPayments.Calculator.Infrastructure.DcContext
{
    public class DcContextWrapper
    {
        public IExternalContext Context { get; }

        public DcContextWrapper(IExternalContext context)
        {
            Context = context;
        }

        public string GetPropertyValue(string key, string defaultValue = null)
        {
            if (!Context.Properties.ContainsKey(key))
            {
                return defaultValue;
            }

            return Context.Properties[key];
        }
    }
}
