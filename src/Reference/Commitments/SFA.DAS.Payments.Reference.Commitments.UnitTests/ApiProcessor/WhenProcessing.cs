using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Payments.Reference.Commitments.Application.AddErrorCommand;
using SFA.DAS.Payments.Reference.Commitments.Application.AddOrUpdateCommitmentCommand;
using SFA.DAS.Payments.Reference.Commitments.Application.GetLastSeenEventIdQuery;
using SFA.DAS.Payments.Reference.Commitments.Application.GetNextBatchOfCommitmentEventsQuery;
using SFA.DAS.Payments.Reference.Commitments.Application.SetLastSeenEventIdCommand;

namespace SFA.DAS.Payments.Reference.Commitments.UnitTests.ApiProcessor
{
    public class WhenProcessing
    {
        private Mock<IMediator> _mediator;
        private Mock<ILogger> _logger;
        private Commitments.ApiProcessor _processor;
        private ApprenticeshipEventView[] _apprenticeshipEventViews;
        private long _lastIdToReturn;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _logger = new Mock<ILogger>();

            ArrangeGetLastSeenEventIdQuery();
            ArrangeGetNextBatchOfCommitmentEventsQuery();

            _processor = new Commitments.ApiProcessor(_mediator.Object, _logger.Object);
        }

