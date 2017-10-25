namespace ProviderPayments.TestStack.Domain.Mapping
{
    public interface IMapper
    {
        TDestination Map<TDestination>(object source);
    }
}
