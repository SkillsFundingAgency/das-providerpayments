﻿using System;
using System.Linq;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.Provider.Events.DataLock.Application.GetCurrentCollectionPeriod;
using SFA.DAS.Provider.Events.DataLock.Application.GetCurrentProviderEvents;
using SFA.DAS.Provider.Events.DataLock.Application.GetLastSeenProviderEvents;
using SFA.DAS.Provider.Events.DataLock.Application.GetProviders;
using SFA.DAS.Provider.Events.DataLock.Application.WriteDataLockEvent;
using SFA.DAS.Provider.Events.DataLock.Domain;

namespace SFA.DAS.Provider.Events.DataLock.UnitTests.DataLockEventsProcessor
{
    public class WhenProcessing
    {
        private static readonly object[] EventErrors =
        {
            new object[] { null, new DataLockEventError[0] },
            new object[] { new DataLockEventError[0], null },
            new object[] { new DataLockEventError[0], new DataLockEventError[1] },
            new object[] { new [] { new DataLockEventError { ErrorCode = "1"} }, new [] { new DataLockEventError { ErrorCode = "2"} } },
            new object[] { new [] { new DataLockEventError { SystemDescription = "1"} }, new [] { new DataLockEventError { SystemDescription = "2"} } }
        };

        private static readonly object[] EventPeriods =
        {
            new object[] { null, new DataLockEventPeriod[0] },
            new object[] { new DataLockEventPeriod[0], null },
            new object[] { new DataLockEventPeriod[0], new DataLockEventPeriod[1] },
            new object[] { new [] { new DataLockEventPeriod { CollectionPeriod = new CollectionPeriod {Name = "1"} } }, new [] { new DataLockEventPeriod { CollectionPeriod = new CollectionPeriod {Name = "2"} } } },
            new object[] { new [] { new DataLockEventPeriod { CollectionPeriod = new CollectionPeriod {Month = 1} } }, new [] { new DataLockEventPeriod { CollectionPeriod = new CollectionPeriod {Month = 2} } } },
            new object[] { new [] { new DataLockEventPeriod { CollectionPeriod = new CollectionPeriod {Year = 1} } }, new [] { new DataLockEventPeriod { CollectionPeriod = new CollectionPeriod {Year = 2} } } },
            new object[] { new [] { new DataLockEventPeriod { CommitmentVersion = "1-001" } }, new [] { new DataLockEventPeriod { CommitmentVersion = "1-002" } } },
            new object[] { new [] { new DataLockEventPeriod { IsPayable = true } }, new [] { new DataLockEventPeriod { IsPayable = false } } },
            new object[] { new [] { new DataLockEventPeriod { TransactionTypesFlag = CensusDateType.First16To18Incentive } },
                            new [] { new DataLockEventPeriod { TransactionTypesFlag = CensusDateType.OnProgLearning} }                             }
        };

