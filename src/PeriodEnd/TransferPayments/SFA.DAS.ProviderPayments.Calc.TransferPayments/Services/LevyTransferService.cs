using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NLog;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dto;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.Services
{
    public class LevyTransferService : IProcessLevyTransfers
    {
        private readonly ILogger _logger;
        private readonly IAmAnAccountRepository _accountRepository;

        [UsedImplicitly]
        public LevyTransferService(
            ILogger logger, 
            IAmAnAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public List<TransferPaymentSet> ProcessSendingAccount(long sendingAccountId, IEnumerable<RequiredTransferPayment> requiredPayments)
        {
            _logger.Info($"Starting transfers for sending account id: {sendingAccountId}");

            _logger.Info("Getting sending account");
            var sendingAccount = _accountRepository.Account(sendingAccountId);
            if (sendingAccount == null)
            {
                _logger.Error($"Could not find account with id {sendingAccountId} - aborting processing for account");
                return new List<TransferPaymentSet>();
            }

            _logger.Info("Found sending account");

            var transferListForAccount = new List<TransferPaymentSet>();

            var groupedPaymentsByApprovedDate = requiredPayments
                .GroupBy(x => x.TransferApprovedDate)
                .OrderBy(x => x.Key);

            foreach (var paymentList in groupedPaymentsByApprovedDate)
            {
                var sortedPayments = paymentList.OrderBy(x => x.Uln);
                foreach (var requiredTransferPayment in sortedPayments)
                {
                    var receivingAccount = _accountRepository.Account(requiredTransferPayment.AccountId);
                    if (receivingAccount == null)
                    {
                        _logger.Error($"Could not find account with id: {requiredTransferPayment.AccountId}");
                        continue;
                    }
                    _logger.Info($"Processing transfer from {sendingAccountId} to {receivingAccount.Id}");
                    var results = ProcessTransfers(sendingAccount, receivingAccount, new List<RequiredTransferPayment>{requiredTransferPayment});
                    transferListForAccount.Add(results);
                }
            }

            return transferListForAccount;
        }

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
                
                result.AddTransferWithPayment(
                    transferResult.AccountLevyTransfer, 
                    transferResult.TransferLevyPayment);
            }

            return result;
        }
    }
}
