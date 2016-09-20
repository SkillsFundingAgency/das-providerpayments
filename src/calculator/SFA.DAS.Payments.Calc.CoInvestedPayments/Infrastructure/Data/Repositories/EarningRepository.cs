using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class EarningRepository : DcfsRepository, IEarningRepository
    {
        private const string EarningSource = "PaymentsDue.vw_CommitmentEarning";
        private const string EarningColumns = "CommitmentId," +
                                              "LearnRefNumber [LearnerRefNumber]," +
                                              "AimSeqNumber [AimSequenceNumber]," +
                                              "Ukprn," +
                                              "MonthlyInstallment," +
                                              "MonthlyInstallmentUncapped," +
                                              "CompletionPayment," +
                                              "CompletionPaymentUncapped," +
                                              "LearnStartDate [LearningStartDate]," +
                                              "LearnPlanEndDate [LearningPlannedEndDate]," +
                                              "LearnActEndDate [LearningActualEndDate]";
        private const string SelectEarnings = "SELECT " + EarningColumns + " FROM " + EarningSource;
        private const string SelectProviderEarnings = SelectEarnings + " WHERE Ukprn = @Ukprn";

        public EarningRepository(string connectionString)
            : base(connectionString)
        {
        }

        public EarningEntity[] GetProviderEarnings(long ukprn)
        {
            return Query<EarningEntity>(SelectProviderEarnings, new { ukprn });
        }
    }
}