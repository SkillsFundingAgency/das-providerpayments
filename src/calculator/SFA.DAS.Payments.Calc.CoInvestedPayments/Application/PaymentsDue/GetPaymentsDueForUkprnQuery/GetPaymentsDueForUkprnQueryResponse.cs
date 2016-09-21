using SFA.DAS.ProviderPayments.Calc.Common.Application;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue.GetPaymentsDueForUkprnQuery
{
    public class GetPaymentsDueForUkprnQueryResponse : QueryResponse<PaymentDue>
    {
        public bool DoesHavePaymentsDue()
        {
            return HasAnyItems();
        }
    }
}