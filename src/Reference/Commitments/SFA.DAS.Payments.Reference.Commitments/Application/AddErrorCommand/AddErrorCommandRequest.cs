using System;
using MediatR;

namespace SFA.DAS.Payments.Reference.Commitments.Application.AddErrorCommand
{
    public class AddErrorCommandRequest : IRequest
    {
        public Exception Error { get; set; }
    }
}
