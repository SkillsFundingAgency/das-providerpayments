using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsRequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        private const string PaymentsDestination = "PaymentsDue.RequiredPayments";

        private const string PaymentHistorySource = "PaymentsDue.vw_PaymentHistory";
        private const string PaymentHistoryWithoutEarningSource = "PaymentsDue.vw_PaymentHistoryWithoutEarnings";
        private const string PaymentHistoryColumns = "CommitmentId, "
                                                    + "CommitmentVersionId,"
                                                   + "AccountId,"
                                                   + "AccountVersionId,"
                                                   + "LearnRefNumber, "
                                                   + "AimSeqNumber, "
                                                   + "Ukprn, "
                                                   + "DeliveryMonth, "
                                                   + "DeliveryYear, "
                                                   + "AmountDue, "
                                                   + "TransactionType,"
                                                   + "Uln,"
                                                   + "StandardCode ,"
                                                   + "ProgrammeType,"
                                                   + "FrameworkCode,"
                                                   + "PathwayCode,"
                                                   + "LearnAimRef,"
                                                   + "LearningStartDate,"
                                                   + "ApprenticeshipContractType,"
                                                   + "FundingLineType,"
                                                   + "PriceEpisodeIdentifier,"
                                                   + "SfaContributionPercentage,"
                                                   + "UseLevyBalance,"
                                                   + "CollectionPeriodMonth,"
                                                   + "CollectionPeriodYear";

        private const string SelectPayments = "SELECT " + PaymentHistoryColumns + " FROM " + PaymentHistorySource;
        private const string SelectProviderPayments = SelectPayments + " WHERE Ukprn = @ukprn";
        private const string SelectPaymentsWithoutEarnings = "SELECT " + PaymentHistoryColumns + ",IlrSubmissionDateTime FROM " + PaymentHistoryWithoutEarningSource;
        private const string SelectLearnerPayments = SelectProviderPayments + " AND LearnRefNumber = @LearnRefNumber";
        private const string SelectLearnersPayments = SelectProviderPayments + " AND LearnRefNumber IN (@LearnRefNumbers)";

        public DcfsRequiredPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddRequiredPayments(RequiredPaymentEntity[] payments)
        {
            ExecuteBatch(payments, PaymentsDestination);
        }

        public HistoricalRequiredPaymentEntity[] GetPreviousPayments(long ukprn, string learnRefNumber)
        {
            return Query<HistoricalRequiredPaymentEntity>(SelectLearnerPayments, new { ukprn, learnRefNumber });
        }

        public HistoricalRequiredPaymentEntity[] GetPreviousPaymentsForMultipleLearners(
            long ukprn,
            IEnumerable<string> learnRefNumbers)
        {
            return Query<HistoricalRequiredPaymentEntity>(SelectLearnersPayments, new { ukprn, learnRefNumbers });
        }

        public RequiredPaymentEntity[] GetPreviousPaymentsWithoutEarnings()
        {
            return Query<RequiredPaymentEntity>(SelectPaymentsWithoutEarnings);
        }
    }
}