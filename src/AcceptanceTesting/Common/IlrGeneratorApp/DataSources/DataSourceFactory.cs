using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using IlrGeneratorApp.Configuration;
using IlrGeneratorApp.DataSources.Provider;

namespace IlrGeneratorApp.DataSources
{
    public static class DataSourceFactory
    {
        private static readonly Type[] ProviderDataSourceTypes;

        static DataSourceFactory()
        {
            var assm = typeof(DataSourceFactory).Assembly;

            ProviderDataSourceTypes = assm.GetTypes().Where(t => t.GetInterface("IProviderDataSource") != null).ToArray();
        }

        public static IProviderDataSource[] GetProviderDataSources()
        {
            var dataSourcesConfiguration = DataSourcesConfigurationSection.Get();
            return MakeDataSources<IProviderDataSource>(dataSourcesConfiguration.ProviderSources);
        }

        private static T[] MakeDataSources<T>(DataSourcesCollectionElement sources)
            where T : IDataSource
        {
            var providerDataSources = new List<T>();
            foreach (var configElement in sources)
            {
                providerDataSources.Add(MakeDataSource<T>((DataSourceElement)configElement, ProviderDataSourceTypes));
            }
            return providerDataSources.ToArray();
        }
        private static T MakeDataSource<T>(DataSourceElement configElement, Type[] types)
            where T : IDataSource
        {
            var type = types.SingleOrDefault(t => t.Name == configElement.Type);
            if (type == null)
            {
                throw new ConfigurationErrorsException($"Cannot find data source with type {configElement.Type}");
            }

            var attributes = new Dictionary<string, string>();
            foreach (var attributeElement in configElement.Attributes)
            {
                var elem = (AttributeElement)attributeElement;
                attributes.Add(elem.Key, elem.Value);
            }

            var instance = (T)Activator.CreateInstance(type);
            instance.Init(configElement.Name, attributes);
            return instance;
        }
    }
}
