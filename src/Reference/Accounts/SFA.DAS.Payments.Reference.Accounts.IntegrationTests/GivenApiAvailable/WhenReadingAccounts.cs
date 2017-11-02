﻿using System;
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

        [SetUp]
        public void Arrange()
        {
            StubbedApiClient.Accounts.Clear();

            _task = new ImportAccountsTask();

            _context = new IntegrationTaskContext();
        }

        [Test]
        public void ThenItShouldAddAcountsThatDoNotExist()
        {
            // Arrange
            var accountId = DateTime.Now.Ticks;
            var hashId = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            var balance = 123456.7878m;

            StubbedApiClient.Accounts.Add(new AccountWithBalanceViewModel
            {
                AccountId = accountId,
                AccountHashId = hashId,
                AccountName = name,
                Balance = balance,
                IsLevyPayer = true
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

        }


        [Test]
        public void ThenItShouldUpdateAccountsTheDoExist()
        {
            // Arrange
            var accountId = DateTime.Now.Ticks;
            var hashId = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            var balance = 98765.3232m;

            StubbedApiClient.Accounts.Add(new AccountWithBalanceViewModel
            {
                AccountId = accountId,
                AccountHashId = hashId,
                AccountName = name,
                Balance = balance,
                IsLevyPayer=true
            });
            AccountDataHelper.AddAccount(accountId, "ThenItShouldUpdateAccountsTheDoExist", "ThenItShouldUpdateAccountsTheDoExist", 99999, "11112233",true);

            // Act
            _task.Execute(_context);

            // Assert
            var account = AccountDataHelper.GetAccountById(accountId);
            Assert.IsNotNull(account);
            Assert.AreEqual(hashId, account.AccountHashId);
            Assert.AreEqual(name, account.AccountName);
            Assert.AreEqual(balance, account.Balance);
            Assert.IsTrue(account.IsLevyPayer);

        }


        [Test]
        public void ThenItShouldWriteAuditRecord()
        {
            // Arrange
            var accountId = DateTime.Now.Ticks;
            var hashId = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            var balance = 98765.3232m;

            StubbedApiClient.Accounts.Add(new AccountWithBalanceViewModel
            {
                AccountId = accountId,
                AccountHashId = hashId,
                AccountName = name,
                Balance = balance
            });

            // Act
            _task.Execute(_context);

            // Assert
            var audit = AuditDataHelper.GetLatestAuditRecord();
            Assert.IsNotNull(audit);
            Assert.AreEqual(DateTime.Today, audit.ReadDateTime);
            Assert.AreEqual(1, audit.AccountsRead);
            Assert.IsTrue(audit.CompletedSuccessfully);
            
        }
    }
}