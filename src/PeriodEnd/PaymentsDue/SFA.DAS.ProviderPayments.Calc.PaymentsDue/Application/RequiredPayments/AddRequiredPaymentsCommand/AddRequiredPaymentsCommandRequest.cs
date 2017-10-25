using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand
{
    public class AddRequiredPaymentsCommandRequest : IRequest<AddRequiredPaymentsCommandResponse>
    {
         public RequiredPayment[] Payments { get; set; }
    }
}