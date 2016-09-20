using MediatR;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentCommand
{
    public class ProcessPaymentCommandRequest : IRequest
    {
        public Payment Payment { get; set; }
    }
}
