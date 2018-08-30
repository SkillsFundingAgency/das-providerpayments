using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class CommitmentsContext
    {
        public CommitmentsContext()
        {
            Commitments = new List<CommitmentReferenceData>();
            CommitmentsForPeriod = new Dictionary<string, List<CommitmentReferenceData>>();
        }
        public List<CommitmentReferenceData> Commitments { get; set; }

        public Dictionary<string, List<CommitmentReferenceData>> CommitmentsForPeriod { get; }

    }
}
