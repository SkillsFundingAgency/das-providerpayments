using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Domain;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Services;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.ServiceTests
{
    [TestFixture]
    public class WhenProcessingSendingAccounts
    {
        [Test, TransfersAutoData]
        public void ThenTheTransactionsAreProperlySortedByTransferApprovedDate(
            [Frozen] Mock<IAmAnAccountRepository> accountRepository,
            LevyTransferService sut,
            List<DasAccount> testAccountEntities,
            List<RequiredTransferPayment> testRequiredPayments
        )
        {
            var expectedTransferApprovedDate = testRequiredPayments.Min(x => x.TransferApprovalDate);
            var expectedRequiredPaymentId = testRequiredPayments
                .First(x => x.TransferApprovalDate == expectedTransferApprovedDate).RequiredPaymentId;

            testAccountEntities[0].TransferAllowance = 1;

            var accounts = testAccountEntities.Select(x => new Account(x)).ToList();
            accountRepository.Setup(x => x.Account(testAccountEntities[0].AccountId)).Returns(accounts[0]);
            accountRepository.Setup(x => x.Account(testAccountEntities[1].AccountId)).Returns(accounts[1]);
            accountRepository.Setup(x => x.Account(testAccountEntities[2].AccountId)).Returns(accounts[2]);
            
            testRequiredPayments[0].TransferSendingEmployerAccountId = testAccountEntities[0].AccountId;
            testRequiredPayments[0].AccountId = testAccountEntities[2].AccountId;

            testRequiredPayments[1].TransferSendingEmployerAccountId = testAccountEntities[0].AccountId;
            testRequiredPayments[1].AccountId = testAccountEntities[2].AccountId;

            testRequiredPayments[2].TransferSendingEmployerAccountId = testAccountEntities[0].AccountId;
            testRequiredPayments[2].AccountId = testAccountEntities[2].AccountId;

            var results = sut.ProcessSendingAccount(testAccountEntities[0].AccountId, testRequiredPayments);

            var payments = results.SelectMany(x => x.TransferPayments).ToList();
            payments.Count.Should().Be(1);
            payments.First().RequiredPaymentId.Should().Be(expectedRequiredPaymentId);
        }

        [Test, TransfersAutoData]
        public void ThenTheTransactionsAreProperlySortedByUln(
            [Frozen] Mock<IAmAnAccountRepository> accountRepository,
            LevyTransferService sut,
            List<DasAccount> testAccountEntities,
            List<RequiredTransferPayment> testRequiredPayments
        )
        {
            var expectedUln = testRequiredPayments.Min(x => x.Uln);
            var expectedRequiredPaymentId = testRequiredPayments.First(x => x.Uln == expectedUln).RequiredPaymentId;

            testAccountEntities[0].TransferAllowance = 1;

            var accounts = testAccountEntities.Select(x => new Account(x)).ToList();
            accountRepository.Setup(x => x.Account(testAccountEntities[0].AccountId)).Returns(accounts[0]);
            accountRepository.Setup(x => x.Account(testAccountEntities[1].AccountId)).Returns(accounts[1]);
            accountRepository.Setup(x => x.Account(testAccountEntities[2].AccountId)).Returns(accounts[2]);


            testRequiredPayments[0].TransferSendingEmployerAccountId = testAccountEntities[0].AccountId;
            testRequiredPayments[0].AccountId = testAccountEntities[2].AccountId;

            testRequiredPayments[1].TransferSendingEmployerAccountId = testAccountEntities[0].AccountId;
            testRequiredPayments[1].AccountId = testAccountEntities[2].AccountId;
            testRequiredPayments[1].TransferApprovalDate = testRequiredPayments[0].TransferApprovalDate;

            testRequiredPayments[2].TransferSendingEmployerAccountId = testAccountEntities[0].AccountId;
            testRequiredPayments[2].AccountId = testAccountEntities[2].AccountId;
            testRequiredPayments[2].TransferApprovalDate = testRequiredPayments[0].TransferApprovalDate;

            var results = sut.ProcessSendingAccount(testAccountEntities[0].AccountId, testRequiredPayments);

            var payments = results.SelectMany(x => x.TransferPayments).ToList();
            payments.Count.Should().Be(1);
            payments.First().RequiredPaymentId.Should().Be(expectedRequiredPaymentId);
        }
    }
}
