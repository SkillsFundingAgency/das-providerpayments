using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.AccountTests
{
    [TestFixture]
    class WhenConstructing
    {
        [Test]
        [InlineAutoData(0.0, 0.0, false)]
        [InlineAutoData(100.0, 0.0, false)]
        [InlineAutoData(0.0, 100.0, false)]
        [InlineAutoData(100.0, 100.0, true)]
        public void ThenTheHasTransferBalanceIsCorrect(
            double accountBalance, 
            double transferBalance, 
            bool exptected, DasAccount entity)
        {
            entity.Balance = (decimal)accountBalance;
            entity.TransferBalance = (decimal)transferBalance;
            entity.IsLevyPayer = true;

            var sut = new Account(entity);

            sut.HasTransferBalance.Should().Be(exptected);
        }
    }
}
