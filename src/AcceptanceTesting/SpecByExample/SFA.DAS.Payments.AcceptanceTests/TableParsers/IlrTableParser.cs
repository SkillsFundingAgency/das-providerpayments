﻿using System;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    public class IlrTableParser
    {
        public static void ParseIlrTableIntoContext(Submission context, Table ilrDetails)
        {
            ParseIlrTableIntoContext(context.IlrLearnerDetails, ilrDetails);
        }

        public static void ParseIlrTableIntoContext(List<IlrLearnerReferenceData> ilrLearnerDetails, Table ilrDetails)
        {
            if (ilrDetails.RowCount < 1)
            {
                throw new ArgumentException("ILR table must have at least 1 row");
            }

            var structure = ParseTableStructure(ilrDetails);
            foreach (var row in ilrDetails.Rows)
            {
                ilrLearnerDetails.Add(ParseCommitmentsTableRow(row, structure));
            }
        }

        private static IlrTableStructure ParseTableStructure(Table ilrDetails)
        {
            var structure = new IlrTableStructure();

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
                    case "submission period":
                        structure.SubmissionPeriod = c;
                        break;
                    default:
                        throw new ArgumentException($"Unexpected column in ILR table: {header}");
                }
            }

            return structure;
        }
        private static IlrLearnerReferenceData ParseCommitmentsTableRow(TableRow row, IlrTableStructure structure)
        {
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
                AimSequenceNumber = row.ReadRowColumnValue<int>(structure.AimSequenceNumberIndex, "aim sequence number"),
                SubmissionPeriod = row.ReadRowColumnValue<string>(structure.SubmissionPeriod, "submission period"),
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
        
        private class IlrTableStructure
        {
            public int LearnerReferenceIndex { get; set; } = -1;
            public int AgreedPriceIndex { get; set; } = -1;
            public int LearnerTypeIndex { get; set; } = -1;
            public int StartDateIndex { get; set; } = -1;
            public int PlannedEndDateIndex { get; set; } = -1;
            public int ActualEndDateIndex { get; set; } = -1;
            public int CompletionStatusIndex { get; set; } = -1;
            public int ProviderIndex { get; set; } = -1;
            public int TotalTrainingPrice1Index { get; set; } = -1;
            public int TotalTrainingPrice1EffectiveDateIndex { get; set; } = -1;
            public int TotalAssessmentPrice1Index { get; set; } = -1;
            public int TotalAssessmentPrice1EffectiveDateIndex { get; set; } = -1;
            public int ResidualTrainingPrice1Index { get; set; } = -1;
            public int ResidualTrainingPrice1EffectiveDateIndex { get; set; } = -1;
            public int ResidualAssessmentPrice1Index { get; set; } = -1;
            public int ResidualAssessmentPrice1EffectiveDateIndex { get; set; } = -1;
            public int TotalTrainingPrice2Index { get; set; } = -1;
            public int TotalTrainingPrice2EffectiveDateIndex { get; set; } = -1;
            public int TotalAssessmentPrice2Index { get; set; } = -1;
            public int TotalAssessmentPrice2EffectiveDateIndex { get; set; } = -1;
            public int AimTypeIndex { get; set; } = -1;
            public int AimRateIndex { get; set; } = -1;
            public int StandardCodeIndex { get; set; } = -1;
            public int FrameworkCodeIndex { get; set; } = -1;
            public int ProgrammeTypeIndex { get; set; } = -1;
            public int PathwayCodeIndex { get; set; } = -1;
            public int HomePostcodeDeprivationIndex { get; set; } = -1;
            public int EmploymentStatusIndex { get; set; } = -1;
            public int EmploymentStatusAppliesIndex { get; set; } = -1;
            public int EmployerIdIndex { get; set; } = -1;
            public int SmallEmployerIndex { get; set; } = -1;
            public int LearnDelFamIndex { get; set; } = -1;
            public int ResidualTrainingPrice2Index { get; set; } = -1;
            public int ResidualTrainingPrice2EffectiveDateIndex { get; set; } = -1;
            public int ResidualAssessmentPrice2Index { get; set; } = -1;
            public int ResidualAssessmentPrice2EffectiveDateIndex { get; set; } = -1;

            public int AimSequenceNumberIndex { get; set; } = -1;
            public int UlnIndex { get; set; } = -1;
            public int LearnAimRefIndex { get; set; } = -1;
            public int RestartIndicatorIndex { get; set; } = -1;
            public int FundingAdjustmentForPriorLearningIndex { get; set; } = -1;
            public int OtherFundingAdjustmentIndex { get; set; } = -1;
            public int SubmissionPeriod { get; set; } = -1;
        }
    }
}
