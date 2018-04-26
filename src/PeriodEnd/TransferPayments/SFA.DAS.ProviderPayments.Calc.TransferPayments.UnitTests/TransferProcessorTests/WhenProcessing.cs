using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.DatabaseEntities;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.UnitTests.TransferProcessorTests
{
    [TestFixture]
    public class WhenProcessing
    {
        [TestFixture]
        public class AndThereIsOneSendingAccount
        {
            [Test, TransfersAutoData]
            public void ThenProcessSendingAccountIsCalledOnce(
                [Frozen] Mock<IAmATransferRepository> transferRepository,
                [Frozen] Mock<IProcessLevyTransfers> levyTransferProcessor,
                TransfersProcessor sut,
                List<RequiredTransferPayment> testPayments
            )
            {
                testPayments[0].TransferSendingEmployerAccountId = 1;
                testPayments[1].TransferSendingEmployerAccountId = 1;
                testPayments[2].TransferSendingEmployerAccountId = 1;

                transferRepository.Setup(x => x.RequiredTransferPayments()).Returns(testPayments);

                sut.Process();

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), It.IsAny<IEnumerable<RequiredTransferPayment>>()),
                    Times.Once);
            }

            [Test, TransfersAutoData]
            public void ThenTheCorrectPaymentsArePassed(
                [Frozen] Mock<IAmATransferRepository> transferRepository,
                [Frozen] Mock<IProcessLevyTransfers> levyTransferProcessor,
                TransfersProcessor sut,
                List<RequiredTransferPayment> testPayments
            )
            {
                testPayments[0].TransferSendingEmployerAccountId = 1;
                testPayments[1].TransferSendingEmployerAccountId = 1;
                testPayments[2].TransferSendingEmployerAccountId = 1;

                transferRepository.Setup(x => x.RequiredTransferPayments()).Returns(testPayments);

                sut.Process();

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), testPayments),
                    Times.Once);
            }
        }

        [TestFixture]
        public class AndThereAreTwoSendingAccounts
        {
            [Test, TransfersAutoData]
            public void ThenProcessSendingAccountIsCalledTwice(
                [Frozen] Mock<IAmATransferRepository> transferRepository,
                [Frozen] Mock<IProcessLevyTransfers> levyTransferProcessor,
                TransfersProcessor sut,
                List<RequiredTransferPayment> testPayments
            )
            {
                testPayments[0].TransferSendingEmployerAccountId = 1;
                testPayments[1].TransferSendingEmployerAccountId = 2;
                testPayments[2].TransferSendingEmployerAccountId = 2;

                transferRepository.Setup(x => x.RequiredTransferPayments()).Returns(testPayments);

                sut.Process();

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), It.IsAny<IEnumerable<RequiredTransferPayment>>()),
                    Times.Exactly(2));
            }

            [Test, TransfersAutoData]
            public void ThenTheCorrectPaymentsArePassed(
                [Frozen] Mock<IAmATransferRepository> transferRepository,
                [Frozen] Mock<IProcessLevyTransfers> levyTransferProcessor,
                TransfersProcessor sut,
                List<RequiredTransferPayment> testPayments
            )
            {
                testPayments[0].TransferSendingEmployerAccountId = 1;
                testPayments[1].TransferSendingEmployerAccountId = 1;
                testPayments[2].TransferSendingEmployerAccountId = 2;

                transferRepository.Setup(x => x.RequiredTransferPayments()).Returns(testPayments);

                sut.Process();

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), new List<RequiredTransferPayment> { testPayments[0], testPayments[1] }),
                    Times.Once);

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), new List<RequiredTransferPayment> { testPayments[2] }),
                    Times.Once);
            }
        }

        [TestFixture]
        public class AndThereAreThreeSendingAccounts
        {
            [Test, TransfersAutoData]
            public void ThenProcessSendingAccountIsCalledThreeTimes(
                [Frozen] Mock<IAmATransferRepository> transferRepository,
                [Frozen] Mock<IProcessLevyTransfers> levyTransferProcessor,
                TransfersProcessor sut,
                List<RequiredTransferPayment> testPayments
            )
            {
                testPayments[0].TransferSendingEmployerAccountId = 1;
                testPayments[1].TransferSendingEmployerAccountId = 2;
                testPayments[2].TransferSendingEmployerAccountId = 3;

                transferRepository.Setup(x => x.RequiredTransferPayments()).Returns(testPayments);

                sut.Process();

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), It.IsAny<IEnumerable<RequiredTransferPayment>>()),
                    Times.Exactly(3));
            }

            [Test, TransfersAutoData]
            public void ThenTheCorrectPaymentsArePassed(
                [Frozen] Mock<IAmATransferRepository> transferRepository,
                [Frozen] Mock<IProcessLevyTransfers> levyTransferProcessor,
                TransfersProcessor sut,
                List<RequiredTransferPayment> testPayments
            )
            {
                testPayments[0].TransferSendingEmployerAccountId = 1;
                testPayments[1].TransferSendingEmployerAccountId = 2;
                testPayments[2].TransferSendingEmployerAccountId = 3;

                transferRepository.Setup(x => x.RequiredTransferPayments()).Returns(testPayments);

                sut.Process();

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), new List<RequiredTransferPayment> { testPayments[0] }),
                    Times.Once);

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), new List<RequiredTransferPayment> { testPayments[1] }),
                    Times.Once);

                levyTransferProcessor.Verify(x =>
                        x.ProcessSendingAccount(It.IsAny<long>(), new List<RequiredTransferPayment> { testPayments[2] }),
                    Times.Once);
            }
        }
    }
}
