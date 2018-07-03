using System;
using System.Linq;
using SFA.DAS.Payments.DCFS.Context;
using StructureMap;
using StructureMap.Pipeline;

namespace SFA.DAS.Payments.DCFS.StructureMap.Infrastructure.DependencyResolution
{
    public class DcfsConnectionStringPolicy : ConfiguredInstancePolicy
    {
        private readonly ContextWrapper _context;

        public DcfsConnectionStringPolicy(ContextWrapper context)
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
                return;
            }

            connectionString = instance?.Constructor?.GetParameters().FirstOrDefault(x => x.Name.Equals("ilrConnectionString", StringComparison.OrdinalIgnoreCase));
            if (connectionString != null)
            {
                var ilrConnectionString = _context.GetPropertyValue(ContextPropertyKeys.IlrDatabaseConnectionString);
                instance.Dependencies.AddForConstructorParameter(connectionString, ilrConnectionString);
            }
        }
    }
}
