using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Services
{
    public class LevyTransferService
    {
        public TransferPaymentSet ProcessTransfers(
            Account sendingAccount, 
            Account receiver,
            IEnumerable<RequiredTransferPayment> requiredPayments)
        {
            var result = new TransferPaymentSet();

            foreach (var requiredPayment in requiredPayments)
            {
                if (!sendingAccount.HasTransferBalance)
                {
                    break;
                }

                var transferResult = sendingAccount.CreateTransfer(receiver, requiredPayment);
                var payment = sendingAccount.CreateTransferPayment(requiredPayment, transferResult.Amount);

                result.AddTransferWithPayment(transferResult.AccountLevyTransfer, payment);
            }

            return result;
        }
    }
}
