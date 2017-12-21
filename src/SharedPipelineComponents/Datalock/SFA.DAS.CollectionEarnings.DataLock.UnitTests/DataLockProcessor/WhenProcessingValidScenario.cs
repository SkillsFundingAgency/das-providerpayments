using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockProcessor
{
    public class WhenProcessingValidScenario
    {
        private static readonly object[] EmptyItems =
        {
            new object[] {null},
            new object[] {new PriceEpisode[] {}}
        };

        private static readonly Provider[] Providers =
        {
            new Provider
            {
                Ukprn = 10007459
            },
            new Provider
            {
                Ukprn = 10007458
            }
        };

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
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<AddPriceEpisodeMatchesCommandRequest>()))
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
        public void ThenItShouldCallGetProviderCommitmentsQueryMultipleTimesForMultipleProviders()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetProviderCommitmentsQueryRequest>()), Times.Exactly(2));
            _mediator.Verify(m => m.Send(It.Is<GetProviderCommitmentsQueryRequest>(it => it.Ukprn == Providers[0].Ukprn)), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<GetProviderCommitmentsQueryRequest>(it => it.Ukprn == Providers[1].Ukprn)), Times.Once);
        }

        [Test]
        public void ThenItShouldCallGetProviderPriceEpisodesQueryMultipleTimesForMultipleProviders()
        {
            // Arrange

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetProviderPriceEpisodesQueryRequest>()), Times.Exactly(2));
            _mediator.Verify(m => m.Send(It.Is<GetProviderPriceEpisodesQueryRequest>(it => it.Ukprn == Providers[0].Ukprn)), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<GetProviderPriceEpisodesQueryRequest>(it => it.Ukprn == Providers[1].Ukprn)), Times.Once);
        }

        [Test]
        public void ThenItShouldRunDataLockValidationQueryMultipleTimesForMultipleProviders()
        {
            // Arrange

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<RunDataLockValidationQueryRequest>()), Times.Exactly(2));
        }

        [Test]
        public void ThenItShouldCallAddValidationErrorsCommandMultipleTimesForMultipleProviders()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()), Times.Exactly(2));
        }

        [Test]
        public void ThenItShouldCallAddLearnerCommitmentsCommandMultipleTimesForMultipleProviders()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<AddPriceEpisodeMatchesCommandRequest>()), Times.Exactly(2));
        }

        [Test]
        [TestCaseSource(nameof(EmptyItems))]
        public void ThenNoDataLockValidationIsExecutedForGetProviderPriceEpisodesQueryResponseWithNoItems(PriceEpisode[] items)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderPriceEpisodesQueryRequest>()))
                .Returns(new GetProviderPriceEpisodesQueryResponse
                {
                    IsValid = true,
                    Items = items
                }
                );

            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.IsRegex("No price episodes found.")), Times.Once);
        }

        [Test]
        public void ThenOutputsCorrectLogMessagesForGetProviderPriceEpisodesQueryResponseWithItems()
        {
            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.IsRegex("Started Data Lock Processor.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Performing Data Lock Validation for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Reading commitments for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Reading price episodes for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started Data Lock Validation.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished Data Lock Validation.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started writing Data Lock Validation Errors.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished writing Data Lock Validation Errors.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started writing price episode matches.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished writing price episode matches")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished Data Lock Processor.")), Times.Once);

            _logger.Verify(l => l.Info(It.IsRegex("No price episodes found.")), Times.Never);
        }
    }
}