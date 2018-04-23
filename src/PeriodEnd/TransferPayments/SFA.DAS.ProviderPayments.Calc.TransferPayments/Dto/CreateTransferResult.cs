using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto
{
    public class CreateTransferResult
    {
        public AccountLevyTransfer AccountLevyTransfer { get; set; }
        public decimal Amount { get; set; }
    }
}