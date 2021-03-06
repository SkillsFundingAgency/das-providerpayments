﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery;
using SFA.DAS.Payments.Reference.Accounts.Processor;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Processor
{
    [TestFixture]
    public class GivenAnImportAccountLegalEntitiesOrchestrator
    {
        [TestFixture]
        public class WhenCallingImportAgreements
        {
            [Test, AccountsAutoData]
            public void ThenItShouldRetrieveAllPages(
                List<GetPageOfAccountLegalEntitiesQueryResponse> getPageOfAgreementsResponses,
                [Frozen] Mock<IMediator> mockMediator,
                ImportAccountLegalEntitiesOrchestrator sut)
            {
                getPageOfAgreementsResponses.ForEach(response =>
                {
                    response.IsValid = true;
                    response.HasMorePages = true;
                });
                getPageOfAgreementsResponses.Last().HasMorePages = false;
                mockMediator
                    .SetupSequence(mediator => mediator.Send(It.IsAny<GetPageOfAccountLegalEntitiesQueryRequest>()))
                    .Returns(getPageOfAgreementsResponses[0])
                    .Returns(getPageOfAgreementsResponses[1])
                    .Returns(getPageOfAgreementsResponses[2]);

                sut.ImportAccountLegalEntities();

                mockMediator.Verify(mediator => mediator.Send(It.IsAny<GetPageOfAccountLegalEntitiesQueryRequest>()), 
                    Times.Exactly(getPageOfAgreementsResponses.Count));
                mockMediator.Verify(m => m.Send(It.Is<GetPageOfAccountLegalEntitiesQueryRequest>(r => r.PageNumber == 1)), Times.Once);
                mockMediator.Verify(m => m.Send(It.Is<GetPageOfAccountLegalEntitiesQueryRequest>(r => r.PageNumber == 2)), Times.Once);
                mockMediator.Verify(m => m.Send(It.Is<GetPageOfAccountLegalEntitiesQueryRequest>(r => r.PageNumber == 3)), Times.Once);
            }

            [Test, AccountsAutoData]
            public void AndExceptionReadingPages_ThenThrowsException(
                GetPageOfAccountLegalEntitiesQueryResponse errorResponse,
                string errorMessage,
                [Frozen] Mock<IMediator> mockMediator,
                ImportAccountLegalEntitiesOrchestrator sut)
            {
                errorResponse.IsValid = false;
                errorResponse.Exception = new InvalidOperationException(errorMessage);
                errorResponse.HasMorePages = false; // needed to stop infinite loop lol
                mockMediator
                    .Setup(mediator => mediator.Send(It.IsAny<GetPageOfAccountLegalEntitiesQueryRequest>()))
                    .Returns(errorResponse);

                Action act = sut.ImportAccountLegalEntities;

                act.Should()
                    .Throw<InvalidOperationException>()
                    .WithMessage(errorMessage);
            }

            [Test, AccountsAutoData]
            public void ThenItShouldBatchSaveEachPage(
                List<GetPageOfAccountLegalEntitiesQueryResponse> getPageOfAgreementsResponses,
                [Frozen] Mock<IMediator> mockMediator,
                ImportAccountLegalEntitiesOrchestrator sut)
            {
                getPageOfAgreementsResponses.ForEach(response =>
                {
                    response.IsValid = true;
                    response.HasMorePages = true;
                });
                getPageOfAgreementsResponses.Last().HasMorePages = false;
                mockMediator
                    .SetupSequence(mediator => mediator.Send(It.IsAny<GetPageOfAccountLegalEntitiesQueryRequest>()))
                    .Returns(getPageOfAgreementsResponses[0])
                    .Returns(getPageOfAgreementsResponses[1])
                    .Returns(getPageOfAgreementsResponses[2]);

                sut.ImportAccountLegalEntities();

                mockMediator.Verify(mediator => mediator.Send(It.IsAny<AddManyAccountLegalEntitiesCommandRequest>()), 
                    Times.Exactly(getPageOfAgreementsResponses.Count));
            }

            [Test, AccountsAutoData]
            public void ThenItShouldWriteAnAuditRecord(
                List<GetPageOfAccountLegalEntitiesQueryResponse> getPageOfAgreementsResponses,
                [Frozen] Mock<IMediator> mockMediator,
                ImportAccountLegalEntitiesOrchestrator sut)
            {
                var numberOfRecords = 0;
                getPageOfAgreementsResponses.ForEach(response =>
                {
                    response.IsValid = true;
                    response.HasMorePages = true;
                    numberOfRecords += response.Items.Length;
                });
                getPageOfAgreementsResponses.Last().HasMorePages = false;
                mockMediator
                    .SetupSequence(mediator => mediator.Send(It.IsAny<GetPageOfAccountLegalEntitiesQueryRequest>()))
                    .Returns(getPageOfAgreementsResponses[0])
                    .Returns(getPageOfAgreementsResponses[1])
                    .Returns(getPageOfAgreementsResponses[2]);

                sut.ImportAccountLegalEntities();

                mockMediator.Verify(m => m.Send(It.Is<AddAuditCommandRequest>(r =>
                    r.AccountsRead == numberOfRecords &&
                    r.CorrelationDate >= DateTime.Today &&
                    r.AuditType == AuditType.AccountLegalEntity)), Times.Once());
            }
        }
    }
}
