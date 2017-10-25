using System.Threading.Tasks;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IDatabaseCleaner
    {
        Task Clean(string pattern);
    }
}