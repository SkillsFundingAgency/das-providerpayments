using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.AccountTests
{
    [TestFixture]
    public partial class GivenAnAccount
    {
        [TestFixture]
        public class WhenCreatingTransferPayments
        {
            [Test, AutoData]
            public void ThenTheRequiredPaymentIdIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.RequiredPaymentId.Should().Be(requiredPayment.RequiredPaymentId);
            }

            [Test, AutoData]
            public void ThenTheDeliveryMonthIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.DeliveryMonth.Should().Be(requiredPayment.DeliveryMonth);
            }

            [Test, AutoData]
            public void ThenTheDeliveryYearIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.DeliveryYear.Should().Be(requiredPayment.DeliveryYear);
            }

            [Test, AutoData]
            public void ThenTheCollectionPeriodNameIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.CollectionPeriodName.Should().Be(requiredPayment.CollectionPeriodName);
            }

            [Test, AutoData]
            public void ThenTheCollectionPeriodMonthIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.CollectionPeriodMonth.Should().Be(requiredPayment.CollectionPeriodMonth);
            }

            [Test, AutoData]
            public void ThenTheCollectionPeriodYearIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.CollectionPeriodYear.Should().Be(requiredPayment.CollectionPeriodYear);
            }

            [Test, AutoData]
            public void ThenTheFundingSourceIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.FundingSource.Should().Be(FundingSource.Transfer);
            }

            [Test, AutoData]
            public void ThenTheTransactionTypeIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account sut,
                Account receiver)
            {
                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.TransactionType.Should().Be(requiredPayment.TransactionType);
            }

            [Test, AutoData]
            public void ThenTheAmountIsCorrect(
                RequiredTransferPayment requiredPayment,
                DasAccount sender,
                Account receiver)
            {
                sender.TransferAllowance = 1000000;
                sender.Balance = 1000000;
                var sut = new Account(sender);

                var actual = sut.CreateTransfer(receiver, requiredPayment).TransferLevyPayment;

                actual.Amount.Should().Be(requiredPayment.AmountDue);
            }
        }
    }
}
