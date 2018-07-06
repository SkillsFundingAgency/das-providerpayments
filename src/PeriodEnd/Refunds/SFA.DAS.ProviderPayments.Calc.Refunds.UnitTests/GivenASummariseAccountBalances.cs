using System;
using System.Linq;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.NUnit3;
using Dapper;
using FluentAssertions;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests
{
    [TestFixture]
    public class GivenASummariseAccountBalances
    {
        protected SummariseAccountBalances _sut;

        [SetUp]
        public void Init()
        {
            _sut = new SummariseAccountBalances(Mock.Of<ILogger>());
        }

        [TestFixture]
        public class WhenAddingAGroupOfRefundsWhichHaveUniqueAccountIds : GivenASummariseAccountBalances
        {
            private List<Refund> _refunds;

            [SetUp]
            public void Setup()
            {
                var fixture = new Fixture();
                _refunds = fixture.Build<Refund>().CreateMany(3).ToList();
                long i = 0;

                _refunds.ForEach(x =>
                {
                    x.AccountId = ++i;
                    x.Amount = -100 * i;
                });

                _sut.IncrementAccountLevyBalance(_refunds);
            }

            [Test]
            public void ThenItReturns3Accounts()
            {
                var result = _sut.AsList();
                result.Count.Should().Be(3);
            }

            [TestCase(1, 100)]
            [TestCase(2, 200)]
            [TestCase(3, 300)]
            public void ThenItReturnsCorrectCreditForEachAccount(
                long accountId,
                decimal expectedCredit)
            {

                var result = _sut.AsList();
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
                var fixture = new Fixture();
                _refunds = fixture.Build<Refund>().CreateMany(3).ToList();

                _refunds.ForEach(x =>
                {
                    x.AccountId = 1;
                    x.Amount = -100;
                });

                _sut.IncrementAccountLevyBalance(_refunds);
            }

            [Test]
            public void ThenItReturns1Account()
            {
                var result = _sut.AsList();
                result.Count.Should().Be(1);
            }

            [Test]
            public void ThenItReturnsCorrectCreditForThisAccount()
            {
                var result = _sut.AsList();
                result.First().LevyCredit.Should().Be(300);
            }

        }
    }
}
