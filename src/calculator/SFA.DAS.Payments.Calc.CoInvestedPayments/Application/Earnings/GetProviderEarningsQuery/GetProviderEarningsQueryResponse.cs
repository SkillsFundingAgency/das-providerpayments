using System.Linq;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryResponse : ProviderPayments.Calc.Common.Application.QueryResponse<Earning>
    {
        public bool DoesHaveEarnings()
        {
            return Items != null && Items.Any();
        }
    }
}