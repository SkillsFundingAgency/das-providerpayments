using System;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand
{
    public class AllocateLevyCommandHandler : IRequestHandler<AllocateLevyCommandRequest, AllocateLevyCommandResponse>
    {
        public AllocateLevyCommandResponse Handle(AllocateLevyCommandRequest message)
        {
            // take as much of the requested amount from levy.
            // Store running total or levy decremented from balance
            // Return amount taken
            throw new NotImplementedException();
        }
    }
}