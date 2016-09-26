using System;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using StructureMap;
using StructureMap.Pipeline;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution
{
    public class ConnectionStringPolicy : ConfiguredInstancePolicy
    {
        private readonly ContextWrapper _context;

        public ConnectionStringPolicy(ContextWrapper context)
        {
            _context = context;
        }

        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var connectionString = instance?.Constructor?.GetParameters().FirstOrDefault(x => x.Name.Equals("connectionString", StringComparison.OrdinalIgnoreCase));
            if (connectionString != null)
            {
                var transientConnectionString = _context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString);
                instance.Dependencies.AddForConstructorParameter(connectionString, transientConnectionString);
            }
        }
    }
}
