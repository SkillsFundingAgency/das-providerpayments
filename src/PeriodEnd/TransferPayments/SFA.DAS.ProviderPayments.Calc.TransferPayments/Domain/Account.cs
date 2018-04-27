using System;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
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

        public long Id
        {
            get { return AccountId; }
        }

        private long AccountId { get; }
        private decimal Balance { get; set; }
        private bool IsLevyPayer { get; }
        private decimal TransferBalance { get; set; }
        private decimal AvailableTransferBalance { get; set; }

        private void UpdateHasTransferBalance()
        {
            AvailableTransferBalance = Math.Min(TransferBalance, Balance);
            AvailableTransferBalance = Math.Max(AvailableTransferBalance, 0); // Set the floor to 0

            HasTransferBalance = (AvailableTransferBalance > 0) && IsLevyPayer;
        }

        public bool HasTransferBalance { get; private set; }

        public CreateTransferResult CreateTransfer(Account receiver, RequiredTransferPayment requiredPayment)
        {
            //if (receiver.Id != requiredPayment.AccountId)
            //{
            //    throw new ArgumentException($"There is a mismatch between the required payment and the receiving account " +
            //                                $"The receiving account id is: {receiver.AccountId} and the required payment " +
            //                                $"is for {requiredPayment.AccountId}");
            //}

            var result = new CreateTransferResult();
            var amountToTransfer = Math.Min(requiredPayment.AmountDue, AvailableTransferBalance);
            
            var transfer = new AccountLevyTransfer(requiredPayment, amountToTransfer);
            var payment = CreateTransferPayment(requiredPayment, transfer.Amount);

            result.AccountLevyTransfer = transfer;
            result.TransferLevyPayment = payment;

            result.Amount = transfer.Amount;

            Balance -= transfer.Amount;
            TransferBalance -= transfer.Amount;

            UpdateHasTransferBalance();

            return result;
        }

        private TransferLevyPayment CreateTransferPayment(RequiredTransferPayment requiredPayment, decimal amount)
        {
            var payment = new TransferLevyPayment(requiredPayment, amount);

            return payment;
        }
    }
}
