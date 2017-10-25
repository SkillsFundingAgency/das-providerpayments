using System.Collections.Generic;
using ProviderPayments.TestStack.Domain;

namespace ProviderPayments.TestStack.UI.Models
{
    public class IlrSubmissionModel : IlrSubmission
    {
        public new List<IlrLearnerModel> Learners { get; set; }

        public Provider[] Providers { get; set; }
        public Standard[] Standards { get; set; }
        public Framework[] Frameworks { get; set; }
    }
}