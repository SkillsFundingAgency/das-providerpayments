using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand
{
    public class ReversePaymentCommandRequest : IRequest<ReversePaymentCommandResponse>
    {
        public string RequiredPaymentIdToReverse { get; set; }
        public string YearOfCollection { get; set; }
    }
}
