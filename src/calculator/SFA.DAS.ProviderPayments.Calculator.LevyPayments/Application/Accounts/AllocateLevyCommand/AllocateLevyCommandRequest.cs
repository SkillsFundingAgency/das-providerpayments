using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand
{
    public class AllocateLevyCommandRequest : IRequest<AllocateLevyCommandResponse>
    {
        public Account Account { get; set; }
        public decimal AmountRequested { get; set; }
    }
}
