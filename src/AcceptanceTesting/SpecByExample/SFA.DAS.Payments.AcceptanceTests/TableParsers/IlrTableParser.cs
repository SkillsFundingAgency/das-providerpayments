using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    public class IlrTableParser
    {
        private static FullIlrStructure structure;

        public static void ParseIlrTableIntoSubmission(List<IlrLearnerReferenceData> ilrLearnerDetails, Table ilrDetails)
        {
            if (ilrDetails.RowCount < 1)
            {
                throw new ArgumentException("ILR table must have at least 1 row");
            }

            structure = ParseTableStructure(ilrDetails);
            foreach (var row in ilrDetails.Rows)
            {
                var parsedLearner = ParseCommitmentsTableRow(row, structure.IlrTableStructure);
                if (parsedLearner != null)
                    ilrLearnerDetails.Add(parsedLearner);
            }
        }

        public static void ParseIlrTableIntoSubmission(Submission submission, Table ilrDetails, LookupContext lookupContext)
        {
            if (ilrDetails.RowCount < 1)
            {
                throw new ArgumentException("ILR table must have at least 1 row");
            }

            structure = ParseTableStructure(ilrDetails);
            foreach (var row in ilrDetails.Rows)
            {
                var parsedLearner = ParseCommitmentsTableRow(row, structure.IlrTableStructure);
                if (parsedLearner != null)
                {
                    long uln;
                    if (long.TryParse(parsedLearner.Uln, out uln))
                    {
                        lookupContext.AddUln(parsedLearner.LearnerReference, uln);
                    }
                    else
                    {
                        lookupContext.AddOrGetUln(parsedLearner.LearnerReference);
                    }

                    submission.IlrLearnerDetails.Add(parsedLearner);
                }

                var parsedLearningSupportStatus = ParseLearningSupportTableRow(row, structure.LearningSupportTableColumnStructure);
                if(parsedLearningSupportStatus != null)
                    submission.LearningSupportStatus.Add(parsedLearningSupportStatus);

                var parsedContractType = ParseContractTypeTableRow(row, structure.ContractTypesTableColumnStructure);
                if (parsedContractType != null)
                    submission.ContractTypes.Add(parsedContractType);

                var parsedEmploymentStatus = ParseEmploymentStatusTableRow(row, structure.EmploymentStatusTableColumnStructure);
                if (parsedEmploymentStatus != null)
                    submission.EmploymentStatus.Add(parsedEmploymentStatus);
            }
            
        }

        private static FullIlrStructure ParseTableStructure(Table ilrDetails)
        {
            var tableStructure = new IlrTableStructure();
            var learningSupportTableColumnStructure = new LearningSupportTableParser.LearningSupportTableColumnStructure();
            var contractTypeTableColumnStructure = new ContractTypeTableParser.ContractTypesTableColumnStructure();
            var employmentStatusTableColumnStructure = new EmploymentStatusTableParser.EmploymentStatusTableColumnStructure();

            for (var c = 0; c < ilrDetails.Header.Count; c++)
            {
                var header = ilrDetails.Header.ElementAt(c).ToLowerInvariant();
                switch (header)
                {
                    case "learner reference number":
                        tableStructure.LearnerReferenceIndex = c;
                        break;
                    case "uln":
                        tableStructure.UlnIndex = c;
                        break;
                    case "agreed price":
                        tableStructure.AgreedPriceIndex = c;
                        break;
                    case "learner type":
                        tableStructure.LearnerTypeIndex = c;
                        break;
                    case "start date":
                        tableStructure.StartDateIndex = c;
                        break;
                    case "planned end date":
                        tableStructure.PlannedEndDateIndex = c;
                        break;
                    case "actual end date":
                        tableStructure.ActualEndDateIndex = c;
                        break;
                    case "completion status":
                        tableStructure.CompletionStatusIndex = c;
                        break;
                    case "provider":
                        tableStructure.ProviderIndex = c;
                        break;
                    case "total training price":
                    case "total training price 1": // duplicate
                        tableStructure.TotalTrainingPrice1Index = c;
                        break;
                    case "total training price effective date":
                    case "total training price 1 effective date": // duplicate
                        tableStructure.TotalTrainingPrice1EffectiveDateIndex = c;
                        break;
                    case "total assessment price":
                    case "total assessment price 1": // duplicate
                        tableStructure.TotalAssessmentPrice1Index = c;
                        break;
                    case "total assessment price effective date":
                    case "total assessment price 1 effective date": // duplicate
                        tableStructure.TotalAssessmentPrice1EffectiveDateIndex = c;
                        break;
                    case "total training price 2":
                        tableStructure.TotalTrainingPrice2Index = c;
                        break;
                    case "total training price 2 effective date":
                        tableStructure.TotalTrainingPrice2EffectiveDateIndex = c;
                        break;
                    case "total assessment price 2":
                        tableStructure.TotalAssessmentPrice2Index = c;
                        break;
                    case "total assessment price 2 effective date":
                        tableStructure.TotalAssessmentPrice2EffectiveDateIndex = c;
                        break;
                    case "residual training price":
                    case "residual training price 1":
                        tableStructure.ResidualTrainingPrice1Index = c;
                        break;
                    case "residual training price effective date":
                    case "residual training price 1 effective date":
                        tableStructure.ResidualTrainingPrice1EffectiveDateIndex = c;
                        break;
                    case "residual assessment price":
                    case "residual assessment price 1":
                        tableStructure.ResidualAssessmentPrice1Index = c;
                        break;
                    case "residual assessment price effective date":
                    case "residual assessment price 1 effective date":
                        tableStructure.ResidualAssessmentPrice1EffectiveDateIndex = c;
                        break;
                    case "residual training price 2":
                        tableStructure.ResidualTrainingPrice2Index = c;
                        break;
                    case "residual training price 2 effective date":
                        tableStructure.ResidualTrainingPrice2EffectiveDateIndex = c;
                        break;
                    case "residual assessment price 2":
                        tableStructure.ResidualAssessmentPrice2Index = c;
                        break;
                    case "residual assessment price 2 effective date":
                        tableStructure.ResidualAssessmentPrice2EffectiveDateIndex = c;
                        break;
                    case "aim type":
                        tableStructure.AimTypeIndex = c;
                        break;
                    case "aim rate":
                        tableStructure.AimRateIndex = c;
                        break;
                    case "standard code":
                        tableStructure.StandardCodeIndex = c;
                        break;
                    case "framework code":
                        tableStructure.FrameworkCodeIndex = c;
                        break;
                    case "programme type":
                        tableStructure.ProgrammeTypeIndex = c;
                        break;
                    case "pathway code":
                        tableStructure.PathwayCodeIndex = c;
                        break;
                    case "home postcode deprivation":
                        tableStructure.HomePostcodeDeprivationIndex = c;
                        break;
                    case "employment status":
                        tableStructure.EmploymentStatusIndex = c;
                        break;
                    case "employment status applies":
                        tableStructure.EmploymentStatusAppliesIndex = c;
                        break;
                    case "employer id":
                        tableStructure.EmployerIdIndex = c;
                        break;
                    case "small employer":
                        tableStructure.SmallEmployerIndex = c;
                        break;
                    case "learndelfam":
                        tableStructure.LearnDelFamIndex = c;
                        break;
                    case "aim sequence number":
                        tableStructure.AimSequenceNumberIndex = c;
                        break;
                    case "aim reference":
                        tableStructure.LearnAimRefIndex = c;
                        break;
                    case "restart indicator":
                        tableStructure.RestartIndicatorIndex = c;
                        break;
                    case "funding adjustment for prior learning":
                        tableStructure.FundingAdjustmentForPriorLearningIndex = c;
                        break;
                    case "other funding adjustment":
                        tableStructure.OtherFundingAdjustmentIndex = c;
                        break;
                    case "learning support code":
                        learningSupportTableColumnStructure.LearningSupportCodeIndex = c;
                        break;
                    case "learning support date from":
                        learningSupportTableColumnStructure.DateFromIndex = c;
                        break;
                    case "learning support date to":
                        learningSupportTableColumnStructure.DateToIndex = c;
                        break;
                    case "contract type":
                        contractTypeTableColumnStructure.ContractTypeIndex = c;
                        break;
                    case "contract type date from":
                        contractTypeTableColumnStructure.DateFromIndex = c;
                        break;
                    case "contract type date to":
                        contractTypeTableColumnStructure.DateToIndex = c;
                        break;
                    case "employer":
                        employmentStatusTableColumnStructure.EmployerIndex = c;
                        break;
                    case "employer employment status":
                        employmentStatusTableColumnStructure.EmploymentStatusIndex = c;
                        break;
                    case "employer employment status applies":
                        employmentStatusTableColumnStructure.EmploymentStatusAppliesIndex = c;
                        break;
                    case "employer small employer":
                        employmentStatusTableColumnStructure.SmallEmployerIndex = c;
                        break;
                    default:
                        throw new ArgumentException($"Unexpected column in ILR table: {header}");
                }
            }

            return new FullIlrStructure
            {
                IlrTableStructure = tableStructure,
                LearningSupportTableColumnStructure = learningSupportTableColumnStructure,
                ContractTypesTableColumnStructure = contractTypeTableColumnStructure,
                EmploymentStatusTableColumnStructure = employmentStatusTableColumnStructure
            };
        }

        private static LearningSupportReferenceData ParseLearningSupportTableRow(TableRow row, LearningSupportTableParser.LearningSupportTableColumnStructure learningSupportTableColumnStructure)
        {
            if (string.IsNullOrWhiteSpace(row.ReadRowColumnValue<string>(learningSupportTableColumnStructure.LearningSupportCodeIndex, "Learning support code")))
                return null;

            return new LearningSupportReferenceData
            {
                LearningSupportCode = row.ReadRowColumnValue<int>(learningSupportTableColumnStructure.LearningSupportCodeIndex, "Learning support code"),
                DateFrom = row.ReadRowColumnValue<DateTime>(learningSupportTableColumnStructure.DateFromIndex, "date from"),
                DateTo = row.ReadRowColumnValue<DateTime>(learningSupportTableColumnStructure.DateToIndex, "date to")
            };
        }

        private static ContractTypeReferenceData ParseContractTypeTableRow(TableRow row, ContractTypeTableParser.ContractTypesTableColumnStructure contractTypesTableColumnStructure)
        {
            if (string.IsNullOrWhiteSpace(row.ReadRowColumnValue<string>(contractTypesTableColumnStructure.ContractTypeIndex, "contract type")))
                return null;

            return new ContractTypeReferenceData
            {
                ContractType = (ContractType)row.ReadRowColumnValue<string>(contractTypesTableColumnStructure.ContractTypeIndex, "contract type").ToEnumByDescription(typeof(ContractType)),
                DateFrom = row.ReadRowColumnValue<DateTime>(contractTypesTableColumnStructure.DateFromIndex, "date from"),
                DateTo = row.ReadRowColumnValue<DateTime?>(contractTypesTableColumnStructure.DateToIndex, "date to")
            };
        }

        private static EmploymentStatusReferenceData ParseEmploymentStatusTableRow(TableRow row, EmploymentStatusTableParser.EmploymentStatusTableColumnStructure structure)
        {
            var employerReference = row.ReadRowColumnValue<string>(structure.EmployerIndex, "Employer");
            var employmentStatus = row.ReadRowColumnValue<string>(structure.EmploymentStatusIndex, "Employment Status");

            if (string.IsNullOrWhiteSpace(employmentStatus))
            {
                return null;
            }

            int? employerId = null;

            if (!string.IsNullOrWhiteSpace(employerReference))
            {
                var employerMatch = Regex.Match(employerReference, "^employer ([0-9]{1,})$");
                if (!employerMatch.Success)
                {
                    throw new ArgumentException($"Employer '{employerReference}' is not a valid employer reference");
                }

                employerId = int.Parse(employerMatch.Groups[1].Value);
            }

            var status = new EmploymentStatusReferenceData
            {
                EmployerId = employerId,
                EmploymentStatus = (EmploymentStatus)employmentStatus.ToEnumByDescription(typeof(EmploymentStatus)),
                EmploymentStatusApplies = row.ReadRowColumnValue<DateTime>(structure.EmploymentStatusAppliesIndex, "Employment Status Applies")
            };

            var smallEmployer = row.ReadRowColumnValue<string>(structure.SmallEmployerIndex, "Small Employer");
            if (smallEmployer?.Length > 3)
            {
                status.MonitoringType = (EmploymentStatusMonitoringType)smallEmployer.Substring(0, 3).ToEnumByDescription(typeof(EmploymentStatusMonitoringType));
                status.MonitoringCode = int.Parse(smallEmployer.Substring(3));
            }

            return status;
        }

        private static IlrLearnerReferenceData ParseCommitmentsTableRow(TableRow row, IlrTableStructure tableStructure)
        {
            if (string.IsNullOrWhiteSpace(row.ReadRowColumnValue<string>(tableStructure.LearnerReferenceIndex, "learner reference number"))
                && string.IsNullOrWhiteSpace(row.ReadRowColumnValue<string>(tableStructure.UlnIndex, "ULN")))
            {
                return null;
            }
            var rowData = new IlrLearnerReferenceData
            {
                LearnerReference = row.ReadRowColumnValue(tableStructure.LearnerReferenceIndex, "learner reference number", string.Empty),
                Uln = row.ReadRowColumnValue(tableStructure.UlnIndex, "ULN", Defaults.LearnerId),
                AgreedPrice = row.ReadRowColumnValue<int>(tableStructure.AgreedPriceIndex, "agreed price"),
                LearnerType = (LearnerType)row.ReadRowColumnValue(tableStructure.LearnerTypeIndex, "learner type", "programme only DAS").ToEnumByDescription(typeof(LearnerType)),
                StartDate = row.ReadRowColumnValue<DateTime>(tableStructure.StartDateIndex, "start date"),
                PlannedEndDate = row.ReadRowColumnValue<DateTime>(tableStructure.PlannedEndDateIndex, "planned end date"),
                ActualEndDate = row.ReadRowColumnValue<DateTime?>(tableStructure.ActualEndDateIndex, "actual end date"),
                CompletionStatus = (CompletionStatus)row.ReadRowColumnValue<string>(tableStructure.CompletionStatusIndex, "completion status").ToEnumByDescription(typeof(CompletionStatus)),
                Provider = row.ReadRowColumnValue(tableStructure.ProviderIndex, "provider", Defaults.ProviderId),
                TotalTrainingPrice1 = row.ReadRowColumnValue<int>(tableStructure.TotalTrainingPrice1Index, "total training price 1"),
                TotalTrainingPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.TotalTrainingPrice1EffectiveDateIndex, "total training price 1 effective date"),
                TotalAssessmentPrice1 = row.ReadRowColumnValue<int>(tableStructure.TotalAssessmentPrice1Index, "total assessment price 1"),
                TotalAssessmentPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.TotalAssessmentPrice1EffectiveDateIndex, "total assessment price 1 effective date"),
                TotalTrainingPrice2 = row.ReadRowColumnValue<int>(tableStructure.TotalTrainingPrice2Index, "total training price 2"),
                TotalTrainingPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.TotalTrainingPrice2EffectiveDateIndex, "total training price 2 effective date"),
                TotalAssessmentPrice2 = row.ReadRowColumnValue<int>(tableStructure.TotalAssessmentPrice2Index, "total assessment price 2"),
                TotalAssessmentPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.TotalAssessmentPrice2EffectiveDateIndex, "total assessment price 2 effective date"),
                ResidualTrainingPrice1 = row.ReadRowColumnValue<int>(tableStructure.ResidualTrainingPrice1Index, "residual training price 1"),
                ResidualTrainingPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.ResidualTrainingPrice1EffectiveDateIndex, "residual training price 1 effective date"),
                ResidualAssessmentPrice1 = row.ReadRowColumnValue<int>(tableStructure.ResidualAssessmentPrice1Index, "residual assessment price 1"),
                ResidualAssessmentPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.ResidualAssessmentPrice1EffectiveDateIndex, "residual assessment price 1 effective date"),
                ResidualTrainingPrice2 = row.ReadRowColumnValue<int>(tableStructure.ResidualTrainingPrice2Index, "residual training price 2"),
                ResidualTrainingPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.ResidualTrainingPrice2EffectiveDateIndex, "residual training price 2 effective date"),
                ResidualAssessmentPrice2 = row.ReadRowColumnValue<int>(tableStructure.ResidualAssessmentPrice2Index, "residual assessment price 2"),
                ResidualAssessmentPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(tableStructure.ResidualAssessmentPrice2EffectiveDateIndex, "residual assessment price 2 effective date"),
                AimType = (AimType)row.ReadRowColumnValue(tableStructure.AimTypeIndex, "aim type", "Programme").ToEnumByDescription(typeof(AimType)),
                AimRate = row.ReadRowColumnValue<string>(tableStructure.AimRateIndex, "aim rate"),
                LearnAimRef = row.ReadRowColumnValue<string>(tableStructure.LearnAimRefIndex, "aim reference"),
                StandardCode = row.ReadRowColumnValue<long>(tableStructure.StandardCodeIndex, "standard code"),
                FrameworkCode = row.ReadRowColumnValue<int>(tableStructure.FrameworkCodeIndex, "framework code"),
                ProgrammeType = row.ReadRowColumnValue<int>(tableStructure.ProgrammeTypeIndex, "programme type"),
                PathwayCode = row.ReadRowColumnValue<int>(tableStructure.PathwayCodeIndex, "pathway code"),
                HomePostcodeDeprivation = row.ReadRowColumnValue<string>(tableStructure.HomePostcodeDeprivationIndex, "home postcode deprivation"),
                EmploymentStatus = row.ReadRowColumnValue<string>(tableStructure.EmploymentStatusIndex, "employment status"),
                EmploymentStatusApplies = row.ReadRowColumnValue<string>(tableStructure.EmploymentStatusAppliesIndex, "employment status applies"),
                EmployerId = row.ReadRowColumnValue<string>(tableStructure.EmployerIdIndex, "employer id"),
                SmallEmployer = row.ReadRowColumnValue<string>(tableStructure.SmallEmployerIndex, "small employer"),
                LearnDelFam = row.ReadRowColumnValue<string>(tableStructure.LearnDelFamIndex, "LearnDelFam"),
                AimSequenceNumber = row.ReadRowColumnValue<int>(tableStructure.AimSequenceNumberIndex, "aim sequence number")
            };

            rowData.RestartIndicator =
                row.ReadRowColumnValue<string>(tableStructure.RestartIndicatorIndex, "restart indicator")?
                    .Equals("YES", StringComparison.InvariantCultureIgnoreCase)??false;
            rowData.LearningAdjustmentForPriorLearning =
                row.ParseColumnValue(tableStructure.FundingAdjustmentForPriorLearningIndex);
            rowData.OtherFundingAdjustments = row.ParseColumnValue(tableStructure.OtherFundingAdjustmentIndex);

            var learnRefNumber = string.Empty;
            if (string.IsNullOrEmpty(rowData.LearnerReference) && !string.IsNullOrEmpty(rowData.Uln))
            {
                learnRefNumber = rowData.Uln;
                rowData.Uln = string.Empty;
            }

            if (string.IsNullOrEmpty(rowData.LearnerReference))
            {
                rowData.LearnerReference = learnRefNumber;
            }

            if (rowData.StandardCode == 0 && rowData.FrameworkCode == 0)
            {
                rowData.StandardCode = Defaults.StandardCode;
            }
            
            if (rowData.FrameworkCode > 0 && rowData.TotalAssessmentPrice2 > 0)
            {
                throw new Exception("Framework code and TotalAssessmentPrice2 can't be in the same scenario");
            }

            return rowData;
        }
    }
}
