using System.Collections.Generic;
using MediatR;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddManyAgreementsCommand
{
    public class AddManyAgreementsCommandRequest : IRequest
    {
        public IEnumerable<AccountLegalEntityViewModel> AccountLegalEntityViewModels { get; set; }
    }
}