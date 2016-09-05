using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments.ProcessPaymentCommand
{
    public class ProcessPaymentCommandRequest : IRequest
    {
        public Payment Payment { get; set; }
    }
}
