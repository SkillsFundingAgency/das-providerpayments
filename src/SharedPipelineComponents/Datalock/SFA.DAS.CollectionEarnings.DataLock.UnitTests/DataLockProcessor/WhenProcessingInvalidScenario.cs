using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockProcessor
{
    public class WhenProcessingInvalidScenario
    {
        private DataLock.DataLockProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new DataLock.DataLockProcessor(_logger.Object, _mediator.Object);

            MediatorSetup();
        }

        private void MediatorSetup()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Provider
                        {
                            Ukprn = 10007459
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderCommitmentsQueryRequest>()))
                .Returns(new GetProviderCommitmentsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new CommitmentBuilder().Build()
                    }
                });

            _mediator
              .Setup(m => m.Send(It.IsAny<GetDasAccountsQueryRequest>()))
              .Returns(new GetDasAccountsQueryResponse
              {
                  IsValid = true,
                  Items = new[]
                  {
                        new DasAccountBuilder().Build()
                  }
              });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderPriceEpisodesQueryRequest>()))
                .Returns(new GetProviderPriceEpisodesQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new PriceEpisodeBuilder().Build()
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<RunDataLockValidationQueryRequest>()))
                .Returns(new RunDataLockValidationQueryResponse
                {
                    IsValid = true,
                    ValidationErrors = new[]
                    {
                        new ValidationError
                        {
                            Ukprn = 10007459,
                            LearnerReferenceNumber = "Lrn001",
                            AimSequenceNumber = 1,
                            RuleId = DataLockErrorCodes.MismatchingUkprn,
                            PriceEpisodeIdentifier = "20-25-01/08/2016"
                        }
                    },
                    PriceEpisodeMatches = new[]
                    {
                        new PriceEpisodeMatch
                        {
                            Ukprn = 10007459,
                            LearnerReferenceNumber = "Lrn001",
                            AimSequenceNumber = 1,
                            CommitmentId = 1
                        }
                    },
                    PriceEpisodePeriodMatches = new []
                    {
                        new PriceEpisodePeriodMatch
                        {
                            Ukprn = 10007459,
                            PriceEpisodeIdentifier = "25-27-01/08/2016",
                            LearnerReferenceNumber = "Lrn001",
                            AimSequenceNumber = 1,
                            CommitmentId = 1,
                            CommitmentVersionId = "1-001",
                            Period = 1,
                            Payable = true
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<AddPriceEpisodeMatchesCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<AddPriceEpisodePeriodMatchesCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
          .Setup(m => m.Send(It.IsAny<Get16To18IncentiveEarningsQueryRequest>()))
          .Returns(new Get16To18IncentiveEarningsQueryResponse
          {
              IsValid = true,
              Items = new[]
              {
                        new IncentiveEarningsBuilder().Build()
              }
          });

        }

        [Test]
        public void ThenExpectingExceptionForGetProvidersQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                });

            // Assert
            var ex = Assert.Throws<DataLockException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockException.ErrorReadingProvidersMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetProviderCommitmentsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderCommitmentsQueryRequest>()))
                .Returns(new GetProviderCommitmentsQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                });

            // Assert
            var ex = Assert.Throws<DataLockException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockException.ErrorReadingCommitmentsMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetProviderPriceEpisodesQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderPriceEpisodesQueryRequest>()))
                .Returns(new GetProviderPriceEpisodesQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                });

            // Assert
            var ex = Assert.Throws<DataLockException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockException.ErrorReadingPriceEpisodesMessage));
        }

        [Test]
        public void ThenExpectingExceptionForRunDataLockValidationQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<RunDataLockValidationQueryRequest>()))
                .Returns(new RunDataLockValidationQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                });

            // Assert
            var ex = Assert.Throws<DataLockException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockException.ErrorPerformingDataLockMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddValidationErrorsCommandRequestFailure()
        {

            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<DataLockException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockException.ErrorWritingDataLockValidationErrorsMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddAddPriceEpisodeMatchesCommandRequestFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddPriceEpisodeMatchesCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<DataLockException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockException.ErrorWritingPriceEpisodeMatchesMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddPriceEpisodePeriodMatchesCommandRequestFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddPriceEpisodePeriodMatchesCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<DataLockException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockException.ErrorWritingPriceEpisodePeriodMatchesMessage));
        }
    }
}