using ProviderPayments.TestStack.Domain;

namespace ProviderPayments.TestStack.UI.Models
{
    public class CommitmentModel : Commitment
    {
        public string SelectedCourse { get; set; }

        public Account[] Accounts { get; set; }
        public Provider[] Providers { get; set; }
        public Learner[] Learners { get; set; }
        public Standard[] Standards { get; set; }
        public Framework[] Frameworks { get; set; }
    }
}