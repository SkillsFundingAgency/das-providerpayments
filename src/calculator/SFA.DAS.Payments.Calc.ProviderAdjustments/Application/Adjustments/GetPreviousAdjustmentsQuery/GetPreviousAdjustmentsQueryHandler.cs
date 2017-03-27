using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetPreviousAdjustmentsQuery
{
    public class GetPreviousAdjustmentsQueryHandler : IRequestHandler<GetPreviousAdjustmentsQueryRequest, GetPreviousAdjustmentsQueryResponse>
    {
        private readonly IAdjustmentRepository _providerAdjustmentRepository;

        public GetPreviousAdjustmentsQueryHandler(IAdjustmentRepository providerAdjustmentRepository)
        {
            _providerAdjustmentRepository = providerAdjustmentRepository;
        }

        public GetPreviousAdjustmentsQueryResponse Handle(GetPreviousAdjustmentsQueryRequest message)
        {
            try
            {
                var providerAdjustmentEntities = _providerAdjustmentRepository.GetPreviousProviderAdjustments(message.Ukprn);

                return new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = providerAdjustmentEntities == null
                        ? null
                        : providerAdjustmentEntities.Select(pa =>
                            new Adjustment
                            {
                                Ukprn = pa.Ukprn,
                                SubmissionId = pa.SubmissionId,
                                SubmissionCollectionPeriod = pa.SubmissionCollectionPeriod,
                                PaymentType = pa.PaymentType,
                                PaymentTypeName = pa.PaymentTypeName,
                                Amount = pa.Amount
                            }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}