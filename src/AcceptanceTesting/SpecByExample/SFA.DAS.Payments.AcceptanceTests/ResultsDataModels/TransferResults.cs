using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.ResultsDataModels
{
    public class TransferResults
    {
        public TransferResults()
        {
            Values = new List<TransferPeriodValueEntity>();
        }

        public int ReceivingEmployerAccountId { get; set; }
        public List<TransferPeriodValueEntity> Values { get; set; }
    }
}