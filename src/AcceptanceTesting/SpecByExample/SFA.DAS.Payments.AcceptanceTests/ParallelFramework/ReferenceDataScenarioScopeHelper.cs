using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.ParallelFramework
{
    public static class ReferenceDataScenarioScopeHelper
    {
        public static List<CommitmentReferenceData> ScopeToScenario(List<CommitmentReferenceData> data, int scenarioId)
        {
            return data.Select(x => ScopeToScenario(x, scenarioId)).ToList();
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

        public static Submission ScopeToScenario(Submission submission, int scenarioId)
        {
            for (int i = 0; i < submission.EmploymentStatus.Count; i++)
            {
                if(submission.EmploymentStatus[i].EmployerId.HasValue)
                    submission.EmploymentStatus[i].EmployerId = PrefixId(scenarioId, submission.EmploymentStatus[i].EmployerId.Value);
            }

            for (int i = 0; i < submission.IlrLearnerDetails.Count; i++)
            {
                submission.IlrLearnerDetails[i].
            }

            return submission;
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
