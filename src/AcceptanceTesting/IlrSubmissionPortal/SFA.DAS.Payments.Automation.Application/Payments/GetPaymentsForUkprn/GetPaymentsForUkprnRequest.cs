using MediatR;

namespace SFA.DAS.Payments.Automation.Application.Payments.GetPaymentsForUkprn
{
    public class GetPaymentsForUkprnRequest : IAsyncRequest<GetPaymentsForUkprnResponse>
    {
        public long Ukprn { get; set; }
    }
}
