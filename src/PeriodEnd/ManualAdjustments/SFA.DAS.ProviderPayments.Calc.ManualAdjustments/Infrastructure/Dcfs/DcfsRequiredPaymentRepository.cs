using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs
{
    public class DcfsRequiredPaymentRepository : DcfsRepository, IRequiredPaymentRepository
    {
        private const string HistorySource = "Reference.RequiredPaymentsHistory";
        private const string CurrentSource = "PaymentsDue.RequiredPayments";

        public DcfsRequiredPaymentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public RequiredPaymentEntity GetRequiredPayment(string requiredPaymentId)
        {
            return QuerySingle<RequiredPaymentEntity>($"SELECT * FROM {HistorySource} WHERE Id = @requiredPaymentId", new { requiredPaymentId });
        }

        public void CreateRequiredPayment(RequiredPaymentEntity requiredPayment)
        {
            CreateCurrentRequiredPayment(requiredPayment );
            CreateHistoryRequiredPayment(requiredPayment);
        }

        private void CreateCurrentRequiredPayment(RequiredPaymentEntity requiredPayment)
        {
            Execute($"INSERT INTO {CurrentSource} (Id,CommitmentId,CommitmentVersionId,AccountId,AccountVersionId,Uln,LearnRefNumber," +
                    $"AimSeqNumber,Ukprn,IlrSubmissionDateTime,PriceEpisodeIdentifier,StandardCode,ProgrammeType,FrameworkCode,PathwayCode," +
                    $"ApprenticeshipContractType,DeliveryMonth,DeliveryYear,TransactionType,AmountDue,SfaContributionPercentage,FundingLineType,UseLevyBalance,LearnAimRef,LearningStartDate) " +
                    $"VALUES  (@Id,@CommitmentId,@CommitmentVersionId,@AccountId,@AccountVersionId,@Uln,@LearnRefNumber," +
                    $"@AimSeqNumber,@Ukprn,@IlrSubmissionDateTime,@PriceEpisodeIdentifier,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode," +
                    $"@ApprenticeshipContractType,@DeliveryMonth,@DeliveryYear,@TransactionType,@AmountDue,@SfaContributionPercentage,@FundingLineType,@UseLevyBalance,@LearnAimRef,@LearningStartDate)",
                requiredPayment);
        }

        private void CreateHistoryRequiredPayment(RequiredPaymentEntity requiredPayment)
        {
            Execute($"INSERT INTO {HistorySource} (Id,CommitmentId,CommitmentVersionId,AccountId,AccountVersionId,Uln,LearnRefNumber," +
                    $"AimSeqNumber,Ukprn,IlrSubmissionDateTime,PriceEpisodeIdentifier,StandardCode,ProgrammeType,FrameworkCode,PathwayCode," +
                    $"ApprenticeshipContractType,DeliveryMonth,DeliveryYear,TransactionType,AmountDue,SfaContributionPercentage,FundingLineType,UseLevyBalance,LearnAimRef,LearningStartDate,CollectionPeriodName,CollectionPeriodMonth,CollectionPeriodYear) " +
                    $"VALUES  (@Id,@CommitmentId,@CommitmentVersionId,@AccountId,@AccountVersionId,@Uln,@LearnRefNumber," +
                    $"@AimSeqNumber,@Ukprn,@IlrSubmissionDateTime,@PriceEpisodeIdentifier,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode," +
                    $"@ApprenticeshipContractType,@DeliveryMonth,@DeliveryYear,@TransactionType,@AmountDue,@SfaContributionPercentage,@FundingLineType,@UseLevyBalance,@LearnAimRef,@LearningStartDate,@CollectionPeriodName,@CollectionPeriodMonth,@CollectionPeriodYear)",
                   requiredPayment);
        }
    }

}
