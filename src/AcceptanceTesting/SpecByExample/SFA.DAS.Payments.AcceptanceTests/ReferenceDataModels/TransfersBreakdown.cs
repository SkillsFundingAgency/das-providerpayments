using System.Collections.Generic;

namespace SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels
{
    public class TransfersBreakdown
    {
        public TransfersBreakdown()
        {
            TransferLines = new List<TransferLine>();
        }

        public int SendingEmployerAccountId { get; set; }
        public List<TransferLine> TransferLines { get; set; }
    }

    public class TransferLine
    {
        public TransferLine()
        {
            TransferAmounts = new List<PeriodValue>();
        }

        public int ReceivingEmployerAccountId { get; set; }
        public List<PeriodValue> TransferAmounts { get; set; }
    }
}