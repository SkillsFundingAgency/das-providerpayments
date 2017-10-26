using System;
using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.DataLockTask
{
    public class WhenExecuteCalledDuringAnIlrPeriodEnd
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.PeriodEndConnectionString;

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
            TestDataHelper.PeriodEndAddCollectionPeriod();

            _task = new DataLock.DataLockTask();

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString },
                    { ContextPropertyKeys.LogLevel, "Trace" },
                    { DataLockContextPropertyKeys.YearOfCollection, "1617" }
                }
            };
        }

        private void SetupCommitmentData(CommitmentEntity commitment)
        {
            TestDataHelper.PeriodEndAddCommitment(commitment);
        }
        private void SetupAccountData(DasAccount account)
        {
            TestDataHelper.PeriodEndAddDasAccount(account);
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUkprn()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndUkprnMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().Withukprn(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUkprn && e.PriceEpisodeIdentifier == "27-25-2016-09-01"));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUln()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndUlnMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUln));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.IsEmpty(priceEpisodeMatches);
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingStandard()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndStandardMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingStandard));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingFramework()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndFrameworkMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithFrameworkCode(999).Build());
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingFramework));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingProgramme()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndProgrammeMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithProgrammeType(999).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingProgramme));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingPathway()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndPathwayMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithPathwayCode(999).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPathway));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorsAddedForMismatchingPrice()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndPriceMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPrice));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(2, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorsAddedForIlrEarlierStartDate()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndEarlierStartDateMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.EarlierStartDate));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(2, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorsAddedForMultipleMatchingCommitments()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndMultipleMatches.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithCommitmentId(3).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(4).WithUln(1000000027).WithStandardCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MultipleMatches));

            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(false);

            Assert.AreEqual(4, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenPriceEpisodeMatchesAddedForMatchFound()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build()
            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndMatchFound.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches();
            var priceEpisodePeriodMatches = TestDataHelper.PeriodEndGetPriceEpisodePeriodMatches();
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            Assert.IsNotNull(priceEpisodeMatches);
            Assert.AreEqual(2, priceEpisodeMatches.Length);
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-17"));
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-19"));

            Assert.IsNotNull(priceEpisodePeriodMatches);
            Assert.AreEqual(242, priceEpisodePeriodMatches.Length);
            Assert.AreEqual(121, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-17"));
            Assert.AreEqual(121, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-19"));
        }

        [Test]
        public void ThenForAChangeOfEmployersPriceEpisodeMatchesAreAddedWhenMatchesAreFound()
        {
            // Arrange
            _context.Properties[DataLockContextPropertyKeys.YearOfCollection] = "1718";

            var commitments = new[]
            {
                new CommitmentEntityBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2017, 10, 31))
                    .Build(),
                new CommitmentEntityBuilder()
                    .WithCommitmentId(2)
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 11, 1))
                    .WithEffectiveFrom(new DateTime(2017, 11, 1))
                    .WithEndDate(new DateTime(2018, 8, 31))
                    .WithAgreedCost(5625m)
                    .Build()
            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerChangesEmployers.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);

            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches();
            var priceEpisodePeriodMatches = TestDataHelper.PeriodEndGetPriceEpisodePeriodMatches();
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            Assert.IsNotNull(priceEpisodeMatches);
            Assert.AreEqual(2, priceEpisodeMatches.Length);
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-08-01"));
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-11-01"));

            Assert.AreEqual(110, priceEpisodePeriodMatches.Length);
            Assert.AreEqual(33, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-08-01"));
            Assert.AreEqual(77, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-11-01"));
        }

        [Test]
        public void ThenWhereOnProgrammeNotPayableAdditionalPaymentsArePayable()
        {
            // Arrange
            _context.Properties[DataLockContextPropertyKeys.YearOfCollection] = "1718";

            var commitments = new[]
            {
                new CommitmentEntityBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 08, 01))
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .WithEffectiveTo(new DateTime(2017, 11, 14))
                    .WithVersionId("1-001")
                    .WithPaymentStatus(UnitTests.Tools.Enums.PaymentStatus.Active)
                    .WithAgreedCost(7500)
                    .Build(),
                new CommitmentEntityBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 08, 01))
                    .WithEffectiveFrom(new DateTime(2017, 11, 15))
                    .WithVersionId("1-002")
                    .WithPaymentStatus(UnitTests.Tools.Enums.PaymentStatus.Cancelled)
                    .WithAgreedCost(7500)
                    .Build()
             
            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerFirstIncentiveThreshhold.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();
            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            var priceEpisodePayable = TestDataHelper.GetPriceEpisodePeriodMatchForPeriodEnd(false, 4);

            Assert.IsTrue(priceEpisodePayable.Length > 0);
            Assert.IsTrue(priceEpisodePayable.Single(x=> x.TransactionType == Payments.DCFS.Domain.TransactionType.First16To18EmployerIncentive && x.VersionId == "1-001" && x.CommitmentId ==1).Payable);
            Assert.IsTrue(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.First16To18ProviderIncentive && x.VersionId == "1-001" && x.CommitmentId == 1).Payable);

            Assert.IsFalse(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.Learning && x.VersionId == "1-002" && x.CommitmentId == 1).Payable);
            Assert.IsFalse(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.Completion && x.VersionId == "1-002" && x.CommitmentId == 1).Payable);
            Assert.IsFalse(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.Balancing && x.VersionId == "1-002" && x.CommitmentId == 1).Payable);
            
        }

        [Test]
        public void ThenMultipleValidationErrorsFound()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndPriceMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(9991).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2)
                                                            .WithUln(1000000027)
                                                            .WithStandardCode(11)
                                                            .WithFrameworkCode(299)
                                                            .WithPathwayCode(11)
                                                            .WithProgrammeType(200)
                                                            .Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(6, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPrice));
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingFramework));
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingProgramme));
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPathway));
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingStandard));


        }

        [Test]
        public void ThenLevyPayerFlagWasFalseAndDataLockFailed()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithUln(1000000000).WithStandardCode(999).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build()
            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndMatchFound.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);

            Assert.IsTrue(errors.Any(x => x.RuleId == DataLockErrorCodes.NotLevyPayer));
        }

        [Test]
        [TestCase("1985-001", "1985-002", "1985-003")]
        [TestCase("911", "922", "923")]
        [TestCase("0", "1", "2")]
        public void ThenForAChangeOfPriorityPriceEpisodeMatchesAreAddedWhenMatchesAreFound(string versionId1, string versionId2, string versionId3)

        {
            // Arrange
            _context.Properties[DataLockContextPropertyKeys.YearOfCollection] = "1718";

            var commitments = new[]
            {
                new CommitmentEntityBuilder()
                    .WithCommitmentId(1)
                    .WithVersionId(versionId1)
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2017, 08, 31))
                    .WithPriority(1)
                    .Build(),
                 new CommitmentEntityBuilder()
                    .WithCommitmentId(1)
                    .WithVersionId(versionId2)
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEffectiveFrom(new DateTime(2017, 9, 1))
                    .WithEndDate(new DateTime(2017, 10, 13))
                    .WithPriority(2)
                    .Build(),
                new CommitmentEntityBuilder()
                    .WithCommitmentId(1)
                    .WithVersionId(versionId3)
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEffectiveFrom(new DateTime(2017, 10, 14))
                    .WithPriority(3)
                    .Build(),

            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndCommirmentPriorityChanges.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupCommitmentData(commitments[2]);

            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodeMatches = TestDataHelper.PeriodEndGetPriceEpisodeMatches();
            var period1PeriodMatches = TestDataHelper.GetPriceEpisodePeriodMatchForPeriodEnd(false,1);
            var period2PeriodMatches = TestDataHelper.GetPriceEpisodePeriodMatchForPeriodEnd(false, 2);
            var period3PeriodMatches = TestDataHelper.GetPriceEpisodePeriodMatchForPeriodEnd(false, 3);

            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            Assert.IsNotNull(priceEpisodeMatches);
            Assert.IsNotNull(period1PeriodMatches);
            Assert.IsNotNull(period2PeriodMatches);
            Assert.IsNotNull(period3PeriodMatches);

            Assert.IsTrue(period1PeriodMatches.Where(x => x.VersionId == versionId1 && x.Payable == true).Any());
            Assert.IsTrue(period2PeriodMatches.Where(x => x.VersionId == versionId2 && x.Payable == true).Any());
            Assert.IsTrue(period3PeriodMatches.Where(x => x.VersionId == versionId3 && x.Payable == true).Any());

        }


        [Test]
        public void ThenWhereOnProgrammeMatchesToDifferentVersionForDifferentPeriods()
        {
            // Arrange
            _context.Properties[DataLockContextPropertyKeys.YearOfCollection] = "1718";

            var commitments = new[]
            {
                new CommitmentEntityBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 08, 01))
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .WithEffectiveTo(new DateTime(2017, 9, 27))
                    .WithVersionId("891")
                    .WithPaymentStatus(UnitTests.Tools.Enums.PaymentStatus.Active)
                    .WithAgreedCost(7500)
                    .Build(),
                new CommitmentEntityBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 08, 01))
                    .WithEffectiveFrom(new DateTime(2017, 9, 28))
                    .WithEffectiveTo(new DateTime(2017, 9, 28))
                    .WithVersionId("52947")
                    .WithPaymentStatus(UnitTests.Tools.Enums.PaymentStatus.Active)
                    .WithAgreedCost(7500)
                    .Build()
                    ,
                new CommitmentEntityBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2018, 08, 01))
                    .WithEffectiveFrom(new DateTime(2017, 9, 29))
                    .WithVersionId("54450")
                    .WithPaymentStatus(UnitTests.Tools.Enums.PaymentStatus.Active)
                    .WithAgreedCost(7500)
                    .Build()

            };

            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerFirstIncentiveThreshhold.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupCommitmentData(commitments[2]);
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.PeriodEndCopyReferenceData();
            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            var priceEpisodePayable = TestDataHelper.GetPriceEpisodePeriodMatchForPeriodEnd(false, 1);
            Assert.IsTrue(priceEpisodePayable.Length > 0);
            Assert.IsTrue(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.Learning && x.VersionId == "891" && x.CommitmentId == 1).Payable);

            priceEpisodePayable = TestDataHelper.GetPriceEpisodePeriodMatchForPeriodEnd(false, 2);
            Assert.IsTrue(priceEpisodePayable.Length > 0);
            Assert.IsTrue(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.Learning && x.VersionId == "54450" && x.CommitmentId == 1).Payable);

            priceEpisodePayable = TestDataHelper.GetPriceEpisodePeriodMatchForPeriodEnd(false, 3);
            Assert.IsTrue(priceEpisodePayable.Length > 0);
            Assert.IsTrue(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.Learning && x.VersionId == "54450" && x.CommitmentId == 1).Payable);
            Assert.IsTrue(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.First16To18EmployerIncentive && x.VersionId == "54450" && x.CommitmentId == 1).Payable);
            Assert.IsTrue(priceEpisodePayable.Single(x => x.TransactionType == Payments.DCFS.Domain.TransactionType.First16To18ProviderIncentive && x.VersionId == "54450" && x.CommitmentId == 1).Payable);


        }

    }
}