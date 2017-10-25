using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ParseGherkinQuery.ParseGherkinQueryRequestHandlerTests
{
    public class WhenHandlingInvalidGherkin
    {
        private ParseGherkinQueryRequest _request;
        private ParseGherkinQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ParseGherkinQueryRequest
            {
                GherkinSpecs = "This is just some text"
            };

            _handler = new ParseGherkinQueryRequestHandler();
        }

        [Test]
        public void ThenItShouldReturnUnsuccessfulResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccess);
        }

        [Test]
        public void ThenItShouldReturnParserException()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Error);
            Assert.IsInstanceOf<ParserException>(response.Error);
        }
    }
}
