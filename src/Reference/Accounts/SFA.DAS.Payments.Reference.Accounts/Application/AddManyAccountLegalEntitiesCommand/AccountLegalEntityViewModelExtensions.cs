using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand
{
    public static class AccountLegalEntityViewModelExtensions
    {
        public static AccountLegalEntityEntity ToEntity(this AccountLegalEntityViewModel viewModel)
        {
            return new AccountLegalEntityEntity
            {
                Id = viewModel.AccountLegalEntityId,
                PublicHashedId = viewModel.AccountLegalEntityPublicHashedId,
                AccountId = viewModel.AccountId,
                LegalEntityId = viewModel.LegalEntityId
            };
        }
    }
}