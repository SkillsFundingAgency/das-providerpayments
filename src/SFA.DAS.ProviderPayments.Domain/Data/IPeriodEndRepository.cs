using System.Threading.Tasks;
using SFA.DAS.ProdiverPayments.Domain.Data.Entities;

namespace SFA.DAS.ProdiverPayments.Domain.Data
{
    public interface IPeriodEndRepository
    {
        Task<PageOfEntities<PeriodEndEntity>> GetPageAsync(int pageNumber);
    }
}
