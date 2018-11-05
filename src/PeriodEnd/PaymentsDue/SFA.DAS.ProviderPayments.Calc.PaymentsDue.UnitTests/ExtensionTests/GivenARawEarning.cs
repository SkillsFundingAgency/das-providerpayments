using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ExtensionTests
{
    [TestFixture]
    public class GivenARawEarning
    {
        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsFalseForAClearEarning(
            RawEarning test)
        {
            ClearRawEarning(test);

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT01IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType01 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT02IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType02 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT03IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType03 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT04IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType04 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT05IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType05 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT06IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType06 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }
        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT07IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType07 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT08IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType08 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT09IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType09 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT10IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType10 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT11IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType11 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT12IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType12 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT13IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType13 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT14IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType14 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenT15IsPresent(
            RawEarning test)
        {
            ClearRawEarning(test);
            test.TransactionType15 = 10;

            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenHasNonZeroTransactionsIsTrueWhenMultipleTransactionTypesPresent(
            RawEarning test)
        {
            var actual = test.HasNonZeroTransactions();

            actual.Should().BeTrue();
        }

        private void ClearRawEarning(RawEarning target)
        {
            target.TransactionType01 = 0;
            target.TransactionType02 = 0;
            target.TransactionType03 = 0;
            target.TransactionType04 = 0;
            target.TransactionType05 = 0;
            target.TransactionType06 = 0;
            target.TransactionType07 = 0;
            target.TransactionType08 = 0;
            target.TransactionType09 = 0;
            target.TransactionType10 = 0;
            target.TransactionType11 = 0;
            target.TransactionType12 = 0;
            target.TransactionType13 = 0;
            target.TransactionType14 = 0;
            target.TransactionType15 = 0;
            target.TransactionType16 = 0;
        }
    }
}
