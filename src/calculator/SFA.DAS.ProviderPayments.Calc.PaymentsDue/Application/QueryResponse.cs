namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public abstract class QueryResponse<T> : Response
    {
        public T[] Items { get; set; }
    }
}