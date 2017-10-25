using System;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.Payments.DCFS.Context;
using StructureMap;
using StructureMap.Pipeline;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.DependencyResolution
{
    public class YearOfCollectionPolicy : ConfiguredInstancePolicy
    {
        private readonly ContextWrapper _contextWrapper;

        public YearOfCollectionPolicy(ContextWrapper contextWrapper)
        {
            _contextWrapper = contextWrapper;
        }

        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var parameter = instance.Constructor.GetParameters().FirstOrDefault(x => x.Name == "yearOfCollection");

            if (parameter != null)
            {
                var yearOfCollection = _contextWrapper.GetPropertyValue(DataLockContextPropertyKeys.YearOfCollection);
                instance.Dependencies.AddForConstructorParameter(parameter, yearOfCollection);
            }
        }
    }
}