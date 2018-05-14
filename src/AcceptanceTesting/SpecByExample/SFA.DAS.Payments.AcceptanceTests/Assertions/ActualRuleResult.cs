using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions
{
    public class ActualRuleResult
    {
        public List<LearnerResults> LearnerResults { get; set; }
        public List<TransferResult> TransferResults { get; set; }
    }
}