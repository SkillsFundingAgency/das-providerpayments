﻿using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.AcceptanceTests.Contexts;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    public static class CommitmentsTableParser
    {

        public static void ParseCommitmentsIntoContext(CommitmentsContext context, Table commitments, LookupContext lookupContext)
        {
            if (commitments.Rows.Count < 1)
            {
                throw new ArgumentOutOfRangeException("commitments table must have at least 1 row");
            }

            var structure = ParseCommitmentsTableStructure(commitments);
            foreach (var row in commitments.Rows)
            {
                context.Commitments.Add(ParseCommitmentsTableRow(row, structure, context.Commitments.Count, lookupContext));
            }
        }

        public static void ParseAdditionalCommitmentsIntoContext(CommitmentsContext context, Table commitments, LookupContext lookupContext, string submissionPeriod)
        {
            if (commitments.Rows.Count < 1)
            {
                throw new ArgumentOutOfRangeException("Additional commitments table must have at least 1 row");
            }

            var additionalCommitments = new List<CommitmentReferenceData>(); 
            var structure = ParseCommitmentsTableStructure(commitments);
            foreach (var row in commitments.Rows)
            {
                additionalCommitments.Add(ParseCommitmentsTableRow(row, structure, context.Commitments.Count, lookupContext));
            }

            context.CommitmentsForPeriod.Add(submissionPeriod, additionalCommitments);
        }

        private static CommitmentsTableColumnStructure ParseCommitmentsTableStructure(Table commitments)
        {
            var structure = new CommitmentsTableColumnStructure();

            for (var c = 0; c < commitments.Header.Count; c++)
            {
                var header = commitments.Header.ElementAt(c).ToLowerInvariant();
                switch (header)
                {
                    case "uln":
                        structure.UlnIndex = c;
                        break;
                    case "priority":
                        structure.PriorityIndex = c;
                        break;
                    case "employer":
                        structure.EmployerIndex = c;
                        break;
                    case "employer of apprentice":
                        structure.EmployerIndex = c;
                        break;
                    case "employer paying for training":
                        structure.EmployerPayingForTrainingIndex = c;
                        break;
                    case "provider":
                        structure.ProviderIndex = c;
                        break;
                    case "agreed price":
                        structure.PriceIndex = c;
                        break;
                    case "commitment id":
                        structure.CommitmentIdIndex = c;
                        break;
                    case "version id":
                        structure.VersionIdIndex = c;
                        break;
                    case "start date":
                        structure.StartDateIndex = c;
                        break;
                    case "end date":
                        structure.EndDateIndex = c;
                        break;
                    case "status":
                        structure.StatusIndex = c;
                        break;
                    case "effective from":
                        structure.EffectiveFromIndex = c;
                        break;
                    case "effective to":
                        structure.EffectiveToIndex = c;
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
                    case "transfer approval date":
                        structure.TransferApprovalDateIndex = c;
                        break;
                    case "stop effective from":
                        structure.WithdrawnOnDate = c;
                        break;
                    default:
                        throw new ArgumentException($"Unexpected column in commitments table: {header}");
                }
            }
            if (structure.UlnIndex == -1)
            {
                throw new ArgumentException("Commitments table is missing ULN column");
            }
            if (structure.StartDateIndex == -1)
            {
                throw new ArgumentException("Commitments table is missing start date column");
            }
            if (structure.EndDateIndex == -1)
            {
                throw new ArgumentException("Commitments table is missing end date column");
            }

            return structure;
        }

        private static CommitmentReferenceData ParseCommitmentsTableRow(TableRow row,
            CommitmentsTableColumnStructure structure, int rowIndex, LookupContext lookupContext)
        {
            var learnerId = row[structure.UlnIndex];
            var uln = lookupContext.AddOrGetUln(learnerId);
            var providerId = structure.ProviderIndex > -1 ? row[structure.ProviderIndex] : Defaults.ProviderId;
            var ukprn = lookupContext.AddOrGetUkprn(providerId);
            var status = (CommitmentPaymentStatus) row
                .ReadRowColumnValue(structure.StatusIndex, "status", Defaults.CommitmentStatus)
                .ToEnumByDescription(typeof(CommitmentPaymentStatus));

            int priority = Defaults.CommitmentPriority;
            if (structure.PriorityIndex > -1 && !int.TryParse(row[structure.PriorityIndex], out priority))
            {
                throw new ArgumentException($"'{row[structure.PriorityIndex]}' is not a valid priority");
            }

            var sendingEmployerAccountId = (int?) null;
            if (structure.EmployerPayingForTrainingIndex > -1)
            {
                var nonNullableSendingEmployerId = 0;
                if (row[structure.EmployerPayingForTrainingIndex].Length < 10 || !int.TryParse(
                        row[structure.EmployerPayingForTrainingIndex].Substring(9),
                        out nonNullableSendingEmployerId))
                {
                    throw new ArgumentException(
                        $"'{row[structure.EmployerPayingForTrainingIndex]}' is not a valid employer reference");
                }

                sendingEmployerAccountId = nonNullableSendingEmployerId;
            }

            var employerAccountId = Defaults.EmployerAccountId;
            if (structure.EmployerIndex > -1 && (row[structure.EmployerIndex].Length < 10 ||
                                                 !int.TryParse(row[structure.EmployerIndex].Substring(9),
                                                     out employerAccountId)))
            {
                throw new ArgumentException($"'{row[structure.EmployerIndex]}' is not a valid employer reference");
            }

            var price = Defaults.AgreePrice;
            if (structure.PriceIndex > -1 && !int.TryParse(row[structure.PriceIndex], out price))
            {
                throw new ArgumentException($"'{row[structure.PriceIndex]}' is not a valid agreed price");
            }

            var commitmentId = rowIndex + 1;
            if (structure.CommitmentIdIndex > -1 && !int.TryParse(row[structure.CommitmentIdIndex], out commitmentId))
            {
                throw new ArgumentException($"'{row[structure.CommitmentIdIndex]}' is not a valid commitment id");
            }

            var versionId = Defaults.CommitmentVersionId;
            if (structure.VersionIdIndex > -1)
            {
                if (string.IsNullOrEmpty(row[structure.VersionIdIndex]))
                {
                    throw new ArgumentException($"'{row[structure.VersionIdIndex]}' is not a valid version id");
                }

                versionId = row[structure.VersionIdIndex];
                if (!versionId.Contains('-'))
                {
                    versionId = int.Parse(versionId).ToString("000");
                    versionId = $"{commitmentId}-{versionId}";
                }
            }

            DateTime startDate;
            if (!DateTime.TryParse(row[structure.StartDateIndex], out startDate))
            {
                throw new ArgumentException($"'{row[structure.StartDateIndex]}' is not a valid start date");
            }

            DateTime endDate;
            if (!DateTime.TryParse(row[structure.EndDateIndex], out endDate))
            {
                throw new ArgumentException($"'{row[structure.EndDateIndex]}' is not a valid end date");
            }

            DateTime? effectiveFrom = null;
            if (structure.EffectiveFromIndex > -1 &&
                !TryParseNullableDateTime(row[structure.EffectiveFromIndex], out effectiveFrom))
            {
                throw new ArgumentException($"'{row[structure.EffectiveFromIndex]}' is not a valid effective from");
            }

            DateTime? effectiveTo = null;
            if (structure.EffectiveToIndex > -1 && string.IsNullOrEmpty(row[structure.EffectiveToIndex]))
            {
                effectiveTo = null;
            }
            else if (structure.EffectiveToIndex > -1 &&
                     !TryParseNullableDateTime(row[structure.EffectiveToIndex], out effectiveTo))
            {
                throw new ArgumentException($"'{row[structure.EffectiveToIndex]}' is not a valid effective to");
            }

            DateTime? transferApprovalDate = null;
            if (structure.TransferApprovalDateIndex > -1 &&
                !TryParseNullableDateTime(row[structure.TransferApprovalDateIndex], out transferApprovalDate))
            {
                throw new ArgumentException(
                    $"'{row[structure.TransferApprovalDateIndex]}' is not a valid effective from");
            }

            var standardCode =
                row.ReadRowColumnValue<long>(structure.StandardCodeIndex, "standard code", Defaults.StandardCode);
            var frameworkCode = row.ReadRowColumnValue<int>(structure.FrameworkCodeIndex, "framework code");
            var programmeType = row.ReadRowColumnValue<int>(structure.ProgrammeTypeIndex, "programme type");
            var pathwayCode = row.ReadRowColumnValue<int>(structure.PathwayCodeIndex, "pathway code");

            DateTime? withdrawnOnDate = null;
            if (structure.WithdrawnOnDate > -1 &&
                !string.IsNullOrEmpty(row[structure.WithdrawnOnDate]) &&
                !TryParseNullableDateTime(row[structure.WithdrawnOnDate], out withdrawnOnDate))
            {
                throw new ArgumentException($"'{row[structure.WithdrawnOnDate]}' is not a valid stop effective from date");
            }

            if (status == CommitmentPaymentStatus.Cancelled && withdrawnOnDate == null)
            {
                throw new ArgumentException($"Please supply a stop effective from date for a cancelled commitment");
            }

            if (withdrawnOnDate != null && status != CommitmentPaymentStatus.Cancelled)
            {
                throw new ArgumentException("There is a stop effective from date but the commitment is not cancelled");
            }

            if (effectiveFrom == null)
            {
                effectiveFrom = startDate;
            }

            if (frameworkCode > 0)
            {
                standardCode = 0;
            }

            if (standardCode > 0)
            {
                programmeType = 25;
            }

            DateTime? pausedOnDate = null;
            if (status == CommitmentPaymentStatus.Paused)
            {
                pausedOnDate = effectiveFrom;
            }

            return new CommitmentReferenceData
            {
                EmployerAccountId = employerAccountId,
                LearnerId = learnerId,
                Uln = uln,
                Priority = priority,
                ProviderId = providerId,
                Ukprn = ukprn,
                AgreedPrice = price,
                CommitmentId = commitmentId,
                VersionId = versionId,
                StartDate = startDate,
                EndDate = endDate,
                EffectiveFrom = effectiveFrom.Value,
                EffectiveTo = effectiveTo,
                Status = status,
                StandardCode = standardCode,
                FrameworkCode = frameworkCode,
                ProgrammeType = programmeType,
                PathwayCode = pathwayCode,
                TransferSendingEmployerAccountId = sendingEmployerAccountId,
                TransferApprovalDate = transferApprovalDate,
                WithdrawnOnDate = withdrawnOnDate,
                PausedOnDate = pausedOnDate,
            };
        }

        private static bool TryParseNullableDateTime(string value, out DateTime? dateTime)
        {
            DateTime temp;
            if (DateTime.TryParse(value, out temp))
            {
                dateTime = temp;
                return true;
            }

            dateTime = null;
            return false;
        }
        
        private class CommitmentsTableColumnStructure
        {
            public int CommitmentIdIndex { get; set; } = -1;
            public int VersionIdIndex { get; set; } = -1;
            public int UlnIndex { get; set; } = -1;
            public int PriorityIndex { get; set; } = -1;
            public int EmployerIndex { get; set; } = -1;
            public int EmployerPayingForTrainingIndex { get; set; } = -1;
            public int ProviderIndex { get; set; } = -1;
            public int PriceIndex { get; set; } = -1;
            public int StartDateIndex { get; set; } = -1;
            public int EndDateIndex { get; set; } = -1;
            public int StatusIndex { get; set; } = -1;
            public int EffectiveFromIndex { get; set; } = -1;
            public int EffectiveToIndex { get; set; } = -1;
            public int StandardCodeIndex { get; set; } = -1;
            public int FrameworkCodeIndex { get; set; } = -1;
            public int ProgrammeTypeIndex { get; set; } = -1;
            public int PathwayCodeIndex { get; set; } = -1;
            public int TransferApprovalDateIndex { get; set; } = -1;
            public int WithdrawnOnDate { get; set; } = -1;
        }
    }
}
