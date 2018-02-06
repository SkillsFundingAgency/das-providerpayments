using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.Provider.Events.Submission.Domain;
using SFA.DAS.Provider.Events.Submission.Domain.Data;

namespace SFA.DAS.Provider.Events.Submission.Infrastructure.Data
{
    public class DcfsSubmissionEventRepository : DcfsRepository, ISubmissionEventRepository
    {
        public DcfsSubmissionEventRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void StoreSubmissionEvents(SubmissionEvent[] events)
        {
            var columns = typeof(SubmissionEvent).GetProperties().ToDictionary(p => p.Name, p =>
            {
                if (p.Name == "Ukprn" || p.Name == "Uln") return p.Name.ToUpperInvariant();
                if (p.Name == "NiNumber") return "NINumber";
                return p.Name;
            });

            ExecuteBatch(events, "Submissions.SubmissionEvents", columns);
        }

    }
}
