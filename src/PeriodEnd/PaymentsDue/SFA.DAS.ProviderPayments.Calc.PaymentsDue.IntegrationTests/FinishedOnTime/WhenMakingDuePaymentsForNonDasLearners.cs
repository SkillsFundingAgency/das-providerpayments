using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenMakingDuePaymentsForNonDasLearners
    {
        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        private static readonly EarningsToPaymentEntity[] _earningsToPayments = new[]
      {
            new EarningsToPaymentEntity {
                            StartDate =new DateTime(2016,8,10),
                            PlannedEndDate =new DateTime(2017,8,10),
                            ActualEndDate = null,
                            CompletionStatus=1
            },
              new EarningsToPaymentEntity {
                            StartDate =new DateTime(2016,8,10),
                            PlannedEndDate =new DateTime(2017,8,10),
                            ActualEndDate = new DateTime(2017,06,10),
                            CompletionStatus=2,
                            EndpointAssessorId="test-as"
            }

        };

        [Test]
        [TestCase(25L, null, null, null)]
        [TestCase(null, 550, 20, 6)]
        public void ThenItShouldWriteCorrectDetailsForPaymentsDue(long? standardCode, int? frameworkCode, int? programmeType, int? pathwayCode)
        {
            // Arrange
            var ukprn = 863145;
            var uln = 834734;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(1);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, uln: uln,
                standardCode: standardCode, programmeType: programmeType, frameworkCode: frameworkCode,
                pathwayCode: pathwayCode);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(1, duePayments.Length);

            Assert.IsNull(duePayments[0].CommitmentId);
            Assert.IsNull(duePayments[0].CommitmentVersionId);
            Assert.IsNull(duePayments[0].AccountId);
            Assert.IsNull(duePayments[0].AccountVersionId);
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
            Assert.AreEqual("Non-Levy Funding Line", duePayments[0].FundingLineType);
        }

        [Test]
        public void ThenItShouldMakePaymentsForEachPeriodUptoAndIncludingTheCurrent()
        {
            // Arrange
            var ukprn = 863145;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            Assert.IsNull(duePayments[0].CommitmentId);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual(1000, duePayments[0].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);

            Assert.IsNull(duePayments[1].CommitmentId);
            Assert.AreEqual(9, duePayments[1].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[1].DeliveryYear);
            Assert.AreEqual(1000, duePayments[1].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[1].TransactionType);

            Assert.IsNull(duePayments[2].CommitmentId);
            Assert.AreEqual(10, duePayments[2].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[2].DeliveryYear);
            Assert.AreEqual(1000, duePayments[2].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[2].TransactionType);

            Assert.IsNull(duePayments[3].CommitmentId);
            Assert.AreEqual(11, duePayments[3].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[3].DeliveryYear);
            Assert.AreEqual(1000, duePayments[3].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[3].TransactionType);

            Assert.IsNull(duePayments[4].CommitmentId);
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
            var uln = 1765935903;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 5, uln: uln);

            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 8, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber, learningStartDate: startDate);

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
            var uln = 1765935903;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 5, uln: uln);

            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 8, 2016, (int)TransactionType.Learning, 500, learnRefNumber: learnerRefNumber,learningStartDate: startDate);

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
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 12);

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
        public void ThenItShouldMakePaymentsForLearningCompletionAndBalancingForAnEarlyCompletion()
        {
            // Arrange
            var ukprn = 863145;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(10);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 10, earlyFinisher: true);
            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 10, "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment", 360);
            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 10, "PriceEpisodeApplic1618FrameworkUpliftBalancing", 240);


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
            Assert.AreEqual(5, period10Payments.Length);

            Assert.True(period10Payments.Any(x => x.TransactionType == (int)TransactionType.Learning));
            Assert.AreEqual(1000, period10Payments.Single(x => x.TransactionType == (int)TransactionType.Learning).AmountDue);

            Assert.True(period10Payments.Any(x => x.TransactionType == (int)TransactionType.Completion));
            Assert.AreEqual(3000, period10Payments.Single(x => x.TransactionType == (int)TransactionType.Completion).AmountDue);

            Assert.True(period10Payments.Any(x => x.TransactionType == (int)TransactionType.Balancing));
            Assert.AreEqual(2000, period10Payments.Single(x => x.TransactionType == (int)TransactionType.Balancing).AmountDue);

            Assert.True(period10Payments.Any(x => x.TransactionType == (int)TransactionType.Balancing16To18FrameworkUplift));
            Assert.AreEqual(240, period10Payments.Single(x => x.TransactionType == (int)TransactionType.Balancing16To18FrameworkUplift).AmountDue);


            Assert.True(period10Payments.Any(x => x.TransactionType == (int)TransactionType.Completion16To18FrameworkUplift));
            Assert.AreEqual(360, period10Payments.Single(x => x.TransactionType == (int)TransactionType.Completion16To18FrameworkUplift).AmountDue);

        }

        [Test]
        public void ThenItShouldMakeFrameworkUpliftOnProgrammePayments()
        {
            // Arrange
            var ukprn = 863145;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 12);
            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 1, "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment", 120);
            TestDataHelper.CopyReferenceData();


            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            var period1Payments = duePayments.Where(p => p.DeliveryMonth == 08 && p.DeliveryYear == 2016).ToArray();

            var onProgrammeUpluft = period1Payments.Single(x => x.TransactionType == (int)TransactionType.OnProgramme16To18FrameworkUplift);

            Assert.NotNull(onProgrammeUpluft);
            Assert.AreEqual(120, onProgrammeUpluft.AmountDue);

        }

        public void ThenItShouldMakeDisadvantagePayments()
        {
            // Arrange
            var ukprn = 863145;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 12);
            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 1, "PriceEpisodeFirstDisadvantagePayment", 100);
            TestDataHelper.AddAdditionalPayments(ukprn, startDate, learnerRefNumber, 5, "PriceEpisodeSecondDisadvantagePayment", 100);

            TestDataHelper.CopyReferenceData();


            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            var period1Payments = duePayments.Where(p => p.DeliveryMonth == 08 && p.DeliveryYear == 2016).ToArray();
            var firstDisadvantge = period1Payments.Single(x => x.TransactionType == (int)TransactionType.FirstDisadvantagePayment);
            Assert.NotNull(firstDisadvantge);
            Assert.AreEqual(100, firstDisadvantge.AmountDue);


            var period5Payments = duePayments.Where(p => p.DeliveryMonth == 12 && p.DeliveryYear == 2016).ToArray();
            var secondDisadvantge = period1Payments.Single(x => x.TransactionType == (int)TransactionType.FirstDisadvantagePayment);
            Assert.NotNull(secondDisadvantge);
            Assert.AreEqual(100, secondDisadvantge.AmountDue);

        }

        [Test]
        public void ThenItShouldMakeRefundPaymentsForPeriodWhereEarningsNeedToBeRefunded()
        {
            // Arrange
            var ukprn = 863145;
            var uln = 1765935903;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 8, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber,learningStartDate:startDate);
            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 9, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber, learningStartDate: startDate);
            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 10, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber, learningStartDate: startDate);
            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 11, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber, learningStartDate: startDate);
            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 12, 2016, (int)TransactionType.Learning, 1000, learnRefNumber: learnerRefNumber, learningStartDate: startDate);


            TestDataHelper.SetOpenCollection(6);
            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 5,uln:uln);

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
            Assert.True(duePayments.Any(p => p.DeliveryMonth == 11 && p.DeliveryYear == 2016 && p.AmountDue ==(decimal)-1000.00));
            Assert.True(duePayments.Any(p => p.DeliveryMonth == 12 && p.DeliveryYear == 2016 && p.AmountDue == (decimal)-1000.00));

        }


        [Test]
        public void ThenItShouldMakeCorrectPaymentsWhereAimSequenceNumberIsChanged()
        {
            // Arrange
            var ukprn = 863145;
            var uln = 1765935903;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, aimSequenceNumber: 2, currentPeriod: 5, uln: uln);

            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 8, 2016, (int)TransactionType.Learning, 200,aimSequenceNumber:1,learnRefNumber:learnerRefNumber,learnAimRef: "ZPROG001", learningStartDate:startDate);

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
            Assert.AreEqual(800m, actualPayment.AmountDue);
        }

        [Test]
        public void ThenItShouldMakePaymentsWhereAimSequenceAndUlnIsChanged()
        {
            // Arrange
            var ukprn = 863145;
            var uln = 1765935903;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, aimSequenceNumber: 2, currentPeriod: 5, uln: uln);

            TestDataHelper.AddPaymentForNonDas(ukprn, uln + 100, 8, 2016, (int)TransactionType.Learning, 200, aimSequenceNumber: 1,learnRefNumber:learnerRefNumber,learningStartDate:startDate);

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
            Assert.AreEqual(800m, actualPayment.AmountDue);
        }

        [Test]
        [TestCaseSource(nameof(_earningsToPayments))]
        public void ThenItShouldReturnCorrectDetailsForEarningsToPayments(EarningsToPaymentEntity sourceData)
        {
            // Arrange
            var ukprn = 863145;
            var uln = 834734;
            var startDate = sourceData.StartDate;
            var plannedEndDate = sourceData.PlannedEndDate;
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);


            var periods = ((sourceData.PlannedEndDate.Year - sourceData.StartDate.Year) * 12) 
                            + sourceData.PlannedEndDate.Month - sourceData.StartDate.Month;
                    

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(1);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, 
                                                uln: uln,numberOfPeriods:periods,
                                                completionStatus:sourceData.CompletionStatus,actualEndDate:sourceData.ActualEndDate,opaOrgId:sourceData.EndpointAssessorId);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var requiredPaymentId = TestDataHelper.GetRequiredPaymentId(ukprn);
            Assert.IsNotNull(requiredPaymentId);

            var earningsToPayments = TestDataHelper.GetEarningsToPaymentsData(requiredPaymentId);
            Assert.IsNotNull(earningsToPayments);

            Assert.AreEqual(earningsToPayments.StartDate, sourceData.StartDate);
            Assert.AreEqual(earningsToPayments.PlannedEndDate, sourceData.PlannedEndDate);
            Assert.AreEqual(earningsToPayments.ActualEndDate, sourceData.ActualEndDate);
            Assert.AreEqual(earningsToPayments.TotalInstallments, periods);
            Assert.AreEqual(earningsToPayments.CompletionAmount, 12000 * 0.2);
            Assert.AreEqual(earningsToPayments.MonthlyInstallment, 15000 *0.8/periods);
            Assert.AreEqual(earningsToPayments.CompletionStatus, sourceData.CompletionStatus);
            Assert.AreEqual(earningsToPayments.EndpointAssessorId, sourceData.EndpointAssessorId);


        }
    }
}