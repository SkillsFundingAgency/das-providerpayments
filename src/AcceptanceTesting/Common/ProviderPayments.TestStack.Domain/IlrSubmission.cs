using System.Collections.Generic;

namespace ProviderPayments.TestStack.Domain
{
    public class IlrSubmission
    {
        public string Id { get; set; }
        public long Ukprn { get; set; }
        public string YearOfCollection { get; set; }
        public List<IlrLearner> Learners { get; set; }
    }
}
