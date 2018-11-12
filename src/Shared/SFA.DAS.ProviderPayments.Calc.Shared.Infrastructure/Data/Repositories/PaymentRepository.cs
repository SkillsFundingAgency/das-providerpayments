using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public class PaymentRepository : DcfsRepository, IPaymentRepository
    {
        public PaymentRepository(string connectionString) : base(connectionString)
        {
        }

        public void AddMany(List<PaymentEntity> payments, PaymentSchema schema)
        {
            ExecuteBatch(payments.ToArray(), $"{schema.ToString()}.Payments");
        }

        public IEnumerable<LearnerSummaryPaymentEntity> GetRoundedDownEmployerPaymentsForProvider(long ukprn)
        {
            const string sql = @"
            SELECT 
                LearnRefNumber,
                TransactionType,
                SUM(FLOOR(Amount)) [Amount],
                StandardCode,
                ProgrammeType,
                FrameworkCode,
                PathwayCode,
                ApprenticeshipContractType,
                SfaContributionPercentage,
                FundingLineType,
                AccountId
            FROM Reference.PaymentsHistory
            WHERE Ukprn = @ukprn AND FundingSource = 3
            GROUP BY LearnRefNumber, TransactionType,
                StandardCode, ProgrammeType, FrameworkCode,
                PathwayCode, ApprenticeshipContractType, 
                SfaContributionPercentage, FundingLineType,
                AccountId";
            return Query<LearnerSummaryPaymentEntity>(sql, new { ukprn });
        }
    }
}

