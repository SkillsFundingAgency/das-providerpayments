using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Dcfs
{
    public class DcfsProcessErrorRepository : DcfsRepository, IProcessErrorRepository
    {
        private const string Source = "ProcessError";
        private const string Columns = "ErrorDetails";
        private const string InsertCommand = "INSERT INTO " + Source + " (" + Columns + ") VALUES (@details)";

        public DcfsProcessErrorRepository(ContextWrapper context)
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public void WriteError(string details)
        {
            Execute(InsertCommand, new { details });
        }
    }
}
