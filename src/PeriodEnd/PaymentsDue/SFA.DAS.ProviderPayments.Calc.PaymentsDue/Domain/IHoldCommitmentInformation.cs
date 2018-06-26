namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface IHoldCommitmentInformation
    {
        long CommitmentId { get; }
        string CommitmentVersionId { get; }
        long AccountId { get; }
        string AccountVersionId { get; }
    }
}
