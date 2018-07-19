using System.Collections.Generic;
using MediatR;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand
{
    public class AddManyAccountLegalEntitiesCommandRequest : IRequest
    {
        public IEnumerable<AccountLegalEntityViewModel> AccountLegalEntityViewModels { get; set; }
    }
}