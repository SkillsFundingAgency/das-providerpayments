using System.ComponentModel.DataAnnotations;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities
{
    public class LearnerSummaryPaymentEntity
    {
        public LearnerSummaryPaymentEntity()
        { }

        public LearnerSummaryPaymentEntity(LearnerSummaryPaymentEntity payment)
        {
            LearnRefNumber = payment.LearnRefNumber;
            Amount = payment.Amount;
            TransactionType = payment.TransactionType;
            StandardCode = payment.StandardCode;
            ProgrammeType = payment.ProgrammeType;
            FrameworkCode = payment.FrameworkCode;
            PathwayCode = payment.PathwayCode;
            ApprenticeshipContractType = payment.ApprenticeshipContractType;
            SfaContributionPercentage = payment.SfaContributionPercentage;
            FundingLineType = payment.FundingLineType;
            AccountId = payment.AccountId;
        }

        [StringLength(12)]
        public string LearnRefNumber { get; set; }

        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public int StandardCode { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        public ApprenticeshipContractType ApprenticeshipContractType { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        public string FundingLineType { get; set; }
        public long AccountId { get; set; }
    }
}