using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsLearnerFAMRepository : DcfsRepository, ILearnerFAMRepository
    {
        public const string LearnerFAMSource = "Valid.LearnerFAM";
        public const string LearnerFAMColumns = "LearnRefNumber, LearnFAMType, LearnFAMCode";
        public const string SelectLearnerFAMRecords = "SELECT " + LearnerFAMColumns + " FROM " + LearnerFAMSource + " WHERE LearnRefNumber = @LearnRefNumber";

        public DcfsLearnerFAMRepository(string connectionString) : base(connectionString)
        {
        }

        public LearnerFAMEntity[] GetLearnerFAMRecords(string learnRefNumber)
        {
            return Query<LearnerFAMEntity>(SelectLearnerFAMRecords, new {learnRefNumber});
        }
    }
}
