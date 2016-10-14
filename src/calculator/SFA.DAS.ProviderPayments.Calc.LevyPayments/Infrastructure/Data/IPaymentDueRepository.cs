using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data
{
    public interface IPaymentDueRepository
    {
        PaymentDueEntity[] GetPaymentsDueForCommitment(long commitmentId);
    }
}