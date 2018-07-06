using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public class ProviderRepository : DcfsRepository, IProviderRepository
    {

        public ProviderRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<ProviderEntity> GetAllProviders()
        {
            return Query<ProviderEntity>("SELECT UKPRN, IlrSubmissionDateTime FROM Reference.Providers");
        }

    }
}