using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Repositories
{
    public class FinancialRecordRepository : DcfsRepository, IFinancialRecordRepository
    {
        private const string FinancialRecordsSource = "Valid.TrailblazerApprenticeshipFinancialRecord";
        private const string FinancialRecordsColumns = "LearnRefNumber," +
                                                       "AimSeqNumber," +
                                                       "TBFinType AS FinType," +
                                                       "TBFinCode AS FinCode," +
                                                       "TBFinDate AS FinDate ," +
                                                       "TBFinAmount AS FinAmount";
        private const string SelectFinancialRecords = "SELECT " + FinancialRecordsColumns + " FROM " + FinancialRecordsSource;
        private const string SelectLearningDeliveryFinancialRecords = SelectFinancialRecords + " WHERE TBFinType = @finType AND LearnRefNumber = @learnRefNumber AND AimSeqNumber = @aimSeqNumber" +
                                                                                                " ORDER BY TBFinDate ASC, TBFinCode ASC";

        public FinancialRecordRepository(string connectionString)
            : base(connectionString)
        {
        }

        public FinancialRecordEntity[] GetLearningDeliveryFinancialRecords(string learnRefNumber, int aimSeqNumber)
        {
            return Query<FinancialRecordEntity>(SelectLearningDeliveryFinancialRecords, new { finType  = "TNP", learnRefNumber, aimSeqNumber});
        }
    }
}