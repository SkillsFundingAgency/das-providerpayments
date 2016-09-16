using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand
{
    public class MarkAccountAsProcessedCommandRequest : IRequest
    {
        public string AccountId { get; set; }
    }
}
