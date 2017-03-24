using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetCoInvestedPaymentsHistoryQuery
{
    public class GetCoInvestedPaymentsHistoryQueryRequest : IRequest<GetCoInvestedPaymentsHistoryQueryResponse>
    {
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int TransactionType { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
    }
}