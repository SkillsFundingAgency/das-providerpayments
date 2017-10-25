using MediatR;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentsCommand
{
    public class ProcessPaymentsCommandRequest : IRequest<ProcessPaymentsCommandResponse>
    {
         public Payment[] Payments { get; set; }
    }
}