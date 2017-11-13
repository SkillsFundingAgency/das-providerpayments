using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery
{
    public class Get16To18IncentiveEarningsQueryHandler : IRequestHandler<Get16To18IncentiveEarningsQueryRequest, Get16To18IncentiveEarningsQueryResponse>
    {
        private readonly IIncentiveEarningsRepository _incentivesRepository;

        public Get16To18IncentiveEarningsQueryHandler(IIncentiveEarningsRepository priceEpisodeRepository)
        {
            _incentivesRepository = priceEpisodeRepository;
        }

        public Get16To18IncentiveEarningsQueryResponse Handle(Get16To18IncentiveEarningsQueryRequest message)
        {
            try
            {
                var earningEntities = _incentivesRepository.GetIncentiveEarnings(message.Ukprn);

                return new Get16To18IncentiveEarningsQueryResponse
                {
                    IsValid = true,
                    Items = earningEntities?.Select(l =>
                    new IncentiveEarnings
                    {
                        Ukprn = l.Ukprn,
                        LearnRefNumber = l.LearnRefNumber,
                        Period = l.Period,
                        PriceEpisodeFirstEmp1618Pay = l.PriceEpisodeFirstEmp1618Pay,
                        PriceEpisodeSecondEmp1618Pay = l.PriceEpisodeSecondEmp1618Pay,
                        PriceEpisodeIdentifier = l.PriceEpisodeIdentifier
                    }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new Get16To18IncentiveEarningsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}