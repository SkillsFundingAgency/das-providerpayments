using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenMakingDuePayments
    {
        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        [TestCase(25L, null, null, null)]
        [TestCase(null, 550, 20, 6)]
        public void ThenItShouldWriteCorrectDetailsForPaymentsDue(long? standardCode, int? frameworkCode, int? programmeType, int? pathwayCode)
        {
            // Arrange
            var ukprn = 863145;
            var uln = 834734;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, uln: uln, startDate: startDate, endDate: plannedEndDate, standardCode: standardCode, frameworkCode: frameworkCode, programmeType: programmeType, pathwayCode: pathwayCode);

            TestDataHelper.SetOpenCollection(1);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(1, duePayments.Length);

            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.AreEqual("1", duePayments[0].CommitmentVersionId);
            Assert.AreEqual("123", duePayments[0].AccountId);
            Assert.AreEqual("20170401", duePayments[0].AccountVersionId);
            Assert.AreEqual(uln, duePayments[0].Uln);
            Assert.AreEqual(learnerRefNumber, duePayments[0].LearnRefNumber);
            Assert.AreEqual(1, duePayments[0].AimSeqNumber);
            Assert.AreEqual(ukprn, duePayments[0].Ukprn);
            Assert.AreEqual(DateTime.Today, duePayments[0].IlrSubmissionDateTime);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);
            Assert.AreEqual(1000, duePayments[0].AmountDue);

            Assert.AreEqual(standardCode, duePayments[0].StandardCode);
            Assert.AreEqual(frameworkCode, duePayments[0].FrameworkCode);
            Assert.AreEqual(programmeType, duePayments[0].ProgrammeType);
            Assert.AreEqual(pathwayCode, duePayments[0].PathwayCode);

            Assert.AreEqual(0.9m, duePayments[0].SfaContributionPercentage);
            Assert.AreEqual("Levy Funding Line", duePayments[0].FundingLineType);
        }

        [Test]
        public void ThenItShouldMakePaymentsForEachPeriodUptoAndIncludingTheCurrent()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual(1000, duePayments[0].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[1].CommitmentId);
            Assert.AreEqual(9, duePayments[1].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[1].DeliveryYear);
            Assert.AreEqual(1000, duePayments[1].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[1].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[2].CommitmentId);
            Assert.AreEqual(10, duePayments[2].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[2].DeliveryYear);
            Assert.AreEqual(1000, duePayments[2].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[2].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[3].CommitmentId);
            Assert.AreEqual(11, duePayments[3].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[3].DeliveryYear);
            Assert.AreEqual(1000, duePayments[3].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[3].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[4].CommitmentId);
            Assert.AreEqual(12, duePayments[4].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[4].DeliveryYear);
            Assert.AreEqual(1000, duePayments[4].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[4].TransactionType);
        }

        [Test]
        public void ThenItShouldNotMakePaymentsForPeriodWhereEarningHasBeenFullyPaidAlready()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(4, duePayments.Length);
            Assert.False(duePayments.Any(p => p.DeliveryMonth == 8 && p.DeliveryYear == 2016));
        }

        [Test]
        public void ThenItShouldMakePartPaymentsForPeriodWhereEarningHasBeenPartiallyPaidAlready()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 500, learnRefNumber: learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            var actualPayment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 8 && p.DeliveryYear == 2016);
            Assert.IsNotNull(actualPayment);
            Assert.AreEqual(500m, actualPayment.AmountDue);
        }

        [Test]
        public void ThenItShouldMakePaymentsForLearningAndCompletionInFinalMonth()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 12);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            var period12Payments = duePayments.Where(p => p.DeliveryMonth == 7 && p.DeliveryYear == 2017)
                                              .OrderBy(p => p.TransactionType)
                                              .ToArray();
            Assert.AreEqual(2, period12Payments.Length);

            Assert.AreEqual((int)TransactionType.Learning, period12Payments[0].TransactionType);
            Assert.AreEqual(1000, period12Payments[0].AmountDue);

            Assert.AreEqual((int)TransactionType.Completion, period12Payments[1].TransactionType);
            Assert.AreEqual(3000, period12Payments[1].AmountDue);
        }

        [Test]
        public void ThenItShouldNotMakePaymentsIfDoesNotPassDataLock()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, passedDataLock: false);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(0, duePayments.Length);
        }

        [Test]
        public void ThenItShouldMakePaymentsForLearningCompletionAndBalancingForAnEarlyCompletion()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(10);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 10, earlyFinisher: true);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            var period10Payments = duePayments.Where(p => p.DeliveryMonth == 5 && p.DeliveryYear == 2017)
                                              .OrderBy(p => p.TransactionType)
                                              .ToArray();
            Assert.AreEqual(3, period10Payments.Length);

            Assert.AreEqual((int)TransactionType.Learning, period10Payments[0].TransactionType);
            Assert.AreEqual(1000, period10Payments[0].AmountDue);

            Assert.AreEqual((int)TransactionType.Completion, period10Payments[1].TransactionType);
            Assert.AreEqual(3000, period10Payments[1].AmountDue);

            Assert.AreEqual((int)TransactionType.Balancing, period10Payments[2].TransactionType);
            Assert.AreEqual(2000, period10Payments[2].AmountDue);
        }

        [Test]
        public void ThenItShouldMake16To18FirstIncentivePayments()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 06);
            var plannedEndDate = new DateTime(2017, 8, 08);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate,
                            transactionTypes: new[] { TransactionType.First16To18EmployerIncentive, TransactionType.First16To18ProviderIncentive });

            TestDataHelper.SetOpenCollection(4);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 4);

            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 4, "PriceEpisodeFirstEmp1618Pay");
            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 4, "PriceEpisodeFirstProv1618Pay");

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            var period4Payments = duePayments.Where(p => p.DeliveryMonth == 11 && p.DeliveryYear == 2016).ToArray();


            var employerIncentive = period4Payments.Single(x => x.TransactionType == (int)TransactionType.First16To18EmployerIncentive);
            var providerIncentive = period4Payments.Single(x => x.TransactionType == (int)TransactionType.First16To18ProviderIncentive);


            Assert.NotNull(employerIncentive);
            Assert.AreEqual(500, employerIncentive.AmountDue);

            Assert.NotNull(providerIncentive);
            Assert.AreEqual(500, providerIncentive.AmountDue);
        }

        [Test]
        public void ThenItShouldMake16To18SecondIncentivePayments()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 06);
            var plannedEndDate = new DateTime(2017, 8, 08);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate,
                transactionTypes: new[] { TransactionType.Second16To18EmployerIncentive, TransactionType.Second16To18ProviderIncentive });

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 12);

            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 12, "PriceEpisodeSecondEmp1618Pay");
            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 12, "PriceEpisodeSecondProv1618Pay");

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            var period4Payments = duePayments.Where(p => p.DeliveryMonth == 07 && p.DeliveryYear == 2017).ToArray();


            var employerIncentive = period4Payments.Single(x => x.TransactionType == (int)TransactionType.Second16To18EmployerIncentive);
            var providerIncentive = period4Payments.Single(x => x.TransactionType == (int)TransactionType.Second16To18ProviderIncentive);


            Assert.NotNull(employerIncentive);
            Assert.AreEqual(500, employerIncentive.AmountDue);

            Assert.NotNull(providerIncentive);
            Assert.AreEqual(500, providerIncentive.AmountDue);
        }

        [Test]
        public void ThenItShouldNotMakePaymentsForThePeriodsThatTheDataLockFlaggedAsNonPayable()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, notPayablePeriods: new[] { 2, 3, 4 });

            TestDataHelper.SetOpenCollection(6);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 6);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(3, duePayments.Length);

            var period2Payment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 9 && p.DeliveryYear == 2016);
            Assert.IsNull(period2Payment);

            var period3Payment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 10 && p.DeliveryYear == 2016);
            Assert.IsNull(period3Payment);

            var period4Payment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 11 && p.DeliveryYear == 2016);
            Assert.IsNull(period4Payment);
        }

        [Test]
        public void ThenItShouldNotMakePaymentsForPeriodEarningsThatTheDataLockFoundNoMatch()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, notMatchedPeriods: new[] { 2, 3, 4 });

            TestDataHelper.SetOpenCollection(6);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 6);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(3, duePayments.Length);

            var period2Payment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 9 && p.DeliveryYear == 2016);
            Assert.IsNull(period2Payment);

            var period3Payment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 10 && p.DeliveryYear == 2016);
            Assert.IsNull(period3Payment);

            var period4Payment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 11 && p.DeliveryYear == 2016);
            Assert.IsNull(period4Payment);
        }

        [Test]
        public void ThenItShouldMakeOnProgrammeMathsAndEnglishPaymentsForEachPeriodUptoAndIncludingTheCurrent()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 7, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate,
                transactionTypes: new[] { TransactionType.OnProgrammeMathsAndEnglish, TransactionType.BalancingMathsAndEnglish });

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);
            TestDataHelper.AddMathsAndEnglishEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual(39, duePayments[0].AmountDue);
            Assert.AreEqual((int)TransactionType.OnProgrammeMathsAndEnglish, duePayments[0].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[1].CommitmentId);
            Assert.AreEqual(9, duePayments[1].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[1].DeliveryYear);
            Assert.AreEqual(39, duePayments[1].AmountDue);
            Assert.AreEqual((int)TransactionType.OnProgrammeMathsAndEnglish, duePayments[1].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[2].CommitmentId);
            Assert.AreEqual(10, duePayments[2].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[2].DeliveryYear);
            Assert.AreEqual(39, duePayments[2].AmountDue);
            Assert.AreEqual((int)TransactionType.OnProgrammeMathsAndEnglish, duePayments[2].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[3].CommitmentId);
            Assert.AreEqual(11, duePayments[3].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[3].DeliveryYear);
            Assert.AreEqual(39, duePayments[3].AmountDue);
            Assert.AreEqual((int)TransactionType.OnProgrammeMathsAndEnglish, duePayments[3].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[4].CommitmentId);
            Assert.AreEqual(12, duePayments[4].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[4].DeliveryYear);
            Assert.AreEqual(39, duePayments[4].AmountDue);
            Assert.AreEqual((int)TransactionType.OnProgrammeMathsAndEnglish, duePayments[4].TransactionType);
        }

        [Test]
        public void ThenItShouldMakeOnProgrammeAndBalancingMathsAndEnglishPaymentsForEachPeriodUptoAndIncludingTheCurrent()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 7, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate,
                transactionTypes: new[] { TransactionType.OnProgrammeMathsAndEnglish, TransactionType.BalancingMathsAndEnglish });

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5, earlyFinisher: true);
            TestDataHelper.AddMathsAndEnglishEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5, earlyFinisher: true);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(6, duePayments.Length);

            Assert.AreEqual(5, duePayments.Count(p => p.TransactionType == (int)TransactionType.OnProgrammeMathsAndEnglish && p.AmountDue == 39));
            Assert.AreEqual(1, duePayments.Count(p => p.TransactionType == (int)TransactionType.BalancingMathsAndEnglish && p.AmountDue == 274.75m));
        }

        [Test]
        public void ThenItShouldMakeOnProgrammeAndBalancingMathsAndEnglishPaymentsByCombiningAllAimSequencesForAGivenMonth()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2018, 9, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);


            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate,
                transactionTypes: new[] { TransactionType.OnProgrammeMathsAndEnglish, TransactionType.BalancingMathsAndEnglish });

            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2016, 13, 19, learnerRefNumber, 5, "50086832");

            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2016, 14, 100, learnerRefNumber, 5, "50086832");


            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5, earlyFinisher: true);

            TestDataHelper.AddMathsAndEnglishEarningForCommitment(commitmentId, learnerRefNumber, 2, currentPeriod: 5, earlyFinisher: true);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(6, duePayments.Length);

            Assert.AreEqual(1, duePayments.Count(p => p.DeliveryMonth == 10 && p.DeliveryYear == 2016 && p.TransactionType == (int)TransactionType.OnProgrammeMathsAndEnglish && p.AmountDue == 20));
            Assert.AreEqual(1, duePayments.Count(p => p.DeliveryMonth == 12 && p.DeliveryYear == 2016 && p.TransactionType == (int)TransactionType.OnProgrammeMathsAndEnglish && p.AmountDue == 39));
            Assert.AreEqual(1, duePayments.Count(p => p.DeliveryMonth == 12 && p.DeliveryYear == 2016 && p.TransactionType == (int)TransactionType.BalancingMathsAndEnglish && p.AmountDue == 174.75m));

        }

        [Test]
        public void ThenItShouldMakeOnProgrammeMathsAndEnglishPaymentsForEachPeriodWhichGoesbeyondMainAim()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 4, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate,
                transactionTypes: new[] { TransactionType.OnProgrammeMathsAndEnglish, TransactionType.BalancingMathsAndEnglish });

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber);
            TestDataHelper.AddMathsAndEnglishEarningForCommitment(commitmentId, learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(12, duePayments.Length);

            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.Greater(duePayments.Single(x => x.DeliveryMonth == 10 && x.TransactionType == (int)TransactionType.OnProgrammeMathsAndEnglish).AmountDue, 0);
            Assert.Greater(duePayments.Single(x => x.DeliveryMonth == 11 && x.TransactionType == (int)TransactionType.OnProgrammeMathsAndEnglish).AmountDue, 0);
            Assert.Greater(duePayments.Single(x => x.DeliveryMonth == 12 && x.TransactionType == (int)TransactionType.OnProgrammeMathsAndEnglish).AmountDue, 0);

        }

        [Test]
        public void ThenItShouldMakeRefundPaymentsForPeriodWhereEarningsNeedToBeRefunded()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber);

            TestDataHelper.SetOpenCollection(6);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);
            TestDataHelper.ClearApprenticeshipPriceEpisodePeriod();

            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 1, 1000);
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 2, 1000);
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 3, 1000);
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 4, 0);
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 5, 0);
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 6, 0);


            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(2, duePayments.Length);
            Assert.True(duePayments.Any(p => p.DeliveryMonth == 11 && p.DeliveryYear == 2016 && p.AmountDue == (decimal)-1000.00));
            Assert.True(duePayments.Any(p => p.DeliveryMonth == 12 && p.DeliveryYear == 2016 && p.AmountDue == (decimal)-1000.00));

        }

        [Test]
        public void ThenItShouldRefundPreviousPeriodIfPriceChangesRetrospectively()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2017, 6, 12);
            var plannedEndDate = new DateTime(2018, 6, 27);
            var learnerRefNumber = "1"; //Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 6, 2017, (int)TransactionType.Learning, 210.52632m, accountVersionId: "20170401");

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 12);
            TestDataHelper.ClearApprenticeshipPriceEpisodePeriod();

            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 11, 190.47619m);  //06/17
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 12, 190.47619m);  //07/17


            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn).OrderBy(p => p.DeliveryMonth).ToArray();
            Assert.AreEqual(2, duePayments.Length);

            var assertionFunc = new Func<RequiredPaymentEntity, int, decimal, bool>((actualPaymentDue, expectedMonth, expectedAmount) =>
            {
                Assert.AreEqual("123", actualPaymentDue.AccountId);
                Assert.AreEqual("20170401", actualPaymentDue.AccountVersionId);
                Assert.AreEqual(1, actualPaymentDue.AimSeqNumber);
                Assert.AreEqual(expectedAmount, actualPaymentDue.AmountDue);
                Assert.AreEqual(1, actualPaymentDue.ApprenticeshipContractType);
                Assert.AreEqual(commitmentId, actualPaymentDue.CommitmentId);
                Assert.AreEqual("1", actualPaymentDue.CommitmentVersionId);
                Assert.AreEqual(expectedMonth, actualPaymentDue.DeliveryMonth);
                Assert.AreEqual(2017, actualPaymentDue.DeliveryYear);
                Assert.IsNull(actualPaymentDue.FrameworkCode);
                Assert.AreEqual("Levy Funding Line", actualPaymentDue.FundingLineType);
                Assert.AreEqual(DateTime.Today, actualPaymentDue.IlrSubmissionDateTime.Date);
                Assert.AreEqual("ZPROG001", actualPaymentDue.LearnAimRef);
                Assert.AreEqual(startDate, actualPaymentDue.LearningStartDate);
                Assert.AreEqual(learnerRefNumber, actualPaymentDue.LearnRefNumber);
                Assert.IsNull(actualPaymentDue.PathwayCode);
                Assert.AreEqual("99-99-99-2017-06-12", actualPaymentDue.PriceEpisodeIdentifier);
                Assert.IsNull(actualPaymentDue.ProgrammeType);
                Assert.AreEqual(0.9m, actualPaymentDue.SfaContributionPercentage);
                Assert.AreEqual(123456, actualPaymentDue.StandardCode);
                Assert.AreEqual(1, actualPaymentDue.TransactionType);
                Assert.AreEqual(ukprn, actualPaymentDue.Ukprn);
                Assert.AreEqual(uln, actualPaymentDue.Uln);
                Assert.AreEqual(false, actualPaymentDue.UseLevyBalance);
                return true;
            });

            Assert.IsTrue(assertionFunc(duePayments[0], 6, -20.05013m));
            Assert.IsTrue(assertionFunc(duePayments[1], 7, 190.47619m));
        }

        [Test]
        public void ThenItShouldMakeRefundPaymentsIfEarningIsNegativeByRefundingPreviousPeriodsToRequiredAmount()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = "1"; //Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2016, (int)TransactionType.Learning, 100);

            TestDataHelper.SetOpenCollection(6);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);
            TestDataHelper.ClearApprenticeshipPriceEpisodePeriod();

            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 1, 100);  //08/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 2, 100);  //09/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 3, 100);  //10/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 4, 100);  //11/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 5, 100);  //12/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 6, -250); //01/17


            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn).OrderBy(p => p.DeliveryMonth).ToArray();
            Assert.AreEqual(3, duePayments.Length);

            var assertionFunc = new Func<RequiredPaymentEntity, int, decimal, bool>((actualPaymentDue, expectedMonth, expectedAmount) =>
            {
                Assert.AreEqual("123", actualPaymentDue.AccountId);
                Assert.AreEqual("20170401", actualPaymentDue.AccountVersionId);
                Assert.AreEqual(1, actualPaymentDue.AimSeqNumber);
                Assert.AreEqual(expectedAmount, actualPaymentDue.AmountDue);
                Assert.AreEqual(1, actualPaymentDue.ApprenticeshipContractType);
                Assert.AreEqual(commitmentId, actualPaymentDue.CommitmentId);
                Assert.AreEqual("1", actualPaymentDue.CommitmentVersionId);
                Assert.AreEqual(expectedMonth, actualPaymentDue.DeliveryMonth);
                Assert.AreEqual(2016, actualPaymentDue.DeliveryYear);
                Assert.IsNull(actualPaymentDue.FrameworkCode);
                Assert.AreEqual("Levy Funding Line", actualPaymentDue.FundingLineType);
                Assert.AreEqual(DateTime.Today, actualPaymentDue.IlrSubmissionDateTime.Date);
                Assert.AreEqual("ZPROG001", actualPaymentDue.LearnAimRef);
                Assert.AreEqual(startDate, actualPaymentDue.LearningStartDate);
                Assert.AreEqual(learnerRefNumber, actualPaymentDue.LearnRefNumber);
                Assert.IsNull(actualPaymentDue.PathwayCode);
                Assert.AreEqual("99-99-99-2016-08-12", actualPaymentDue.PriceEpisodeIdentifier);
                Assert.IsNull(actualPaymentDue.ProgrammeType);
                Assert.AreEqual(0.9m, actualPaymentDue.SfaContributionPercentage);
                Assert.AreEqual(123456, actualPaymentDue.StandardCode);
                Assert.AreEqual(1, actualPaymentDue.TransactionType);
                Assert.AreEqual(ukprn, actualPaymentDue.Ukprn);
                Assert.AreEqual(uln, actualPaymentDue.Uln);
                Assert.AreEqual(false, actualPaymentDue.UseLevyBalance);
                return true;
            });

            Assert.IsTrue(assertionFunc(duePayments[0], 10, -50m));
            Assert.IsTrue(assertionFunc(duePayments[1], 11, -100m));
            Assert.IsTrue(assertionFunc(duePayments[2], 12, -100m));
        }

        [Test]
        public void ThenItShouldRefundAnyPeriodsAfterARetrospectiveNegativeEarningIsProduced()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = "1"; //Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 100, accountVersionId: "20170401");
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2016, (int)TransactionType.Learning, 100, accountVersionId: "20170401");
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2016, (int)TransactionType.Learning, 100, accountVersionId: "20170401");
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2016, (int)TransactionType.Learning, 100, accountVersionId: "20170401");
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2016, (int)TransactionType.Learning, 100, accountVersionId: "20170401");

            TestDataHelper.SetOpenCollection(6);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);
            TestDataHelper.ClearApprenticeshipPriceEpisodePeriod();

            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 1, 100);  //08/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 2, 100);  //09/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 3, 100);  //10/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 4, -250);  //11/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 5, 0);  //12/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 6, 0); //01/17


            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn).OrderBy(p => p.DeliveryMonth).ToArray();
            Assert.AreEqual(5, duePayments.Length);

            var assertionFunc = new Func<RequiredPaymentEntity, int, decimal, bool>((actualPaymentDue, expectedMonth, expectedAmount) =>
            {
                Assert.AreEqual("123", actualPaymentDue.AccountId);
                Assert.AreEqual("20170401", actualPaymentDue.AccountVersionId);
                Assert.AreEqual(1, actualPaymentDue.AimSeqNumber);
                Assert.AreEqual(expectedAmount, actualPaymentDue.AmountDue);
                Assert.AreEqual(1, actualPaymentDue.ApprenticeshipContractType);
                Assert.AreEqual(commitmentId, actualPaymentDue.CommitmentId);
                Assert.AreEqual("1", actualPaymentDue.CommitmentVersionId);
                Assert.AreEqual(expectedMonth, actualPaymentDue.DeliveryMonth);
                Assert.AreEqual(2016, actualPaymentDue.DeliveryYear);
                Assert.IsNull(actualPaymentDue.FrameworkCode);
                Assert.AreEqual("Levy Funding Line", actualPaymentDue.FundingLineType);
                Assert.AreEqual(DateTime.Today, actualPaymentDue.IlrSubmissionDateTime.Date);
                Assert.AreEqual("ZPROG001", actualPaymentDue.LearnAimRef);
                Assert.AreEqual(startDate, actualPaymentDue.LearningStartDate);
                Assert.AreEqual(learnerRefNumber, actualPaymentDue.LearnRefNumber);
                Assert.IsNull(actualPaymentDue.PathwayCode);
                Assert.AreEqual("99-99-99-2016-08-12", actualPaymentDue.PriceEpisodeIdentifier);
                Assert.IsNull(actualPaymentDue.ProgrammeType);
                Assert.AreEqual(0.9m, actualPaymentDue.SfaContributionPercentage);
                Assert.AreEqual(123456, actualPaymentDue.StandardCode);
                Assert.AreEqual(1, actualPaymentDue.TransactionType);
                Assert.AreEqual(ukprn, actualPaymentDue.Ukprn);
                Assert.AreEqual(uln, actualPaymentDue.Uln);
                Assert.AreEqual(false, actualPaymentDue.UseLevyBalance);
                return true;
            });

            Assert.IsTrue(assertionFunc(duePayments[0], 8, -50m));
            Assert.IsTrue(assertionFunc(duePayments[1], 9, -100m));
            Assert.IsTrue(assertionFunc(duePayments[2], 10, -100m));
            Assert.IsTrue(assertionFunc(duePayments[3], 11, -100m));
            Assert.IsTrue(assertionFunc(duePayments[4], 12, -100m));
        }


        [Test]
        public void ThenItShouldNotReverseApportionedRefundsFromPreviousMonths()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = "1"; //Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2016, (int)TransactionType.Learning, -50);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2016, (int)TransactionType.Learning, -100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2016, (int)TransactionType.Learning, 100);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2016, (int)TransactionType.Learning, -100);

            TestDataHelper.SetOpenCollection(7);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);
            TestDataHelper.ClearApprenticeshipPriceEpisodePeriod();

            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 1, 100);  //08/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 2, 100);  //09/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 3, 100);  //10/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 4, 100);  //11/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 5, 100);  //12/16
            TestDataHelper.AddApprenticeEarning(ukprn, startDate, learnerRefNumber, 6, -250); //01/17


            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(0, duePayments.Length);
        }

        [Test]
        public void ThenItShouldWriteCorrectDetailsForPaymentsDueWhenNoHistoryAndNegativeEarnings()
        {
            // Arrange
            var ukprn = 863145;
            var uln = 834734;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, uln: uln,
                                    startDate: startDate, endDate: plannedEndDate,
                                    agreedCost: -15000m);

            TestDataHelper.SetOpenCollection(1);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(1, duePayments.Length);

            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.AreEqual("1", duePayments[0].CommitmentVersionId);
            Assert.AreEqual("123", duePayments[0].AccountId);
            Assert.AreEqual("20170401", duePayments[0].AccountVersionId);
            Assert.AreEqual(uln, duePayments[0].Uln);
            Assert.AreEqual(learnerRefNumber, duePayments[0].LearnRefNumber);
            Assert.AreEqual(1, duePayments[0].AimSeqNumber);
            Assert.AreEqual(ukprn, duePayments[0].Ukprn);
            Assert.AreEqual(DateTime.Today, duePayments[0].IlrSubmissionDateTime);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);
            Assert.AreEqual(-1000, duePayments[0].AmountDue);


            Assert.AreEqual(0.9m, duePayments[0].SfaContributionPercentage);
            Assert.AreEqual("Levy Funding Line", duePayments[0].FundingLineType);
        }

    }
}