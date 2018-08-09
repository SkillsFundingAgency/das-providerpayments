using System;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers;
using SFA.DAS.Payments.Reference.Accounts.IntegrationTests.StubbedInfrastructure;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.GivenApiAvailable
{
    public class WhenReadingAccounts
    {
        private ImportAccountsTask _task;
        private IntegrationTaskContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            AccountDataHelper.ClearAccounts();
        }

        [SetUp]
        public void Arrange()
        {
            StubbedApiClient.Accounts.Clear();
            
            _task = new ImportAccountsTask();

            _context = new IntegrationTaskContext();
        }

        [Test]
        public void ThenItShouldAddAccountsThatDoNotExist()
        {
            // Arrange
            var accountId = DateTime.Now.Ticks;
            var hashId = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            const decimal balance = 123456.7878m;
            const decimal transferAllowance = 98765m;

            StubbedApiClient.Accounts.Add(new AccountWithBalanceViewModel
            {
                AccountId = accountId,
                AccountHashId = hashId,
                AccountName = name,
                Balance = balance,
                IsLevyPayer = true,
                TransferAllowance = transferAllowance
            });

            // Act
            _task.Execute(_context);

            // Assert
            var account = AccountDataHelper.GetAccountById(accountId);
            Assert.IsNotNull(account);
            Assert.AreEqual(hashId, account.AccountHashId);
            Assert.AreEqual(name, account.AccountName);
            Assert.AreEqual(balance, account.Balance);
            Assert.IsTrue(account.IsLevyPayer);
            Assert.AreEqual(transferAllowance, account.TransferAllowance);
        }


        [Test]
        public void ThenItShouldUpdateAccountsTheDoExist()
        {
            // Arrange
            var accountId = DateTime.Now.Ticks;
            var hashId = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            const decimal balance = 98765.3232m;
            const decimal transferAllowance = 12345m;

            StubbedApiClient.Accounts.Add(new AccountWithBalanceViewModel
            {
                AccountId = accountId,
                AccountHashId = hashId,
                AccountName = name,
                Balance = balance,
                IsLevyPayer=true,
                TransferAllowance =  transferAllowance
            });
            AccountDataHelper.AddAccount(accountId, "ThenItShouldUpdateAccountsTheDoExist", "ThenItShouldUpdateAccountsTheDoExist", 99999, "11112233",true, transferAllowance);

            // Act
            _task.Execute(_context);

            // Assert
            var account = AccountDataHelper.GetAccountById(accountId);
            Assert.IsNotNull(account);
            Assert.AreEqual(hashId, account.AccountHashId);
            Assert.AreEqual(name, account.AccountName);
            Assert.AreEqual(balance, account.Balance);
            Assert.IsTrue(account.IsLevyPayer);
            Assert.AreEqual(transferAllowance, account.TransferAllowance);
        }


        [Test]
        public void ThenItShouldWriteAuditRecord()
        {
            // Arrange
            var accountId = DateTime.Now.Ticks;
            var hashId = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            const decimal balance = 98765.3232m;
            const decimal transferAllowance = 12345m;

            StubbedApiClient.Accounts.Add(new AccountWithBalanceViewModel
            {
                AccountId = accountId,
                AccountHashId = hashId,
                AccountName = name,
                Balance = balance,
                TransferAllowance = transferAllowance
            });

            // Act
            _task.Execute(_context);

            // Assert
            var audit = AuditDataHelper.GetLatestAccountAuditRecord();
            Assert.IsNotNull(audit);
            Assert.AreEqual(DateTime.Today, audit.ReadDateTime);
            Assert.AreEqual(1, audit.AccountsRead);
            Assert.IsTrue(audit.CompletedSuccessfully);
        }
    }
}