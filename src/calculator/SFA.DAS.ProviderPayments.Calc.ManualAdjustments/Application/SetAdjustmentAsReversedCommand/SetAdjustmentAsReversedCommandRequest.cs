using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.SetAdjustmentAsReversedCommand
{
    public class SetAdjustmentAsReversedCommandRequest : IRequest<SetAdjustmentAsReversedCommandResponse>
    {
        public string RequiredPaymentIdToReverse { get; set; }
        public string RequiredPaymentIdForReversal { get; set; }
    }
}
