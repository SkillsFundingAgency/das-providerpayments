using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand
{
    public class AddRequiredPaymentsCommandRequest : IRequest<AddRequiredPaymentsCommandResponse>
    {
        public RequiredPayment[] Payments { get; set; }
        public PeriodEarning[] Earnings { get; set; }
    }
}