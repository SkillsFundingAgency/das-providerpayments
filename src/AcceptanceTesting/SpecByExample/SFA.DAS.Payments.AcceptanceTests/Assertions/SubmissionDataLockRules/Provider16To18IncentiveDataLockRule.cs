using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Assertions.SubmissionDataLockRules
{
    public class Provider16To18IncentiveDataLockRule : SubmissionDataLockRuleBase
    {
        public Provider16To18IncentiveDataLockRule() : base("provider 16-18 incentive")
        {
        }

        protected override IEnumerable<SubmissionDataLockResult> FilterPeriodStatuses(SubmissionDataLockPeriodResults periodStatuses)
        {
            return periodStatuses.Matches.Where(m => m.TransactionTypesFlag == ReferenceDataModels.TransactionTypesFlag.FirstEmployerProviderIncentives
                                                  || m.TransactionTypesFlag == ReferenceDataModels.TransactionTypesFlag.SecondEmployerProviderIncentives);
        }
    }
}
