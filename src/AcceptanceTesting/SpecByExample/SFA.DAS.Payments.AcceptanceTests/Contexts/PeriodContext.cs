using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class PeriodContext
    {
        public PeriodContext()
        {
            PeriodResults = new List<LearnerResults>();
        }

        public List<LearnerResults> PeriodResults { get; set; }
        public List<TransferResult> TransferResults { get; set; }
    }
}