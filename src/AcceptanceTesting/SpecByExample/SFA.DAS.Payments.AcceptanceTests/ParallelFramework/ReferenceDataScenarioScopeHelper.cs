using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.ParallelFramework
{
    public static class ReferenceDataScenarioScopeHelper
    {
        public static List<CommitmentReferenceData> ScopeToScenario(List<CommitmentReferenceData> data, int scenarioId)
        {

        }

        public static CommitmentReferenceData ScopeToScenario(CommitmentReferenceData data, int scenarioId)
        {
            data.CommitmentId = PrefixId(scenarioId, data.CommitmentId);
            data.EmployerAccountId = PrefixId(scenarioId, data.EmployerAccountId);
            data.ProviderId = PrefixString(scenarioId, data.ProviderId);
            data.LearnerId = PrefixString(scenarioId, data.LearnerId);
            if(data.TransferSendingEmployerAccountId.HasValue)
                data.TransferSendingEmployerAccountId = PrefixId(scenarioId, data.TransferSendingEmployerAccountId.Value);
            data.Ukprn = PrefixLong(scenarioId, data.Ukprn);
            data.Uln = PrefixLong(scenarioId, data.Uln);
            return data;
        }

        private static long PrefixLong(int scenarioId, long referenceDataLong)
        {
            return long.Parse(scenarioId.ToString() + referenceDataLong.ToString());
        }

        private static int PrefixId(int scenarioId, int referenceDataId)
        {
            return int.Parse(scenarioId.ToString() + referenceDataId.ToString());
        }

        private static string PrefixString(int scenarioId, string referenceDataString)
        {
            return scenarioId.ToString() + referenceDataString;
        }
    }
}
