namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application
{
    public abstract class QueryResponse<T> : Response
    {
        public T[] Items { get; set; }
    }
}