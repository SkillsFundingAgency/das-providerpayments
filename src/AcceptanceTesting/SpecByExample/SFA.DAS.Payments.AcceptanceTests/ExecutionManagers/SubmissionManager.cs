using System;
using System.Collections.Generic;
using System.Linq;
using IlrGenerator;
using ProviderPayments.TestStack.Core.ExecutionStatus;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.DataCollectors;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;
using System.IO;

namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    internal static class SubmissionManager
    {
        private const string FamCodeAct = "ACT";
        private const short FamCodeActDasValue = 1;
        private const short FamCodeActNonDasValue = 2;

        internal static List<LearnerResults> SubmitIlrAndRunMonthEndAndCollateResults(
            List<IlrLearnerReferenceData> ilrLearnerDetails,
            DateTime? firstSubmissionDate,
            LookupContext lookupContext,
            List<EmployerAccountReferenceData> employers,
            List<ContractTypeReferenceData> contractTypes,
            List<EmploymentStatusReferenceData> employmentStatus,
            List<LearningSupportReferenceData> learningSupportStatus,
            string[] periodsToSubmitTo = null,
            string ilrPeriod = null)
        {
            var results = new List<LearnerResults>();
            if (TestEnvironment.ValidateSpecsOnly)
            {
                return results;
            }
            
            var periods = periodsToSubmitTo ?? ExtractPeriods(ilrLearnerDetails, firstSubmissionDate);
            var providerLearners = GroupLearnersByProvider(ilrLearnerDetails, lookupContext);
            foreach (var period in periods)
            {
                SetEnvironmentToPeriod(period);
                EmployerAccountManager.UpdateAccountBalancesForPeriod(employers, period);

                foreach (var providerDetails in providerLearners)
                {
                    //continue here if period is explicit and does not match
                    if(!string.IsNullOrEmpty(ilrPeriod) && !string.Equals(ilrPeriod, period, StringComparison.CurrentCultureIgnoreCase)) //need the providerDetails to include the period maybs?
                        continue;

                    SetupDisadvantagedPostcodeUplift(providerDetails);
                    //here submits ilr
                    BuildAndSubmitIlr(providerDetails, period, lookupContext, contractTypes, employmentStatus,
                        learningSupportStatus);
                }
                //here runs month end
                RunMonthEnd(period);

                EarningsCollector.CollectForPeriod(period, results, lookupContext);
                LevyAccountBalanceCollector.CollectForPeriod(period, results, lookupContext);
                SubmissionDataLockResultCollector.CollectForPeriod(period, results, lookupContext);

                SavedDataCollector.CaptureAccountsDataForScenario();
                SavedDataCollector.CaptureCommitmentsDataForScenario();
            }

            DataLockEventsDataCollector.CollectDataLockEventsForAllPeriods(results, lookupContext);
            PaymentsDataCollector.CollectForPeriod(results, lookupContext);

            return results;
        }

        private static string[] ExtractPeriods(List<IlrLearnerReferenceData> ilrLearnerDetails, DateTime? firstSubmissionDate)
        {
            var periods = new List<string>();

            var earliestDate = ilrLearnerDetails.Select(x => x.StartDate).Min();
            var latestPlannedDate = ilrLearnerDetails.Select(x => x.PlannedEndDate).Max();
            var latestActualDate = ilrLearnerDetails.Select(x => x.ActualEndDate).Max();
            var latestDate = latestActualDate.HasValue && latestActualDate > latestPlannedDate ? latestActualDate : latestPlannedDate;

            var date = earliestDate;
            while (date <= latestDate)
            {
                if (firstSubmissionDate.HasValue && date < firstSubmissionDate.Value.AddDays(-firstSubmissionDate.Value.Day + 1))
                {
                    date = date.AddMonths(1);
                    continue;
                }

                periods.Add($"{date.Month:00}/{date.Year - 2000}");
                date = date.AddMonths(1);
            }

            var minExplicitSubmissionPeriod = ilrLearnerDetails.Min(x => GetDateFromPeriodName(x.SubmissionPeriod, earliestDate));
            var maxExplicitSubmissionPeriod = ilrLearnerDetails.Max(x => GetDateFromPeriodName(x.SubmissionPeriod, earliestDate));

            while(minExplicitSubmissionPeriod.HasValue && minExplicitSubmissionPeriod < new DateTime(earliestDate.Year, earliestDate.Month, 1))
            {
                periods.Add($"{minExplicitSubmissionPeriod.Value.Month:00}/{minExplicitSubmissionPeriod.Value.Year - 2000}");
                minExplicitSubmissionPeriod = minExplicitSubmissionPeriod.Value.AddMonths(1);
            }

            if (maxExplicitSubmissionPeriod.HasValue && maxExplicitSubmissionPeriod.Value > new DateTime(latestDate.Value.Year, latestDate.Value.Month, 1))
                throw new Exception("You have added an ILR for a period later than the last period which is checked in the 'Then' table.");

            while (maxExplicitSubmissionPeriod.HasValue && maxExplicitSubmissionPeriod > new DateTime(latestDate.Value.Year, latestDate.Value.Month, 1))
            {
                periods.Add($"{maxExplicitSubmissionPeriod.Value.Month:00}/{maxExplicitSubmissionPeriod.Value.Year - 2000}");
                maxExplicitSubmissionPeriod = maxExplicitSubmissionPeriod.Value.AddMonths(-1);
            }

            return periods.ToArray();
        }

        private static DateTime? GetDateFromPeriodName(string periodInRNotation, DateTime yearStartDate)
        {
            if (string.IsNullOrEmpty(periodInRNotation))
                return null;

            switch (periodInRNotation.ToUpper())
            {
                case "R01": return new DateTime(yearStartDate.Year, 8, 1);
                case "R02": return new DateTime(yearStartDate.Year, 9, 1);
                case "R03": return new DateTime(yearStartDate.Year, 10, 1);
                case "R04": return new DateTime(yearStartDate.Year, 11, 1);
                case "R05": return new DateTime(yearStartDate.Year, 12, 1);
                case "R06": return new DateTime(yearStartDate.Year + 1, 1, 1);
                case "R07": return new DateTime(yearStartDate.Year + 1, 2, 1);
                case "R08": return new DateTime(yearStartDate.Year + 1, 3, 1);
                case "R09": return new DateTime(yearStartDate.Year + 1, 4, 1);
                case "R10": return new DateTime(yearStartDate.Year + 1, 5, 1);
                case "R11": return new DateTime(yearStartDate.Year + 1, 6, 1);
                case "R12": return new DateTime(yearStartDate.Year + 1, 7, 1);
                case "R13": return new DateTime(yearStartDate.Year + 1, 8, 1);
                case "R14": return new DateTime(yearStartDate.Year + 1, 9, 1);
                default: return new DateTime(yearStartDate.Year, 8, 1);
            }

        }

        private static string GetPeriodFromStringDate(string periodDate)
        {
            if (string.IsNullOrEmpty(periodDate))
                return null;

            switch (periodDate.ToUpper())
            {
                case "08/17": return "R01";
                case "09/17": return "R02";
                case "10/17": return "R03";
                case "11/17": return "R04";
                case "12/17": return "R05";
                case "01/18": return "R06";
                case "02/18": return "R07";
                case "03/18": return "R08";
                case "04/18": return "R09";
                case "05/18": return "R10";
                case "06/18": return "R11";
                case "07/18": return "R12";
                case "09/18": return "R13";
                case "10/18": return "R14";
                default: return null;
            }

        }

        private static ProviderSubmissionDetails[] GroupLearnersByProvider(List<IlrLearnerReferenceData> ilrLearnerDetails, LookupContext lookupContext)
        {
            return (from x in ilrLearnerDetails
                    group x by x.Provider into g
                    select new ProviderSubmissionDetails
                    {
                        ProviderId = g.Key,
                        Ukprn = lookupContext.AddOrGetUkprn(g.Key),
                        LearnerDetails = g.ToArray()
                    }).ToArray();
        }

        private static void SetEnvironmentToPeriod(string period)
        {
            var month = int.Parse(period.Substring(0, 2));
            var year = int.Parse(period.Substring(3, 2)) + 2000;
            var date = new DateTime(year, month, 1);
            var periodNumber = date.GetPeriodNumber();

            TestEnvironment.Variables.CurrentYear = date.GetAcademicYear();
            TestEnvironment.Variables.CollectionPeriod = new ProviderPayments.TestStack.Core.Domain.CollectionPeriod
            {
                PeriodId = date.GetPeriodNumber(),
                Period = "R" + periodNumber.ToString("00"),
                CalendarMonth = date.Month,
                CalendarYear = date.Year,
                ActualsSchemaPeriod = date.Year + date.Month.ToString("00"),
                CollectionOpen = 1
            };
            TestEnvironment.Variables.IlrFileDirectory = $"{TestEnvironment.BaseScenarioDirectory}\\{month:00}_{year}";
            if (!Directory.Exists(TestEnvironment.Variables.IlrFileDirectory))
            {
                Directory.CreateDirectory(TestEnvironment.Variables.IlrFileDirectory);
            }
        }

        private static void SetupDisadvantagedPostcodeUplift(ProviderSubmissionDetails providerDetails)
        {
            var homePostcodeDeprivation = providerDetails.LearnerDetails.Select(l => l.HomePostcodeDeprivation)
                                                                        .FirstOrDefault(d => !string.IsNullOrEmpty(d));
            if (!string.IsNullOrEmpty(homePostcodeDeprivation))
            {
                ReferenceDataManager.AddDisadvantagedPostcodeUplift(homePostcodeDeprivation);
            }
        }

        private static IlrLearnerReferenceData[] FilterLearnersForPeriod(IlrLearnerReferenceData[] learnerDetails, string period)
        {
            if (learnerDetails.All(x => x.SubmissionPeriod == null))
            {
                return learnerDetails;
            }

            var filteredLearners = new List<IlrLearnerReferenceData>();

            var learnersWithMultipleSubmissions = learnerDetails
                .GroupBy(x => new { x.Provider, x.LearnerReference, })
                .Select(x => x.ToList());

            var periodName = GetPeriodFromStringDate(period);
            foreach (var learnerWithMultipleSubmissions in learnersWithMultipleSubmissions)
            {
                var submissions = learnerWithMultipleSubmissions
                    .OrderByDescending(x => x.SubmissionPeriod)
                    .SkipWhile(x => string.Compare(x.SubmissionPeriod, periodName, StringComparison.OrdinalIgnoreCase) > 0)
                    .ToList();
                if (submissions.Any())
                {
                    filteredLearners.Add(submissions.First());
                }
            }

            return filteredLearners.ToArray();
        }

        private static void BuildAndSubmitIlr(ProviderSubmissionDetails providerDetails, string period, LookupContext lookupContext, List<ContractTypeReferenceData> contractTypes, List<EmploymentStatusReferenceData> employmentStatus, List<LearningSupportReferenceData> learningSupportStatus)
        {
            IlrSubmission submission = BuildIlrSubmission(providerDetails, period, lookupContext, contractTypes, employmentStatus, learningSupportStatus);
            TestEnvironment.ProcessService.RunIlrSubmission(submission, TestEnvironment.Variables, new LoggingStatusWatcher($"ILR submission for provider {providerDetails.ProviderId} in {period}"));
        }

        private static void RunMonthEnd(string period)
        {
            TestEnvironment.ProcessService.RunSummarisation(TestEnvironment.Variables, new LoggingStatusWatcher($"Month end for {period}"));
        }

        private static IlrSubmission BuildIlrSubmission(ProviderSubmissionDetails providerDetails, string period, LookupContext lookupContext, List<ContractTypeReferenceData> contractTypes, List<EmploymentStatusReferenceData> employmentStatus, List<LearningSupportReferenceData> learningSupportStatus)
        {
            var filteredLearnerDetails = FilterLearnersForPeriod(providerDetails.LearnerDetails, period);

            var learners = (from x in filteredLearnerDetails
                group x by x.LearnerReference into g
                select BuildLearner(g.ToArray(), period, lookupContext, contractTypes, employmentStatus, learningSupportStatus)).ToArray();
            var submission = new IlrSubmission
            {
                Ukprn = providerDetails.Ukprn,
                AcademicYear = period.ToPeriodDateTime().GetAcademicYear(),
                PreperationDate = period.ToPeriodDateTime().AddDays(27),
                Learners = learners
            };
            for (var i = 0; i < submission.Learners.Length; i++)
            {
                if (string.IsNullOrEmpty(submission.Learners[i].LearnRefNumber))
                {
                    submission.Learners[i].LearnRefNumber = (i + 1).ToString();
                }
                i++;
            }
            return submission;
        }

        private static Learner BuildLearner(IlrLearnerReferenceData[] learnerDetails, string period, LookupContext lookupContext, List<ContractTypeReferenceData> contractTypes, List<EmploymentStatusReferenceData> employmentStatus, List<LearningSupportReferenceData> learningSupportStatus)
        {
            var periodMonth = int.Parse(period.Substring(0, 2));
            var periodYear = int.Parse(period.Substring(3)) + 2000;
            var endOfPeriod = new DateTime(periodYear, periodMonth, 1).AddMonths(1).AddSeconds(-1);

            var deliveries = learnerDetails
                .Where(x => x.StartDate <= endOfPeriod)
                .Select(x =>
                {
                    var financialRecords = BuildLearningDeliveryFinancials(x);

                    return new LearningDelivery
                    {
                        StandardCode = x.StandardCode,
                        FrameworkCode = x.FrameworkCode,
                        ProgrammeType = x.ProgrammeType,
                        PathwayCode = x.PathwayCode,
                        ActualStartDate = x.StartDate,
                        PlannedEndDate = x.PlannedEndDate,
                        ActualEndDate = x.ActualEndDate <= endOfPeriod ? x.ActualEndDate : (DateTime?) null,
                        FamRecords = BuildLearningDeliveryFamCodes(x, contractTypes, learningSupportStatus),
                        CompletionStatus = (IlrGenerator.CompletionStatus) (int) x.CompletionStatus,
                        Type = (IlrGenerator.AimType) (int) x.AimType,
                        LearnAimRef = x.LearnAimRef,
                        FinancialRecords = financialRecords,
                        AimSequenceNumber = x.AimSequenceNumber,
                        LearningAdjustmentForPriorLearning = x.LearningAdjustmentForPriorLearning,
                        OtherFundingAdjustments = x.OtherFundingAdjustments,
                    };
                })
                .ToArray();

            var employmentStatuses = employmentStatus.Select(s =>
            {
                EmploymentStatusMonitoring monitoringStatus = null;
                if (s.MonitoringCode > 0)
                {
                    monitoringStatus = new EmploymentStatusMonitoring
                    {
                        Type = s.MonitoringType.ToString(),
                        Code = s.MonitoringCode
                    };
                }
                return new IlrGenerator.EmploymentStatus
                {
                    EmployerId = s.EmployerId,
                    DateFrom = s.EmploymentStatusApplies,
                    EmploymentStatusMonitoring = monitoringStatus,
                    StatusCode = (int)s.EmploymentStatus
                };
            }).ToArray();

            var learner = new Learner
            {
                LearnRefNumber= learnerDetails[0].LearnerReference,
                DateOfBirth = GetDateOfBirthBasedOnLearnerType(learnerDetails[0].LearnerType),
                LearningDeliveries = deliveries,
                EmploymentStatuses = employmentStatuses.Any() ? employmentStatuses : null
            };

            long uln;
            long.TryParse(learnerDetails[0].Uln, out uln);

            learner.Uln = uln > 0 ? uln : lookupContext.AddOrGetUln(learnerDetails[0].LearnerReference);
        
            return learner;
        }

        private static FinancialRecord[] BuildLearningDeliveryFinancials(IlrLearnerReferenceData learnerReferenceData)
        {
            var agreedTrainingPrice = learnerReferenceData.FrameworkCode > 0 ? learnerReferenceData.AgreedPrice :
                                     (int)Math.Floor(learnerReferenceData.AgreedPrice * 0.8m);
            var agreedAssesmentPrice = learnerReferenceData.AgreedPrice - agreedTrainingPrice;

            var financialRecords = new List<FinancialRecord>();

            ////////////////////////////////////////////////////////////////////
            // TNP1 & 2
            ////////////////////////////////////////////////////////////////////
            if (learnerReferenceData.TotalTrainingPrice1 > 0)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 1,
                    Type = "TNP",
                    Amount = learnerReferenceData.TotalTrainingPrice1 == 0 ? agreedTrainingPrice : learnerReferenceData.TotalTrainingPrice1,
                    Date = learnerReferenceData.TotalTrainingPrice1EffectiveDate == DateTime.MinValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalTrainingPrice1EffectiveDate
                });
            }

            if (learnerReferenceData.TotalAssessmentPrice1 > 0)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 2,
                    Type = "TNP",
                    Amount = learnerReferenceData.TotalAssessmentPrice1 == 0 ? agreedAssesmentPrice : learnerReferenceData.TotalAssessmentPrice1,
                    Date = learnerReferenceData.TotalAssessmentPrice1EffectiveDate == DateTime.MinValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalAssessmentPrice1EffectiveDate
                });
            }

            if (learnerReferenceData.TotalTrainingPrice2 > 0 && learnerReferenceData.TotalTrainingPrice2 != learnerReferenceData.TotalTrainingPrice1)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 1,
                    Type = "TNP",
                    Amount = learnerReferenceData.TotalTrainingPrice2,
                    Date = learnerReferenceData.TotalTrainingPrice2EffectiveDate == DateTime.MinValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalTrainingPrice2EffectiveDate
                });
            }

            if (learnerReferenceData.TotalAssessmentPrice2 > 0 && learnerReferenceData.TotalAssessmentPrice2 != learnerReferenceData.TotalAssessmentPrice1)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 2,
                    Type = "TNP",
                    Amount = learnerReferenceData.TotalAssessmentPrice2,
                    Date = learnerReferenceData.TotalAssessmentPrice2EffectiveDate == DateTime.MinValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalAssessmentPrice2EffectiveDate
                });
            }


            ////////////////////////////////////////////////////////////////////
            // TNP3 & 4
            ////////////////////////////////////////////////////////////////////
            if (learnerReferenceData.ResidualTrainingPrice1 > 0 || learnerReferenceData.ResidualAssessmentPrice1 > 0)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 3,
                    Type = "TNP",
                    Amount = learnerReferenceData.ResidualTrainingPrice1,
                    Date = learnerReferenceData.ResidualTrainingPrice1EffectiveDate == DateTime.MinValue ? learnerReferenceData.StartDate : learnerReferenceData.ResidualTrainingPrice1EffectiveDate
                });
                financialRecords.Add(new FinancialRecord
                {
                    Code = 4,
                    Type = "TNP",
                    Amount = learnerReferenceData.ResidualAssessmentPrice1,
                    Date = learnerReferenceData.ResidualAssessmentPrice1EffectiveDate == DateTime.MinValue ? learnerReferenceData.StartDate : learnerReferenceData.ResidualAssessmentPrice1EffectiveDate
                });
            }

            // Change in residual
            if ((learnerReferenceData.ResidualTrainingPrice2 > 0 && learnerReferenceData.ResidualTrainingPrice2 != learnerReferenceData.ResidualTrainingPrice1)
                || (learnerReferenceData.ResidualAssessmentPrice2 > 0 && learnerReferenceData.ResidualAssessmentPrice2 != learnerReferenceData.ResidualAssessmentPrice1))
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 3,
                    Type = "TNP",
                    Amount = learnerReferenceData.ResidualTrainingPrice2 == 0 ? learnerReferenceData.ResidualTrainingPrice1 : learnerReferenceData.ResidualTrainingPrice2,
                    Date = learnerReferenceData.ResidualTrainingPrice2EffectiveDate == DateTime.MinValue ? learnerReferenceData.ResidualAssessmentPrice2EffectiveDate : learnerReferenceData.ResidualTrainingPrice2EffectiveDate
                });
                financialRecords.Add(new FinancialRecord
                {
                    Code = 4,
                    Type = "TNP",
                    Amount = learnerReferenceData.ResidualAssessmentPrice2 == 0 ? learnerReferenceData.ResidualAssessmentPrice1 : learnerReferenceData.ResidualAssessmentPrice2,
                    Date = learnerReferenceData.ResidualAssessmentPrice2EffectiveDate == DateTime.MinValue ? learnerReferenceData.ResidualTrainingPrice2EffectiveDate : learnerReferenceData.ResidualAssessmentPrice2EffectiveDate
                });
            }

            ////////////////////////////////////////////////////////////////////
            // Old style agreed price
            ////////////////////////////////////////////////////////////////////
            if (financialRecords.Count == 0)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 1,
                    Type = "TNP",
                    Amount = agreedTrainingPrice,
                    Date = learnerReferenceData.StartDate
                });
                if (learnerReferenceData.FrameworkCode <= 0)
                {
                    financialRecords.Add(new FinancialRecord
                    {
                        Code = 2,
                        Type = "TNP",
                        Amount = agreedAssesmentPrice,
                        Date = learnerReferenceData.StartDate
                    });
                }
            }

            return financialRecords.ToArray();
        }

        private static LearningDeliveryFamRecord[] BuildLearningDeliveryFamCodes(IlrLearnerReferenceData learnerDetails,
            List<ContractTypeReferenceData> contractTypes, List<LearningSupportReferenceData> learningSupportStatus)
        {
            var learningEndDate = (!learnerDetails.ActualEndDate.HasValue || learnerDetails.PlannedEndDate > learnerDetails.ActualEndDate.Value)
                ? learnerDetails.PlannedEndDate : learnerDetails.ActualEndDate.Value;

            var actFamCodes = BuildActFamCodes(learnerDetails.LearnerType, learnerDetails.StartDate, learningEndDate, contractTypes);
            var lsfFamCodes = BuildLsfFamCodes(learningSupportStatus);
            var eefFamCodes = BuildEefFamCodes(learnerDetails);
            var restartFamCode = BuildRestartIndicatorFamCode(learnerDetails);

            return actFamCodes.Concat(lsfFamCodes).Concat(eefFamCodes).Concat(restartFamCode).ToArray();
        }

        private static LearningDeliveryFamRecord[] BuildActFamCodes(LearnerType learnerType, DateTime learningStart, DateTime learningEnd, List<ContractTypeReferenceData> contractTypes)
        {
            if (contractTypes.Any())
            {
                return contractTypes.Select(x => new LearningDeliveryFamRecord
                {
                    FamType = FamCodeAct,
                    Code = x.ContractType == ContractType.ContractWithEmployer ? FamCodeActDasValue.ToString() : FamCodeActNonDasValue.ToString(),
                    From = x.DateFrom,
                    To = x.DateTo
                }).ToArray();
            }
            return new[]
            {
                new LearningDeliveryFamRecord
                {
                    FamType = FamCodeAct,
                    Code = IsLearnerTypeLevy(learnerType) ? FamCodeActDasValue.ToString() : FamCodeActNonDasValue.ToString(),
                    From = learningStart,
                    To = learningEnd
                }
            };
        }

        private static LearningDeliveryFamRecord[] BuildLsfFamCodes(List<LearningSupportReferenceData> learningSupportStatus)
        {
            return learningSupportStatus.Select(s => new LearningDeliveryFamRecord
            {
                FamType = "LSF",
                Code = s.LearningSupportCode.ToString(),
                From = s.DateFrom,
                To = s.DateTo
            }).ToArray();
        }

        private static LearningDeliveryFamRecord[] BuildEefFamCodes(IlrLearnerReferenceData learnerDetails)
        {
            if (learnerDetails.LearnDelFam == null || 
                !learnerDetails.LearnDelFam.ToUpper().StartsWith("EEF"))
            {
                return new LearningDeliveryFamRecord[0];
            }

            return new[]
            {
                new LearningDeliveryFamRecord
                {
                    FamType = "EEF",
                    Code = learnerDetails.LearnDelFam.Substring(3),
                    //From = learnerDetails.StartDate,
                    //To = learnerDetails.PlannedEndDate,
                }
            };
        }

        private static IEnumerable<LearningDeliveryFamRecord> BuildRestartIndicatorFamCode(IlrLearnerReferenceData learnerDetails)
        {
            if (!learnerDetails.RestartIndicator)
            {
                return new LearningDeliveryFamRecord[0];
            }

            return new[]
            {
                new LearningDeliveryFamRecord
                {
                    FamType = "RES",
                    Code = "RES1",
                    //From = learnerDetails.StartDate,
                    //To = learnerDetails.PlannedEndDate,
                }
            };
        }

        private static bool IsLearnerTypeLevy(LearnerType learnerType)
        {
            if (learnerType == LearnerType.ProgrammeOnlyDas
                || learnerType == LearnerType.ProgrammeOnlyDas1618
                || learnerType == LearnerType.ProgrammeOnlyDas1924)
            {
                return true;
            }
            return false;
        }

        private static DateTime GetDateOfBirthBasedOnLearnerType(LearnerType learnerType)
        {
            if (learnerType == LearnerType.ProgrammeOnlyDas1618 || learnerType == LearnerType.ProgrammeOnlyNonDas1618)
            {
                return DateTime.Today.AddYears(-17);
            }
            if (learnerType == LearnerType.ProgrammeOnlyDas1924 || learnerType == LearnerType.ProgrammeOnlyNonDas1924)
            {
                return DateTime.Today.AddYears(-20);
            }
            return DateTime.Today.AddYears(-25);
        }
        
        private class ProviderSubmissionDetails
        {
            public string ProviderId { get; set; }
            public IlrLearnerReferenceData[] LearnerDetails { get; set; }
            public long Ukprn { get; set; }
            public string SubmissionPeriod { get; set; }
        }

        private class LoggingStatusWatcher : StatusWatcherBase
        {
            private readonly string _processName;

            public LoggingStatusWatcher(string processName)
            {
                _processName = processName;
            }

            public override void ExecutionStarted(TaskDescriptor[] tasks)
            {
                TestEnvironment.Logger.Info($"Started execution of {_processName}");
            }

            public override void TaskStarted(string taskId)
            {
                TestEnvironment.Logger.Info($"Started task {taskId} of {_processName}");
            }

            public override void TaskCompleted(string taskId, Exception error)
            {
                if (error != null)
                {
                    TestEnvironment.Logger.Error(error, $"Error running task {taskId} or {_processName}");
                }
                else
                {
                    TestEnvironment.Logger.Info($"Completed task {taskId} of {_processName}");
                }
            }

            public override void ExecutionCompleted(Exception error)
            {
                if (error != null)
                {
                    TestEnvironment.Logger.Error(error, $"Error running {_processName}");
                }
                else
                {
                    TestEnvironment.Logger.Info($"Completed execution of {_processName}");
                }
            }
        }
    }
}
