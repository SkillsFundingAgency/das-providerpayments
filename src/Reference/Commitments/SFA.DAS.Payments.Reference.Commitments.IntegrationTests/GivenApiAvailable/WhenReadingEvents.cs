using System;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.StubbedInfrastructure;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Events.Api.Types;
using PaymentStatus = SFA.DAS.Payments.Reference.Commitments.Application.PaymentStatus;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.GivenApiAvailable
{
    public class WhenReadingEvents
    {
        private ImportCommitmentsTask _task;
        private IntegrationTaskContext _context;

        [SetUp]
        public void Arrange()
        {
            CommitmentDataHelper.Clean();

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 1,
                ApprenticeshipId = 1,
                ProviderId = "123",
                LearnerId = "456",
                EmployerAccountId = "1",
                TrainingType = TrainingTypes.Standard,
                TrainingId = "987",
                TrainingStartDate = new DateTime(2017, 9, 1),
                TrainingEndDate = new DateTime(2018, 10, 1),
                PaymentStatus = Events.Api.Types.PaymentStatus.Active,
                PaymentOrder=1,
                LegalEntityName="name1",
                PriceHistory = new List<PriceHistory>() {
                    new PriceHistory {
                        TotalCost=12345,
                        EffectiveFrom= new DateTime(2017,10,10),
                        EffectiveTo = new DateTime(2017, 12, 10)
                    }
                }

            });
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 2,
                ApprenticeshipId = 2,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = "854-965-621",
                TrainingStartDate = new DateTime(2019, 9, 1),
                TrainingEndDate = new DateTime(2020, 10, 1),
                TrainingTotalCost = 14523,
                PaymentStatus = Events.Api.Types.PaymentStatus.Withdrawn,
                PaymentOrder=2,
                LegalEntityName = "name2",
                PriceHistory = new List<PriceHistory>() {
                    new PriceHistory {
                        TotalCost=14523,
                        EffectiveFrom= new DateTime(2019, 9, 1)
                    }
                }
            });

            _task = new ImportCommitmentsTask();

            _context = new IntegrationTaskContext();
        }

        [Test]
        public void ThenItShouldAddCommitmentsThatDidNotExist()
        {
            // Act
            _task.Execute(_context);

            // Assert
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(2, commitments.Length);

            Assert.AreEqual(1, commitments[0].CommitmentId);
            Assert.AreEqual(1, commitments[0].Priority);
            Assert.AreEqual("1-001", commitments[0].VersionId);
            Assert.AreEqual(123, commitments[0].Ukprn);
            Assert.AreEqual(456, commitments[0].Uln);
            Assert.AreEqual(1, commitments[0].AccountId);
            Assert.AreEqual(987, commitments[0].StandardCode);
            Assert.AreEqual(new DateTime(2017, 9, 1), commitments[0].StartDate);
            Assert.AreEqual(new DateTime(2018, 10, 1), commitments[0].EndDate);
            Assert.AreEqual(12345, commitments[0].AgreedCost);
            Assert.AreEqual((int)PaymentStatus.Active, commitments[0].PaymentStatus);
            Assert.AreEqual(PaymentStatus.Active.ToString(), commitments[0].PaymentStatusDescription);
            Assert.AreEqual("name1", commitments[0].LegalEntityName);
            Assert.AreEqual(new DateTime(2017, 10, 10), commitments[0].EffectiveFromDate);
            Assert.AreEqual(new DateTime(2017, 12, 10), commitments[0].EffectiveToDate);



            Assert.AreEqual(2, commitments[1].CommitmentId);
            Assert.AreEqual(2, commitments[1].Priority);
            Assert.AreEqual("2-001", commitments[1].VersionId);
            Assert.AreEqual(369, commitments[1].Ukprn);
            Assert.AreEqual(258, commitments[1].Uln);
            Assert.AreEqual(2, commitments[1].AccountId);
            Assert.AreEqual(854, commitments[1].FrameworkCode);
            Assert.AreEqual(965, commitments[1].ProgrammeType);
            Assert.AreEqual(621, commitments[1].PathwayCode);
            Assert.AreEqual(new DateTime(2019, 9, 1), commitments[1].StartDate);
            Assert.AreEqual(new DateTime(2020, 10, 1), commitments[1].EndDate);
            Assert.AreEqual(14523, commitments[1].AgreedCost);
            Assert.AreEqual((int)PaymentStatus.Withdrawn, commitments[1].PaymentStatus);
            Assert.AreEqual(PaymentStatus.Withdrawn.ToString(), commitments[1].PaymentStatusDescription);
            Assert.AreEqual("name2", commitments[1].LegalEntityName);
            Assert.AreEqual(commitments[1].StartDate, commitments[1].EffectiveFromDate);
            Assert.AreEqual(null, commitments[1].EffectiveToDate);
        }

        [Test]
        public void ThenItShouldCreateNewVersionOfCommitments()
        {
            var commitment = new DataHelpers.Entities.CommitmentEntity
            {
                CommitmentId = 2,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "1",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName = "ACME Ltd."
            };
            // Arrange
            CommitmentDataHelper.AddCommitment(commitment.CommitmentId,commitment.AccountId
                    , commitment.Uln,commitment.Ukprn,commitment.StartDate,commitment.EndDate,commitment.AgreedCost,
                    null, commitment.ProgrammeType,commitment.FrameworkCode,commitment.PathwayCode, commitment.Priority, 
                    commitment.PaymentStatus, commitment.PaymentStatusDescription, 
                    commitment.VersionId,commitment.EffectiveFromDate,commitment.EffectiveToDate,legalEntityName: commitment.LegalEntityName);

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 3,
                ApprenticeshipId = 2,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = "854-965-621",
                TrainingStartDate = new DateTime(2019, 9, 1),
                TrainingEndDate = new DateTime(2020, 10, 1),
                TrainingTotalCost = 99999,
                PaymentStatus = Events.Api.Types.PaymentStatus.Withdrawn,
                PaymentOrder = 99,
                CreatedOn = new DateTime(2019, 12, 1),
                LegalEntityName= "ACME Ltd.",
                PriceHistory = new List<PriceHistory> {
                    new PriceHistory {
                        EffectiveFrom = new DateTime(2019, 9, 1),
                        TotalCost=14523m,
                        EffectiveTo = new DateTime(2019, 10, 31)
                    },
                    new PriceHistory {
                        EffectiveFrom = new DateTime(2019, 11, 1),
                        TotalCost=9999m
                    }
                }
            });

            // Act
            _task.Execute(_context);

            // Assert
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(2, commitments.Length);

            AssertCommitmentEntity(commitment, commitments[0]);
            AssertCommitmentEntity(commitment, commitments[1]);
            
            Assert.IsNotNull(commitments.Single(x=> x.AgreedCost == 14523m 
                                                && x.EffectiveFromDate == new DateTime(2019, 9, 1) 
                                                && x.EffectiveToDate == new DateTime(2019, 10, 31) 
                                                && x.Priority == 99
                                                && x.VersionId == "3-001"));

            Assert.IsNotNull(commitments.Single(x => x.AgreedCost == 9999m 
                                                && x.EffectiveFromDate == new DateTime(2019, 11, 1) 
                                                && x.EffectiveToDate == null 
                                                && x.Priority == 99
                                                && x.VersionId == "3-002"));


            
        }


        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ThenItShouldIgnoreNewEventIfThereIsNoMaterialChange(int? commitmentId,
                                                            DateTime? startDate,
                                                            DateTime? endDate,
                                                            int? priority,
                                                            string course,
                                                            DateTime? effectiveFromDate,
                                                            decimal? agreedCost, 
                                                            PaymentStatus? status,
                                                            DateTime? dateofBirth)
        {
            var commitment = new DataHelpers.Entities.CommitmentEntity
            {
                CommitmentId = 2,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Active,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "1",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName = "ACME Ltd."
            };
            // Arrange
            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId
                    , commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
                    null, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode, commitment.Priority,
                    commitment.PaymentStatus, commitment.PaymentStatusDescription,
                    commitment.VersionId, commitment.EffectiveFromDate, commitment.EffectiveToDate, legalEntityName: commitment.LegalEntityName);

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 3,
                ApprenticeshipId = commitmentId ?? 2,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = course ?? "854-965-621",
                TrainingStartDate = startDate?? new DateTime(2019, 9, 1),
                TrainingEndDate = endDate ?? new DateTime(2020, 10, 1),
                PaymentStatus = Events.Api.Types.PaymentStatus.Active,
                PaymentOrder = priority?? 1,
                CreatedOn = new DateTime(2019, 12, 1),
                LegalEntityName = "ACME Ltd.",
                DateOfBirth= dateofBirth ?? new DateTime(2000,10,10),
                PriceHistory = new List<PriceHistory> {
                        new PriceHistory {
                            EffectiveFrom=effectiveFromDate?? new DateTime(2019, 9, 1),
                            EffectiveTo=new DateTime(2020, 10, 1),
                            TotalCost = agreedCost ?? 14523m
                        }
                    }
            });

            // Act
            _task.Execute(_context);

            // Assert
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(1, commitments.Length);

           
        }

        [Test]
        public void ThenItShouldAddDeleteExistingAndAddNewCommitmentVersions()
        {
            var commitment = new DataHelpers.Entities.CommitmentEntity
            {
                CommitmentId = 1,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "1",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName = "ACME Ltd."
            };

            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId
              , commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
              null, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode, commitment.Priority,
              commitment.PaymentStatus, commitment.PaymentStatusDescription,
              commitment.VersionId, commitment.EffectiveFromDate, commitment.EffectiveToDate, legalEntityName: commitment.LegalEntityName);


            // Assert
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(1, commitments.Length);

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 3,
                ApprenticeshipId = 1,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = "854-965-621",
                TrainingStartDate = new DateTime(2019, 9, 1),
                TrainingEndDate = new DateTime(2020, 10, 1),
                PaymentStatus = Events.Api.Types.PaymentStatus.Withdrawn,
                PaymentOrder = 1,
                CreatedOn = new DateTime(2019, 12, 1),
                LegalEntityName = "ACME Ltd.",
                PriceHistory = new List<PriceHistory> {
                    new PriceHistory {
                        EffectiveFrom = new DateTime(2019, 9, 1),
                        TotalCost=14523m
                    },
                    new PriceHistory {
                        EffectiveFrom = new DateTime(2019, 11, 1),
                        TotalCost=9999m
                    },
                     new PriceHistory {
                        TotalCost=1234599,
                        EffectiveFrom= new DateTime(2019,10,10),
                        EffectiveTo = new DateTime(2019, 12, 10)
                    }
                }
            });

            _task.Execute(_context);

            // Assert
            commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(3, commitments.Length);

            Assert.IsNotNull(commitments.Any(x => x.CommitmentId == 1 && x.VersionId == "1-001"  && x.EffectiveFromDate == new DateTime(2019, 9, 1) && x.EffectiveToDate == null  && x.AgreedCost == 14523m));
            Assert.IsNotNull(commitments.Any(x => x.CommitmentId == 1 && x.VersionId == "1-002" && x.EffectiveFromDate == new DateTime(2019, 11, 1) && x.EffectiveToDate == null  && x.AgreedCost == 9999m));
            Assert.IsNotNull(commitments.Any(x => x.CommitmentId == 1 && x.VersionId == "1-003" && x.EffectiveFromDate == new DateTime(2019, 10, 10) && x.EffectiveToDate == new DateTime(2019, 12, 10) && x.AgreedCost == 1234599));

        }

        [Test]
        public void ThenItShouldUpdateAllVersionsWithNewPriority()
        {
            var commitment = new DataHelpers.Entities.CommitmentEntity
            {
                CommitmentId = 1,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "1",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                EffectiveToDate= new DateTime(2019, 9, 30),
                LegalEntityName = "ACME Ltd."
            };

            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId
              , commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
              null, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode, 99,
              commitment.PaymentStatus, commitment.PaymentStatusDescription,
             "1-001", commitment.EffectiveFromDate, commitment.EffectiveToDate, legalEntityName: commitment.LegalEntityName);

            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId
           , commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
           null, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode, 999,
           commitment.PaymentStatus, commitment.PaymentStatusDescription,
           "1-002", new DateTime(2019,10,1), null, legalEntityName: commitment.LegalEntityName);


           
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(2, commitments.Length);

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 3,
                ApprenticeshipId = 1,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = "854-965-621",
                TrainingStartDate = new DateTime(2019, 9, 1),
                TrainingEndDate = new DateTime(2020, 10, 1),
                PaymentStatus = Events.Api.Types.PaymentStatus.Withdrawn,
                PaymentOrder = 1000,
                CreatedOn = new DateTime(2019, 12, 1),
                LegalEntityName = "ACME Ltd.",
                PriceHistory = new List<PriceHistory> {
                    
                    new PriceHistory {
                        EffectiveFrom = new DateTime(2019, 9, 1),
                        EffectiveTo =  new DateTime(2019, 9, 30),
                        TotalCost=14523m
                    },
                      new PriceHistory {
                        EffectiveFrom = new DateTime(2019, 10, 1),
                        TotalCost=14523m
                    }
                }
            });

            _task.Execute(_context);

            // Assert
            commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(2, commitments.Length);

            Assert.IsNotNull(commitments.Any(x => x.CommitmentId == 1 && x.VersionId == "1-001" && x.Priority == 1000 && x.EffectiveFromDate == new DateTime(2019, 9, 1) && x.EffectiveToDate == new DateTime(2019, 9, 30) && x.AgreedCost == 14523m));
            Assert.IsNotNull(commitments.Any(x => x.CommitmentId == 1 && x.VersionId == "1-002" && x.Priority == 1000 && x.EffectiveFromDate == new DateTime(2019, 10, 1) && x.EffectiveToDate == null && x.AgreedCost == 14523m));

        }

        [Test]
        public void ThenItShouldIgnoreEventWhenPriceHistoryIsNull()
        {
            var commitment = new DataHelpers.Entities.CommitmentEntity
            {
                CommitmentId = 2,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "2-001",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName = "ACME Ltd."
            };
            // Arrange
            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId
                    , commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
                    null, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode, commitment.Priority,
                    commitment.PaymentStatus, commitment.PaymentStatusDescription,
                    commitment.VersionId, commitment.EffectiveFromDate, commitment.EffectiveToDate, legalEntityName: commitment.LegalEntityName);

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 3,
                ApprenticeshipId = 2,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = "854-965-621",
                TrainingStartDate = new DateTime(2020, 9, 1), 
                TrainingEndDate = new DateTime(2021, 10, 1),
                TrainingTotalCost = 99999,
                PaymentStatus = Events.Api.Types.PaymentStatus.Withdrawn,
                PaymentOrder = 99, //changed
                CreatedOn = new DateTime(2019, 12, 1),
                LegalEntityName = "ACME Ltd.",
                PriceHistory = null
            });

            // Act
            _task.Execute(_context);

            // Assert
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(1, commitments.Length);

            //check that its same commitment and no data is changed
            Assert.IsNotNull(commitments.Any(x => x.CommitmentId == 2 && x.VersionId == "2-001" && x.Priority == 1 && x.AgreedCost == 14523m));
        }

        [Test]
        public void ThenItShouldIgnoreEventPriceHistoryWhereEffectiveFromIsAfterEffectiveTo()
        {
            var effectiveDate = DateTime.Today;

            var commitment = new DataHelpers.Entities.CommitmentEntity
            {
                CommitmentId = 2,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "2-001",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName = "ACME Ltd."
            };
            // Arrange
            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId
                    , commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
                    null, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode, commitment.Priority,
                    commitment.PaymentStatus, commitment.PaymentStatusDescription,
                    commitment.VersionId, commitment.EffectiveFromDate, commitment.EffectiveToDate, legalEntityName: commitment.LegalEntityName);

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 3,
                ApprenticeshipId = 2,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = "854-965-621",
                TrainingStartDate = new DateTime(2020, 9, 1),
                TrainingEndDate = new DateTime(2021, 10, 1),
                TrainingTotalCost = 99999,
                PaymentStatus = Events.Api.Types.PaymentStatus.Withdrawn,
                PaymentOrder = 99, //changed
                CreatedOn = new DateTime(2019, 12, 1),
                LegalEntityName = "ACME Ltd.",
                PriceHistory = new List<PriceHistory>
                {
                    new PriceHistory
                    {
                        EffectiveFrom = effectiveDate,
                        EffectiveTo = effectiveDate.AddDays(-1)
                    }
                }
            });

            // Act
            _task.Execute(_context);

            // Assert
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(1, commitments.Length);

            var history = CommitmentDataHelper.GetCommitmentHistory();
            Assert.AreEqual(0, history.Length);
        }

        [Test]
        public void ThenItShouldAddEventPriceHistoryWhereEffectiveFromIsBeforeEffectiveTo()
        {
            var effectiveDate = DateTime.Today;

            var commitment = new DataHelpers.Entities.CommitmentEntity
            {
                CommitmentId = 2,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "2-001",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName = "ACME Ltd."
            };
            // Arrange
            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId
                    , commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
                    null, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode, commitment.Priority,
                    commitment.PaymentStatus, commitment.PaymentStatusDescription,
                    commitment.VersionId, commitment.EffectiveFromDate, commitment.EffectiveToDate, legalEntityName: commitment.LegalEntityName);

            StubbedEventsApi.Events.Clear();
            StubbedEventsApi.Events.Add(new ApprenticeshipEventView
            {
                Id = 3,
                ApprenticeshipId = 2,
                ProviderId = "369",
                LearnerId = "258",
                EmployerAccountId = "2",
                TrainingType = TrainingTypes.Framework,
                TrainingId = "854-965-621",
                TrainingStartDate = new DateTime(2020, 9, 1),
                TrainingEndDate = new DateTime(2021, 10, 1),
                TrainingTotalCost = 99999,
                PaymentStatus = Events.Api.Types.PaymentStatus.Withdrawn,
                PaymentOrder = 99, //changed
                CreatedOn = new DateTime(2019, 12, 1),
                LegalEntityName = "ACME Ltd.",
                PriceHistory = new List<PriceHistory>
                {
                    new PriceHistory
                    {
                        EffectiveFrom = effectiveDate.AddDays(-1),
                        EffectiveTo = effectiveDate
                    }
                }
            });

            // Act
            _task.Execute(_context);

            // Assert
            var commitments = CommitmentDataHelper.GetCommitments();
            Assert.AreEqual(1, commitments.Length);

            var history = CommitmentDataHelper.GetCommitmentHistory();
            Assert.AreEqual(1, history.Length);
        }


        private static object[] TestCases => new object[]
       {
           //commitmentId,startDate,endDate,priority,course,effectiveFromDate,price, status,dateofBirth
            new object[] {2, new DateTime(2019, 9, 1), new DateTime(2020, 10, 1), 1, "854-965-621", new DateTime(2019, 9, 1), 14523m, PaymentStatus.Active ,new DateTime(1992,10,10)},
            new object[] {2, null, new DateTime(2020, 10, 1), 1, "854-965-621", new DateTime(2019, 9, 1), 14523m, PaymentStatus.Active ,new DateTime(1992,10,10)},
            new object[] {2, new DateTime(2019, 9, 1), null, 1, "854-965-621", new DateTime(2019, 9, 1), 14523m, PaymentStatus.Active ,new DateTime(1992,10,10)},
            new object[] {2, new DateTime(2019, 9, 1), new DateTime(2020, 10, 1), null, "854-965-621", new DateTime(2019, 9, 1), 14523m, PaymentStatus.Active ,new DateTime(1992,10,10)},
            new object[] {2, new DateTime(2019, 9, 1), new DateTime(2020, 10, 1), 1, null, new DateTime(2019, 9, 1), 14523m, PaymentStatus.Active ,new DateTime(1992,10,10)},
            new object[] {2, new DateTime(2019, 9, 1), new DateTime(2020, 10, 1), 1, "854-965-621", null, 14523m, PaymentStatus.Active ,new DateTime(1992,10,10)},
            new object[] {2, new DateTime(2019, 9, 1), new DateTime(2020, 10, 1), 1, "854-965-621", new DateTime(2019, 9, 1), null, PaymentStatus.Active ,new DateTime(1992,10,10)},
            new object[] {2, new DateTime(2019, 9, 1), new DateTime(2020, 10, 1), 1, "854-965-621", new DateTime(2019, 9, 1), 14523m, PaymentStatus.Active ,null}

       };



        private void AssertCommitmentEntity(DataHelpers.Entities.CommitmentEntity source, DataHelpers.Entities.CommitmentEntity destination)
        {
            Assert.AreEqual(source.CommitmentId, destination.CommitmentId);
            Assert.AreEqual(source.Ukprn, destination.Ukprn);
            Assert.AreEqual(source.Uln, destination.Uln);
            Assert.AreEqual(source.AccountId, destination.AccountId);
            Assert.AreEqual(source.FrameworkCode, destination.FrameworkCode);
            Assert.AreEqual(source.ProgrammeType, destination.ProgrammeType);
            Assert.AreEqual(source.PathwayCode, destination.PathwayCode);
            Assert.AreEqual(source.StartDate, destination.StartDate);
            Assert.AreEqual(source.EndDate , destination.EndDate);
            Assert.AreEqual(source.PaymentStatus, destination.PaymentStatus);
            Assert.AreEqual(source.PaymentStatusDescription, destination.PaymentStatusDescription);
            Assert.AreEqual(source.LegalEntityName, destination.LegalEntityName);
        }
    }
}
