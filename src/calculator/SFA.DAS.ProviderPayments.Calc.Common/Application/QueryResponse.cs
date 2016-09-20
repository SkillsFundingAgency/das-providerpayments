namespace SFA.DAS.ProviderPayments.Calc.Common.Application
{
    public abstract class QueryResponse<T> : Response
    {
        public T[] Items { get; set; }
    }
}