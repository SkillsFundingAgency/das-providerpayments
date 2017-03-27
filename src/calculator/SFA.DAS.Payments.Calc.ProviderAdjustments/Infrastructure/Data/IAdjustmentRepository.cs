using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data
{
    public interface IAdjustmentRepository
    {
        AdjustmentEntity[] GetCurrentProviderAdjustments(long ukprn);
        AdjustmentEntity[] GetPreviousProviderAdjustments(long ukprn);
    }
}