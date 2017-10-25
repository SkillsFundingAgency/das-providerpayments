using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.CreateSubmissionCommand;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery;
using SFA.DAS.Payments.Automation.Application.Submission.TransformSubmissionCommand;
using SFA.DAS.Payments.Automation.WebUI.Infrastructure;

namespace SFA.DAS.Payments.Automation.WebUI.UnitTests.Infrastructure.IlrBuilderServiceTests
{
    public class WhenBuildingAnInvalidRequest
    {
        private Mock<IMediator> _mediator;
        private IlrBuilderService _service;

        [SetUp]
        public void Arrange()
        {

            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<ParseGherkinQueryRequest>()))
                .Returns(new ParseGherkinQueryResponse
                {
                    Results = new[]
                    {
                        new Domain.Specifications.Specification()
                    }
                });
            _mediator.Setup(m => m.Send(It.IsAny<TransformSubmissionCommandRequest>()))
                .Returns(new TransformSubmissionCommandResponse());
            _mediator.Setup(m => m.Send(It.IsAny<CreateSubmissionCommandRequest>()))
                .Returns(new CreateSubmissionCommandResponse());
            _mediator.Setup(m => m.Send(It.IsAny<ValidateSpecificationsQueryRequest>()))
                .Returns(new ValidateSpecificationsQueryResponse());

            _service = new IlrBuilderService(_mediator.Object);
        }

        [Test]
        public void ThenItShouldReturnAnUnsuccessfulResponseIfItCannotParseGherkin()
        {
            // Arrange
            var exception = new Exception("Unit tests");
            _mediator.Setup(m => m.Send(It.IsAny<ParseGherkinQueryRequest>()))
                .Returns(new ParseGherkinQueryResponse { Error = exception });

            // Act
            var actual = _service.BuildIlrWithRefenceData(new IlrBuilderRequest());

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual(exception.Message, actual.Exception.Message);
            Assert.AreSame(exception, actual.Exception.InnerException);
        }

        [Test]
        public void ThenItShouldReturnAnUnsuccessfulResponseIfItCannotTransformSpecifications()
        {
            // Arrange
            var exception = new Exception("Unit tests");
            _mediator.Setup(m => m.Send(It.IsAny<TransformSubmissionCommandRequest>()))
                .Returns(new TransformSubmissionCommandResponse { Error = exception });

            // Act
            var actual = _service.BuildIlrWithRefenceData(new IlrBuilderRequest { ShiftToMonth = 5 });

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual("Error transforming specifications - Unit tests", actual.Exception.Message);
            Assert.AreSame(exception, actual.Exception.InnerException);
        }

        [Test]
        public void ThenItShouldReturnAnUnsuccessfulResponseIfItCannotCreateSubmissionContent()
        {
            // Arrange
            var exception = new Exception("Unit tests");
            _mediator.Setup(m => m.Send(It.IsAny<CreateSubmissionCommandRequest>()))
                .Returns(new CreateSubmissionCommandResponse { Error = exception });

            // Act
            var actual = _service.BuildIlrWithRefenceData(new IlrBuilderRequest());

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual("Error creating content response - Unit tests", actual.Exception.Message);
            Assert.AreSame(exception, actual.Exception.InnerException);
        }

        [Test]
        public void ThenItShouldReturnAnUnsuccessfulResponseIfSpecificationsAreNotValidatedSuccessfully()
        {
            // Arrange
            var exception1 = new ValidationViolation { Description = "Unit tests" };
            var exception2 = new ValidationViolation { Description = "More Unit tests" };
            _mediator.Setup(m => m.Send(It.IsAny<ValidateSpecificationsQueryRequest>()))
                .Returns(new ValidateSpecificationsQueryResponse { Errors = new[] { exception1, exception2 } });

            // Act
            var actual = _service.BuildIlrWithRefenceData(new IlrBuilderRequest());

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsSuccess);
            Assert.IsInstanceOf<InvalidSpecificationsException>(actual.Exception);
            Assert.AreEqual("Specifications are not valid.\nUnit tests\nMore Unit tests", actual.Exception.Message);
        }
    }
}
