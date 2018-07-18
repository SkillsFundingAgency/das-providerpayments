using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAgreementsQuery;
using SFA.DAS.Payments.Reference.Accounts.Processor;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Processor
{
    [TestFixture]
    public class GivenAnImportAgreementsOrchestrator
    {
        [TestFixture]
        public class WhenCallingImportAgreements
        {
            [Test, AccountsAutoData]
            public void ThenItShouldReadAllPages(
                List<GetPageOfAgreementsQueryResponse> getPageOfAgreementsResponses,
                [Frozen] Mock<IMediator> mockMediator,
                ImportAgreementsOrchestrator sut)
            {
                getPageOfAgreementsResponses.ForEach(response =>
                {
                    response.IsValid = true;
                    response.HasMorePages = true;
                });
                getPageOfAgreementsResponses.Last().HasMorePages = false;
                mockMediator
                    .SetupSequence(mediator => mediator.Send(It.IsAny<GetPageOfAgreementsQueryRequest>()))
                    .Returns(getPageOfAgreementsResponses[0])
                    .Returns(getPageOfAgreementsResponses[1])
                    .Returns(getPageOfAgreementsResponses[2]);

                sut.ImportAccounts();

                mockMediator.Verify(mediator => mediator.Send(It.IsAny<GetPageOfAgreementsQueryRequest>()), 
                    Times.Exactly(getPageOfAgreementsResponses.Count));
                mockMediator.Verify(m => m.Send(It.Is<GetPageOfAgreementsQueryRequest>(r => r.PageNumber == 1)), Times.Once);
                mockMediator.Verify(m => m.Send(It.Is<GetPageOfAgreementsQueryRequest>(r => r.PageNumber == 2)), Times.Once);
                mockMediator.Verify(m => m.Send(It.Is<GetPageOfAgreementsQueryRequest>(r => r.PageNumber == 3)), Times.Once);
            }

            [Test, Ignore("todo")]
            public void ThenItShouldBatchSaveEachPage()
            {

            }

            [Test, Ignore("todo")]
            public void ThenItShouldWriteAnAuditRecordAtEndOfProcess()
            {

            }
        }
    }
}
