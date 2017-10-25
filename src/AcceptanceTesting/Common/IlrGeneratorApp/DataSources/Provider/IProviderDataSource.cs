namespace IlrGeneratorApp.DataSources.Provider
{
    public interface IProviderDataSource : IDataSource
    {
        string Name { get; }

        Provider[] SearchForProvider(string criteria);
    }
}