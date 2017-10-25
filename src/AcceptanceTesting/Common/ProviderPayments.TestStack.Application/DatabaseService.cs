using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;

namespace ProviderPayments.TestStack.Application
{
    public interface IDatabaseService
    {
        Task CleanDeds();
    }

    public class DatabaseService : IDatabaseService
    {
        private const string AllTablesExceptThoseInDboPattern = @"^(?!.*dbo\.).*$";

        private readonly IDatabaseCleaner _dedsCleaner;

        public DatabaseService(IDatabaseCleaner dedsCleaner)
        {
            _dedsCleaner = dedsCleaner;
        }

        public async Task CleanDeds()
        {
            await _dedsCleaner.Clean(AllTablesExceptThoseInDboPattern);
        }
    }
}
