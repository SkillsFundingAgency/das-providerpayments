using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        private static ConcurrentDictionary<long, Account> _accounts;

        public LevyTransferService(
            ILogger logger, 
            IAmAnAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            LoadAccounts();
        }

        public void LoadAccounts()
        {
            _logger.Info("Loading accounts");
            var accounts = _accountRepository.AllAccounts();
            _accounts = new ConcurrentDictionary<long, Account>
                (accounts.Select(x => new KeyValuePair<long, Account>(x.Id, x)));
            _logger.Info("Finished loading accounts");
        }

        public List<TransferPaymentSet> ProcessSendingAccount(long sendingAccountId, IEnumerable<RequiredTransferPayment> requiredPayments)
        {

            _logger.Info($"Starting transfers for sending account id: {sendingAccountId}");

            _logger.Info("Getting sending account");
            Account sendingAccount;
            if (!_accounts.TryGetValue(sendingAccountId, out sendingAccount))
            {
                _logger.Error($"Could not find account with id {sendingAccountId} - aborting processing for account");
                return new List<TransferPaymentSet>();
            }
            _logger.Info("Found sending account");

            var transferListForAccount = new List<TransferPaymentSet>();

            var groupedPayments = requiredPayments.GroupBy(x => x.AccountId);
            foreach (var paymentList in groupedPayments)
            {
                _logger.Info($"Processing transfers from {sendingAccountId} to {paymentList.Key}");
                Account receivingAccount;
                if (!_accounts.TryGetValue(paymentList.Key, out receivingAccount))
                {
                    _logger.Error($"Could not find receiving account with id {paymentList.Key} - aborting processing for account");
                    continue;
                }

                var sortedPayments = paymentList.OrderBy(x => x.TransferApprovedDate).ThenBy(x => x.Uln);
                _logger.Info($"Creating transfer payment sets for sending account {sendingAccountId} and receiving account: {receivingAccount.Id}");
                var results = ProcessTransfers(sendingAccount, receivingAccount, sortedPayments);
                transferListForAccount.Add(results);
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
                var payment = sendingAccount.CreateTransferPayment(requiredPayment, transferResult.Amount);

                result.AddTransferWithPayment(transferResult.AccountLevyTransfer, payment);
            }

            return result;
        }
    }
}
