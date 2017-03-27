using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Repositories
{
    public class DcfsProviderRepository : DcfsRepository, IProviderRepository
    {
        private const string ProviderSource = "Reference.ProviderAdjustmentsProviders";
        private const string ProviderColumns = "UKPRN";
        private const string SelectProviders = "SELECT " + ProviderColumns + " FROM " + ProviderSource;

        public DcfsProviderRepository(string connectionString)
            : base(connectionString)
        {
        }

        public ProviderEntity[] GetAllProviders()
        {
            return Query<ProviderEntity>(SelectProviders);
        }
    }
}