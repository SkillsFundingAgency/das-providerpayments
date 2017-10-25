using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application
{
    public abstract class QueryResponse<T> : Response
    {
        public T[] Items { get; set; }
    }
}