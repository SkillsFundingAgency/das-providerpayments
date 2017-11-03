using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using SFA.DAS.Payments.Automation.IlrBuilder;
using AimType = SFA.DAS.Payments.Automation.IlrBuilder.AimType;
using CompletionStatus = SFA.DAS.Payments.Automation.IlrBuilder.CompletionStatus;
using SFA.DAS.Payments.Automation.Application.Submission;
using SFA.DAS.Payments.Automation.Application.Entities;
using SFA.DAS.Payments.Automation.Lars;
using SFA.DAS.Payments.Automation.Application.GetAllUsedUlns;

namespace SFA.DAS.Payments.Automation.Application.CreateSubmissionCommand
{
    public class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommandRequest, CreateSubmissionCommandResponse>
    {
        private const string FamCodeAct = "ACT";
        private const short FamCodeActDasValue = 1;
        private const short FamCodeActNonDasValue = 2;


        private readonly IMediator _mediator;
        private readonly ISqlRefenceDataGenerator _refernceDataGenerator;
        private readonly ILarsRepository _larsRepository;


        public CreateSubmissionCommandHandler(IMediator mediator, ISqlRefenceDataGenerator refernceDataGenerator, ILarsRepository larsRepository)
        {
            _mediator = mediator;
            _refernceDataGenerator = refernceDataGenerator;
            _larsRepository = larsRepository;
        }


