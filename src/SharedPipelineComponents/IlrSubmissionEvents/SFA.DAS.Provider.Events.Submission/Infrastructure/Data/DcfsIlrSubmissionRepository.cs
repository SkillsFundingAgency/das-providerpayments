using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.Provider.Events.Submission.Domain;
using SFA.DAS.Provider.Events.Submission.Domain.Data;

namespace SFA.DAS.Provider.Events.Submission.Infrastructure.Data
{
    public class DcfsIlrSubmissionRepository : DcfsRepository, IIlrSubmissionRepository
    {
        private readonly string _connectionString;

        public DcfsIlrSubmissionRepository(string connectionString)
            : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public IlrDetails[] GetCurrentVersions()
        {
            return Query<IlrDetails>("SELECT * FROM Submissions.CurrentVersion");
        }

        public IlrDetails[] GetLastSeenVersions()
        {
            return Query<IlrDetails>("SELECT * FROM Submissions.LastSeenVersion");
        }

        public void StoreLastSeenVersions(IlrDetails[] details)
        {
            if (details == null || details.Length == 0)
                return;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new IlrIdentifierTableValueParam(details);
                connection.Execute("[Submissions].[DeleteLastSeenVersions]", parameters, commandType: CommandType.StoredProcedure);
            }

            var columns = typeof(IlrDetails).GetProperties().ToDictionary(p => p.Name, p =>
            {
                if (p.Name == "Ukprn" || p.Name == "Uln") return p.Name.ToUpperInvariant();
                if (p.Name == "NiNumber") return "NINumber";
                return p.Name;
            });

            ExecuteBatch(details, "Submissions.LastSeenVersion", columns);
        }
    }
}
