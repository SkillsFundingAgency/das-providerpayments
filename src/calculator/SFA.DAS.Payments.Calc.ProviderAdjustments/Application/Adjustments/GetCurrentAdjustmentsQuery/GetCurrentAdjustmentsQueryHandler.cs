using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery
{
    public class GetCurrentAdjustmentsQueryHandler : IRequestHandler<GetCurrentAdjustmentsQueryRequest, GetCurrentAdjustmentsQueryResponse>
    {
        private readonly IAdjustmentRepository _providerAdjustmentRepository;

        public GetCurrentAdjustmentsQueryHandler(IAdjustmentRepository providerAdjustmentRepository)
        {
            _providerAdjustmentRepository = providerAdjustmentRepository;
        }

        public GetCurrentAdjustmentsQueryResponse Handle(GetCurrentAdjustmentsQueryRequest message)
        {
            try
            {
                var providerAdjustmentEntities = _providerAdjustmentRepository.GetCurrentProviderAdjustments(message.Ukprn);

                return new GetCurrentAdjustmentsQueryResponse
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
                return new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}