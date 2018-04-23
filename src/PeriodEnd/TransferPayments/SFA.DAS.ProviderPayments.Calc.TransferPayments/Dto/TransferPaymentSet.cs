using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto
{
    public class TransferPaymentSet
    {
        public void AddTransferWithPayment(AccountLevyTransfer transfer, TransferLevyPayment payment)
        {
            TransferLevyPayments.Add(payment);
            AccountLevyTransfers.Add(transfer);
        }

        private List<TransferLevyPayment> TransferLevyPayments { get; } = new List<TransferLevyPayment>();
        private List<AccountLevyTransfer> AccountLevyTransfers { get; } = new List<AccountLevyTransfer>();


        public IReadOnlyList<TransferLevyPayment> TransferPayments
        {
            get { return TransferLevyPayments; }
        }

        public IReadOnlyList<AccountLevyTransfer> AccountTransfers
        {
            get { return AccountLevyTransfers; }
        }
    }
}