using System;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.SetAdjustmentAsReversedCommand
{
    public class SetAdjustmentAsReversedCommandHandler : IRequestHandler<SetAdjustmentAsReversedCommandRequest, SetAdjustmentAsReversedCommandResponse>
    {
        private readonly IManualAdjustmentRepository _manualAdjustmentRepository;

        public SetAdjustmentAsReversedCommandHandler(IManualAdjustmentRepository manualAdjustmentRepository)
        {
            _manualAdjustmentRepository = manualAdjustmentRepository;
        }

        public SetAdjustmentAsReversedCommandResponse Handle(SetAdjustmentAsReversedCommandRequest message)
        {
            try
            {
                _manualAdjustmentRepository.SetRequiredPaymentIdAsReversed(message.RequiredPaymentIdToReverse,
                                                                           message.RequiredPaymentIdForReversal);
                return new SetAdjustmentAsReversedCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new SetAdjustmentAsReversedCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}