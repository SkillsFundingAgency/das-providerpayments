﻿using System.Linq;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests
{
    [TestFixture]
    public class GivenASummariseAccountBalances
    {
        protected SummariseAccountBalances Sut;

        [SetUp]
        public void Init()
        {
            Sut = new SummariseAccountBalances(Mock.Of<ILogger>());
        }

        [TestFixture]
        public class WhenAddingAGroupOfRefundsWhichHaveUniqueAccountIds : GivenASummariseAccountBalances
        {
            private List<Refund> _refunds;

            [SetUp]
            public void Setup()
            {
                _refunds = GenerateListOfRefundsForDifferentAccounts();
                Sut.IncrementAccountLevyBalance(_refunds);
            }

            [Test]
            public void ThenItReturns3Accounts()
            {
                var result = Sut.AsList();
                result.Count.Should().Be(3);
            }

            [TestCase(1, 100)]
            [TestCase(2, 200)]
            [TestCase(3, 300)]
            public void ThenItReturnsCorrectCreditForEachAccount(
                long accountId,
                decimal expectedCredit)
            {
                var result = Sut.AsList();
                result.First(x => x.AccountId == accountId).LevyCredit.Should().Be(expectedCredit);
            }
        }

        [TestFixture]
        public class WhenAddingAGroupOfRefundsWithDuplicateAccountIds : GivenASummariseAccountBalances
        {
            private List<Refund> _refunds;

            [SetUp]
            public void Setup()
            {
                _refunds = GenerateListOfRefundsForSameAccount();
                Sut.IncrementAccountLevyBalance(_refunds);
            }

            [Test]
            public void ThenItReturns1Account()
            {
                var result = Sut.AsList();
                result.Count.Should().Be(1);
            }

            [Test]
            public void ThenItReturnsCorrectCreditForThisAccount()
            {
                var result = Sut.AsList();
                result.First().LevyCredit.Should().Be(300);
            }

        }
        public class WhenAddingASecondSetOfRefundsWhichHaveExistingAccountIds : GivenASummariseAccountBalances
        {
            private List<Refund> _refunds;

            [SetUp]
            public void Setup()
            {
                _refunds = GenerateListOfRefundsForDifferentAccounts();
                Sut.IncrementAccountLevyBalance(_refunds);
                Sut.IncrementAccountLevyBalance(_refunds);
            }

            [Test]
            public void ThenTheCountShouldStillBe3()
            {
                var result = Sut.AsList();
                result.Count.Should().Be(3);
            }

            [TestCase(1, 200)]
            [TestCase(2, 400)]
            [TestCase(3, 600)]
            public void ThenItReturnsTheCollectiveCreditsForEachAccount(
                long accountId,
                decimal expectedCredit)
            {
                var result = Sut.AsList();
                result.First(x => x.AccountId == accountId).LevyCredit.Should().Be(expectedCredit);
            }
        }

        protected List<Refund> GenerateListOfRefundsForSameAccount()
        {
            var fixture = new Fixture();
            var refunds = fixture.Build<Refund>()
                .With(x => x.AccountId, 1)
                .With(x => x.Amount, -100)
                .With(x => x.FundingSource, FundingSource.Levy)
                .CreateMany(3)
                .ToList();

            var nonLearningRefunds = GenerateListOfRefundsForNonLearningTransactionAccounts();
            refunds.AddRange(nonLearningRefunds);

            return refunds;
        }

        protected List<Refund> GenerateListOfRefundsForDifferentAccounts()
        {
            var fixture = new Fixture();
            var refunds = fixture.Build<Refund>()
                .With(x => x.FundingSource, FundingSource.Levy)
                .CreateMany(3)
                .ToList();
            long i = 0;

            refunds.ForEach(x =>
            {
                x.AccountId = ++i;
                x.Amount = -100 * i;
            });

            var nonLearningRefunds = GenerateListOfRefundsForNonLearningTransactionAccounts();

            refunds.AddRange(nonLearningRefunds);

            return refunds;
        }

        protected List<Refund> GenerateListOfRefundsForNonLearningTransactionAccounts()
        {
            var fixture = new Fixture();
            return fixture.Build<Refund>()
                .With(x => x.FundingSource,
                    fixture.Create<Generator<FundingSource>>().First(x => x != FundingSource.Levy))
                .CreateMany(3).ToList();
        }
    }
}
