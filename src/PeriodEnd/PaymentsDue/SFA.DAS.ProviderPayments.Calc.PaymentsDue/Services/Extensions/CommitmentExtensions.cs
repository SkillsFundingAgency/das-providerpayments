using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions
{
    public static class CommitmentExtensions
    {
        public static void CopyCommitmentInformationTo(this IHoldCommitmentInformation source,
            ICanStoreCommitmentInformation target)
        {
            target.AccountId = source.AccountId;
            target.AccountVersionId = source.AccountVersionId;
            target.CommitmentId = source.CommitmentId;
            target.CommitmentVersionId = source.CommitmentVersionId;
        }
    }
}
