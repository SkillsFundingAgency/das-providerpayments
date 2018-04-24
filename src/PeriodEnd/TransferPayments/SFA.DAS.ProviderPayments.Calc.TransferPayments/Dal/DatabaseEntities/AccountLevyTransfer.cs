using System;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities
{
    public class AccountLevyTransfer
    {
        public AccountLevyTransfer(RequiredTransferPayment requiredPayment, decimal amount)
        {
            SendingAccountId = requiredPayment.TransferSendingEmployerAccountId;
            ReceivingAccountId = requiredPayment.AccountId;
            RequiredPaymentId = requiredPayment.RequiredPaymentId;
            CommitmentId = requiredPayment.CommitmentId;
            FundingSource = FundingSource.Transfer;
            CollectionPeriodMonth = requiredPayment.CollectionPeriodMonth;
            CollectionPeriodYear = requiredPayment.CollectionPeriodYear;
            CollectionPeriodName = requiredPayment.CollectionPeriodName;
            Amount = amount;
        }

        public long SendingAccountId { get; set; }
        public long ReceivingAccountId { get; set; }
        public Guid RequiredPaymentId { get; set; }
        public long CommitmentId { get; set; }
        public decimal Amount { get; set; }
        public FundingSource FundingSource { get; set; }
        public DateTime TransferedDate { get; set; }
        public string CollectionPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
    }
}
