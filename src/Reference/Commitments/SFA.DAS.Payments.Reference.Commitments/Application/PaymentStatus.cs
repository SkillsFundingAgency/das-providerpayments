namespace SFA.DAS.Payments.Reference.Commitments.Application
{
    public enum PaymentStatus
    {
        PendingApproval = 0,
        Active = 1,
        Paused = 2,
        Withdrawn = 3,
        Completed = 4,
        Deleted = 5
    }
}