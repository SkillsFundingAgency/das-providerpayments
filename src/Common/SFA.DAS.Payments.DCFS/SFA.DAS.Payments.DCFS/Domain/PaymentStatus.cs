namespace SFA.DAS.Payments.DCFS.Domain
{
    public enum PaymentStatus
    {
        PendingApproval = 0,
        Active = 1,
        Paused = 2,
        Cancelled = 3,
        Completed = 4,
        Deleted = 5
    }
}