namespace SFA.DAS.Payments.DCFS.StructureMap.Infrastructure
{
    public interface IHoldDcConfiguration
    {
        string CollectionYear { get; }
        string TransientConnectionString { get; }
    }
}
