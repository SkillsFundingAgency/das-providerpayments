using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.ResultsDataModels
{
    public class TransferResult
    {
        public long SendingAccountId { get; set; }
        public long ReceivingEmployerAccountId { get; set; }
        public string CollectionPeriodName { get; set; }
        public decimal Amount { get; set; }
    }
}