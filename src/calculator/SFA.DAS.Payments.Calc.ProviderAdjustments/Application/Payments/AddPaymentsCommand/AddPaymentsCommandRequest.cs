using MediatR;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments.AddPaymentsCommand
{
    public class AddPaymentsCommandRequest : IRequest
    {
        public Payment[] Payments { get; set; }
    }
}