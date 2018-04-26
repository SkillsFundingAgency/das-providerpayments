using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.AccountTests
{
    [TestFixture]
    public partial class WhenCreatingTransfers
    {
        [TestFixture]
        public class AndThereIsExtraAvailableBalance
        {
            [Test, AutoData]
            public void ThenTheTransferIsForTheFullAmount(
                RequiredTransferPayment requiredPayment,
                Account receiver,
                decimal expected)
            {
                requiredPayment.AmountDue = expected;

                var sut = new Account(new DasAccount
                {
                    Balance = expected * 2, TransferBalance = expected * 2, IsLevyPayer = true
                });

                var actual = sut.CreateTransfer(receiver, requiredPayment);
                actual.Amount.Should().Be(expected);
            }
        }

        [TestFixture]
        public class AndTheAvailableBalanceIsTheSameAsTheTransferAmount
        {
            [Test, AutoData]
            public void ThenTheTransferIsForTheFullAmount(
                RequiredTransferPayment requiredPayment,
                Account receiver,
                decimal expected)
            {
                requiredPayment.AmountDue = expected;
                var sut = new Account(new DasAccount
                {
                    Balance = expected,
                    TransferBalance = expected,
                    IsLevyPayer = true
                });

                var actual = sut.CreateTransfer(receiver, requiredPayment);
                actual.Amount.Should().Be(expected);
            }
        }

        [TestFixture]
        public class AndTheAvailableBalanceIsLowerThanTheTransferAmount
        {
            [Test, AutoData]
            public void ThenTheTransferAmountIsTheSameAsTheTransferBalance(
                RequiredTransferPayment requiredPayment,
                Account receiver,
                decimal expected)
            {
                requiredPayment.AmountDue = expected * 2;
                var sut = new Account(new DasAccount
                {
                    Balance = expected,
                    TransferBalance = expected,
                    IsLevyPayer = true
                });

                var actual = sut.CreateTransfer(receiver, requiredPayment);
                actual.Amount.Should().Be(expected);
            }
        }

        [TestFixture]
        public class AndTheAvailableBalanceIsZero
        {
            [Test, AutoData]
            public void ThenTheTransferAmountIsZero(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                requiredPayment.AmountDue = 100;
                var sut = new Account(new DasAccount
                {
                    Balance = 0,
                    TransferBalance = 0,
                    IsLevyPayer = true
                });

                var actual = sut.CreateTransfer(receiver, requiredPayment);
                actual.Amount.Should().Be(0);
            }
        }

        [TestFixture]
        public class AndTheAvailableBalanceIsNegative
        {
            [Test, AutoData]
            public void ThenTheTransferAmountIsZero(
                RequiredTransferPayment requiredPayment,
                Account receiver)
            {
                requiredPayment.AmountDue = 100;
                var sut = new Account(new DasAccount
                {
                    Balance = -100,
                    TransferBalance = 0,
                    IsLevyPayer = true
                });

                var actual = sut.CreateTransfer(receiver, requiredPayment);
                actual.Amount.Should().Be(0);
            }
        }

        [TestFixture]
        public class AndThereAreMultipleTransfers
        {
            [TestFixture]
            public class AndTheCombinedTotalIsLessThanTheBalance
            {
                [Test, AutoData]
                public void ThenTheAmountsAreAllUsed(
                    RequiredTransferPayment requiredPayment1,
                    RequiredTransferPayment requiredPayment2,
                    Account receiver,
                    DasAccount entity
                        )
                {
                    var expectedTotal = requiredPayment1.AmountDue + requiredPayment2.AmountDue;
                    entity.TransferBalance = 1000000;
                    entity.Balance = expectedTotal;

                    var sut = new Account(entity);

                    var result1 = sut.CreateTransfer(receiver, requiredPayment1);
                    var result2 = sut.CreateTransfer(receiver, requiredPayment2);
                    var actualTotal = result1.Amount + result2.Amount;

                    actualTotal.Should().Be(expectedTotal);
                }
            }

            [TestFixture]
            public class AndTheCombinedTotalIsMoreThanTheTransferBalance
            {
                [Test, AutoData]
                public void ThenThereWillBeATransferForLessThanTheRequestedAmount(
                    RequiredTransferPayment requiredPayment1,
                    RequiredTransferPayment requiredPayment2,
                    Account receiver,
                    DasAccount entity
                )
                {
                    var expectedTotal = requiredPayment1.AmountDue + requiredPayment2.AmountDue - 1;
                    entity.TransferBalance = expectedTotal;
                    entity.Balance = 1000000;

                    var sut = new Account(entity);

                    var result1 = sut.CreateTransfer(receiver, requiredPayment1);
                    var result2 = sut.CreateTransfer(receiver, requiredPayment2);
                    var actualTotal = result1.Amount + result2.Amount;

                    actualTotal.Should().Be(expectedTotal);
                }
            }

            [TestFixture]
            public class AndTheCombinedTotalIsMoreThanTheBalance
            {
                [Test, AutoData]
                public void ThenThereWillBeATransferForLessThanTheRequestedAmount(
                    RequiredTransferPayment requiredPayment1,
                    RequiredTransferPayment requiredPayment2,
                    Account receiver,
                    DasAccount entity
                )
                {
                    var expectedTotal = requiredPayment1.AmountDue + requiredPayment2.AmountDue - 1;
                    entity.TransferBalance = 1000000;
                    entity.Balance = expectedTotal;

                    var sut = new Account(entity);

                    var result1 = sut.CreateTransfer(receiver, requiredPayment1);
                    var result2 = sut.CreateTransfer(receiver, requiredPayment2);
                    var actualTotal = result1.Amount + result2.Amount;

                    actualTotal.Should().Be(expectedTotal);
                }
            }
        }
    }
}
