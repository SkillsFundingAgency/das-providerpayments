using System;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class FundingDue : RequiredPaymentEntity
    {
    }

    public interface IFundingDue
    {
        string LearnRefNumber { get; set; }
        long Ukprn { get; set; }
        int AimSeqNumber { get; set; }
        DateTime LearnStartDate { get; set; }
        int Period { get; set; }
        long Uln { get; set; }
        int? ProgType { get; set; }
        int? FworkCode { get; set; }
        int? PwayCode { get; set; }
        int? StdCode { get; set; }
        decimal? LearnDelSfaContribPct { get; set; }
        string FundLineType { get; set; }
        string LearnAimRef { get; set; }
        short Act { get; set; }
    }
}
