namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    interface IHoldCommitmentInformation
    {
        long? CommitmentId { get; set; }
        string CommitmentVersionId { get; set; }
        long? AccountId { get; set; }
        string AccountVersionId { get; set; }
    }
}
