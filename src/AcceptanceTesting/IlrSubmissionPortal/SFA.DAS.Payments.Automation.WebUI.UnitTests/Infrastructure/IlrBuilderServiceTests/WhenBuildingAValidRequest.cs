using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.CreateSubmissionCommand;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery;
using SFA.DAS.Payments.Automation.Application.Submission.TransformSubmissionCommand;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using SFA.DAS.Payments.Automation.WebUI.Infrastructure;

namespace SFA.DAS.Payments.Automation.WebUI.UnitTests.Infrastructure.IlrBuilderServiceTests
{
    public class WhenBuildingAValidRequest
    {
        private const string Gherkin = "Feature: Test\n" +
                                       "Scenario: Test 1\n" +
                                       "Given something exists\n" +
                                       "When I do something\n" +
                                       "Then something happens";

        private const string FileName = "File1";
        private const string IlrContent = "SomeXml";
        private const string AccountsSql = "AccountCreationSql";
        private const string CommitmentsSql = "CommitmentCreationSql";
        private const string CommitmentsBulkCsv = "CommitmentsBulkCsv";
        private const string UsedUlnsCsv = "UsedUlnsCsv";

        private Specification _spec;
        private IlrBuilderRequest _request;
        private Mock<IMediator> _mediator;
        private IlrBuilderService _service;

        [SetUp]
        public void Arrange()
        {
            _spec = new Specification
            {
                Name = "Test 1",
                Arrangement = new SpecificationArrangement
                {
                    Commitments = new List<Commitment>
                    {
                        new Commitment
                        {
                            LearnerKey="learner a",
                            StartDate = new DateTime(2017, 9, 1),
                            EndDate = new DateTime(2019, 10 ,1)
                        }
                    }
                }
            };

            _request = new IlrBuilderRequest
            {
                Gherkin = Gherkin
            };

            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.Is<ParseGherkinQueryRequest>(r => r.GherkinSpecs == Gherkin)))
                .Returns(new ParseGherkinQueryResponse
                {
                    Results = new[] { _spec }
                });
            _mediator.Setup(m => m.Send(It.IsAny<TransformSubmissionCommandRequest>()))
                .Returns(new TransformSubmissionCommandResponse());
            _mediator.Setup(m => m.Send(It.IsAny<CreateSubmissionCommandRequest>()))
                .Returns(new CreateSubmissionCommandResponse
                {
                    FileName = FileName,
                    IlrContent = IlrContent,
                    AccountsSql = AccountsSql,
                    CommitmentsSql = CommitmentsSql,
                    CommitmentsBulkCsv = CommitmentsBulkCsv,
                    UsedUlnCsv =UsedUlnsCsv
                });
            _mediator.Setup(m => m.Send(It.IsAny<ValidateSpecificationsQueryRequest>()))
                .Returns(new ValidateSpecificationsQueryResponse());

            _service = new IlrBuilderService(_mediator.Object);

        }

        [Test]
        public void ThenItShouldReturnAValidResponse()
        {
            // Act
            var response = _service.BuildIlrWithRefenceData(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess, response.Exception?.Message);
        }

        [Test]
        public void ThenItShouldParseGherkin()
        {
            // Act
            _service.BuildIlrWithRefenceData(_request);

            // Assert
            _mediator.Verify(m => m.Send(It.Is<ParseGherkinQueryRequest>(r => r.GherkinSpecs == Gherkin)), Times.Once());
        }

        [Test]
        public void ThenItShouldNotApplyTransformsIfNoTransformsRequested()
        {
            // Act
            _service.BuildIlrWithRefenceData(_request);

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<TransformSubmissionCommandRequest>()), Times.Never);
        }

        [Test]
        public void ThenItShouldApplyTransformsIfShiftToDatesAreSet()
        {
            // Arrange
            _request.ShiftToMonth = 5;
            _request.ShiftToYear = 2017;

            // Act
            _service.BuildIlrWithRefenceData(_request);

            // Assert
            _mediator.Verify(m => m.Send(It.Is<TransformSubmissionCommandRequest>(r => r.Specification == _spec
                                                                                    && r.ShiftToMonth == 5
                                                                                    && r.ShiftToYear == 2017)), Times.Once);
        }

        [Test]
        public void ThenItShouldCreateSubmission()
        {
            // Act
            _service.BuildIlrWithRefenceData(_request);

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<CreateSubmissionCommandRequest>()), Times.Once());
        }

        [Test]
        public void ThenItShouldReturnAResponseWithCorrectContent()
        {
            // Act
            var response = _service.BuildIlrWithRefenceData(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(FileName, response.FileName);
            Assert.AreEqual(IlrContent, response.IlrContent);
            Assert.AreEqual(AccountsSql, response.AccountsContent);
            Assert.AreEqual(CommitmentsSql, response.CommitmentsContent);
            Assert.AreEqual(CommitmentsBulkCsv, response.CommitmentsBulkCsv);
            Assert.AreEqual(UsedUlnsCsv, response.UsedUlnCSV);
        }
    }
}