        private void ArrangeGetLastSeenEventIdQuery()
        {
            var counter = 0;
            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenEventIdQueryRequest>()))
                .Returns(() =>
                {
                    counter += 2;
                    return new GetLastSeenEventIdQueryResponse
                    {
                        EventId = counter - 2
                    };
                });
        }

        private void ArrangeGetNextBatchOfCommitmentEventsQuery()
        {
            _apprenticeshipEventViews = new[]
            {
                new ApprenticeshipEventView
                {
                    Id = 1,
                    ApprenticeshipId = 10,
                    LearnerId = "11",
                    ProviderId = "12",
                    EmployerAccountId = "13",
                    TrainingStartDate = new DateTime(2017, 9, 1),
                    TrainingEndDate = new DateTime(2018, 10, 1),
                    TrainingType = TrainingTypes.Standard,
                    TrainingId = "1000",
                    PaymentStatus = Events.Api.Types.PaymentStatus.Active,
                    PausedOnDate = DateTime.Today,
                    AccountLegalEntityPublicHashedId = "A1B1C1",
                    PriceHistory =new List<PriceHistory> {
                        new PriceHistory {
                            EffectiveFrom=new DateTime(2017,10,10),
                            EffectiveTo=new DateTime(2017,12,10),
                            TotalCost = 10000m
                        }
                    }

                },
                new ApprenticeshipEventView
                {
                    Id = 2,
                    ApprenticeshipId = 20,
                    LearnerId = "21",
                    ProviderId = "22",
                    EmployerAccountId = "23",
                    TrainingStartDate = new DateTime(2019, 9, 1),
                    TrainingEndDate = new DateTime(2020, 10, 1),
                    TrainingType = TrainingTypes.Framework,
                    TrainingId = "2000-2001-2002",
                    PaymentStatus = Events.Api.Types.PaymentStatus.Active,
                    StoppedOnDate = DateTime.Today,
                    AccountLegalEntityPublicHashedId = "A2B2C2",
                    PriceHistory = new List<PriceHistory> {
                        new PriceHistory {
                            EffectiveFrom=new DateTime(2019, 9, 1),
                            EffectiveTo=new DateTime(2020, 10, 1),
                            TotalCost = 20000m
                        }
                    }
                }
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetNextBatchOfCommitmentEventsQueryRequest>()))
                .Returns((GetNextBatchOfCommitmentEventsQueryRequest request) =>
                {
                    if (request.LastSeenEventId > _lastIdToReturn)
                    {
                        return new GetNextBatchOfCommitmentEventsQueryResponse
                        {
                            IsValid = true,
                            Items = new ApprenticeshipEventView[0]
                        };
                    }

                    return new GetNextBatchOfCommitmentEventsQueryResponse
                    {
                        IsValid = true,
                        Items = _apprenticeshipEventViews
                    };
                });
        }

        [TestFixture]
        public class AnEventWithAStandardCode : WhenProcessing
        {
            [Test]
            public void ThenTheProgrammeTypeIsTwentyFive()
            {
                _mediator.SetupSequence(x => x.Send(It.IsAny<GetLastSeenEventIdQueryRequest>()))
                    .Returns(new GetLastSeenEventIdQueryResponse { EventId = 0 });
                _apprenticeshipEventViews = _apprenticeshipEventViews.Take(1).ToArray();
                _apprenticeshipEventViews[0].TrainingType = TrainingTypes.Standard;
                _apprenticeshipEventViews[0].TrainingId = "312";

                _lastIdToReturn = 0;

                var commitmentList = new List<AddOrUpdateCommitmentCommandRequest>();

                _mediator.Setup(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()))
                    .Callback<IRequest<Unit>>(x =>
                    {
                        var command = x as AddOrUpdateCommitmentCommandRequest;
                        commitmentList.Add(command);
                    });

                // Act
                _processor.Process();

                // Assert
                commitmentList.Where(x => x.ProgrammeType == 25).Should().HaveCount(2);
            }

            [Test]
            public void ThenThereAreNoRecordsWithProgrammeTypeZero()
            {
                _mediator.SetupSequence(x => x.Send(It.IsAny<GetLastSeenEventIdQueryRequest>()))
                    .Returns(new GetLastSeenEventIdQueryResponse { EventId = 0 });
                _apprenticeshipEventViews = _apprenticeshipEventViews.Take(1).ToArray();
                _apprenticeshipEventViews[0].TrainingType = TrainingTypes.Standard;
                _apprenticeshipEventViews[0].TrainingId = "312";

                _lastIdToReturn = 0;

                var commitmentList = new List<AddOrUpdateCommitmentCommandRequest>();

                _mediator.Setup(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()))
                    .Callback<IRequest<Unit>>(x =>
                    {
                        var command = x as AddOrUpdateCommitmentCommandRequest;
                        commitmentList.Add(command);
                    });

                // Act
                _processor.Process();

                // Assert
                commitmentList.Where(x => x.ProgrammeType == 0).Should().BeEmpty();
            }
        }
        
        [Test]
        public void ThenItShouldGetBatchesOfCommitmentEventsUntilNoMoreAreAvailable()
        {
            // Arrange
            _mediator.SetupSequence(x => x.Send(It.IsAny<GetLastSeenEventIdQueryRequest>()))
                .Returns(new GetLastSeenEventIdQueryResponse { EventId = 0 })
                .Returns(new GetLastSeenEventIdQueryResponse { EventId = 1 })
                .Returns(new GetLastSeenEventIdQueryResponse { EventId = 2 });

            _lastIdToReturn = 2;

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetLastSeenEventIdQueryRequest>()), Times.Exactly(3));
            _mediator.Verify(m => m.Send(It.IsAny<GetNextBatchOfCommitmentEventsQueryRequest>()), Times.Exactly(3));
        }

        [Test]
        public void ThenItShouldAddOrUpdateTheCommitmentsForEvents()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()), Times.Exactly(2));
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[0]))), Times.Once);
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[1]))), Times.Once);
        }

        [Test]
        public void ThenItShouldTreatNullsFromTheApiAsAnEmptyListOfEvents()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetNextBatchOfCommitmentEventsQueryRequest>()))
                .Returns(new GetNextBatchOfCommitmentEventsQueryResponse { Items = null });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetLastSeenEventIdQueryRequest>()), Times.Once);
            _mediator.Verify(m => m.Send(It.IsAny<GetNextBatchOfCommitmentEventsQueryRequest>()), Times.Once);
        }

        [Test]
        public void WhenThereIsAProblematicEventThenItShouldReadUpToThatEvent()
        {
            // Arrange
            _apprenticeshipEventViews[1].LearnerId = "xxx";
            _apprenticeshipEventViews[1].ProviderId = "yyy";

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()), Times.Once);
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[0]))), Times.Once);
            
            _mediator.Verify(m => m.Send(It.IsAny<AddErrorCommandRequest>()), Times.Once);

            _mediator.Verify(m => m.Send(It.Is<SetLastSeenEventIdCommandRequest>(r => r.LastSeenEventId ==(long)1)), Times.AtLeastOnce);
        }

        [Test]
        public void ThenTheGetNextBatchOfCommitmentEventsQueryReturnsAnInvalidResponseItShouldWriteTheError()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetNextBatchOfCommitmentEventsQueryRequest>()))
                .Returns(new GetNextBatchOfCommitmentEventsQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Fail for test")
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<AddErrorCommandRequest>(r => r.Error.Message.EndsWith("Fail for test"))), Times.Once);
        }

        [Test]
        public void WhenAnExceptionOccursAfterTheFirstBatchOfEventsThenItShouldPersistEventsFromPreviousBatches()
        {
            // Arrange
            _mediator.SetupSequence(m => m.Send(It.IsAny<GetNextBatchOfCommitmentEventsQueryRequest>()))
                .Returns(new GetNextBatchOfCommitmentEventsQueryResponse { IsValid = true, Items = _apprenticeshipEventViews })
                .Throws<Exception>();

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()), Times.Exactly(2));
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[0]))), Times.Once);
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[1]))), Times.Once);

            _mediator.Verify(m => m.Send(It.IsAny<AddErrorCommandRequest>()), Times.Once);

            _mediator.Verify(m => m.Send(It.Is<SetLastSeenEventIdCommandRequest>(r => r.LastSeenEventId == 2)), Times.AtLeastOnce);
        }

        [Test]
        public void WhenThereIsAPendingApprovalEventThenItShouldBeIgnored()
        {
            // Arrange
            _apprenticeshipEventViews[0].PaymentStatus = PaymentStatus.PendingApproval;

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()), Times.Once());
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[1]))), Times.Once);

            _mediator.Verify(m => m.Send(It.IsAny<AddErrorCommandRequest>()), Times.Never);

            _mediator.Verify(m => m.Send(It.Is<SetLastSeenEventIdCommandRequest>(r => r.LastSeenEventId == 2)), Times.AtLeastOnce);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("NULL")]
        [TestCase("null")]
        [TestCase("abcd")]
        [TestCase("123a")]
        public void WhenThereIsAnEventWithAnInvalidLearnerIdThenItUseTheDefaultValueForUln(string learnerId)
        {
            // Arrange
            _apprenticeshipEventViews = new[] { _apprenticeshipEventViews[0] };
            _apprenticeshipEventViews[0].LearnerId = learnerId;
            _apprenticeshipEventViews[0].PausedOnDate = DateTime.Today;

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()), Times.Once());
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[0], "0"))), Times.Once);

            _mediator.Verify(m => m.Send(It.IsAny<AddErrorCommandRequest>()), Times.Never);

            _mediator.Verify(m => m.Send(It.Is<SetLastSeenEventIdCommandRequest>(r => r.LastSeenEventId == 1)), Times.AtLeastOnce);
        }

        [Test]
        public void WhenThereIsADeletedEventThenItShouldBeIgnored()
        {
            // Arrange
            _apprenticeshipEventViews[0].PaymentStatus = PaymentStatus.Deleted;

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<AddOrUpdateCommitmentCommandRequest>()), Times.Once());
            _mediator.Verify(x => x.Send(It.Is<AddOrUpdateCommitmentCommandRequest>(y => CommandEqualsEvent(y, _apprenticeshipEventViews[1]))), Times.Once);

            _mediator.Verify(m => m.Send(It.IsAny<AddErrorCommandRequest>()), Times.Never);

            _mediator.Verify(m => m.Send(It.Is<SetLastSeenEventIdCommandRequest>(r => r.LastSeenEventId == 2)), Times.AtLeastOnce);
        }

        private bool CommandEqualsEvent(AddOrUpdateCommitmentCommandRequest command, ApprenticeshipEventView @event)
        {
            return CommandEqualsEvent(command, @event, @event.LearnerId);
        }

        private bool CommandEqualsEvent(AddOrUpdateCommitmentCommandRequest command, ApprenticeshipEventView @event, string learnerId)
        {
            var eventEqualsCommand = @event.ApprenticeshipId == command.CommitmentId &&
                                     @event.EmployerAccountId == command.AccountId.ToString() &&
                                     @event.ProviderId == command.Ukprn.ToString() &&
                                     learnerId == command.Uln.ToString() &&
                                     (@event.TrainingStartDate.HasValue ? @event.TrainingStartDate.Value : DateTime.MinValue) == command.StartDate &&
                                     (@event.TrainingEndDate.HasValue ? @event.TrainingEndDate.Value : DateTime.MinValue) == command.EndDate &&
                                     ((List<PriceHistory>)@event.PriceHistory)[0].TotalCost == command.PriceEpisodes[0].AgreedPrice &&
                                     0 == command.Priority &&
                                     (@event.TrainingStartDate.HasValue ? @event.TrainingStartDate.Value : DateTime.MinValue) == command.StartDate &&
                                     @event.LegalEntityName == command.LegalEntityName &&
                                     ((List<PriceHistory>)@event.PriceHistory)[0].EffectiveFrom == command.PriceEpisodes[0].EffectiveFromDate &&
                                     ((List<PriceHistory>)@event.PriceHistory)[0].EffectiveTo == command.PriceEpisodes[0].EffectiveToDate &&
                                     @event.Id == command.VersionId && 
                                     @event.PausedOnDate == command.PausedOnDate &&
                                     @event.StoppedOnDate == command.WithdrawnOnDate &&
                                     @event.AccountLegalEntityPublicHashedId == command.AccountLegalEntityPublicHashedId;
            
            if (@event.TrainingType == TrainingTypes.Framework)
            {
                var parts = @event.TrainingId.Split('-');
                eventEqualsCommand = eventEqualsCommand &&
                                     command.FrameworkCode == int.Parse(parts[0]) &&
                                     command.ProgrammeType == int.Parse(parts[1]) &&
                                     command.PathwayCode == int.Parse(parts[2]);
            }
            else
            {
                eventEqualsCommand = eventEqualsCommand &&
                                     command.StandardCode.ToString() == @event.TrainingId;
            }
            return eventEqualsCommand;
        }
    }
}
