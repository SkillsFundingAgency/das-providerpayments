using System;
using MediatR;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddOrUpdateAccountCommand
{
    public class AddOrUpdateAccountCommandRequest : IRequest
    {
        public AccountWithBalanceViewModel Account { get; set; }
        public DateTime CorrelationDate { get; set; }
    }
}
