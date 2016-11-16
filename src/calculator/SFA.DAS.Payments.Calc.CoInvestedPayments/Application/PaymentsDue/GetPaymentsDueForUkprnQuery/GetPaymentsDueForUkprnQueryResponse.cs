using SFA.DAS.Payments.DCFS.Application;

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