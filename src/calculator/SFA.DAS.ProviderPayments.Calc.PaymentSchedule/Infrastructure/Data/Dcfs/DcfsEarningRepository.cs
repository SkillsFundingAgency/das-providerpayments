using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Dcfs
{
    public class DcfsEarningRepository : DcfsRepository, IEarningRepository
    {
        private const string EarningSource = "PaymentSchedule.vw_CommitmentEarning";
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

        public DcfsEarningRepository(string connectionString)
            : base(connectionString)
        {
        }

        public EarningEntity[] GetProviderEarnings(long ukprn)
        {
            return Query<EarningEntity>(SelectProviderEarnings, new { ukprn });
        }
    }
}