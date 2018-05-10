using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.AccountTests
{
    [TestFixture]
    public partial class GivenAnAccount
    {
        [TestFixture]
        public partial class WhenCreatingTransfers
        {
            Account Sut()
            {
                return new Account(new DasAccount {Balance = 1000000, IsLevyPayer = true, TransferAllowance = 1000000});
            }

            [Test, AutoData]
            public void ThenTheSendingAccountIdIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                var result = Sut().CreateTransfer(receiver, requiredPayment);
                var actual = result.AccountLevyTransfer.SendingAccountId;

                actual.Should().Be(requiredPayment.TransferSendingEmployerAccountId);
            }

            [Test, AutoData]
            public void ThenTheReceivingAccountIdIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                var result = Sut().CreateTransfer(receiver, requiredPayment);
                var actual = result.AccountLevyTransfer.ReceivingAccountId;

                actual.Should().Be(requiredPayment.AccountId);
            }

            [Test, AutoData]
            public void ThenTheRequiredPaymentIdIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                var result = Sut().CreateTransfer(receiver, requiredPayment);
                var actual = result.AccountLevyTransfer.RequiredPaymentId;

                actual.Should().Be(requiredPayment.RequiredPaymentId);
            }

            [Test, AutoData]
            public void ThenTheFundingSourceIsTransfer(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                var result = Sut().CreateTransfer(receiver, requiredPayment);
                var actual = result.AccountLevyTransfer.TransferType;

                actual.Should().Be(TransferType.Levy);
            }

            [Test, AutoData]
            public void ThenTheCollectionPeriodNameIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                var result = Sut().CreateTransfer(receiver, requiredPayment);
                var actual = result.AccountLevyTransfer.CollectionPeriodName;

                actual.Should().Be(requiredPayment.CollectionPeriodName);
            }

            [Test, AutoData]
            public void ThenTheCollectionPeriodMonthIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                var result = Sut().CreateTransfer(receiver, requiredPayment);
                var actual = result.AccountLevyTransfer.CollectionPeriodMonth;

                actual.Should().Be(requiredPayment.CollectionPeriodMonth);
            }

            [Test, AutoData]
            public void ThenTheCollectionPeriodYearIsCorrect(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                var result = Sut().CreateTransfer(receiver, requiredPayment);
                var actual = result.AccountLevyTransfer.CollectionPeriodYear;

                actual.Should().Be(requiredPayment.CollectionPeriodYear);
            }
        }
    }
}
