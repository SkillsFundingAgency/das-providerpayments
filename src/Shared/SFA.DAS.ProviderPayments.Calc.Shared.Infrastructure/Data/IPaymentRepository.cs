using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IPaymentRepository
    {
        void AddMany(List<PaymentEntity> payments, PaymentSchema schema);
        IEnumerable<PaymentEntity> GetAllHistoricPaymentsForProvider(long ukprn);
        IEnumerable<LearnerSummaryPaymentEntity> GetHistoricEmployerOnProgrammePaymentsForProvider(long ukprn);
    }

    public enum PaymentSchema
    {
        LevyPayments,
        CoInvestedPayments,
        Refunds,
    }
}