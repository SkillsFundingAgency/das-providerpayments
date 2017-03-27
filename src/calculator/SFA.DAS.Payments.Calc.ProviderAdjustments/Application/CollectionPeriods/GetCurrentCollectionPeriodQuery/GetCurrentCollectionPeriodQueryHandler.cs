using System;
using MediatR;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery
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
                            Year = periodEntity.Year,
                            Name = periodEntity.Name
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