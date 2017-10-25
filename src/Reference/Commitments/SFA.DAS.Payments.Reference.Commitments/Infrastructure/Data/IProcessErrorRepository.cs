namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data
{
    public interface IProcessErrorRepository
    {
        void WriteError(string details);
    }
}