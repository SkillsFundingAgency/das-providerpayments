using System;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class FundingDue : RequiredPaymentEntity, IFundingDue
    {
        public int Period { get; set; }
    }

    public interface IFundingDue
    {
        string LearnRefNumber { get; set; }
        long Ukprn { get; set; }
        int AimSeqNumber { get; set; }
        DateTime LearningStartDate { get; set; }
        int Period { get; set; }
        long Uln { get; set; }
        int ProgrammeType { get; set; }
        int FrameworkCode { get; set; }
        int PathwayCode { get; set; }
        int StandardCode { get; set; }
        decimal SfaContributionPercentage { get; set; }
        string FundingLineType { get; set; }
        string LearnAimRef { get; set; }
        int ApprenticeshipContractType { get; set; }
    }
}
