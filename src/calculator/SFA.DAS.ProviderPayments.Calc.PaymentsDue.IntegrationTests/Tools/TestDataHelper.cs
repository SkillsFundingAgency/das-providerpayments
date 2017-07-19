using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.Payments.DCFS.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools
{
    internal static class TestDataHelper
    {
        private static readonly string[] PeriodEndCopyReferenceDataScripts =
        {
            "01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql",
            "02 PeriodEnd.Populate.Reference.Providers.dml.sql",
            "03 PeriodEnd.Populate.Reference.Commitments.dml.sql",
            "04 PeriodEnd.Populate.Reference.Accounts.dml.sql",
            "05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql",
            "06 PeriodEnd.PaymentsDue.Populate.Reference.RequiredPaymentsHistory.dml.sql",
            "07 PeriodEnd.PaymentsDue.PreRun.Staging.CollectionPeriods.sql",
            "08 PeriodEnd.PaymentsDue.PreRun.Staging.NonDasTransactionTypes.sql",
            "09 PeriodEnd.PaymentsDue.PreRun.Staging.LearnerPriceEpisodePerPeriod.sql",
            "10 PeriodEnd.PaymentsDue.PreRun.Staging.ApprenticeshipEarningsRequiringPayments.sql",
            "11 PeriodEnd.PaymentsDue.PreRun.Staging.ApprenticeshipEarnings.sql",
            "12 PeriodEnd.PaymentsDue.PreRun.Staging.ApprenticeshipEarnings1.sql",
            "13 PeriodEnd.PaymentsDue.PreRun.Staging.ApprenticeshipEarnings2.sql",
            "14 PeriodEnd.PaymentsDue.PreRun.Staging.ApprenticeshipEarnings3.sql",
        };

        private static readonly Random Random = new Random();

        internal static long AddProvider(long ukprn)
        {
            Execute("INSERT INTO Valid.LearningProvider" +
                    "(UKPRN) " +
                    "VALUES " +
                    "(@ukprn)",
                new { ukprn }, false);

            Execute("INSERT INTO dbo.FileDetails (UKPRN,SubmittedTime) VALUES (@ukprn, @submissionDate)",
                new { ukprn, submissionDate = DateTime.Today }, false);

            return ukprn;
        }

        internal static void AddCommitment(long id,
                                           long ukprn,
                                           string learnerRefNumber,
                                           int aimSequenceNumber = 1,
                                           long uln = 0L,
                                           DateTime startDate = default(DateTime),
                                           DateTime endDate = default(DateTime),
                                           decimal agreedCost = 15000m,
                                           long? standardCode = null,
                                           int? programmeType = null,
                                           int? frameworkCode = null,
                                           int? pathwayCode = null,
                                           bool passedDataLock = true,
                                           int[] notPayablePeriods = null,
                                           int[] notMatchedPeriods = null,
                                           TransactionType[] transactionTypes = null)
        {
            var minStartDate = new DateTime(2016, 8, 1);

            if (uln == 0)
            {
                uln = Random.Next(1, int.MaxValue);
            }

            if (!standardCode.HasValue && !programmeType.HasValue)
            {
                standardCode = 123456;
            }

            if (startDate < minStartDate)
            {
                startDate = minStartDate;
            }

            if (endDate < startDate)
            {
                endDate = startDate.AddYears(1);
            }

            Execute("INSERT INTO dbo.DasCommitments " +
                    "(CommitmentId,VersionId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,PaymentStatus,PaymentStatusDescription,Priority,EffectiveFromDate) " +
                    "VALUES " +
                    "(@id, 1, '123', @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode, 1, 'Active', 1, @startDate)",
                    new { id, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode }, false);

            var priceEpisodeIdentifier = $"99-99-99-{startDate.ToString("yyyy-MM-dd")}";

            Execute("INSERT INTO DataLock.PriceEpisodeMatch "
                  + "(Ukprn,LearnRefNumber,AimSeqNumber,CommitmentId,PriceEpisodeIdentifier,IsSuccess) "
                  + "VALUES "
                  + "(@ukprn,@learnerRefNumber,@aimSequenceNumber,@id,@priceEpisodeIdentifier,@passedDataLock)",
                  new { id, ukprn, learnerRefNumber, aimSequenceNumber, priceEpisodeIdentifier, passedDataLock });

            var censusDate = startDate.LastDayOfMonth();
            var period = 1;

            transactionTypes = transactionTypes ?? new[] { TransactionType.Balancing, TransactionType.Completion, TransactionType.Learning };

            while (censusDate <= endDate && period <= 12)
            {
                foreach (var traxType in transactionTypes)
                {
                    AddPriceEpisodePeriodMatch(id, ukprn, learnerRefNumber, aimSequenceNumber, priceEpisodeIdentifier, period, passedDataLock, notPayablePeriods, notMatchedPeriods, traxType);
                }

                censusDate = censusDate.AddMonths(1).LastDayOfMonth();
                period++;
            }

            if (endDate != endDate.LastDayOfMonth() && period <= 12)
            {
                foreach (var traxType in transactionTypes)
                {
                    AddPriceEpisodePeriodMatch(id, ukprn, learnerRefNumber, aimSequenceNumber, priceEpisodeIdentifier, period, passedDataLock, notPayablePeriods, notMatchedPeriods, traxType);
                }

            }

            if (!passedDataLock)
            {
                Execute("INSERT INTO DataLock.ValidationError "
                      + "(Ukprn,LearnRefNumber,AimSeqNumber,RuleId,PriceEpisodeIdentifier) "
                      + "VALUES "
                      + "(@ukprn,@learnerRefNumber,@aimSequenceNumber,1,@priceEpisodeIdentifier)",
                      new { id, ukprn, learnerRefNumber, aimSequenceNumber, priceEpisodeIdentifier });
            }
        }

        private static void AddPriceEpisodePeriodMatch(long commitmentId,
                                                       long ukprn,
                                                       string learnerRefNumber,
                                                       int aimSequenceNumber,
                                                       string priceEpisodeIdentifier,
                                                       int period,
                                                       bool passedDataLock,
                                                       int[] notPayablePeriods,
                                                       int[] notMatchedPeriods,
                                                       TransactionType transactionType)
        {
            if (notMatchedPeriods != null && notMatchedPeriods.Contains(period))
            {
                return;
            }

            var payable = 1;

            if (!passedDataLock || (notPayablePeriods != null && notPayablePeriods.Contains(period)))
            {
                payable = 0;
            }

            Execute("INSERT INTO DataLock.PriceEpisodePeriodMatch "
                  + "(Ukprn, PriceEpisodeIdentifier, LearnRefNumber, AimSeqNumber, CommitmentId, VersionId, Period, Payable, TransactionType) "
                  + "VALUES "
                  + "(@ukprn, @priceEpisodeIdentifier, @learnerRefNumber, @aimSequenceNumber, @commitmentId, 1, @period, @payable, @transactionType)",
                  new { commitmentId, ukprn, learnerRefNumber, aimSequenceNumber, priceEpisodeIdentifier, period, payable, transactionType });
        }

        internal static void AddEarningForCommitment(long? commitmentId,
                                                     string learnerRefNumber,
                                                     int aimSequenceNumber = 1,
                                                     int numberOfPeriods = 12,
                                                     int currentPeriod = 1,
                                                     bool earlyFinisher = false)
        {
            Execute("INSERT INTO Rulebase.AEC_ApprenticeshipPriceEpisode "
                 + "([Ukprn],[LearnRefNumber],[PriceEpisodeIdentifier],[EpisodeEffectiveTNPStartDate],[EpisodeStartDate],"
                  + "[PriceEpisodeAimSeqNumber],[PriceEpisodePlannedEndDate],[PriceEpisodeTotalTNPPrice],"
                  + "[PriceEpisodeContractType], [PriceEpisodeFundLineType],[TNP1],[TNP2])"
                  + "SELECT "
                  + "Ukprn, "
                  + "@learnerRefNumber, "
                  + "'99-99-99-' + CONVERT(char(10), StartDate, 126), "
                  + "StartDate, "
                  + "StartDate, "
                  + "@aimSequenceNumber, "
                  + "EndDate, "
                  + "AgreedCost, "
                  + "'Levy Contract', "
                  + "'Levy Funding Line', "
                  + "AgreedCost * 0.8, "
                  + "AgreedCost * 0.2 "

                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber, numberOfPeriods }, false);

            for (var x = 1; x <= 12; x++)
            {
                Execute("INSERT INTO Rulebase.AEC_ApprenticeshipPriceEpisode_Period (Ukprn, LearnRefNumber, PriceEpisodeIdentifier, Period, PriceEpisodeOnProgPayment, "
                    + "PriceEpisodeCompletionPayment, PriceEpisodeBalancePayment, PriceEpisodeFirstEmp1618Pay, PriceEpisodeFirstProv1618Pay, "
                    + "PriceEpisodeSecondEmp1618Pay, PriceEpisodeSecondProv1618Pay, PriceEpisodeSFAContribPct, PriceEpisodeLevyNonPayInd) "
                    + "SELECT "
                    + "Ukprn, "
                    + "@learnerRefNumber, "
                    + "'99-99-99-' + CONVERT(char(10), StartDate, 126), "
                    + "@period, "
                    + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= @period) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= @period) THEN (AgreedCost * 0.8) / @numberOfPeriods ELSE 0 END, "
                    + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = @period) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = @period) THEN AgreedCost * 0.2 ELSE 0 END, "
                    + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = @period THEN ((AgreedCost * 0.8) / @numberOfPeriods) * (@numberOfPeriods - @period) ELSE 0 END, "
                    + "0, "
                    + "0, "
                    + "0, "
                    + "0, "
                    + "0.9, "
                    + "1 "
                    + "FROM dbo.DasCommitments "
                    + "WHERE CommitmentId = @commitmentId",
                    new { learnerRefNumber, period = x, earlyFinisher, currentPeriod, numberOfPeriods, commitmentId }, false);
            }

            Execute("INSERT INTO Valid.Learner "
                    + "(UKPRN,LearnRefNumber,ULN,Ethnicity,Sex,LLDDHealthProb) "
                    + "SELECT Ukprn, @learnerRefNumber,Uln,0,0,0 FROM dbo.DasCommitments "
                    + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber }, false);

            Execute("INSERT INTO Valid.LearningDelivery "
                    + "(UKPRN, LearnRefNumber, LearnAimRef, AimType, AimSeqNumber, LearnStartDate, LearnPlanEndDate, FundModel, StdCode, ProgType, FworkCode, PwayCode) "
                    + "SELECT Ukprn, @learnerRefNumber, 'ZPROG001', 1, @aimSequenceNumber, StartDate, EndDate, 36, StandardCode, ProgrammeType, FrameworkCode, PathwayCode FROM dbo.DasCommitments "
                    + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber }, false);

            Execute("INSERT INTO Valid.LearningDeliveryFAM "
                    + "(UKPRN, LearnRefNumber, AimSeqNumber, LearnDelFAMType, LearnDelFAMCode, LearnDelFAMDateFrom, LearnDelFAMDateTo) "
                    + "SELECT Ukprn, @learnerRefNumber, @aimSequenceNumber, 'ACT', '1', StartDate, EndDate FROM dbo.DasCommitments "
                    + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber }, false);
        }


        internal static void AddEarningForNonDas(long ukprn,
                                        DateTime startDate,
                                        DateTime endDate,
                                        decimal agreedCost,
                                        string learnerRefNumber,
                                        int aimSequenceNumber = 1,
                                        int numberOfPeriods = 12,
                                        int currentPeriod = 1,
                                        bool earlyFinisher = false,
                                        long uln = 0L,
                                        long? standardCode = null,
                                        int? programmeType = null,
                                        int? frameworkCode = null,
                                        int? pathwayCode = null)
        {
            var tnp1 = agreedCost * 0.8m;
            var tnp2 = agreedCost * 0.2m;
            if (uln == 0)
            {
                uln = Random.Next(1, int.MaxValue);
            }

            if (standardCode == null && programmeType == null && frameworkCode == null && pathwayCode == null)
            {
                standardCode = 25;
            }



            Execute("INSERT INTO Rulebase.AEC_ApprenticeshipPriceEpisode "
                  + "([Ukprn],[LearnRefNumber],[PriceEpisodeIdentifier],[EpisodeEffectiveTNPStartDate],[EpisodeStartDate],"
                  + "[PriceEpisodeAimSeqNumber],[PriceEpisodePlannedEndDate],[PriceEpisodeTotalTNPPrice],"
                  + "[PriceEpisodeContractType], [PriceEpisodeFundLineType],[TNP1],[TNP2])"
                  + " SELECT "
                  + "@ukprn, "
                  + "@learnerRefNumber, "
                  + "'99-99-99-' + CONVERT(char(10), @startDate, 126), "
                  + "@startDate, "
                  + "@startDate, "
                  + "@aimSequenceNumber, "
                  + "@endDate, "
                  + "@agreedCost, "
                  + "'Non-Levy Contract',"
                  + "'Non-Levy Funding Line',"
                  + "@tnp1, "
                  + "@tnp2 ",
                new { ukprn, learnerRefNumber, startDate, aimSequenceNumber, endDate, agreedCost, tnp1, tnp2 }, false);

            for (var x = 1; x <= 12; x++)
            {
                Execute("INSERT INTO Rulebase.AEC_ApprenticeshipPriceEpisode_Period (Ukprn, LearnRefNumber, PriceEpisodeIdentifier, Period, PriceEpisodeOnProgPayment, "
                    + "PriceEpisodeCompletionPayment, PriceEpisodeBalancePayment, PriceEpisodeFirstEmp1618Pay, PriceEpisodeFirstProv1618Pay, "
                    + "PriceEpisodeSecondEmp1618Pay, PriceEpisodeSecondProv1618Pay,  PriceEpisodeSFAContribPct,PriceEpisodeLevyNonPayInd) "
                    + "VALUES ("
                    + "@Ukprn, "
                    + "@learnerRefNumber, "
                    + "'99-99-99-' + CONVERT(char(10), @startDate, 126), "
                    + "@period, "
                    + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= @period) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= @period) THEN (@tnp1) / @numberOfPeriods ELSE 0 END, "
                    + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod = @period) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods = @period) THEN @tnp2 ELSE 0 END, "
                    + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = @period THEN (@tnp1 / @numberOfPeriods) * (@numberOfPeriods - @period) ELSE 0 END, "
                    + "0, "
                    + "0, "
                    + "0, "
                    + "0, "
                    + "0.9, "
                    + "1)",
                    new { ukprn, learnerRefNumber, startDate, period = x, earlyFinisher, currentPeriod, numberOfPeriods, agreedCost, tnp1, tnp2 }, false);
            }

            Execute("INSERT INTO Valid.Learner "
                    + "(UKPRN, LearnRefNumber, ULN, Ethnicity, Sex, LLDDHealthProb) "
                    + "VALUES (@Ukprn, @learnerRefNumber,@Uln,0,0,0)",
                new { ukprn, learnerRefNumber, uln }, false);

            Execute("INSERT INTO Valid.LearningDelivery "
                    + "(UKPRN, LearnRefNumber, LearnAimRef, AimType, AimSeqNumber, LearnStartDate, LearnPlanEndDate, FundModel, StdCode, ProgType, FworkCode, PwayCode) "
                    + "VALUES (@ukprn, @learnerRefNumber, 'ZPROG001', 1, @aimSequenceNumber, @startDate, @endDate, 36, @standardCode, @programmeType, @frameworkCode, @pathwayCode)",
                new { ukprn, learnerRefNumber, aimSequenceNumber, startDate, endDate, standardCode, programmeType, frameworkCode, pathwayCode }, false);

            Execute("INSERT INTO Valid.LearningDeliveryFAM "
                    + "(UKPRN, LearnRefNumber, AimSeqNumber, LearnDelFAMType, LearnDelFAMCode, LearnDelFAMDateFrom, LearnDelFAMDateTo) "
                    + "VALUES (@ukprn, @learnerRefNumber, @aimSequenceNumber, 'ACT', '2', @startDate, @endDate)",
                new { ukprn, learnerRefNumber, aimSequenceNumber, startDate, endDate }, false);
        }

        internal static void AddApprenticeEarning(long ukprn,
                                        DateTime startDate,
                                        string learnerRefNumber,
                                        int period,
                                        decimal? onProgPayment = null,
                                        decimal? completionPayment = null,
                                        decimal? balancingPayment = null)
        {
            Execute("INSERT INTO Rulebase.AEC_ApprenticeshipPriceEpisode_Period (Ukprn, LearnRefNumber, PriceEpisodeIdentifier, Period, PriceEpisodeOnProgPayment, "
                    + "PriceEpisodeCompletionPayment, PriceEpisodeBalancePayment, PriceEpisodeFirstEmp1618Pay, PriceEpisodeFirstProv1618Pay, "
                    + "PriceEpisodeSecondEmp1618Pay, PriceEpisodeSecondProv1618Pay,  PriceEpisodeSFAContribPct,PriceEpisodeLevyNonPayInd) "
                    + "VALUES ("
                    + "@Ukprn, "
                    + "@learnerRefNumber, "
                    + "'99-99-99-' + CONVERT(char(10), @startDate, 126), "
                    + "@period, "
                    + "@onProgPayment, "
                    + "@completionPayment, "
                    + "@balancingPayment, "
                    + "0, "
                    + "0, "
                    + "0, "
                    + "0, "
                    + "0.9, "
                    + "1)",
                    new { ukprn, learnerRefNumber, startDate, period, onProgPayment, completionPayment, balancingPayment }, false);

        }

        internal static void AddMathsAndEnglishEarningForCommitment(long? commitmentId,
                                                     string learnerRefNumber,
                                                     int aimSequenceNumber = 2,
                                                     int numberOfPeriods = 12,
                                                     int currentPeriod = 1,
                                                     bool earlyFinisher = false)
        {
            Execute("INSERT INTO Rulebase.AEC_LearningDelivery "
                 + "([Ukprn],[LearnRefNumber],[AimSeqNumber],[LearnAimRef])"
                  + "SELECT "
                  + "Ukprn, "
                  + "@learnerRefNumber, "
                  + "@aimSequenceNumber, "
                  + "'50086832' "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber }, false);

            for (var x = 1; x <= 12; x++)
            {
                Execute("INSERT INTO Rulebase.AEC_LearningDelivery_Period (Ukprn, LearnRefNumber, AimSeqNumber, Period, MathEngOnProgPayment, MathEngBalPayment, "
                    + "LearnSuppFundCash, LearnDelContType, FundLineType, LearnDelSFAContribPct, LearnDelLevyNonPayInd) "
                    + "SELECT "
                    + "Ukprn, "
                    + "@learnerRefNumber, "
                    + "@aimSequenceNumber, "
                    + "@period, "
                    + "CASE WHEN (@earlyFinisher = 'TRUE' AND @currentPeriod >= @period) OR (@earlyFinisher = 'FALSE' AND @numberOfPeriods >= @period) THEN 471 / @numberOfPeriods ELSE 0 END, "
                    + "CASE WHEN @earlyFinisher = 'TRUE' AND @currentPeriod = @period THEN (471.00 / @numberOfPeriods) * (@numberOfPeriods - @period) ELSE 0 END, "
                    + "0, "
                    + "'Levy Contract', "
                    + "'Funding Line', "
                    + "1, "
                    + "1 "
                    + "FROM dbo.DasCommitments "
                    + "WHERE CommitmentId = @commitmentId",
                    new { learnerRefNumber, aimSequenceNumber, period = x, earlyFinisher, currentPeriod, numberOfPeriods, commitmentId }, false);
            }

            Execute("INSERT INTO Valid.LearningDelivery "
                    + "(UKPRN, LearnRefNumber, LearnAimRef, AimType, AimSeqNumber, LearnStartDate, LearnPlanEndDate, FundModel, StdCode, ProgType, FworkCode, PwayCode) "
                    + "SELECT Ukprn, @learnerRefNumber, '50086832', 3, @aimSequenceNumber, StartDate, EndDate, 36, StandardCode, ProgrammeType, FrameworkCode, PathwayCode FROM dbo.DasCommitments "
                    + "WHERE CommitmentId = @commitmentId",
                new { commitmentId, learnerRefNumber, aimSequenceNumber }, false);
        }

        internal static void AddAdditionalPayments(long ukprn,
                                                               DateTime startDate,
                                                               string learnerRefNumber,
                                                               int currentPeriod = 1,
                                                               string incentiveName = "",
                                                               decimal amount = 500m)
        {
            string setClause;

            switch (incentiveName)
            {
                case "PriceEpisodeFirstEmp1618Pay":
                    setClause = "PriceEpisodeFirstEmp1618Pay = @amount ";
                    break;
                case "PriceEpisodeFirstProv1618Pay":
                    setClause = "PriceEpisodeFirstProv1618Pay = @amount ";
                    break;
                case "PriceEpisodeSecondEmp1618Pay":
                    setClause = "PriceEpisodeSecondEmp1618Pay = @amount ";
                    break;
                case "PriceEpisodeSecondProv1618Pay":
                    setClause = "PriceEpisodeSecondProv1618Pay = @amount ";
                    break;
                case "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment":
                    setClause = "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment = @amount ";
                    break;
                case "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment":
                    setClause = "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment= @amount ";
                    break;
                case "PriceEpisodeApplic1618FrameworkUpliftBalancing":
                    setClause = "PriceEpisodeApplic1618FrameworkUpliftBalancing = @amount ";
                    break;
                case "PriceEpisodeFirstDisadvantagePayment":
                    setClause = "PriceEpisodeFirstDisadvantagePayment = @amount ";
                    break;
                case "PriceEpisodeSecondDisadvantagePayment":
                    setClause = "PriceEpisodeSecondDisadvantagePayment = @amount ";
                    break;
                default:
                    throw new ArgumentException($"Unknown incentive name {incentiveName}.");
            }

            Execute("UPDATE Rulebase.AEC_ApprenticeshipPriceEpisode_Period "
                + "SET "
                + setClause
                + "WHERE "
                + "Ukprn = @ukprn "
                + "AND LearnRefNumber = @learnerRefNumber "
                + "AND PriceEpisodeIdentifier = '99-99-99-' + CONVERT(char(10), @startDate, 126) "
                + "AND Period = @currentPeriod",
                new { ukprn, learnerRefNumber, startDate, currentPeriod, amount }, false);
        }

        internal static void AddPaymentForCommitment(long commitmentId, int month, 
            int year, int transactionType, decimal amount,string learnRefNumber = "1", 
            int aimSequenceNumber = 1, string learnAimRef = "ZPROG001")
        {
            var academicYear = year - 2000;
            if (month < 8)
            {
                academicYear--;
            }
            academicYear = (academicYear * 100) + (academicYear + 1);

            Execute("INSERT INTO PaymentsDue.RequiredPayments "
                  + "SELECT "
                  + "NEWID(), " // Id
                  + "CommitmentId, " // CommitmentId
                  + "VersionId, " // CommitmentVersionId
                  + "AccountId, " // AccountId
                  + "'NA', " // AccountVersionId
                  + "Uln, " // Uln
                  + "@learnRefNumber, " // LearnRefNumber
                  + "@aimSequenceNumber, " // AimSeqNumber
                  + "Ukprn, " // Ukprn
                  + "GETDATE(), " // IlrSubmissionDateTime
                  + "'25-27-01/04/2017', " // PriceEpisodeIdentifier
                  + "StandardCode, " // StandardCode
                  + "ProgrammeType, " // ProgrammeType
                  + "FrameworkCode, " // FrameworkCode
                  + "PathwayCode, " // PathwayCode
                  + "'1', " // ApprenticeshipContractType
                  + "@month, " // DeliveryMonth
                  + "@year, " // DeliveryYear
                  + "cast(@academicYear as char(4)) + '-R01', " // CollectionPeriodName
                  + "@month, " // CollectionPeriodMonth
                  + "@year, " // CollectionPeriodYear
                  + "@transactionType, " // TransactionType
                  + "@amount, " // AmountDue
                  + "0.9, " // SfaContributionPercentage
                  + "'Non-Levy Funding Line', " // FundingLineType
                   + "1, " // UseLevyBalane
                   + "@learnAimref,"
                   + "StartDate "
                  + "FROM dbo.DasCommitments "
                  + "WHERE CommitmentId = @commitmentId",
                  new { month, year,learnRefNumber, transactionType, amount, commitmentId,aimSequenceNumber,learnAimRef, academicYear }, false);
        }

        internal static void AddPaymentForNonDas(long ukprn, 
                                                    long uln, 
                                                    int month, 
                                                    int year, 
                                                    int transactionType, 
                                                    decimal amount,
                                                    long? standardCode = null,
                                                    int? programmeType = null,
                                                    int? frameworkCode = null,
                                                    int? pathwayCode = null,
                                                    int? aimSequenceNumber =1,
                                                    string learnRefNumber = "1",
                                                    DateTime? learningStartDate = null,
                                                    string learnAimRef="ZPROG001"
                                                    )
        {
            if (standardCode == null && programmeType == null && frameworkCode == null && pathwayCode == null)
            {
                standardCode = 25;
            }

            learningStartDate = learningStartDate ?? new DateTime(2016,10,10); 

            Execute("INSERT INTO PaymentsDue.RequiredPayments "
                  + "VALUES ("
                  + "NEWID(), " // Id
                  + "NULL, " // CommitmentId
                  + "NULL, " // CommitmentVersionId
                  + "NULL, " // AccountId
                  + "NULL, " // AccountVersionId
                  + "@uln, " // Uln
                  + "@learnRefNumber, " // LearnRefNumber
                  + "@aimSequenceNumber, " // AimSeqNumber
                  + "@ukprn, " // Ukprn
                  + "GETDATE(), " // IlrSubmissionDateTime
                  + "'25-27-01/04/2017', " // PriceEpisodeIdentifier
                  + "@standardCode, " // StandardCode
                  + "@programmeType, " // ProgrammeType
                  + "@frameworkCode, " // FrameworkCode
                  + "@pathwayCode, " // PathwayCode
                  + "'2', " // ApprenticeshipContractType
                  + "@month, " // DeliveryMonth
                  + "@year, " // DeliveryYear
                  + "'R01', " // CollectionPeriodName
                  + "@month, " // CollectionPeriodMonth
                  + "@year, " // CollectionPeriodYear
                  + "@transactionType, " // TransactionType
                  + "@amount, " // AmountDue
                  + "0.9, " // SfaContributionPercentage
                  + "'Non-Levy Funding Line', " // FundingLineType
                  + "1," //UseLevyBalance
                  + "@LearnAimRef,"
                  + "@learningStartDate)",
                  new { uln, ukprn,learnRefNumber, aimSequenceNumber, month, year, transactionType, amount, standardCode, programmeType, frameworkCode, pathwayCode, learnAimRef, learningStartDate }, false);
        }

        internal static void SetOpenCollection(int periodNumber)
        {
            Execute("UPDATE Collection_Period_Mapping "
                    + "SET Collection_Open = 0", null, false);

            Execute("UPDATE Collection_Period_Mapping "
                    + "SET Collection_Open = 1 "
                    + $"WHERE [Return_Code] = 'R{periodNumber:00}'", null, false);
        }


        internal static RequiredPaymentEntity[] GetRequiredPaymentsForProvider(long ukprn)
        {
            return Query<RequiredPaymentEntity>("SELECT * FROM PaymentsDue.RequiredPayments WHERE Ukprn = @Ukprn ORDER BY DeliveryYear, DeliveryMonth", new { ukprn });
        }

        public static void Clean()
        {
            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'Valid', 'Rulebase', 'PaymentsDue')
                            AND o.name NOT IN ('Collection_Period_Mapping', 'DasAccounts')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ");

            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'Valid', 'Rulebase', 'PaymentsDue')
                            AND o.name NOT IN ('Collection_Period_Mapping', 'DasAccounts')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ", null, false);
        }

        internal static void CopyReferenceData()
        {
            foreach (var script in PeriodEndCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\Copy Reference Data\{script}");

                var commands = ReplaceSqlTokens(sql).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in commands)
                {
                    Execute(command);
                }
            }
        }

        internal static void ClearApprenticeshipPriceEpisodePeriod()
        {
            Execute("DELETE FROM Rulebase.AEC_ApprenticeshipPriceEpisode_Period", null, false);
        }

        private static void Execute(string command, object param = null, bool inTransient = true)
        {
            var connectionString = inTransient
                ? GlobalTestContext.Instance.TransientConnectionString
                : GlobalTestContext.Instance.DedsConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static T[] Query<T>(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param)?.ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static string ReplaceSqlTokens(string sql)
        {
            return sql.Replace("${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Accounts.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${YearOfCollection}", "1617");
        }
    }
}