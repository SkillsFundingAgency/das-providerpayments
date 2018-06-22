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
        public class WhenConstructing
        {
            [Test]
            [InlineAutoData(0.0, 0.0, false)]
            [InlineAutoData(100.0, 0.0, false)]
            [InlineAutoData(0.0, 100.0, false)]
            [InlineAutoData(100.0, 100.0, true)]
            public void ThenTheHasTransferBalanceIsCorrect(
                double accountBalance,
                double transferBalance,
                bool expected,
                DasAccount entity)
            {
                entity.Balance = (decimal) accountBalance;
                entity.TransferAllowance = (decimal) transferBalance;
                entity.IsLevyPayer = true;

                var sut = new Account(entity);

                sut.HasTransferBalance.Should().Be(expected);
            }
        }
    }
}
