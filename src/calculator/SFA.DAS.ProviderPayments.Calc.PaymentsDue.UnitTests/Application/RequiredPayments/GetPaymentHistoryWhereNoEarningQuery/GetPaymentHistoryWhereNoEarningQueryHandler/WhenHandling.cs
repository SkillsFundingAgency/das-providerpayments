using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery.GetPaymentHistoryWhereNoEarningQueryHandler
{
    public class WhenHandling
    {
        private RequiredPaymentEntity[] _entities;
        private Mock<IRequiredPaymentRepository> _requiredPaymentRepository;
        private PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery.GetPaymentHistoryWhereNoEarningQueryHandler _handler;
        private GetPaymentHistoryWhereNoEarningQueryRequest _request;

        [SetUp]
        public void Arrange()
        {
            _entities = new[]
            {
                new RequiredPaymentEntity
                {
                    AccountId = "1",
                    AccountVersionId = "20170717",
                    AimSeqNumber = 3,
                    AmountDue = 1234.5678m,
                    ApprenticeshipContractType = 1,
                    CommitmentId = 99,
                    CommitmentVersionId = "1-001",
                    DeliveryMonth = 8,
                    DeliveryYear = 2016,
                    StandardCode = 123,
                    FundingLineType = "abc",
                    IlrSubmissionDateTime = new DateTime(2016, 8, 1),
                    LearnRefNumber = "LRN-001",
                    PriceEpisodeIdentifier = "25-123-0/1/8/2016",
                    SfaContributionPercentage = 0.9m,
                    TransactionType = 1,
                    Ukprn = 12345678,
                    Uln = 9876543210,
                    UseLevyBalance = true
                }
            };

            _requiredPaymentRepository = new Mock<IRequiredPaymentRepository>();
            _requiredPaymentRepository.Setup(r => r.GetPreviousPaymentsWithoutEarnings())
                .Returns(_entities);

            _handler = new PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery
                .GetPaymentHistoryWhereNoEarningQueryHandler(_requiredPaymentRepository.Object);

            _request = new GetPaymentHistoryWhereNoEarningQueryRequest();
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
        public void ThenItShouldReturnDataFromRepositoryAsDomainObjects()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(1, actual.Items.Length);
            Assert.AreEqual(_entities[0].AccountId, actual.Items[0].AccountId);
            Assert.AreEqual(_entities[0].AccountVersionId, actual.Items[0].AccountVersionId);
            Assert.AreEqual(_entities[0].AimSeqNumber, actual.Items[0].AimSequenceNumber);
            Assert.AreEqual(_entities[0].AmountDue, actual.Items[0].AmountDue);
            Assert.AreEqual(_entities[0].ApprenticeshipContractType, actual.Items[0].ApprenticeshipContractType);
            Assert.AreEqual(_entities[0].CommitmentId, actual.Items[0].CommitmentId);
            Assert.AreEqual(_entities[0].CommitmentVersionId, actual.Items[0].CommitmentVersionId);
            Assert.AreEqual(_entities[0].DeliveryMonth, actual.Items[0].DeliveryMonth);
            Assert.AreEqual(_entities[0].DeliveryYear, actual.Items[0].DeliveryYear);
            Assert.AreEqual(_entities[0].FrameworkCode, actual.Items[0].FrameworkCode);
            Assert.AreEqual(_entities[0].FundingLineType, actual.Items[0].FundingLineType);
            Assert.AreEqual(_entities[0].IlrSubmissionDateTime, actual.Items[0].IlrSubmissionDateTime);
            Assert.AreEqual(_entities[0].LearnRefNumber, actual.Items[0].LearnerRefNumber);
            Assert.AreEqual(_entities[0].PathwayCode, actual.Items[0].PathwayCode);
            Assert.AreEqual(_entities[0].PriceEpisodeIdentifier, actual.Items[0].PriceEpisodeIdentifier);
            Assert.AreEqual(_entities[0].ProgrammeType, actual.Items[0].ProgrammeType);
            Assert.AreEqual(_entities[0].SfaContributionPercentage, actual.Items[0].SfaContributionPercentage);
            Assert.AreEqual(_entities[0].StandardCode, actual.Items[0].StandardCode);
            Assert.AreEqual((TransactionType)_entities[0].TransactionType, actual.Items[0].TransactionType);
            Assert.AreEqual(_entities[0].Ukprn, actual.Items[0].Ukprn);
            Assert.AreEqual(_entities[0].Uln, actual.Items[0].Uln);
            Assert.AreEqual(_entities[0].UseLevyBalance, actual.Items[0].UseLevyBalance);
        }

        [Test]
        public void ThenItShouldReturnValidResponseEvenIfRepositoryReturnsNull()
        {
            // Arrange
            _requiredPaymentRepository.Setup(r => r.GetPreviousPaymentsWithoutEarnings())
                .Returns((RequiredPaymentEntity[])null);

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(0, actual.Items.Length);
        }

        [Test]
        public void ThenItShouldReturnAnInvalidResponseIfRepositoryThrowsException()
        {
            // Arrange
            var repoException = new Exception("Some db error");
            _requiredPaymentRepository.Setup(r => r.GetPreviousPaymentsWithoutEarnings())
                .Throws(repoException);

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.AreSame(repoException, actual.Exception);
        }

    }
}
