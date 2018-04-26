using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto
{
    public class CreateTransferResult
    {
        public AccountLevyTransfer AccountLevyTransfer { get; set; }
        public TransferLevyPayment TransferLevyPayment { get; set; }
        public decimal Amount { get; set; }
    }
}