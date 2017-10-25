using SFA.DAS.Payments.Automation.Domain.Specifications;
using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public static class TransformationHelpers
    {
        internal static Dictionary<string, string> TransformLearnerKeysForLearners(Specification[] specifications)
        {
            var index = 1;

            var learnerScenarios = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var spec in specifications)
            {
                var processedLearners = new Dictionary<string, string>();

                foreach (var learner in spec.Arrangement.LearnerRecords)
                {
                    if (!processedLearners.ContainsKey(learner.LearnerKey))
                    {
                        var key = $"{learner.LearnerKey}{index}";
                        processedLearners.Add(learner.LearnerKey, key);
                        learnerScenarios.Add(key, spec.Name);

                        learner.LearnerKey = $"{learner.LearnerKey}{index}";
                    }
                    else
                    {
                        learner.LearnerKey = processedLearners[learner.LearnerKey];
                    }
                }
                index += 1;
            }
            return learnerScenarios;
        }
    }
}
