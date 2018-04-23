using System;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain
{
    public class Account
    {
        public Account(DasAccount persistedEntity)
        {
            AccountId = persistedEntity.AccountId;
            Balance = persistedEntity.Balance;
            IsLevyPayer = persistedEntity.IsLevyPayer;
            TransferBalance = persistedEntity.TransferBalance;

            UpdateHasTransferBalance();
        }

        long AccountId { get; set; }
        decimal Balance { get; set; }
        bool IsLevyPayer { get; set; }
        decimal TransferBalance { get; set; }
        private decimal AvailableTransferBalance { get; set; }

        void UpdateHasTransferBalance()
        {
            AvailableTransferBalance = Math.Min(TransferBalance, Balance);
            AvailableTransferBalance = Math.Max(AvailableTransferBalance, 0); // Set the floor to 0

            HasTransferBalance = (AvailableTransferBalance > 0) && IsLevyPayer;
        }

        public bool HasTransferBalance { get; private set; }

        public CreateTransferResult CreateTransfer(Account receiver, RequiredTransferPayment requiredPayment)
        {
            var result = new CreateTransferResult();
            var amountToTransfer = Math.Min(requiredPayment.AmountDue, AvailableTransferBalance);

            var transfer = new AccountLevyTransfer(requiredPayment, amountToTransfer);

            result.AccountLevyTransfer = transfer;
            result.Amount = transfer.Amount;

            Balance -= transfer.Amount;
            UpdateHasTransferBalance();

            return result;
        }
    }
}
