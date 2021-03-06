﻿using System;
using System.Collections.Generic;
using System.Linq;
using IlrGenerator;
using ProviderPayments.TestStack.Core.ExecutionStatus;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.DataCollectors;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;
using System.IO;
using ProviderPayments.TestStack.Core.Domain;

namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    internal static class SubmissionManager
    {
        private const string FamCodeAct = "ACT";
        private const short FamCodeActDasValue = 1;
        private const short FamCodeActNonDasValue = 2;

        internal static List<LearnerResults> SubmitMultipleIlrAndRunMonthEndAndCollateResults(SubmissionContext multipleSubmissionsContext, LookupContext lookupContext, List<EmployerAccountReferenceData> employerAccounts, DateTime lastAssertionPeriodDate, CommitmentsContext commitmentsContext)
        {
            var results = new List<LearnerResults>();
            if (TestEnvironment.ValidateSpecsOnly)
            {
                return results;
            }

            var periods = new List<string>();
            foreach (var submission in multipleSubmissionsContext.Submissions)
            {
                periods.AddRange(ExtractPeriods(submission.IlrLearnerDetails, submission.FirstSubmissionDate, lastAssertionPeriodDate));
            }
            periods = periods.Distinct().ToList();
            
            foreach (var period in periods)
            {
                SetEnvironmentToPeriod(period);
                EmployerAccountManager.UpdateAccountBalancesForPeriod(employerAccounts, period);
                EmployerAccountManager.UpdateTransferAllowancesForPeriod(employerAccounts, period);

                ReplaceCommitmentsWithNewPeriodCommitments(commitmentsContext, period);

                foreach (var submission in multipleSubmissionsContext.Submissions)
                {
                    if(submission.IlrLearnerDetails.Select(details => details.Provider).Distinct().Count() > 1)
                        throw new Exception("Multiple Providers in the same ILR is invalid.");
                    if (!string.IsNullOrEmpty(submission.SubmissionPeriod) && !string.Equals(submission.SubmissionPeriod, period,
                            StringComparison.CurrentCultureIgnoreCase)
                    )
                        continue;

                    var submissionDetails = new ProviderSubmissionDetails
                    {
                        LearnerDetails = submission.IlrLearnerDetails.ToArray(),
                        ProviderId = submission.IlrLearnerDetails.FirstOrDefault()?.Provider,
                        Ukprn = lookupContext.AddOrGetUkprn(submission.IlrLearnerDetails.FirstOrDefault()?.Provider)
                    };

                    SetupDisadvantagedPostcodeUplift(submissionDetails);
                    BuildAndSubmitIlr(submissionDetails, period, lookupContext, submission.ContractTypes, submission.EmploymentStatus,
                        submission.LearningSupportStatus);
                    submission.HaveSubmissionsBeenDone = true;
                }
                
                RunMonthEnd(period);

                EarningsCollector.CollectForPeriod(period, results, lookupContext);
                LevyAccountBalanceCollector.CollectForPeriod(period, results, lookupContext);
                SubmissionDataLockResultCollector.CollectForPeriod(period, results, lookupContext);

                SavedDataCollector.CaptureAccountsDataForScenario();
                SavedDataCollector.CaptureCommitmentsDataForScenario();
            }

            DataLockEventsDataCollector.CollectDataLockEventsForAllPeriods(results, lookupContext);
            PaymentsDataCollector.CollectForPeriod(results, lookupContext);

            multipleSubmissionsContext.TransferResults = TransfersDataCollector.CollectAllTransfers();

            return results;
        }

        private static void ReplaceCommitmentsWithNewPeriodCommitments(CommitmentsContext commitmentsContext, string period)
        {
            if (commitmentsContext.CommitmentsForPeriod.ContainsKey(period))
            {
                var commitments = commitmentsContext.CommitmentsForPeriod[period];

                CommitmentManager.DeleteCommitments();
                foreach (var commitment in commitments)
                {
                    CommitmentManager.AddCommitment(commitment);
                }
                commitmentsContext.Commitments = commitments;
                commitmentsContext.CommitmentsForPeriod.Remove(period);
            }
        }

        [Obsolete("Superceeded by SubmitMultipleIlrAndRunMonthEndAndCollateResults()")]
        internal static List<LearnerResults> SubmitIlrAndRunMonthEndAndCollateResults(
            List<IlrLearnerReferenceData> ilrLearnerDetails,
            DateTime? firstSubmissionDate,
            LookupContext lookupContext,
            List<EmployerAccountReferenceData> employers,
            List<ContractTypeReferenceData> contractTypes,
            List<EmploymentStatusReferenceData> employmentStatus,
            List<LearningSupportReferenceData> learningSupportStatus,
            CommitmentsContext commitmentsContext,
            List<string> periodsToSubmitTo = null,
            DateTime? lastAssertionPeriodDate = null)
        {
            var results = new List<LearnerResults>();
            if (TestEnvironment.ValidateSpecsOnly)
            {
                return results;
            }
            
            var periods = periodsToSubmitTo ?? ExtractPeriods(ilrLearnerDetails, firstSubmissionDate, lastAssertionPeriodDate);
            var providerLearners = GroupLearnersByProvider(ilrLearnerDetails, lookupContext);
            foreach (var period in periods)
            {
                SetEnvironmentToPeriod(period);
                EmployerAccountManager.UpdateAccountBalancesForPeriod(employers, period);

                ReplaceCommitmentsWithNewPeriodCommitments(commitmentsContext, period);

                foreach (var providerDetails in providerLearners)
                {
                    SetupDisadvantagedPostcodeUplift(providerDetails);
                    BuildAndSubmitIlr(providerDetails, period, lookupContext, contractTypes, employmentStatus,
                        learningSupportStatus);
                }
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

        private static List<string> ExtractPeriods(List<IlrLearnerReferenceData> ilrLearnerDetails, DateTime? firstSubmissionDate, DateTime? lastAssertionPeriodDate)
        {
            var periods = new List<string>();

            var earliestDate = ilrLearnerDetails.Select(x => x.StartDate).Min();
            var latestPlannedDate = ilrLearnerDetails.Select(x => x.PlannedEndDate).Max();
            var latestActualDate = ilrLearnerDetails.Select(x => x.ActualEndDate).Max();
            var latestDate = latestActualDate.HasValue && latestActualDate > latestPlannedDate ? latestActualDate : latestPlannedDate;
            if (lastAssertionPeriodDate.HasValue && lastAssertionPeriodDate < latestDate)
                latestDate = lastAssertionPeriodDate.Value.AddMonths(1).AddDays(-1);
            if (firstSubmissionDate.HasValue && firstSubmissionDate > latestDate)
                latestDate = firstSubmissionDate.Value.AddMonths(1).AddDays(-1);

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

            var minExplicitSubmissionPeriod = ilrLearnerDetails.Min(x => PeriodNameHelper.GetDateFromPeriodName(x.SubmissionPeriod, earliestDate));
            var maxExplicitSubmissionPeriod = ilrLearnerDetails.Max(x => PeriodNameHelper.GetDateFromPeriodName(x.SubmissionPeriod, earliestDate));

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

            return periods;
        }

        private static ProviderSubmissionDetails[] GroupLearnersByProvider(List<IlrLearnerReferenceData> ilrLearnerDetails, LookupContext lookupContext)
        {
            return (ilrLearnerDetails.GroupBy(x => x.Provider)
                .Select(g => new ProviderSubmissionDetails
                {
                    ProviderId = g.Key,
                    Ukprn = lookupContext.AddOrGetUkprn(g.Key),
                    LearnerDetails = g.ToArray()
                })).ToArray();
        }

        private static void SetEnvironmentToPeriod(string period)
        {
            var month = int.Parse(period.Substring(0, 2));
            var year = int.Parse(period.Substring(3, 2)) + 2000;
            var date = new DateTime(year, month, 1);
            var periodNumber = date.GetPeriodNumber();

            TestEnvironment.Variables.CurrentYear = date.GetAcademicYear();
            TestEnvironment.Variables.CollectionPeriod = new CollectionPeriod
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

            var periodName = PeriodNameHelper.GetPeriodFromStringDate(period);
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
                    var financialRecords = BuildLearningDeliveryFinancials(x, endOfPeriod);

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
                DateOfBirth = GetDateOfBirthBasedOnLearnerType(learnerDetails[0].LearnerType, learnerDetails[0].StartDate),
                LearningDeliveries = deliveries,
                EmploymentStatuses = employmentStatuses.Any() ? employmentStatuses : null
            };

            long uln;
            long.TryParse(learnerDetails[0].Uln, out uln);

            learner.Uln = uln > 0 ? uln : lookupContext.AddOrGetUln(learnerDetails[0].LearnerReference);
        
            return learner;
        }

        private static FinancialRecord[] BuildLearningDeliveryFinancials(IlrLearnerReferenceData learnerReferenceData, DateTime endOfPeriod)
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

            if (learnerReferenceData.EmployerContribution > 0)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 1,
                    Type = "PMR",
                    Amount = learnerReferenceData.EmployerContribution,
                    Date = endOfPeriod.Date
                });
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
            var eefFamCodes = BuildEefAndLdmFamCodes(learnerDetails);
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
                    Code = ((int)x.ContractType).ToString(),
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

        private static LearningDeliveryFamRecord[] BuildEefAndLdmFamCodes(IlrLearnerReferenceData learnerDetails)
        {
            if (learnerDetails.LearnDelFam == null || 
                !learnerDetails.LearnDelFam.ToUpper().StartsWith("EEF") && !learnerDetails.LearnDelFam.ToUpper().StartsWith("LDM"))
            {
                return new LearningDeliveryFamRecord[0];
            }

            return new[]
            {
                new LearningDeliveryFamRecord
                {
                    FamType = learnerDetails.LearnDelFam.Substring(0,3),
                    Code = learnerDetails.LearnDelFam.Substring(3)
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

        private static DateTime GetDateOfBirthBasedOnLearnerType(LearnerType learnerType, DateTime startDate)
        {
            if (learnerType == LearnerType.ProgrammeOnlyDas1618 || learnerType == LearnerType.ProgrammeOnlyNonDas1618)
            {
                return startDate.AddYears(-17);
            }
            if (learnerType == LearnerType.ProgrammeOnlyDas1924 || learnerType == LearnerType.ProgrammeOnlyNonDas1924)
            {
                return startDate.AddYears(-20);
            }
            return startDate.AddYears(-25);
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
