using System;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery
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
                            PeriodId = periodEntity.Id,
                            Month = periodEntity.Month,
                            Year = periodEntity.Year,
                            PeriodNumber = int.Parse(periodEntity.Name.Substring(1))
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