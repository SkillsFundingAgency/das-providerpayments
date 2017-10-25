using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand
{
    public class AllocateLevyCommandRequest : IRequest<AllocateLevyCommandResponse>
    {
        public Account Account { get; set; }
        public decimal AmountRequested { get; set; }
    }
}
