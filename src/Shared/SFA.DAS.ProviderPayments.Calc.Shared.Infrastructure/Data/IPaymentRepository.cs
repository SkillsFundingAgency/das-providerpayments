using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IPaymentRepository
    {
        void AddMany(List<PaymentEntity> payments, PaymentSchema schema);
        IEnumerable<LearnerSummaryPaymentEntity> GetHistoricEmployerPaymentsEachRoundedDownForProvider(long ukprn);
    }

    public enum PaymentSchema
    {
        LevyPayments,
        CoInvestedPayments,
        Refunds,
    }
}