        private static readonly object[] EventCommitmentVersions =
        {
            new object[] { null, new DataLockEventCommitmentVersion[0] },
            new object[] { new DataLockEventCommitmentVersion[0], null },
            new object[] { new DataLockEventCommitmentVersion[0], new DataLockEventCommitmentVersion[1] },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentVersion = "1-001" } }, new [] { new DataLockEventCommitmentVersion { CommitmentVersion = "1-002" } } },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentStartDate = DateTime.MinValue } }, new [] { new DataLockEventCommitmentVersion { CommitmentStartDate = DateTime.MaxValue } } },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentStandardCode = 1 } }, new [] { new DataLockEventCommitmentVersion { CommitmentStandardCode = 2 } } },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentProgrammeType = 1 } }, new [] { new DataLockEventCommitmentVersion { CommitmentProgrammeType = 2 } } },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentFrameworkCode = 1 } }, new [] { new DataLockEventCommitmentVersion { CommitmentFrameworkCode = 2 } } },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentPathwayCode = 1 } }, new [] { new DataLockEventCommitmentVersion { CommitmentPathwayCode = 2 } } },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentNegotiatedPrice = 1 } }, new [] { new DataLockEventCommitmentVersion { CommitmentNegotiatedPrice = 2 } } },
            new object[] { new [] { new DataLockEventCommitmentVersion { CommitmentEffectiveDate = DateTime.MinValue } }, new [] { new DataLockEventCommitmentVersion { CommitmentEffectiveDate = DateTime.MaxValue } } }
        };

        private Mock<IMediator> _mediator;
        private DataLock.DataLockEventsProcessor _processor;

        private Domain.Provider _provider;
        private DataLockEvent _currentFirstEvent;
        private DataLockEvent _currentUpdatedEvent;
        private DataLockEvent _lastSeenOriginalEvent;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _provider = new Domain.Provider
            {
                Ukprn = 10000534
            };

            _currentFirstEvent = new DataLockEvent
            {
                IlrFileName = "ILR-1617-10000534-75.xml",
                SubmittedDateTime = new DateTime(2017, 2, 14, 9, 15, 23),
                AcademicYear = "1617",
                Ukprn = 10000534,
                Uln = 1000000027,
                LearnRefnumber = "Lrn-002",
                AimSeqNumber = 1,
                PriceEpisodeIdentifier = "25-27-01/05/2017",
                CommitmentId = 75,
                EmployerAccountId = 10,
                EventSource = EventSource.Submission,
                HasErrors = true,
                IlrStartDate = new DateTime(2017, 4, 1),
                IlrStandardCode = 27,
                IlrTrainingPrice = 12000,
                IlrEndpointAssessorPrice = 3000,
                Periods = new[]
                {
                    new DataLockEventPeriod
                    {
                        CollectionPeriod = new CollectionPeriod
                        {
                            Name = "1617-R09",
                            Month = 4,
                            Year = 2017
                        },
                        CommitmentVersion = "75-001",
                        IsPayable = false,
                        TransactionTypesFlag = CensusDateType.OnProgLearning
                    }
                },
                Errors = new[]
                {
                    new DataLockEventError
                    {
                        ErrorCode = "DLOCK_07",
                        SystemDescription = "DLOCK_07"
                    }
                },
                CommitmentVersions = new[]
                {
                    new DataLockEventCommitmentVersion
                    {
                        CommitmentVersion = "75-001",
                        CommitmentStartDate = new DateTime(2017, 4, 1),
                        CommitmentStandardCode = 27,
                        CommitmentNegotiatedPrice = 17500,
                        CommitmentEffectiveDate = new DateTime(2017, 4, 1)
                    }
                }
            };

            _currentUpdatedEvent = new DataLockEvent
            {
                IlrFileName = "ILR-1617-10000534-75.xml",
                SubmittedDateTime = new DateTime(2017, 2, 14, 9, 15, 23),
                AcademicYear = "1617",
                Ukprn = 10000534,
                Uln = 1000000019,
                LearnRefnumber = "Lrn-001",
                AimSeqNumber = 1,
                PriceEpisodeIdentifier = "20-550-6-01/05/2017",
                CommitmentId = 99,
                EmployerAccountId = 10,
                EventSource = EventSource.Submission,
                HasErrors = true,
                IlrStartDate = new DateTime(2017, 4, 1),
                IlrProgrammeType = 20,
                IlrFrameworkCode = 550,
                IlrPathwayCode = 6,
                IlrTrainingPrice = 12250,
                IlrEndpointAssessorPrice = 3000,
                Periods = new[]
                {
                    new DataLockEventPeriod
                    {
                        CollectionPeriod = new CollectionPeriod
                        {
                            Name = "1617-R09",
                            Month = 4,
                            Year = 2017
                        },
                        CommitmentVersion = "99-015",
                        IsPayable = false,
                        TransactionTypesFlag = CensusDateType.OnProgLearning
                    }
                },
                Errors = new[]
                {
                    new DataLockEventError
                    {
                        ErrorCode = "DLOCK_07",
                        SystemDescription = "DLOCK_07"
                    }
                },
                CommitmentVersions = new[]
                {
                    new DataLockEventCommitmentVersion
                    {
                        CommitmentVersion = "99-015",
                        CommitmentStartDate = new DateTime(2017, 4, 1),
                        CommitmentProgrammeType = 20,
                        CommitmentFrameworkCode = 550,
                        CommitmentPathwayCode = 6,
                        CommitmentNegotiatedPrice = 17500,
                        CommitmentEffectiveDate = new DateTime(2017, 4, 1)
                    }
                }
            };

            _lastSeenOriginalEvent = new DataLockEvent
            {
                DataLockEventId = Guid.NewGuid(),
                ProcessDateTime = new DateTime(2017, 2, 12, 9, 15, 23),
                IlrFileName = "ILR-1617-10000534-15.xml",
                SubmittedDateTime = new DateTime(2017, 2, 12, 9, 15, 23),
                AcademicYear = "1617",
                Ukprn = 10000534,
                Uln = 1000000019,
                LearnRefnumber = "Lrn-001",
                AimSeqNumber = 1,
                PriceEpisodeIdentifier = "20-550-6-01/05/2017",
                CommitmentId = 99,
                EmployerAccountId = 10,
                EventSource = EventSource.Submission,
                HasErrors = true,
                IlrStartDate = new DateTime(2017, 4, 1),
                IlrProgrammeType = 20,
                IlrFrameworkCode = 550,
                IlrPathwayCode = 6,
                IlrTrainingPrice = 12000,
                IlrEndpointAssessorPrice = 3000,
                Periods = new[]
                {
                    new DataLockEventPeriod
                    {
                        CollectionPeriod = new CollectionPeriod
                        {
                            Name = "1617-R09",
                            Month = 4,
                            Year = 2017
                        },
                        CommitmentVersion = "99-015",
                        IsPayable = false,
                        TransactionTypesFlag = CensusDateType.OnProgLearning
                    }
                },
                Errors = new[]
                {
                    new DataLockEventError
                    {
                        ErrorCode = "DLOCK_07",
                        SystemDescription = "DLOCK_07"
                    }
                },
                CommitmentVersions = new[]
                {
                    new DataLockEventCommitmentVersion
                    {
                        CommitmentVersion = "99-015",
                        CommitmentStartDate = new DateTime(2017, 4, 1),
                        CommitmentProgrammeType = 20,
                        CommitmentFrameworkCode = 550,
                        CommitmentPathwayCode = 6,
                        CommitmentNegotiatedPrice = 17500,
                        CommitmentEffectiveDate = new DateTime(2017, 4, 1)
                    }
                }
            };

            _logger = new Mock<ILogger>();

            _mediator = new Mock<IMediator>();

            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        _provider
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        _currentFirstEvent,
                        _currentUpdatedEvent
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        _lastSeenOriginalEvent
                    }
                });

            _mediator.Setup(x => x.Send(It.IsAny<GetCurrentCollectionPeriodRequest>()))
                .Returns(new GetCurrentCollectionPeriodResposne
                {
                    CollectionPeriod = new CollectionPeriod
                    {
                        Month = 10,
                        Year = 2016,
                        Name = "Name"
                       
                    }
                });

            _processor = new DataLock.DataLockEventsProcessor(_logger.Object, _mediator.Object);
        }

        [Test]
        public void ThenItShouldThrowAnExceptionIfGetProvidersRequestFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception")
                });

            // Assert
            Assert.Throws<Exception>(() => _processor.Process());
        }

        [Test]
        public void ThenItShouldThrowAnExceptionIfGetCurrentEventsRequestFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception")
                });

            // Assert
            Assert.Throws<Exception>(() => _processor.Process());
        }

        [Test]
        public void ThenItShouldThrowAnExceptionIfGetLastSeenEventsRequestFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception")
                });

            // Assert
            Assert.Throws<Exception>(() => _processor.Process());
        }

        [Test]
        public void ThenItShouldNotWriteAnyEventsIfNoCurrentEventsAreFound()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new DataLockEvent[0]
                });
            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                 .Returns(new GetLastSeenProviderEventsResponse
                 {
                     IsValid = true,
                     Items = new DataLockEvent[0]
                 });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(0));
        }

        [Test]
        public void ThenItShouldWriteAnEventForEachDataLockEventThatHasChanged()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));

            // First data lock event
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events.Any(e => e == _currentFirstEvent))));

            // Updated data lock event
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events.Any(e => e == _currentUpdatedEvent))));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenCommitmentIdHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                CommitmentId = 1
            };

            var last = new DataLockEvent
            {
                CommitmentId = 2
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(
                c => 
                    c.Events.Length == 2 &&
                    c.Events[1].CommitmentId == current.CommitmentId &&
                    c.Events[0].CommitmentId == last.CommitmentId 
                )));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenEmployerAccountIdHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                EmployerAccountId = 1
            };

            var last = new DataLockEvent
            {
                EmployerAccountId = 2
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenSuccessStatusHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                HasErrors = false
            };

            var last = new DataLockEvent
            {
                HasErrors = true
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrStartDateHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrStartDate = DateTime.MinValue
            };

            var last = new DataLockEvent
            {
                IlrStartDate = DateTime.MaxValue
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrStandardCodeHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrStandardCode = 27
            };

            var last = new DataLockEvent
            {
                IlrStandardCode = 28
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrProgrammeTypeHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrProgrammeType = 27
            };

            var last = new DataLockEvent
            {
                IlrProgrammeType = 28
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrFrameworkCodeHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrFrameworkCode = 27
            };

            var last = new DataLockEvent
            {
                IlrFrameworkCode = 28
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrPathwayCodeHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrPathwayCode = 27
            };

            var last = new DataLockEvent
            {
                IlrPathwayCode = 28
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrTrainingPriceHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrTrainingPrice = 12000
            };

            var last = new DataLockEvent
            {
                IlrTrainingPrice = 12500
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrEndpointAssessorPriceHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrEndpointAssessorPrice = 3000
            };

            var last = new DataLockEvent
            {
                IlrEndpointAssessorPrice = 3250
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenIlrPriceEffectiveDateHasChanged()
        {
            // Arrange
            var current = new DataLockEvent
            {
                IlrPriceEffectiveFromDate = DateTime.Today.AddDays(-10)
            };

            var last = new DataLockEvent
            {
                IlrPriceEffectiveFromDate = DateTime.Today.AddDays(-11)
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        [TestCaseSource(nameof(EventErrors))]
        public void ThenItShouldWriteAnEventWhenErrorsHaveChanged(DataLockEventError[] currentErrors, DataLockEventError[] lastSeenErrors)
        {
            // Arrange
            var current = new DataLockEvent
            {
                Errors = currentErrors
            };

            var last = new DataLockEvent
            {
                Errors = lastSeenErrors
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        [TestCaseSource(nameof(EventPeriods))]
        public void ThenItShouldWriteAnEventWhenPeriodsHaveChanged(DataLockEventPeriod[] currentPeriods, DataLockEventPeriod[] lastSeenPeriods)
        {
            // Arrange
            var current = new DataLockEvent
            {
                Periods = currentPeriods
            };

            var last = new DataLockEvent
            {
                Periods = lastSeenPeriods
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        [TestCaseSource(nameof(EventCommitmentVersions))]
        public void ThenItShouldWriteAnEventWhenCommitmentVersionsHaveChanged(DataLockEventCommitmentVersion[] currentVersions, DataLockEventCommitmentVersion[] lastSeenVersions)
        {
            // Arrange
            var current = new DataLockEvent
            {
                CommitmentVersions = currentVersions
            };

            var last = new DataLockEvent
            {
                CommitmentVersions = lastSeenVersions
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { current }
                });

            _mediator.Setup(m => m.Send(It.IsAny<GetLastSeenProviderEventsRequest>()))
                .Returns(new GetLastSeenProviderEventsResponse
                {
                    IsValid = true,
                    Items = new[] { last }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(c => c.Events[0] == current)));
        }

        [Test]
        public void ThenItShouldWriteAnEventWhenLastSeenEventNotInCurrentEvents()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new DataLockEvent[0]
                });
            _lastSeenOriginalEvent.PriceEpisodeIdentifier += "a";

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(1));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(r => r.Events != null
                                                                                   && r.Events.Length == 1
                                                                                   && r.Events[0].PriceEpisodeIdentifier == _lastSeenOriginalEvent.PriceEpisodeIdentifier
                                                                                   && r.Events[0].Status == EventStatus.Removed)));
        }

        [Test]
        public void ThenItShouldNotWriteAnEventWhenLastSeenStatusIsRemovedAndCurrentEventIsNull()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetCurrentProviderEventsRequest>()))
                .Returns(new GetCurrentProviderEventsResponse
                {
                    IsValid = true,
                    Items = new DataLockEvent[]{}
                });

            _lastSeenOriginalEvent.Status = EventStatus.Removed;

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<WriteDataLockEventCommandRequest>()), Times.Exactly(0));
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(r => r.Events != null
                                                                                      && r.Events.Length == 1
                                                                                      && r.Events[0].PriceEpisodeIdentifier == _lastSeenOriginalEvent.PriceEpisodeIdentifier
                                                                                      && r.Events[0].Status == EventStatus.Removed)), Times.Never);
        }


        [Test]
        public void ThenTheCurrentPeriodDateIsPopulatedFromTheQuery()
        {
            //Act
            _processor.Process();

            //Assert
            _mediator.Verify(x=>x.Send(It.IsAny<GetCurrentCollectionPeriodRequest>()),Times.Once);
            _mediator.Verify(m => m.Send(It.Is<WriteDataLockEventCommandRequest>(r => r.Events != null 
                                                                                && r.Events[0].CurrentPeriodToDate.Equals(new DateTime(2016, 10, 31)))));
        }
    }
}