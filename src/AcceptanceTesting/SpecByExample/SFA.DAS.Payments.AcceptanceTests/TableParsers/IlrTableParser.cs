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

        public static void ParseIlrTableIntoSubmission(Submission submission, Table ilrDetails)
        {
            if (ilrDetails.RowCount < 1)
            {
                throw new ArgumentException("ILR table must have at least 1 row");
            }

            structure = ParseTableStructure(ilrDetails);
            foreach (var row in ilrDetails.Rows)
            {
                var parsedLearner = ParseCommitmentsTableRow(row, structure.IlrTableStructure);
                if(parsedLearner != null)
                    submission.IlrLearnerDetails.Add(parsedLearner);

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
            var structure = new IlrTableStructure();
            var learningSupportTableColumnStructure = new LearningSupportTableParser.LearningSupportTableColumnStructure();
            var contractTypeTableColumnStructure = new ContractTypeTableParser.ContractTypesTableColumnStructure();
            var employmentStatusTableColumnStructure = new EmploymentStatusTableParser.EmploymentStatusTableColumnStructure();

            for (var c = 0; c < ilrDetails.Header.Count; c++)
            {
                var header = ilrDetails.Header.ElementAt(c).ToLowerInvariant();
                switch (header)
                {
                    case "learner reference number":
                        structure.LearnerReferenceIndex = c;
                        break;
                    case "uln":
                        structure.UlnIndex = c;
                        break;
                    case "agreed price":
                        structure.AgreedPriceIndex = c;
                        break;
                    case "learner type":
                        structure.LearnerTypeIndex = c;
                        break;
                    case "start date":
                        structure.StartDateIndex = c;
                        break;
                    case "planned end date":
                        structure.PlannedEndDateIndex = c;
                        break;
                    case "actual end date":
                        structure.ActualEndDateIndex = c;
                        break;
                    case "completion status":
                        structure.CompletionStatusIndex = c;
                        break;
                    case "provider":
                        structure.ProviderIndex = c;
                        break;
                    case "total training price":
                    case "total training price 1": // duplicate
                        structure.TotalTrainingPrice1Index = c;
                        break;
                    case "total training price effective date":
                    case "total training price 1 effective date": // duplicate
                        structure.TotalTrainingPrice1EffectiveDateIndex = c;
                        break;
                    case "total assessment price":
                    case "total assessment price 1": // duplicate
                        structure.TotalAssessmentPrice1Index = c;
                        break;
                    case "total assessment price effective date":
                    case "total assessment price 1 effective date": // duplicate
                        structure.TotalAssessmentPrice1EffectiveDateIndex = c;
                        break;
                    case "total training price 2":
                        structure.TotalTrainingPrice2Index = c;
                        break;
                    case "total training price 2 effective date":
                        structure.TotalTrainingPrice2EffectiveDateIndex = c;
                        break;
                    case "total assessment price 2":
                        structure.TotalAssessmentPrice2Index = c;
                        break;
                    case "total assessment price 2 effective date":
                        structure.TotalAssessmentPrice2EffectiveDateIndex = c;
                        break;
                    case "residual training price":
                    case "residual training price 1":
                        structure.ResidualTrainingPrice1Index = c;
                        break;
                    case "residual training price effective date":
                    case "residual training price 1 effective date":
                        structure.ResidualTrainingPrice1EffectiveDateIndex = c;
                        break;
                    case "residual assessment price":
                    case "residual assessment price 1":
                        structure.ResidualAssessmentPrice1Index = c;
                        break;
                    case "residual assessment price effective date":
                    case "residual assessment price 1 effective date":
                        structure.ResidualAssessmentPrice1EffectiveDateIndex = c;
                        break;
                    case "residual training price 2":
                        structure.ResidualTrainingPrice2Index = c;
                        break;
                    case "residual training price 2 effective date":
                        structure.ResidualTrainingPrice2EffectiveDateIndex = c;
                        break;
                    case "residual assessment price 2":
                        structure.ResidualAssessmentPrice2Index = c;
                        break;
                    case "residual assessment price 2 effective date":
                        structure.ResidualAssessmentPrice2EffectiveDateIndex = c;
                        break;
                    case "aim type":
                        structure.AimTypeIndex = c;
                        break;
                    case "aim rate":
                        structure.AimRateIndex = c;
                        break;
                    case "standard code":
                        structure.StandardCodeIndex = c;
                        break;
                    case "framework code":
                        structure.FrameworkCodeIndex = c;
                        break;
                    case "programme type":
                        structure.ProgrammeTypeIndex = c;
                        break;
                    case "pathway code":
                        structure.PathwayCodeIndex = c;
                        break;
                    case "home postcode deprivation":
                        structure.HomePostcodeDeprivationIndex = c;
                        break;
                    case "employment status":
                        structure.EmploymentStatusIndex = c;
                        break;
                    case "employment status applies":
                        structure.EmploymentStatusAppliesIndex = c;
                        break;
                    case "employer id":
                        structure.EmployerIdIndex = c;
                        break;
                    case "small employer":
                        structure.SmallEmployerIndex = c;
                        break;
                    case "learndelfam":
                        structure.LearnDelFamIndex = c;
                        break;
                    case "aim sequence number":
                        structure.AimSequenceNumberIndex = c;
                        break;
                    case "aim reference":
                        structure.LearnAimRefIndex = c;
                        break;
                    case "restart indicator":
                        structure.RestartIndicatorIndex = c;
                        break;
                    case "funding adjustment for prior learning":
                        structure.FundingAdjustmentForPriorLearningIndex = c;
                        break;
                    case "other funding adjustment":
                        structure.OtherFundingAdjustmentIndex = c;
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
                IlrTableStructure = structure,
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
                DateTo = row.ReadRowColumnValue<DateTime>(contractTypesTableColumnStructure.DateToIndex, "date to")
            };
        }

        private static EmploymentStatusReferenceData ParseEmploymentStatusTableRow(TableRow row, EmploymentStatusTableParser.EmploymentStatusTableColumnStructure structure)
        {
            var employerReference = row.ReadRowColumnValue<string>(structure.EmployerIndex, "Employer");
            int employerId;
            if (string.IsNullOrWhiteSpace(employerReference))
            {
                return null;
            }
            
            var employerMatch = Regex.Match(employerReference, "^employer ([0-9]{1,})$");
            if (!employerMatch.Success)
            {
                throw new ArgumentException($"Employer '{employerReference}' is not a valid employer reference");
            }
            employerId = int.Parse(employerMatch.Groups[1].Value);
            

            var status = new EmploymentStatusReferenceData
            {
                EmployerId = employerId,
                EmploymentStatus = (EmploymentStatus)row.ReadRowColumnValue<string>(structure.EmploymentStatusIndex, "Employment Status").ToEnumByDescription(typeof(EmploymentStatus)),
                EmploymentStatusApplies = row.ReadRowColumnValue<DateTime>(structure.EmploymentStatusAppliesIndex, "Employment Status Applies"),
            };

            var smallEmployer = row.ReadRowColumnValue<string>(structure.SmallEmployerIndex, "Small Employer");
            if (smallEmployer?.Length > 3)
            {
                status.MonitoringType = (EmploymentStatusMonitoringType)smallEmployer.Substring(0, 3).ToEnumByDescription(typeof(EmploymentStatusMonitoringType));
                status.MonitoringCode = int.Parse(smallEmployer.Substring(3));
            }

            return status;
        }

        private static IlrLearnerReferenceData ParseCommitmentsTableRow(TableRow row, IlrTableStructure structure)
        {
            if (string.IsNullOrWhiteSpace(row.ReadRowColumnValue<string>(structure.LearnerReferenceIndex, "learner reference number"))
                && string.IsNullOrWhiteSpace(row.ReadRowColumnValue<string>(structure.UlnIndex, "ULN")))
            {
                return null;
            }
            var rowData = new IlrLearnerReferenceData
            {
                LearnerReference = row.ReadRowColumnValue<string>(structure.LearnerReferenceIndex, "learner reference number", string.Empty),
                Uln = row.ReadRowColumnValue<string>(structure.UlnIndex, "ULN", Defaults.LearnerId),
                AgreedPrice = row.ReadRowColumnValue<int>(structure.AgreedPriceIndex, "agreed price"),
                LearnerType = (LearnerType)row.ReadRowColumnValue<string>(structure.LearnerTypeIndex, "learner type", "programme only DAS").ToEnumByDescription(typeof(LearnerType)),
                StartDate = row.ReadRowColumnValue<DateTime>(structure.StartDateIndex, "start date"),
                PlannedEndDate = row.ReadRowColumnValue<DateTime>(structure.PlannedEndDateIndex, "planned end date"),
                ActualEndDate = row.ReadRowColumnValue<DateTime?>(structure.ActualEndDateIndex, "actual end date"),
                CompletionStatus = (CompletionStatus)row.ReadRowColumnValue<string>(structure.CompletionStatusIndex, "completion status").ToEnumByDescription(typeof(CompletionStatus)),
                Provider = row.ReadRowColumnValue<string>(structure.ProviderIndex, "provider", Defaults.ProviderId),
                TotalTrainingPrice1 = row.ReadRowColumnValue<int>(structure.TotalTrainingPrice1Index, "total training price 1"),
                TotalTrainingPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.TotalTrainingPrice1EffectiveDateIndex, "total training price 1 effective date"),
                TotalAssessmentPrice1 = row.ReadRowColumnValue<int>(structure.TotalAssessmentPrice1Index, "total assessment price 1"),
                TotalAssessmentPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.TotalAssessmentPrice1EffectiveDateIndex, "total assessment price 1 effective date"),
                TotalTrainingPrice2 = row.ReadRowColumnValue<int>(structure.TotalTrainingPrice2Index, "total training price 2"),
                TotalTrainingPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.TotalTrainingPrice2EffectiveDateIndex, "total training price 2 effective date"),
                TotalAssessmentPrice2 = row.ReadRowColumnValue<int>(structure.TotalAssessmentPrice2Index, "total assessment price 2"),
                TotalAssessmentPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.TotalAssessmentPrice2EffectiveDateIndex, "total assessment price 2 effective date"),
                ResidualTrainingPrice1 = row.ReadRowColumnValue<int>(structure.ResidualTrainingPrice1Index, "residual training price 1"),
                ResidualTrainingPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.ResidualTrainingPrice1EffectiveDateIndex, "residual training price 1 effective date"),
                ResidualAssessmentPrice1 = row.ReadRowColumnValue<int>(structure.ResidualAssessmentPrice1Index, "residual assessment price 1"),
                ResidualAssessmentPrice1EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.ResidualAssessmentPrice1EffectiveDateIndex, "residual assessment price 1 effective date"),
                ResidualTrainingPrice2 = row.ReadRowColumnValue<int>(structure.ResidualTrainingPrice2Index, "residual training price 2"),
                ResidualTrainingPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.ResidualTrainingPrice2EffectiveDateIndex, "residual training price 2 effective date"),
                ResidualAssessmentPrice2 = row.ReadRowColumnValue<int>(structure.ResidualAssessmentPrice2Index, "residual assessment price 2"),
                ResidualAssessmentPrice2EffectiveDate = row.ReadRowColumnValue<DateTime>(structure.ResidualAssessmentPrice2EffectiveDateIndex, "residual assessment price 2 effective date"),
                AimType = (AimType)row.ReadRowColumnValue<string>(structure.AimTypeIndex, "aim type", "Programme").ToEnumByDescription(typeof(AimType)),
                AimRate = row.ReadRowColumnValue<string>(structure.AimRateIndex, "aim rate"),
                LearnAimRef = row.ReadRowColumnValue<string>(structure.LearnAimRefIndex, "aim reference"),
                StandardCode = row.ReadRowColumnValue<long>(structure.StandardCodeIndex, "standard code"),
                FrameworkCode = row.ReadRowColumnValue<int>(structure.FrameworkCodeIndex, "framework code"),
                ProgrammeType = row.ReadRowColumnValue<int>(structure.ProgrammeTypeIndex, "programme type"),
                PathwayCode = row.ReadRowColumnValue<int>(structure.PathwayCodeIndex, "pathway code"),
                HomePostcodeDeprivation = row.ReadRowColumnValue<string>(structure.HomePostcodeDeprivationIndex, "home postcode deprivation"),
                EmploymentStatus = row.ReadRowColumnValue<string>(structure.EmploymentStatusIndex, "employment status"),
                EmploymentStatusApplies = row.ReadRowColumnValue<string>(structure.EmploymentStatusAppliesIndex, "employment status applies"),
                EmployerId = row.ReadRowColumnValue<string>(structure.EmployerIdIndex, "employer id"),
                SmallEmployer = row.ReadRowColumnValue<string>(structure.SmallEmployerIndex, "small employer"),
                LearnDelFam = row.ReadRowColumnValue<string>(structure.LearnDelFamIndex, "LearnDelFam"),
                AimSequenceNumber = row.ReadRowColumnValue<int>(structure.AimSequenceNumberIndex, "aim sequence number")
            };

            rowData.RestartIndicator =
                row.ReadRowColumnValue<string>(structure.RestartIndicatorIndex, "restart indicator")?
                    .Equals("YES", StringComparison.InvariantCultureIgnoreCase)??false;
            rowData.LearningAdjustmentForPriorLearning =
                row.ParseColumnValue(structure.FundingAdjustmentForPriorLearningIndex);
            rowData.OtherFundingAdjustments = row.ParseColumnValue(structure.OtherFundingAdjustmentIndex);

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
