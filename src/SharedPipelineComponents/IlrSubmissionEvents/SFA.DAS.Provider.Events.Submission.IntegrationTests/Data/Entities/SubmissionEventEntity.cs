﻿using System;

namespace SFA.DAS.Provider.Events.Submission.IntegrationTests.Data.Entities
{
    public class SubmissionEventEntity
    {
        public long Id { get; set; }
        public string IlrFileName { get; set; }
        public DateTime FileDateTime { get; set; }
        public DateTime SubmittedDateTime { get; set; }
        public int ComponentVersionNumber { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public string LearnRefNumber { get; set; }
        public long AimSeqNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? OnProgrammeTotalPrice { get; set; }
        public decimal? CompletionTotalPrice { get; set; }
        public string NiNumber { get; set; }
        public int EmployerReferenceNumber { get; set; }
        public string AcademicYear { get; set; }
        public string EPAOrgId { get; set; }
        public int? CompStatus { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public int? FundingModel { get; set; }
        public string DelLocPostCode { get; set; }
        public DateTime? LearnActEndDate { get; set; }
        public int? WithdrawReason { get; set; }
        public int? Outcome { get; set; }
        public int? AimType { get; set; }
    }
}
