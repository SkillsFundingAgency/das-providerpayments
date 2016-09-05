using System;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments.ProcessPaymentCommand
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommandRequest, Unit>
    {
        public Unit Handle(ProcessPaymentCommandRequest message)
        {
            throw new NotImplementedException();
        }
    }
}