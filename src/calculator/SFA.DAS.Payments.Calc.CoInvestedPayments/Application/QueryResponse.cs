using SFA.DAS.ProviderPayments.Calc.Common.Application;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application
{
    public abstract class QueryResponse<T> : Response
    {
        public T[] Items { get; set; }
    }
}