        public CreateSubmissionCommandResponse Handle(CreateSubmissionCommandRequest message)
        {
            try
            {
                var writer = new IndividualLearningRecordWriter(message.AcademicYear);
                var learningRecord = new IndividualLearningRecord();

                if (!message.LearnerRecords.Any())
                {
                    throw new Exception("No learners data passed in");
                }

                var response = new CreateSubmissionCommandResponse();

                List<AccountEntity> accounts = null;
                List<CommitmentEntity> commitments = null;

                if (message.Accounts.Any())
                {
                    accounts = GetAccountEntities(message.Accounts);
                    response.AccountsSql = _refernceDataGenerator.GenerateAccountsReferenceDataScript(accounts);

                }
                if (message.Commitments.Any())
                {
                    commitments = GetCommitmentEntities(message.Commitments, accounts, message.Ukprn, message.LearnerScenarios);
                    response.CommitmentsSql = _refernceDataGenerator.GenerateCommitmentsReferenceDataScript(commitments);
                }

                learningRecord.Ukprn = message.Ukprn;
                learningRecord.AcademicYear = message.AcademicYear;
                learningRecord.PreparationDate = DateTime.Now; // message.LearnerRecords.Max(x => x.ActualEndDate.HasValue ? x.ActualEndDate.Value : x.PlannedEndDate);

                var learnerRecordsGroups = message.LearnerRecords.GroupBy(x => x.LearnerKey).Select(x => x.ToArray()).ToArray();
                foreach (var l in learnerRecordsGroups)
                {
                    var learner = BuildLearner(l, message.ContractTypes, message.LearningSupports, message.LearnerScenarios, message.Ukprn);
                    learner.EmploymentStatuses = GetEmploymentStatuses(message.EmploymentStatuses, accounts);

                    learningRecord.Learners.Add(learner);
                }

                response.IlrContent = writer.GetDocumentContent(learningRecord);
                response.UsedUlnCsv = GetUsedUlnCsvContent(message.Ukprn);
                if (commitments != null && commitments.Any())
                {
                    response.CommitmentsBulkCsv = GetCommitmentsBulkCsv(learningRecord.Learners, commitments);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new CreateSubmissionCommandResponse
                {
                    Error = ex
                };
            }
        }

        private List<AccountEntity> GetAccountEntities(List<EmployerAccountLevyBalances> accounts)
        {
            var items = new List<AccountEntity>();
            var accountId = 0;

            foreach (var account in accounts)
            {
                if (!items.Any(x => x.AccountName.Equals(account.EmployerKey, StringComparison.CurrentCultureIgnoreCase)))
                {
                    accountId += 1;
                    items.Add(
                        new AccountEntity
                        {
                            AccountName = account.EmployerKey,
                            VersionId = "1",
                            AccountHashId = account.EmployerKey,
                            Balance = account.BalanceForAllPeriods.Value,
                            AccountId = accountId,
                            IsLevyPayer = true
                        });
                }
            }

            return items;

        }


        private List<CommitmentEntity> GetCommitmentEntities(List<Commitment> commitments,
                                                            List<AccountEntity> accountEntities,
                                                            long ukprn,
                                                             Dictionary<string, string> learnerScenarios)
        {
            var items = new List<CommitmentEntity>();
            var commitmentId = 0;

            foreach (var c in commitments)
            {
                commitmentId += 1;
                items.Add(
                    new CommitmentEntity
                    {
                        AccountId = accountEntities.Single(x => x.AccountName.Equals(c.EmployerKey, StringComparison.CurrentCultureIgnoreCase)).AccountId,
                        AgreedPrice = c.AgreedPrice,
                        CommitmentId = commitmentId,
                        EffectiveFrom = c.EffectiveFrom,
                        EffectiveTo = c.EffectiveTo,
                        EmployerKey = c.EmployerKey,
                        EndDate = c.EndDate,
                        FrameworkCode = c.FrameworkCode,
                        LearnerKey = c.LearnerKey,
                        PathwayCode = c.PathwayCode,
                        Priority = c.Priority,
                        ProgrammeType = c.ProgrammeType,
                        ProviderKey = c.ProviderKey,
                        StandardCode = c.StandardCode,
                        StartDate = c.StartDate,
                        Status = (int)c.Status,
                        Ukprn = ukprn,
                        Uln = GetNextUln(c.LearnerKey, learnerScenarios[c.LearnerKey], ukprn),
                        VersionId = c.VersionId
                    }
                    );
            }

            return items;

        }

        private string GetAcademicYear(DateTime date)
        {
            var startYear = date.Year - 2000;

            if (date.Month < 8)
            {
                startYear -= 1;
            }

            return $"{startYear}{startYear + 1}";
        }

        private string GetUsedUlnCsvContent(long ukprn)
        {

            var response = _mediator.Send(new GetAllUsedUlnsQueryRequest
            {
            });

            if (response != null)
            {

                if (!response.IsSuccess)
                {
                    throw new Exception("Error creating content response", response.Error);
                }
                else
                {
                    return response.UsedUlns.Where(x => x.Ukprn == ukprn).ToCSV();
                }
            }
            return string.Empty;
        }

        private Learner BuildLearner(LearnerRecord[] learnerRecords,
                                    List<ContractTypeRecord> contractTypes,
                                    List<LearningSupportRecord> learningSupports,
                                    Dictionary<string, string> learnerScenarios,
                                    long ukprn)
        {
            var learningDeliveries = new List<LearningDelivery>();

            var onProgrammeRecords = learnerRecords.Where(r => r.AimType == Domain.Specifications.AimType.Programme).ToArray();
            foreach (var progRecord in onProgrammeRecords)
            {
                learningDeliveries.Add(GetLearningDeliveryRecord(progRecord, AimType.OnProgramme, contractTypes, learningSupports, progRecord.AimType));

                var componentRecords = learnerRecords.Where(r => r.AimType == Domain.Specifications.AimType.MathsOrEnglish && r.StartDate == progRecord.StartDate).ToArray();
                if (componentRecords.Any())
                {
                    foreach (var compRecord in componentRecords)
                    {
                        learningDeliveries.Add(GetLearningDeliveryRecord(compRecord, AimType.Component, contractTypes, learningSupports, compRecord.AimType));
                    }
                }
                else
                {
                    learningDeliveries.Add(GetLearningDeliveryRecord(progRecord, AimType.Component, contractTypes, learningSupports, progRecord.AimType));
                }
            }

            var scenarioName = learnerScenarios[learnerRecords[0].LearnerKey];
            return new Learner
            {
                LearnerRefNumber = learnerRecords[0].LearnerKey,
                Uln = GetNextUln(learnerRecords[0].LearnerKey, scenarioName, ukprn),
                DateOfBirth = GetDateOfBirthBasedOnLearnerType(learnerRecords[0].LearnerType),
                LearningDeliveries = learningDeliveries,
            };


        }

        private LearningDelivery GetLearningDeliveryRecord(LearnerRecord record,
                                                            AimType deliveryAimType,
                                                            List<ContractTypeRecord> contractTypes,
                                                            List<LearningSupportRecord> learningSupports,
                                                            Domain.Specifications.AimType specAimType)
        {
            var financialRecords = BuildLearningDeliveryFinancials(record);

            var ld = new LearningDelivery
            {
                StandardCode = record.StandardCode,
                FrameworkCode = record.FrameworkCode,
                ProgrammeType = record.ProgrammeType,
                PathwayCode = record.PathwayCode,
                LearningStartDate = record.StartDate,
                PlannedLearningEndDate = record.PlannedEndDate,
                ActualLearningEndDate = record.ActualEndDate,
                CompletionStatus = (CompletionStatus)(int)record.CompletionStatus,
                AimType = deliveryAimType,
                AimRef = GetAimRef(deliveryAimType, record.StandardCode, record.ProgrammeType, record.FrameworkCode, record.PathwayCode, record.StartDate, specAimType == Domain.Specifications.AimType.MathsOrEnglish),
                FinancialRecords = financialRecords.ToList()
            };

            if (ld.AimRef == "ZPROG001" || specAimType == Domain.Specifications.AimType.MathsOrEnglish)
            {
                ld.FundingAndMonitoringCodes = BuildLearningDeliveryFamCodes(record, contractTypes, learningSupports).ToList();
            }
            return ld;
        }

        private long GetNextUln(string learnerkey, string scenarioname, long ukprn)
        {

            var response = _mediator.Send(new GetNextUlnQueryRequest()
            {
                LearnerKey = learnerkey,
                Scenarioname = scenarioname,
                Ukprn = ukprn
            });
            if (response.IsSuccess)
            {
                return response.Value;
            }
            else
            {
                throw new Exception($"Failed to get next Uln for learnref {learnerkey}");
            }

        }
        private string GetAimRef(AimType aimType, long? standardCode, int? programmeType,
            int? frameworkCode, int? pathwayCode, DateTime effectiveDate, bool needEnglishAndMathsAim)
        {
            if (aimType == AimType.OnProgramme)
            {
                return "ZPROG001";
            }

            LearningAim[] aimRefs;
            if (standardCode.HasValue)
            {
                aimRefs = _larsRepository.GetComponentLearningAims(standardCode.Value, effectiveDate);
                if (aimRefs.Length == 0)
                {
                    throw new Exception($"Cannot find any aim refs for standard {standardCode.Value} effective at {effectiveDate}");
                }
            }
            else
            {
                aimRefs = _larsRepository.GetComponentLearningAims(frameworkCode.Value, programmeType.Value, pathwayCode.Value, effectiveDate);
                if (aimRefs.Length == 0)
                {
                    throw new Exception($"Cannot find any aim refs for prog {programmeType.Value}, fwk {frameworkCode.Value}, pway {pathwayCode.Value} effective at {effectiveDate}");
                }
            }

            if (needEnglishAndMathsAim)
            {
                aimRefs = aimRefs.Where(ar => ar.IsEnglishAndMathsAim).ToArray();
            }

            return aimRefs.First(ar => ar.IsEnglishAndMathsAim == needEnglishAndMathsAim).AimRef;
        }

        private FinancialRecord[] BuildLearningDeliveryFinancials(LearnerRecord learnerReferenceData)
        {
            var agreedTrainingPrice = learnerReferenceData.FrameworkCode > 0 ? learnerReferenceData.TotalTrainingPrice1 :
                                     (int)Math.Floor(learnerReferenceData.TotalTrainingPrice1 * 0.8m);
            var agreedAssesmentPrice = learnerReferenceData.TotalTrainingPrice1 - agreedTrainingPrice;

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
                    From = learnerReferenceData.TotalTrainingPrice1EffectiveDate == DateTime.MinValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalTrainingPrice1EffectiveDate
                });
            }
            if (learnerReferenceData.TotalAssessmentPrice1.HasValue)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 2,
                    Type = "TNP",
                    Amount = learnerReferenceData.TotalAssessmentPrice1.Value,
                    From = !learnerReferenceData.TotalAssessmentPrice1EffectiveDate.HasValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalAssessmentPrice1EffectiveDate.Value
                });
            }

            if (learnerReferenceData.TotalTrainingPrice2.HasValue && learnerReferenceData.TotalTrainingPrice2.Value != learnerReferenceData.TotalTrainingPrice1)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 1,
                    Type = "TNP",
                    Amount = learnerReferenceData.TotalTrainingPrice2.Value,
                    From = !learnerReferenceData.TotalTrainingPrice2EffectiveDate.HasValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalTrainingPrice2EffectiveDate.Value
                });
            }
            if (learnerReferenceData.TotalAssessmentPrice2.HasValue && learnerReferenceData.TotalAssessmentPrice2 != learnerReferenceData.TotalAssessmentPrice1)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 2,
                    Type = "TNP",
                    Amount = learnerReferenceData.TotalAssessmentPrice2.Value,
                    From = !learnerReferenceData.TotalAssessmentPrice2EffectiveDate.HasValue ? learnerReferenceData.StartDate : learnerReferenceData.TotalAssessmentPrice2EffectiveDate.Value
                });
            }


            ////////////////////////////////////////////////////////////////////
            // TNP3 & 4
            ////////////////////////////////////////////////////////////////////
            if (learnerReferenceData.ResidualTrainingPrice1.HasValue || learnerReferenceData.ResidualAssessmentPrice1.HasValue)
            {
                financialRecords.Add(new FinancialRecord
                {
                    Code = 3,
                    Type = "TNP",
                    Amount = learnerReferenceData.ResidualTrainingPrice1.Value,
                    From = !learnerReferenceData.ResidualTrainingPrice1EffectiveDate.HasValue ? learnerReferenceData.StartDate : learnerReferenceData.ResidualTrainingPrice1EffectiveDate.Value
                });
                financialRecords.Add(new FinancialRecord
                {
                    Code = 4,
                    Type = "TNP",
                    Amount = learnerReferenceData.ResidualAssessmentPrice1.Value,
                    From = !learnerReferenceData.ResidualAssessmentPrice1EffectiveDate.HasValue ? learnerReferenceData.StartDate : learnerReferenceData.ResidualAssessmentPrice1EffectiveDate.Value
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
                    Amount = !learnerReferenceData.ResidualTrainingPrice2.HasValue ? learnerReferenceData.ResidualTrainingPrice1.Value : learnerReferenceData.ResidualTrainingPrice2.Value,
                    From = !learnerReferenceData.ResidualTrainingPrice2EffectiveDate.HasValue ? learnerReferenceData.ResidualAssessmentPrice2EffectiveDate.Value : learnerReferenceData.ResidualTrainingPrice2EffectiveDate.Value
                });
                financialRecords.Add(new FinancialRecord
                {
                    Code = 4,
                    Type = "TNP",
                    Amount = !learnerReferenceData.ResidualAssessmentPrice2.HasValue ? learnerReferenceData.ResidualAssessmentPrice1.Value : learnerReferenceData.ResidualAssessmentPrice2.Value,
                    From = !learnerReferenceData.ResidualAssessmentPrice2EffectiveDate.HasValue ? learnerReferenceData.ResidualTrainingPrice2EffectiveDate.Value : learnerReferenceData.ResidualAssessmentPrice2EffectiveDate.Value
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
                    From = learnerReferenceData.StartDate
                });
                if (!learnerReferenceData.FrameworkCode.HasValue || learnerReferenceData.FrameworkCode <= 0)
                {
                    financialRecords.Add(new FinancialRecord
                    {
                        Code = 2,
                        Type = "TNP",
                        Amount = agreedAssesmentPrice,
                        From = learnerReferenceData.StartDate
                    });
                }
            }

            return financialRecords.ToArray();
        }
        private FundingAndMonitoringCode[] BuildLearningDeliveryFamCodes(LearnerRecord learnerDetails,
            List<ContractTypeRecord> contractTypes, List<LearningSupportRecord> learningSupportStatus)
        {
            var learningEndDate = (!learnerDetails.ActualEndDate.HasValue || learnerDetails.PlannedEndDate > learnerDetails.ActualEndDate.Value)
                ? learnerDetails.PlannedEndDate : learnerDetails.ActualEndDate.Value;

            var actFamCodes = BuildActFamCodes(learnerDetails.LearnerType, learnerDetails.StartDate, learningEndDate, contractTypes);
            var lsfFamCodes = BuildLsfFamCodes(learningSupportStatus);
            var eefFamCodes = BuildEefFamCodes(learnerDetails);

            return actFamCodes.Concat(lsfFamCodes).Concat(eefFamCodes).ToArray();
        }
        private FundingAndMonitoringCode[] BuildActFamCodes(LearnerType learnerType, DateTime learningStart, DateTime learningEnd, List<ContractTypeRecord> contractTypes)
        {
            if (contractTypes.Any())
            {
                return contractTypes.Select(x => new FundingAndMonitoringCode
                {
                    Type = FamCodeAct,
                    Code = x.ContractType == ContractType.ContractWithEmployer ? FamCodeActDasValue : FamCodeActNonDasValue,
                    From = x.DateFrom,
                    To = x.DateTo
                }).ToArray();
            }
            return new[]
            {
                new FundingAndMonitoringCode
                {
                    Type = FamCodeAct,
                    Code = IsLearnerTypeLevy(learnerType) ? FamCodeActDasValue : FamCodeActNonDasValue,
                    From = learningStart,
                    To = learningEnd
                }
            };
        }
        private FundingAndMonitoringCode[] BuildLsfFamCodes(List<LearningSupportRecord> learningSupportStatus)
        {
            return learningSupportStatus.Select(s => new FundingAndMonitoringCode
            {
                Type = "LSF",
                Code = s.LearningSupportCode,
                From = s.DateFrom,
                To = s.DateTo
            }).ToArray();
        }
        private FundingAndMonitoringCode[] BuildEefFamCodes(LearnerRecord learnerDetails)
        {
            if (learnerDetails.LearnDelFam == null || !learnerDetails.LearnDelFam.ToUpper().StartsWith("EEF"))
            {
                return new FundingAndMonitoringCode[0];
            }

            return new[]
            {
                new FundingAndMonitoringCode
                {
                    Type = "EEF",
                    Code = int.Parse(learnerDetails.LearnDelFam.Substring(3)),
                    //From = learnerDetails.StartDate,
                    //To = learnerDetails.PlannedEndDate
                }
            };
        }
        private bool IsLearnerTypeLevy(LearnerType learnerType)
        {
            if (learnerType == LearnerType.ProgrammeOnlyDas
                || learnerType == LearnerType.ProgrammeOnlyDas1618
                || learnerType == LearnerType.ProgrammeOnlyDas1924)
            {
                return true;
            }
            return false;
        }
        private DateTime GetDateOfBirthBasedOnLearnerType(LearnerType learnerType)
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

        private List<IlrBuilder.EmploymentStatus> GetEmploymentStatuses(List<LearnerEmploymentStatus> employmentStatus, List<AccountEntity> accounts)
        {

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
                return new IlrBuilder.EmploymentStatus
                {
                    DateFrom = s.EmploymentStatusApplies,
                    EmploymentStatusMonitoring = monitoringStatus,
                    StatusCode = (int)s.EmploymentStatus
                };
            });

            if (employmentStatuses.Count() > 1)
            {
                var statuses = employmentStatuses.ToArray();
                statuses[0].EmployerId = 101183291;
                statuses[1].EmployerId = 101322135;

                return statuses.ToList();
            }


            return employmentStatuses.ToList();
        }


        private string GetCommitmentsBulkCsv(List<Learner> learners, List<CommitmentEntity> commitments)
        {
            var data = commitments.Select(x =>
                new CommitmentBulkLoadEntity
                {
                    Uln = x.Uln.ToString(),
                    FamilyName = learners.Single(l => l.Uln == x.Uln).FamilyName,
                    GivenNames = learners.Single(l => l.Uln == x.Uln).GivenNames,
                    DateOfBirth = learners.Single(l => l.Uln == x.Uln).DateOfBirth.ToString("yyyy-MM-dd"),
                    ProgType = x.ProgrammeType.ToString(),
                    FworkCode = x.FrameworkCode.ToString(),
                    PwayCode = x.PathwayCode.ToString(),
                    StdCode = x.StandardCode.ToString(),
                    StartDate = x.StartDate.ToString("yyyy-MM"),
                    EndDate = x.EndDate.ToString("yyyy-MM"),
                    TotalPrice = x.AgreedPrice.ToString()
                }
                );
            return data.ToCSV();
        }


    }
}