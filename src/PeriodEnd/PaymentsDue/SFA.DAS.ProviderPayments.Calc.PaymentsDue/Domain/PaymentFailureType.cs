namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public enum PaymentFailureType
    {
        CouldNotFindSuccessfulDatalock,
        MultipleMatchingSuccessfulDatalocks,
        CouldNotFindMatchingOnprog,
        HeldBackCompletionPayment
    }
}
