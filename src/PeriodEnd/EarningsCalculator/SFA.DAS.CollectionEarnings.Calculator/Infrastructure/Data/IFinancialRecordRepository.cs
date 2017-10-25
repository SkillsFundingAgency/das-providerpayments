using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data
{
    public interface IFinancialRecordRepository
    {
        FinancialRecordEntity[] GetLearningDeliveryFinancialRecords(string learnRefNumber, int aimSeqNumber);
    }
}