namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface ICanStoreCommitmentInformation
    {
        long CommitmentId { set; }
        string CommitmentVersionId { set; }
        long AccountId { set; }
        string AccountVersionId { set; }
    }
}