using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Services;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.ServiceTests
{
    [TestFixture]
    public class WhenProcessingTransfers
    {
        [TestFixture]
        public class AndThereIsSufficientTransferBalanceForAllPayments
        {
            [TestFixture]
            public class AndThereIsSufficientBalanceForAllPayments
            {
                [Test, TransfersAutoData]
                public void ThenThreePaymentSetsAreCreated(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                {
                    var entity = new DasAccount {Balance = 1000000, TransferAllowance = 1000000, IsLevyPayer = true};
                    var sender = new Account(entity);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Count;

                    actual.Should().Be(requiredPayments.Count);
                }

                [Test, TransfersAutoData]
                public void ThenThePaymentAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var entity = new DasAccount { Balance = 1000000, TransferAllowance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    var expected = requiredPayments.Sum(x => x.AmountDue);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.TransferPayments.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }

                [Test, TransfersAutoData]
                public void ThenTheTransferAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var entity = new DasAccount { Balance = 1000000, TransferAllowance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    var expected = requiredPayments.Sum(x => x.AmountDue);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }
            }

            [TestFixture]
            public class AndThereIsNotSufficientBalanceForAllPayments
            {
                [Test, TransfersAutoData]
                public void ThenThePaymentAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = expected, TransferAllowance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    
                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.TransferPayments.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }

                [Test, TransfersAutoData]
                public void ThenTheTransferAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = expected, TransferAllowance = 1000000, IsLevyPayer = true };
                    var sender = new Account(entity);
                    
                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class AndTheTransferBalanceDoesNotCoverAllPayments
        {
            [TestFixture]
            public class AndThereIsSufficientBalanceForAllPayments
            {
                [Test, TransfersAutoData]
                public void ThenThePaymentAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = 1000000, TransferAllowance = expected, IsLevyPayer = true };
                    var sender = new Account(entity);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.TransferPayments.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }

                [Test, TransfersAutoData]
                public void ThenTheTransferAmountsAreCorrect(
                    LevyTransferService sut,
                    List<RequiredTransferPayment> requiredPayments,
                    Account receiver
                )
                {
                    var expected = requiredPayments.Sum(x => x.AmountDue) / 2;

                    var entity = new DasAccount { Balance = 1000000, TransferAllowance = expected, IsLevyPayer = true };
                    var sender = new Account(entity);

                    var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                    var actual = result.AccountTransfers.Sum(x => x.Amount);

                    actual.Should().Be(expected);
                }
            }

            [TestFixture]
            public class AndThereIsNotSufficientBalanceForAllPayments
            {
                [TestFixture]
                public class WithBalanceHigherThanTheTransferBalance
                {
                    [Test, TransfersAutoData]
                    public void ThenLessThanThreePaymentSetsAreCreated(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var amount = requiredPayments.Sum(x => x.AmountDue);

                        var entity = new DasAccount { Balance = amount / 2, TransferAllowance = amount / 3, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.AccountTransfers.Count;

                        actual.Should().BeLessThan(requiredPayments.Count);
                    }

                    [Test, TransfersAutoData]
                    public void ThenThePaymentAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected * 2, TransferAllowance = expected, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.TransferPayments.Sum(x => x.Amount);

                        actual.Should().Be(expected);
                    }

                    [Test, TransfersAutoData]
                    public void ThenTheTransferAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected * 2, TransferAllowance = expected, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.AccountTransfers.Sum(x => x.Amount);

                        actual.Should().Be(expected);
                    }
                }

                [TestFixture]
                public class WithBalanceLowerThanTheTransferBalance
                {
                    [Test, TransfersAutoData]
                    public void ThenLessThanThreePaymentSetsAreCreated(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        requiredPayments.ForEach(x=>x.AmountDue = 100);
                        var amount = requiredPayments.Sum(x => x.AmountDue);

                        var entity = new DasAccount { Balance = amount / 3, TransferAllowance = amount / 2, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.AccountTransfers.Count;

                        actual.Should().BeLessThan(requiredPayments.Count);
                    }

                    [Test, TransfersAutoData]
                    public void ThenThePaymentAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected, TransferAllowance = expected * 2, IsLevyPayer = true };
                        var sender = new Account(entity);

                        var result = sut.ProcessTransfers(sender, receiver, requiredPayments);
                        var actual = result.TransferPayments.Sum(x => x.Amount);

                        actual.Should().Be(expected);
                    }

                    [Test, TransfersAutoData]
                    public void ThenTheTransferAmountsAreCorrect(
                        LevyTransferService sut,
                        List<RequiredTransferPayment> requiredPayments,
                        Account receiver
                    )
                    {
                        var expected = requiredPayments.Sum(x => x.AmountDue) / 4;

                        var entity = new DasAccount { Balance = expected, TransferAllowance = expected * 2, IsLevyPayer = true };
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
