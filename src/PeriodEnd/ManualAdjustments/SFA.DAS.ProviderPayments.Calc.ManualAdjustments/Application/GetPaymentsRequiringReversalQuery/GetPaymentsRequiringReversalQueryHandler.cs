using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.GetPaymentsRequiringReversalQuery
{
    public class GetPaymentsRequiringReversalQueryHandler : IRequestHandler<GetPaymentsRequiringReversalQueryRequest, GetPaymentsRequiringReversalQueryResponse>
    {
        private readonly IManualAdjustmentRepository _manualAdjustmentRepository;

        public GetPaymentsRequiringReversalQueryHandler(IManualAdjustmentRepository manualAdjustmentRepository)
        {
            _manualAdjustmentRepository = manualAdjustmentRepository;
        }

        public GetPaymentsRequiringReversalQueryResponse Handle(GetPaymentsRequiringReversalQueryRequest message)
        {
            try
            {
                var requiredPaymentIdsToReverse = _manualAdjustmentRepository.GetRequiredPaymentIdsToReverse();
                return new GetPaymentsRequiringReversalQueryResponse
                {
                    IsValid = true,
                    Items = requiredPaymentIdsToReverse.Select(p => p.ToString()).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPaymentsRequiringReversalQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}