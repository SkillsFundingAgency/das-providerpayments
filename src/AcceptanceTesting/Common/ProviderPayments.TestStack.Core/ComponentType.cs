namespace ProviderPayments.TestStack.Core
{
    public enum ComponentType
    {
        DataLockSubmission = 1,
        LevyCalculator = 2,
        EarningsCalculator = 3,
        PaymentsDue = 4,
        CoInvestedPayments = 5,
        ReferenceAccounts = 6,
        ReferenceCommitments = 7,
        DataLockPeriodEnd = 8,
        PeriodEndScripts = 9,
        DataLockEvents = 10,
        SubmissionEvents = 11,
        ManualAdjustments = 12,
        ProviderAdjustments = 13,
        TransferPayments = 14,
        AllComponents = 99,
    }
}