using System.Collections.Generic;
using System.Configuration;

namespace IlrGeneratorApp.DataSources.Provider
{
    public class TestStackDataSource : SqlServerDataSource, IProviderDataSource
    {
        public void Init(string dataSourceName, Dictionary<string, string> attributes)
        {
            Name = dataSourceName;

            if (!attributes.ContainsKey("ConnectionString"))
            {
                throw new ConfigurationErrorsException($"Cannot find ConnectionString attribute for data source {dataSourceName}");
            }
            SetConnectionString(attributes["ConnectionString"]);
        }

        public string Name { get; private set; }

        public Provider[] SearchForProvider(string criteria)
        {
            var command = "SELECT Ukprn, ProviderName Name FROM TestStack.Provider WHERE ProviderName LIKE @criteria " +
                          "OR CAST(ukprn as varchar(50)) LIKE @criteria";
            return Query<Provider>(command, new { criteria = $"%{criteria}%" });
        }
    }
}
