using System;
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
        public class AndTheReceiverAccountIdDoesNotMatch
        {
            [Test, AutoData, Ignore("Do we need this?")]
            public void ThenAnExceptionIsThrow(
                DasAccount entity,
                Account receiver,
                RequiredTransferPayment requiredPayment)
            {
                var sut = new Account(entity);

                Action action = () => sut.CreateTransfer(receiver, requiredPayment);

                action.Should().Throw<ArgumentException>();
            }
        }
    }
}
