using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data
{
    public interface IAuditRepository
    {
        void CreateAudit(AuditEntity entity);
    }
}