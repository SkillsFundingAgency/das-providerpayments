using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Learners.GetLearnerFAMsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.Learners.GetLearnerFAMsQuery
{
    [TestFixture]
    public class GivenAGetLearnerFAMsQueryHandlerWhenHandlingAndAnExceptionIsThrown
    {
        private const string LearnRefNumber = "LEARNER-ONE";

        private Mock<ILearnerFAMRepository> _mockILearnerFAMRepository;

        private GetLearnerFAMsQueryRequest _request;
        private GetLearnerFAMsQueryHandler _handler;
        private GetLearnerFAMsQueryResponse _actual;

        private Exception _expectedException;

        [SetUp]
        public void Arrange()
        {
            _request = new GetLearnerFAMsQueryRequest
            {
                LearnRefNumber = LearnRefNumber
            };

            _expectedException = new Exception("test");

            _mockILearnerFAMRepository = new Mock<ILearnerFAMRepository>();
            _mockILearnerFAMRepository.Setup(r => r.GetLearnerFAMRecords(LearnRefNumber)).Throws(_expectedException);

            _handler = new GetLearnerFAMsQueryHandler(_mockILearnerFAMRepository.Object);

            // Act
            _actual = _handler.Handle(_request);
        }

        [Test]
        public void ThenItShouldReturnAResponse()
        {
            Assert.That(_actual, Is.Not.Null);
        }

        [Test]
        public void ThenTheReponseShouldBeInValid()
        {
            Assert.That(_actual.IsValid, Is.False);
        }

        [Test]
        public void ThenTheExceptionShouldBeTheOneThatWasThrown()
        {
            Assert.That(_actual.Exception, Is.EqualTo(_expectedException));
        }
    }
}