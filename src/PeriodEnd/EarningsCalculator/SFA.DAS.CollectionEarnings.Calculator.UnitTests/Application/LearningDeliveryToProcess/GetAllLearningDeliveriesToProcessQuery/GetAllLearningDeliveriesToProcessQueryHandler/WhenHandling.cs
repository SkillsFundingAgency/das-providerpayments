using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler
{
    public class WhenHandling
    {
        private Mock<ILearningDeliveryToProcessRepository> _learningDeliveryRepository;
        private Mock<IFinancialRecordRepository> _financialRecordRepoository; 

        private GetAllLearningDeliveriesToProcessQueryRequest _request;
        private Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _learningDeliveryRepository = new Mock<ILearningDeliveryToProcessRepository>();
            _financialRecordRepoository = new Mock<IFinancialRecordRepository>();

            _request = new GetAllLearningDeliveriesToProcessQueryRequest();

            _handler = new Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler(_learningDeliveryRepository.Object, _financialRecordRepoository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _learningDeliveryRepository
                .Setup(r => r.GetAllLearningDeliveriesToProcess())
                .Returns(new[] { new Infrastructure.Data.Entities.LearningDeliveryToProcess() });

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
            Assert.AreEqual(1, response.Items.Length);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _learningDeliveryRepository
                .Setup(r => r.GetAllLearningDeliveriesToProcess())
                .Throws(new Exception("Error while reading learning deliveries to process."));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
            Assert.IsNull(response.Items);
        }

        [Test]
        public void ThenThePriceEpisodesAreCalculatedCorrectly()
        {
            // Arrange
            _learningDeliveryRepository
                .Setup(r => r.GetAllLearningDeliveriesToProcess())
                .Returns(new[]
                {
                    new Infrastructure.Data.Entities.LearningDeliveryToProcess
                    {
                        Ukprn = 10007459,
                        LearnRefNumber = "Lrn001",
                        Uln = 123456789,
                        NiNumber = "AB123456C",
                        AimSeqNumber = 1,
                        StandardCode = 27,
                        ProgrammeType = 25,
                        LearnStartDate = new DateTime(2017, 8, 1),
                        LearnPlanEndDate = new DateTime(2018, 8, 15),
                        CompletionStatus = 1
                    }
                });

            _financialRecordRepoository
                .Setup(r => r.GetLearningDeliveryFinancialRecords("Lrn001", 1))
                .Returns(new[]
                {
                    new FinancialRecordEntity
                    {
                        LearnRefNumber = "Lrn001",
                        AimSeqNumber = 1,
                        FinType = "TNP",
                        FinCode = 1,
                        FinDate = new DateTime(2017, 8, 1),
                        FinAmount = 12000
                    },
                    new FinancialRecordEntity
                    {
                        LearnRefNumber = "Lrn001",
                        AimSeqNumber = 1,
                        FinType = "TNP",
                        FinCode = 2,
                        FinDate = new DateTime(2017, 8, 1),
                        FinAmount = 3000
                    },
                    new FinancialRecordEntity
                    {
                        LearnRefNumber = "Lrn001",
                        AimSeqNumber = 1,
                        FinType = "TNP",
                        FinCode = 1,
                        FinDate = new DateTime(2017, 10, 15),
                        FinAmount = 6000
                    },
                    new FinancialRecordEntity
                    {
                        LearnRefNumber = "Lrn001",
                        AimSeqNumber = 1,
                        FinType = "TNP",
                        FinCode = 2,
                        FinDate = new DateTime(2017, 10, 15),
                        FinAmount = 1500
                    },
                    new FinancialRecordEntity
                    {
                        LearnRefNumber = "Lrn001",
                        AimSeqNumber = 1,
                        FinType = "TNP",
                        FinCode = 3,
                        FinDate = new DateTime(2017, 11, 1),
                        FinAmount = 5000
                    },
                    new FinancialRecordEntity
                    {
                        LearnRefNumber = "Lrn001",
                        AimSeqNumber = 1,
                        FinType = "TNP",
                        FinCode = 4,
                        FinDate = new DateTime(2017, 11, 1),
                        FinAmount = 625
                    }
                });

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(1, response.Items.Length);

            var learningDelivery = response.Items[0];

            Assert.AreEqual(3, learningDelivery.PriceEpisodes.Length);
            Assert.AreEqual(1, learningDelivery.PriceEpisodes.Count(pe => pe.StartDate == new DateTime(2017, 8, 1) && pe.EndDate.HasValue && pe.EndDate.Value == new DateTime(2017, 10, 14) && pe.Type == PriceEpisodeType.Initial && pe.NegotiatedPrice == 15000));
            Assert.AreEqual(1, learningDelivery.PriceEpisodes.Count(pe => pe.StartDate == new DateTime(2017, 10, 15) && pe.EndDate.HasValue && pe.EndDate.Value == new DateTime(2017, 10, 31) && pe.Type == PriceEpisodeType.Initial && pe.NegotiatedPrice == 7500));
            Assert.AreEqual(1, learningDelivery.PriceEpisodes.Count(pe => pe.StartDate == new DateTime(2017, 11, 1) && !pe.EndDate.HasValue && pe.Type == PriceEpisodeType.Residual && pe.NegotiatedPrice == 5625));
        }
    }
}