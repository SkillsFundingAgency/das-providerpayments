using System.Collections.Generic;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data
{
    public interface IAccountLegalEntityRepository
    {
        void AddMany(IEnumerable<AccountLegalEntityEntity> entities);
    }
}