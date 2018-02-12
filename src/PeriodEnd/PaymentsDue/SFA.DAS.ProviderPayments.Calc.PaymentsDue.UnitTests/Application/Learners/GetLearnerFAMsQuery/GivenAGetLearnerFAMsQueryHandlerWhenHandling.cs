using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Learners.GetLearnerFAMsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.Learners.GetLearnerFAMsQuery
{
    [TestFixture]
    public class GivenAGetLearnerFAMsQueryHandlerWhenHandling
    {
        private const string LearnRefNumber = "LEARNER-ONE";
        private const int LearnFAMCode1 = 1;
        private const string LearnFAMType1 = "SEM";
        private const int LearnFAMCode2 = 2;
        private const string LearnFAMType2 = "TEM";

        private Mock<ILearnerFAMRepository> _mockILearnerFAMRepository;

        private GetLearnerFAMsQueryRequest _request;
        private GetLearnerFAMsQueryHandler _handler;
        private GetLearnerFAMsQueryResponse _actual;

        [SetUp]
        public void Arrange()
        {
            _request = new GetLearnerFAMsQueryRequest
            {
                LearnRefNumber = LearnRefNumber
            };

            _mockILearnerFAMRepository = new Mock<ILearnerFAMRepository>();
            _mockILearnerFAMRepository.Setup(r => r.GetLearnerFAMRecords(LearnRefNumber)).Returns(new[]
            {
                new LearnerFAMEntity
                {
                    LearnRefNumber = LearnRefNumber,
                    LearnFAMType = LearnFAMType1,
                    LearnFAMCode = LearnFAMCode1
                },
                new LearnerFAMEntity
                {
                    LearnRefNumber = LearnRefNumber,
                    LearnFAMType = LearnFAMType2,
                    LearnFAMCode = LearnFAMCode2
                }
            });

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
        public void ThenTheReponseShouldBeValid()
        {
            Assert.That(_actual.IsValid);
        }

        [Test]
        public void ThenItShouldReturnTheRightNumberOfItems()
        {
            Assert.That(_actual.Items.Length, Is.EqualTo(2));
        }

        [TestCase(0, LearnRefNumber)]
        [TestCase(1, LearnRefNumber)]
        public void ThenItShouldReturnTheRightLearnRefNumber(int position, string learnRefNumber)
        {
            Assert.That(_actual.Items[position].LearnRefNumber, Is.EqualTo(learnRefNumber));
        }

        [TestCase(0, LearnFAMType1)]
        [TestCase(1, LearnFAMType2)]
        public void ThenItShouldReturnTheRightLearnFAMType(int position, string learnFAMType)
        {
            Assert.That(_actual.Items[position].LearnFAMType, Is.EqualTo(learnFAMType));
        }

        [TestCase(0, LearnFAMCode1)]
        [TestCase(1, LearnFAMCode2)]
        public void ThenItShouldReturnTheRightLearnFAMCode(int position, int learnFAMCode)
        {
            Assert.That(_actual.Items[position].LearnFAMCode, Is.EqualTo(learnFAMCode));
        }
    }
}
