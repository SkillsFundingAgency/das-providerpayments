using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Dcfs
{
    public class DcfsEarningRepository : DcfsRepository, IEarningRepository
    {
        private const string EarningSource = "LevyPayments.vw_CommitmentEarning";
        private const string EarningColumns = "CommitmentId," +
                                              "LearnRefNumber [LearnerRefNumber]," +
                                              "AimSeqNumber [AimSequenceNumber]," +
                                              "Ukprn," +
                                              "MonthlyInstallment [MonthlyInstallmentCapped]," +
                                              "MonthlyInstallmentUncapped [MonthlyInstallment]," +
                                              "CompletionPayment [CompletionPaymentCapped]," +
                                              "CompletionPaymentUncapped [CompletionPayment]," +
                                              "LearnStartDate [LearningStartDate]," +
                                              "LearnPlanEndDate [LearningPlannedEndDate]," +
                                              "LearnActEndDate [LearningActualEndDate]";
        private const string SelectEarnings = "SELECT " + EarningColumns + " FROM " + EarningSource;
        private const string SelectEarningsForCommitment = SelectEarnings + " WHERE CommitmentId = @CommitmentId";

        public DcfsEarningRepository(string connectionString)
            : base(connectionString)
        {
        }

        public EarningEntity GetEarningForCommitment(string commitmentId)
        {
            return QuerySingle<EarningEntity>(SelectEarningsForCommitment, new { commitmentId });
        }
    }
}
