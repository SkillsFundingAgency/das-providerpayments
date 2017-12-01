using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery
{
    public class Get16To18IncentiveEarningsQueryRequest : IRequest<Get16To18IncentiveEarningsQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}