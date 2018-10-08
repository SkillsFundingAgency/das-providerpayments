using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions
{
    public static class RequiredPaymentExtensions
    {
        public static void CopySignificantPaymentPropertiesTo(this RequiredPayment from,
            RequiredPayment to)
        {
            to.DeliveryMonth = from.DeliveryMonth;
            to.DeliveryYear = from.DeliveryYear;
            to.AccountId = from.AccountId;
            to.AccountVersionId = from.AccountVersionId;
            to.CommitmentId = from.CommitmentId;
            to.TransactionType = from.TransactionType;
            to.AimSeqNumber = from.AimSeqNumber;
            to.ApprenticeshipContractType = from.ApprenticeshipContractType;
            to.FrameworkCode = from.FrameworkCode;
            to.StandardCode = from.StandardCode;
            to.ProgrammeType = from.ProgrammeType;
            to.PathwayCode = from.PathwayCode;
            to.FundingLineType = from.FundingLineType;
            to.LearnAimRef = from.LearnAimRef;
            to.PriceEpisodeIdentifier = from.PriceEpisodeIdentifier;
        }
    }
}
