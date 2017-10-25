using System.Collections.Generic;

namespace IlrGeneratorApp.DataSources
{
    public interface IDataSource
    {
        void Init(string dataSourceName, Dictionary<string, string> attributes);
    }
}