using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Payments.PaymentHistoryForMultipleLearnersQuery
{
    public class PaymentHistoryForMultipleLearnersRequest : IRequest<PaymentHistoryForMultipleLearnersResponse>
    {
        public long Ukprn { get; set; }
        public IEnumerable<string> LearnRefNumbers { get; set; }
    }
}
