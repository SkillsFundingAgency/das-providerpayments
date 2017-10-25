using System.Configuration;

namespace IlrGeneratorApp.Configuration
{
    public class DataSourcesConfigurationSection : ConfigurationSection
    {
        public static DataSourcesConfigurationSection Get()
        {
            return ConfigurationManager.GetSection("dataSources") as DataSourcesConfigurationSection;
        }

        [ConfigurationProperty("providerSources")]
        public DataSourcesCollectionElement ProviderSources
        {
            get { return (DataSourcesCollectionElement)this["providerSources"]; }
            set { this["providerSources"] = value; }
        }
    }

    public class DataSourcesCollectionElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DataSourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataSourceElement)element).Name;
        }
    }

    public class DataSourceElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("attributes")]
        public DataSourceAttributesCollectionElement Attributes
        {
            get { return (DataSourceAttributesCollectionElement)this["attributes"]; }
            set { this["attributes"] = value; }
        }
    }

    public class DataSourceAttributesCollectionElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AttributeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AttributeElement)element).Key;
        }
    }

    public class AttributeElement : ConfigurationElement
    {
        [ConfigurationProperty("key")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
