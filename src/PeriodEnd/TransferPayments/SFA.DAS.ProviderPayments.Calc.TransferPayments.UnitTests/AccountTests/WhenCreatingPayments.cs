using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.AccountTests
{
    [TestFixture]
    public partial class WhenCreatingPayments
    {
        [Test, AutoData]
        public void ThenTheRequiredPaymentIdIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.RequiredPaymentId.Should().Be(requiredPayment.RequiredPaymentId);
        }

        [Test, AutoData]
        public void ThenTheDeliveryMonthIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.DeliveryMonth.Should().Be(requiredPayment.DeliveryMonth);
        }

        [Test, AutoData]
        public void ThenTheDeliveryYearIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.DeliveryYear.Should().Be(requiredPayment.DeliveryYear);
        }

        [Test, AutoData]
        public void ThenTheCollectionPeriodNameIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.CollectionPeriodName.Should().Be(requiredPayment.CollectionPeriodName);
        }

        [Test, AutoData]
        public void ThenTheCollectionPeriodMonthIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.CollectionPeriodMonth.Should().Be(requiredPayment.CollectionPeriodMonth);
        }

        [Test, AutoData]
        public void ThenTheCollectionPeriodYearIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.CollectionPeriodYear.Should().Be(requiredPayment.CollectionPeriodYear);
        }

        [Test, AutoData]
        public void ThenTheFundingSourceIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.FundingSource.Should().Be(FundingSource.Transfer);
        }

        [Test, AutoData]
        public void ThenTheTransactionTypeIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.TransactionType.Should().Be(requiredPayment.TransactionType);
        }

        [Test, AutoData]
        public void ThenTheAmountIsCorrect(
            RequiredTransferPayment requiredPayment,
            Account sut,
            decimal amount)
        {
            var actual = sut.CreatePayment(requiredPayment, amount);

            actual.Amount.Should().Be(amount);
        }
    }
}
