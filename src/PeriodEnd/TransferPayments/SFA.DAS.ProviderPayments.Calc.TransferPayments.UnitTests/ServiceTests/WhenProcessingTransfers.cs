using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Data;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Services;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.ServiceTests
{
    [TestFixture]
    public class WhenProcessingTransfers
    {
        [Test, Ignore("To keep structure in the results")]
        public void Null() { }

        public class AndThereIsSufficientTransferBalanceForAllPayments
        {
            [Test, Ignore("To keep structure in the results")]
            public void Null() { }

            public class AndThereIsSufficientBalanceForAllPayments
            {
                [Test, AutoData]
                public void ThenThreePaymentSetsAreCreated(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                {
                    var entity = new DasAccount {Balance = 1000000, TransferBalance = 1000000, IsLevyPayer = true};
                    var sender = new Account(entity);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Count;

                    actual.Should().Be(requiredPayments.Count);
                }

                [Test, AutoData]
                public void ThenThePaymentAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var entity = new DasAccount { Balance = 1000000, TransferBalance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    var expected = requiredPayments.Sum(x => x.AmountDue);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.TransferPayments.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }

                [Test, AutoData]
                public void ThenTheTransferAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var entity = new DasAccount { Balance = 1000000, TransferBalance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    var expected = requiredPayments.Sum(x => x.AmountDue);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }
            }

            public class AndThereIsNotSufficientBalanceForAllPayments
            {
                [Test, AutoData]
                public void ThenThePaymentAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = expected, TransferBalance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    
                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.TransferPayments.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }

                [Test, AutoData]
                public void ThenTheTransferAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = expected, TransferBalance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    
                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }
            }
        }

        public class AndTheTransferBalanceDoesNotCoverAllPayments
        {
            [Test, Ignore("To keep structure in the results")]
            public void Null() { }

            public class AndThereIsSufficientBalanceForAllPayments
            {
                [Test, AutoData]
                public void ThenThePaymentAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = 1000000, TransferBalance = expected, IsLevyPayer = true };
                    var sender = new Account(entity);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.TransferPayments.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }

                [Test, AutoData]
                public void ThenTheTransferAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = 1000000, TransferBalance = expected, IsLevyPayer = true };
                    var sender = new Account(entity);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }
            }

            public class AndThereIsNotSufficientBalanceForAllPayments
            {
                [Test, Ignore("To keep structure in the results")]
                public void Null() { }

                public class WithBalanceHigherThanTheTransferBalance
                {
                    [Test, AutoData]
                    public void ThenLessThanThreePaymentSetsAreCreated(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var amount = requiredPayments.Sum(x => x.AmountDue);

                        var entity = new DasAccount { Balance = amount / 2, TransferBalance = amount / 3, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.AccountTransfers.Count;

                        actual.Should().BeLessThan(requiredPayments.Count);
                    }

                    [Test, AutoData]
                    public void ThenThePaymentAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected * 2, TransferBalance = expected, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.TransferPayments.Sum(x => x.Amount);

                        actual.Should().Be(expected);
                    }

                    [Test, AutoData]
                    public void ThenTheTransferAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected * 2, TransferBalance = expected, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.AccountTransfers.Sum(x => x.Amount);

                        actual.Should().Be(expected);
                    }
                }

                public class WithBalanceLowerThanTheTransferBalance
                {
                    [Test, AutoData]
                    public void ThenLessThanThreePaymentSetsAreCreated(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var amount = requiredPayments.Sum(x => x.AmountDue);

                        var entity = new DasAccount { Balance = amount / 3, TransferBalance = amount / 2, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.AccountTransfers.Count;

                        actual.Should().BeLessThan(requiredPayments.Count);
                    }

                    [Test, AutoData]
                    public void ThenThePaymentAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected, TransferBalance = expected * 2, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.TransferPayments.Sum(x => x.Amount);

                        actual.Should().Be(expected);
                    }

                    [Test, AutoData]
                    public void ThenTheTransferAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected, TransferBalance = expected * 2, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.AccountTransfers.Sum(x => x.Amount);

                        actual.Should().Be(expected);
                    }
                }
            }
        }
    }
}
