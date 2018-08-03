using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;
using Common = SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Dcfs
{
    public class DcfsAccountLegalEntityRepository : Common.DcfsRepository, IAccountLegalEntityRepository
    {
        public DcfsAccountLegalEntityRepository(ContextWrapper context)
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public void AddMany(IEnumerable<AccountLegalEntityEntity> entities)
        {
            ExecuteBatch(entities, "dbo.AccountLegalEntity");
        }
    }
}