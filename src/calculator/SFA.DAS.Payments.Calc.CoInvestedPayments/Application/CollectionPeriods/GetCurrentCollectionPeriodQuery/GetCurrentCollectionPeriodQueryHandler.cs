using System;
using MediatR;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery
{
    public class GetCurrentCollectionPeriodQueryHandler : IRequestHandler<GetCurrentCollectionPeriodQueryRequest, GetCurrentCollectionPeriodQueryResponse>
    {
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        public GetCurrentCollectionPeriodQueryHandler(ICollectionPeriodRepository collectionPeriodRepository)
        {
            _collectionPeriodRepository = collectionPeriodRepository;
        }

        public GetCurrentCollectionPeriodQueryResponse Handle(GetCurrentCollectionPeriodQueryRequest message)
        {
            try
            {
                var periodEntity = _collectionPeriodRepository.GetCurrentCollectionPeriod();

                return new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = periodEntity == null
                        ? null
                        : new CollectionPeriod
                        {
                            PeriodId = periodEntity.PeriodId,
                            Month = periodEntity.Month,
                            Year = periodEntity.Year
                        }
                };

            }
            catch (Exception ex)
            {
                return new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}