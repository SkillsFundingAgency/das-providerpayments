using System;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.DCFS.StructureMap.Infrastructure
{
    public class DcConfiguration : IHoldDcConfiguration
    {
        public DcConfiguration(ContextWrapper context)
        {
            CollectionYear = context.GetPropertyValue(ContextPropertyKeys.CollectionYear);
            TransientConnectionString = context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString);

            CheckRequiredProperty(CollectionYear, ContextPropertyKeys.CollectionYear);
            CheckRequiredProperty(TransientConnectionString, ContextPropertyKeys.TransientDatabaseConnectionString);
        }

        private void CheckRequiredProperty(string property, string propertyName)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException($"Could not find {propertyName} in the context", "context");
            }
        }

        public string CollectionYear { get; }
        public string TransientConnectionString { get; }
    }
}
