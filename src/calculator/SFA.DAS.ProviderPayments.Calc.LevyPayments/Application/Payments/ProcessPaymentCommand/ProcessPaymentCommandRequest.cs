using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.ProcessPaymentCommand
{
    public class ProcessPaymentCommandRequest : IRequest
    {
        public Payment Payment { get; set; }
    }
}
