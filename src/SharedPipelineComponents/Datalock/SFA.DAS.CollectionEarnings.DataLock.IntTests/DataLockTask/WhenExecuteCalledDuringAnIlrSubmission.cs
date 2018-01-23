using System;
using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Enums;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.DataLockTask
{
    public class WhenExecuteCalledDuringAnIlrSubmission
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.SubmissionConnectionString;

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
            TestDataHelper.AddCollectionPeriod();

            _task = new DataLock.DataLockTask();

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString},
                    {ContextPropertyKeys.LogLevel, "Trace"},
                    { DataLockContextPropertyKeys.YearOfCollection, "1617" }
                }
            };
        }

        private void SetupCommitmentData(CommitmentEntity commitment)
        {
            TestDataHelper.AddCommitment(commitment);
        }

        private void SetupAccountData(DasAccount account)
        {
            TestDataHelper.AddDasAccount(account);
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUkprn()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionUkprnMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().Withukprn(999).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUkprn && e.PriceEpisodeIdentifier == "27-25-2016-09-01"));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingUln()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionUlnMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingUln));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.IsEmpty(priceEpisodeMatches);
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingStandard()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionStandardMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).WithProgrammeType(null).WithFrameworkCode(null).WithPathwayCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingStandard));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingFramework()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionFrameworkMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithFrameworkCode(999).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingFramework));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingProgramme()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionProgrammeMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithProgrammeType(999).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingProgramme));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorAddedForMismatchingPathway()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionPathwayMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(null).WithPathwayCode(999).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPathway));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(1, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorsAddedForMismatchingPrice()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionPriceMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MismatchingPrice));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(2, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorsAddedForIlrEarlierStartDate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionEarlierStartDateMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.EarlierStartDate));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(2, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationErrorsAddedForMultipleMatchingCommitments()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionMultipleMatches.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(3).WithStandardCode(999).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(4).WithUln(1000000027).WithStandardCode(null).Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(2, errors.Count(e => e.RuleId == DataLockErrorCodes.MultipleMatches));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();

            Assert.AreEqual(4, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenValidationDataLock08ErrorAddedForMultipleMatchingCommitments()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionDLock08Error.sql");

            var baseBuilder = new CommitmentEntityBuilder()
                   .Withukprn(10007459)
                   .WithUln(8628555861)
                   .WithStartDate(new DateTime(2017, 6, 1))
                   .WithEndDate(new DateTime(2018, 06, 01));

            SetupCommitmentData(
                new CommitmentEntityBuilder(baseBuilder)
                    .WithCommitmentId(13458)
                    .WithVersionId("57594")
                    .WithAgreedCost(1500)
                    .WithStandardCode(0)
                    .WithProgrammeType(3)
                    .WithFrameworkCode(488)
                    .WithPathwayCode(1)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithPriority(0)
                    .WithEffectiveFrom(new DateTime(2017, 6, 1))
                    .WithEffectiveTo(new DateTime(2017, 7, 02))
                    .Build());

            SetupCommitmentData(
                new CommitmentEntityBuilder(baseBuilder)
                    .WithCommitmentId(13458)
                    .WithVersionId("57600")
                    .WithAgreedCost(1500)
                    .WithStandardCode(0)
                    .WithProgrammeType(3)
                    .WithFrameworkCode(488)
                    .WithPathwayCode(1)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithPriority(11)
                    .WithEffectiveFrom(new DateTime(2017, 7, 3))
                    .WithEffectiveTo(new DateTime(2017, 5, 31))
                    .Build());

            SetupCommitmentData(
                new CommitmentEntityBuilder(baseBuilder)
                    .WithCommitmentId(13458)
                    .WithVersionId("80959")
                    .WithAgreedCost(1500)
                    .WithStandardCode(0)
                    .WithProgrammeType(3)
                    .WithFrameworkCode(488)
                    .WithPathwayCode(1)
                    .WithPaymentStatus(PaymentStatus.Cancelled)
                    .WithPriority(11)
                    .WithEffectiveFrom(new DateTime(2017, 06, 01))
                    .WithEffectiveTo(null)
                    .Build());

            SetupCommitmentData(
                new CommitmentEntityBuilder(baseBuilder)
                    .WithCommitmentId(17224)
                    .WithVersionId("85900")
                    .WithAgreedCost(4000)
                    .WithStandardCode(122)
                    .WithProgrammeType(0)
                    .WithFrameworkCode(0)
                    .WithPathwayCode(1)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithPriority(0)
                    .WithEffectiveFrom(new DateTime(2017, 06, 01))
                    .WithEffectiveTo(new DateTime(2017, 07, 10))
                    .Build());

            SetupCommitmentData(
                new CommitmentEntityBuilder(baseBuilder)
                    .WithCommitmentId(17224)
                    .WithVersionId("85906")
                    .WithAgreedCost(4000)
                    .WithStandardCode(122)
                    .WithProgrammeType(0)
                    .WithFrameworkCode(0)
                    .WithPathwayCode(1)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithPriority(17)
                    .WithEffectiveFrom(new DateTime(2017, 07, 11))
                    .WithEffectiveTo(null)
                    .Build());

            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Length);
            Assert.AreEqual(1, errors.Count(e => e.RuleId == DataLockErrorCodes.MultipleMatches));

            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();
            Assert.AreEqual(2, priceEpisodeMatches.Count());
            Assert.IsFalse(priceEpisodeMatches.Any(x => x.IsSuccess));
        }

        [Test]
        public void ThenCommitmentsWhichAreStartedAndEndedOnTheSameDayAreIgnored()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionDLock08Error.sql");

            const long ukprn = 10007459L;

            var baseBuilder = new CommitmentEntityBuilder()
                .Withukprn(ukprn)
                .WithUln(8628555861)
                .WithFrameworkCode(550)
                .WithPathwayCode(1);

            SetupCommitmentData(
                new CommitmentEntityBuilder(baseBuilder)
                    .WithStartDate(new DateTime(2017, 06, 01))
                    .WithEndDate(new DateTime(2017, 06, 01))
                    .WithCommitmentId(13458)
                    .WithVersionId("80959")
                    .WithAgreedCost(1500)
                    .WithStandardCode(0)
                    .WithProgrammeType(3)
                    .WithPaymentStatus(PaymentStatus.Cancelled)
                    .WithPriority(11)
                    .WithEffectiveFrom(new DateTime(2017, 06, 01))
                    .WithEffectiveTo(new DateTime(2017, 06, 01))
                    .Build());

            SetupCommitmentData(
                new CommitmentEntityBuilder(baseBuilder)
                    .WithStartDate(new DateTime(2017, 06, 01))
                    .WithEndDate(new DateTime(2018, 06, 01))
                    .WithCommitmentId(17224)
                    .WithVersionId("85900")
                    .WithAgreedCost(4000)
                    .WithStandardCode(122)
                    .WithProgrammeType(0)
                    .WithPaymentStatus(PaymentStatus.Active)
                    .WithPriority(0)
                    .WithEffectiveFrom(new DateTime(2017, 06, 01))
                    .WithEffectiveTo(null)
                    .Build());

            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var providerCommitmentMatches = TestDataHelper.GetProviderCommitmentsForIlrSubmission(ukprn);
            Assert.AreEqual(1, providerCommitmentMatches.Count());
        }

        [Test]
        public void ThenPriceEpisodeMatchesAddedForMatchFound()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithStandardCode(999).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build()
            };

            TestDataHelper.ExecuteScript("IlrSubmissionMatchFound.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();
            var priceEpisodePeriodMatches = TestDataHelper.GetPriceEpisodePeriodMatches();
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            Assert.IsNotNull(priceEpisodeMatches);
            Assert.AreEqual(2, priceEpisodeMatches.Length);
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-17"));
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-19"));

            Assert.IsNotNull(priceEpisodePeriodMatches);
            Assert.AreEqual(22, priceEpisodePeriodMatches.Length);
            Assert.AreEqual(11, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-17"));
            Assert.AreEqual(11, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-19"));
        }

        [Test]
        public void ThenForAChangeOfEmployersMatchingLearnerAndCommitmentsAreAddedWhenMatchesAreFound()
        {
            // Arrange
            _context.Properties[DataLockContextPropertyKeys.YearOfCollection] = "1718";

            var commitments = new[]
            {
                new CommitmentEntityBuilder()
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 8, 1))
                    .WithEndDate(new DateTime(2017, 10, 31))
                    .WithEffectiveFrom(new DateTime(2017, 8, 1))
                    .Build(),
                new CommitmentEntityBuilder()
                    .WithCommitmentId(2)
                    .WithStandardCode(27)
                    .WithStartDate(new DateTime(2017, 11, 1))
                    .WithEndDate(new DateTime(2018, 8, 31))
                    .WithEffectiveFrom(new DateTime(2017, 11, 1))
                    .WithAgreedCost(5625m)
                    .Build()
            };



            TestDataHelper.ExecuteScript("IlrSubmissionLearnerChangesEmployers.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);

            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches().Where(x => x.IsSuccess);
            var priceEpisodePeriodMatches = TestDataHelper.GetPriceEpisodePeriodMatches();
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            Assert.IsNotNull(priceEpisodeMatches);
            Assert.AreEqual(2, priceEpisodeMatches.Count());
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-08-01"));
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-11-01"));

            Assert.AreEqual(10, priceEpisodePeriodMatches.Length);
            Assert.AreEqual(3, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-08-01"));
            Assert.AreEqual(7, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[1].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2017-11-01"));
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

            TestDataHelper.ExecuteScript("IlrSubmissionLearnerChangesProvider.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            var priceEpisodePayable = TestDataHelper.GetPriceEpisodePeriodMatchForPeriod(false, 4);
            Assert.IsTrue(priceEpisodePayable.Length > 0);
            Assert.IsTrue(priceEpisodePayable.Single(x => x.TransactionTypesFlag == Payments.DCFS.Domain.TransactionTypesFlag.FirstEmployerProviderIncentives).Payable);
            Assert.IsFalse(priceEpisodePayable.Single(x => x.TransactionTypesFlag == Payments.DCFS.Domain.TransactionTypesFlag.AllLearning).Payable);

        }

        [Test]
        public void ThenMultipleValidationErrorsFound()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionPriceMismatch.sql");
            SetupCommitmentData(new CommitmentEntityBuilder().WithStandardCode(9991).Build());
            SetupCommitmentData(new CommitmentEntityBuilder().WithCommitmentId(2)
                                                                     .WithUln(1000000027)
                                                                     .WithStandardCode(11)
                                                                     .WithFrameworkCode(299)
                                                                     .WithPathwayCode(11)
                                                                     .WithProgrammeType(200)
                                                                     .Build());
            SetupAccountData(new DasAccountBuilder().Build());

            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

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
                new CommitmentEntityBuilder().WithStandardCode(999).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build()
            };

            TestDataHelper.ExecuteScript("IlrSubmissionMatchFound.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);

            Assert.IsTrue(errors.Any(x => x.RuleId == DataLockErrorCodes.NotLevyPayer));

        }

        [Test]
        public void ThenTheLatestCommitmentVersionIsMatchedWhenMultipleVersionEffectiveAtSameTimeAndNoDataLockErrors()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithCommitmentId(1).WithVersionId("1-001").WithStandardCode(999).WithEffectiveFrom(new DateTime(2016, 9, 1)).WithEffectiveTo(new DateTime(2016, 8, 31)).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(1).WithVersionId("1-002").WithStandardCode(999).WithEffectiveFrom(new DateTime(2016, 9, 1)).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithStandardCode(null).Build()
            };

            TestDataHelper.ExecuteScript("IlrSubmissionMatchFound.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupCommitmentData(commitments[2]);
            SetupAccountData(new DasAccountBuilder().Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();
            var priceEpisodePeriodMatches = TestDataHelper.GetPriceEpisodePeriodMatches();
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);

            Assert.IsNotNull(priceEpisodeMatches);
            Assert.AreEqual(2, priceEpisodeMatches.Length);
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-17"));

            Assert.IsNotNull(priceEpisodePeriodMatches);
            Assert.AreEqual(22, priceEpisodePeriodMatches.Length);
            Assert.AreEqual(11, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId && l.PriceEpisodeIdentifier == "27-25-2016-09-17"));
        }

        [Test]
        public void ThenTheLatestCommitmentVersionIsMatchedWhenMultipleVersionEffectiveAtSameTimeAndHasDataLockErrors()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().WithCommitmentId(1).WithVersionId("1-001").WithStandardCode(999).WithEffectiveFrom(new DateTime(2016, 9, 1)).WithEffectiveTo(new DateTime(2016, 8, 31)).Build(),
                new CommitmentEntityBuilder().WithCommitmentId(1).WithVersionId("1-002").WithStandardCode(999).WithEffectiveFrom(new DateTime(2016, 9, 1)).Build()
            };

            TestDataHelper.ExecuteScript("IlrSubmissionSingleLearnerMismatchPriceAndStandard.sql");
            SetupCommitmentData(commitments[0]);
            SetupCommitmentData(commitments[1]);
            SetupAccountData(new DasAccountBuilder().Build());
            TestDataHelper.CopyReferenceData();

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodeMatches = TestDataHelper.GetPriceEpisodeMatches();
            var priceEpisodePeriodMatches = TestDataHelper.GetPriceEpisodePeriodMatches();
            var errors = TestDataHelper.GetValidationErrors();

            Assert.IsNotNull(errors);
            Assert.AreEqual(2, errors.Length);
            Assert.AreEqual(1, errors.Count(x => x.RuleId == DataLockErrorCodes.MismatchingStandard));
            Assert.AreEqual(1, errors.Count(x => x.RuleId == DataLockErrorCodes.MismatchingPrice));

            Assert.IsNotNull(priceEpisodeMatches);
            Assert.AreEqual(1, priceEpisodeMatches.Length);
            Assert.AreEqual(1, priceEpisodeMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId));

            Assert.IsNotNull(priceEpisodePeriodMatches);
            Assert.AreEqual(11, priceEpisodePeriodMatches.Length);
            Assert.AreEqual(11, priceEpisodePeriodMatches.Count(l => l.CommitmentId == commitments[0].CommitmentId));
        }
    }
}
