using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities
{
    public class FinancialRecordEntity
    {
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public string FinType { get; set; }
        public int FinCode { get; set; }
        public DateTime FinDate { get; set; }
        public int FinAmount { get; set; }
    }
}