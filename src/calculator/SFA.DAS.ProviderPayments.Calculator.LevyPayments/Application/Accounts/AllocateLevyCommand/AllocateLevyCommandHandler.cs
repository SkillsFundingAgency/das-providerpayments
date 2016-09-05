using System;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand
{
    public class AllocateLevyCommandHandler : IRequestHandler<AllocateLevyCommandRequest, AllocateLevyCommandResponse>
    {
        public AllocateLevyCommandResponse Handle(AllocateLevyCommandRequest message)
        {
            throw new NotImplementedException();
        }
    }
}