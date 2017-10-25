using MediatR;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue.GetPaymentsDueForUkprnQuery
{
    public class GetPaymentsDueForUkprnQueryRequest : IRequest<GetPaymentsDueForUkprnQueryResponse>
    {
         public long Ukprn { get; set; }
    }
}