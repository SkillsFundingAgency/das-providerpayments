using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsEarningRepository : DcfsRepository, IEarningRepository
    {
        private const string EarningSource = "PaymentsDue.vw_CommitmentEarning";
        private const string EarningColumns = "CommitmentId, "
                                            + "CommitmentVersionId, "
                                            + "AccountId, "
                                            + "AccountVersionId, "
                                            + "MonthlyInstallment, "
                                            + "CompletionPayment, "
                                            + "Ukprn, "
                                            + "Uln, "
                                            + "LearnRefNumber [LearnerRefNumber], "
                                            + "AimSeqNumber [AimSequenceNumber], "
                                            + "AttributeName [EarningType], "
                                            + "Period_1 [Period1], "
                                            + "Period_2 [Period2], "
                                            + "Period_3 [Period3], "
                                            + "Period_4 [Period4], "
                                            + "Period_5 [Period5], "
                                            + "Period_6 [Period6], "
                                            + "Period_7 [Period7], "
                                            + "Period_8 [Period8], "
                                            + "Period_9 [Period9], "
                                            + "Period_10 [Period10], "
                                            + "Period_11 [Period11], "
                                            + "Period_12 [Period12], "
                                            + "StandardCode, "
                                            + "ProgrammeType, "
                                            + "FrameworkCode, "
                                            + "PathwayCode";
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