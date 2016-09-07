using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand
{
    public class MarkAccountAsProcessedCommandRequest : IRequest
    {
        public string AccountId { get; set; }
    }
}
