using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.RequiredPayments.GetPaymentHistoryQuery.GetPaymentHistoryQueryHandler
{
    public class WhenHandling
    {
       
        private const long Ukprn = 123456;
        private const string LearnRefNumber = "LEARNER-1";
        private const int AimSequenceNumber = 1;
        private const int DeliveryMonth = 8;
        private const int DeliveryYear = 2017;
        private const decimal AmountDue = 123.45m;
        private const long StandardCode = 98888;
        private const long Uln = 999999;

        private GetPaymentHistoryQueryRequest _request;
        private Mock<IRequiredPaymentRepository> _requiredPaymentRepository;
        private PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery.GetPaymentHistoryQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetPaymentHistoryQueryRequest
            {
                Ukprn = Ukprn,
                LearnRefNumber = LearnRefNumber
            };

            _requiredPaymentRepository = new Mock<IRequiredPaymentRepository>();
            _requiredPaymentRepository.Setup(r => r.GetPreviousPayments(Ukprn, LearnRefNumber))
                .Returns(new[]
                {
                    new HistoricalRequiredPaymentEntity
                    {
                        Ukprn = Ukprn,
                        LearnRefNumber = LearnRefNumber,
                        AimSeqNumber = AimSequenceNumber,
                        DeliveryMonth = DeliveryMonth,
                        DeliveryYear = DeliveryYear,
                        AmountDue = AmountDue,
                        StandardCode = StandardCode,
                        Uln = Uln
                    }
                });

            _handler = new PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery.GetPaymentHistoryQueryHandler(_requiredPaymentRepository.Object);
        }

        [Test]
        public void ThenItShouldReturnAValidResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
        }

        [Test]
        public void ThenItShouldReturnItemsFromRepositoryWhenTheyAreAvailable()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(1, actual.Items.Length);
            Assert.AreEqual(Ukprn, actual.Items[0].Ukprn);
            Assert.AreEqual(LearnRefNumber, actual.Items[0].LearnerRefNumber);
            Assert.AreEqual(AimSequenceNumber, actual.Items[0].AimSequenceNumber);
            Assert.AreEqual(DeliveryMonth, actual.Items[0].DeliveryMonth);
            Assert.AreEqual(DeliveryYear, actual.Items[0].DeliveryYear);
            Assert.AreEqual(AmountDue, actual.Items[0].AmountDue);

            Assert.AreEqual(Uln, actual.Items[0].Uln);
            Assert.AreEqual(StandardCode, actual.Items[0].StandardCode);

            Assert.IsNull(actual.Items[0].FrameworkCode);
            Assert.IsNull(actual.Items[0].PathwayCode);
            Assert.IsNull(actual.Items[0].ProgrammeType);

        }

        [Test]
        public void ThenItShouldReturnAnEmptyArrayOfItemsWhenNoneInTheRepository()
        {
            // Arrange
            _requiredPaymentRepository.Setup(r => r.GetPreviousPayments(Ukprn, LearnRefNumber))
                .Returns<RequiredPaymentEntity[]>(null);

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(0, actual.Items.Length);
        }

        [Test]
        public void ThenItShouldReturnAnInvalidResponseWhenRepositoryErrors()
        {
            // Arrange
            _requiredPaymentRepository.Setup(r => r.GetPreviousPayments(Ukprn, LearnRefNumber))
                .Throws<Exception>();

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.Exception);
        }
    }
}
