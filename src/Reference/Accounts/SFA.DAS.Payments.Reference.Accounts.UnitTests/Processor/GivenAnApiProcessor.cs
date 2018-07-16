using AutoFixture.NUnit3;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Accounts.Processor;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Processor
{
    [TestFixture]
    public class GivenAnApiProcessor
    {
        [TestFixture]
        public class WhenCallingProcess
        {
            [Test, AccountsAutoData]
            public void ThenItImportsAccounts(
                [Frozen] Mock<IImportAccountsOrchestrator> mockAccountsOrchestrator,
                ApiProcessor sut
                )
            {
                sut.Process();
                mockAccountsOrchestrator.Verify(orchestrator => orchestrator.ImportAccounts(), Times.Once);
            }

            [Test, AccountsAutoData, Ignore("for now")]
            public void ThenItImportsAgreements(
                [Frozen] Mock<IImportAgreementsOrchestrator> mockAgreementsOrchestrator,
                ApiProcessor sut
            )
            {
                sut.Process();
                mockAgreementsOrchestrator.Verify(orchestrator => orchestrator.ImportAgreements(), Times.Once);
            }
        }
    }

    public interface IImportAgreementsOrchestrator
    {
        void ImportAgreements();
    }
}
