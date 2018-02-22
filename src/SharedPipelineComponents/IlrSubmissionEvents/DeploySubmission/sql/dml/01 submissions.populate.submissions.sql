
---------------------------------------------------------------
-- LatestVersion
---------------------------------------------------------------
INSERT INTO [Submissions].[LastSeenVersion]
(
	IlrFileName,
	FileDateTime,
	SubmittedDateTime,
	ComponentVersionNumber,
	UKPRN,
	ULN,
	LearnRefNumber,
    AimSeqNumber,
	PriceEpisodeIdentifier,
	StandardCode,
	ProgrammeType,
	FrameworkCode,
	PathwayCode,
	ActualStartDate,
	PlannedEndDate,
	ActualEndDate,
	OnProgrammeTotalPrice,
	CompletionTotalPrice,
	NINumber,
	CommitmentId,
	AcademicYear,
	EmployerReferenceNumber,
	EPAOrgId
)
SELECT
	IlrFileName,
	FileDateTime,
	SubmittedDateTime,
	ComponentVersionNumber,
	UKPRN,
	ULN,
	LearnRefNumber,
    AimSeqNumber,
	PriceEpisodeIdentifier,
	StandardCode,
	ProgrammeType,
	FrameworkCode,
	PathwayCode,
	ActualStartDate,
	PlannedEndDate,
	ActualEndDate,
	OnProgrammeTotalPrice,
	CompletionTotalPrice,
	NINumber,
	CommitmentId,
	AcademicYear,
	EmployerReferenceNumber,
	EPAOrgId 
FROM ${DAS_ProviderEvents.FQ}.Submissions.LastSeenVersion lv
WHERE UKPRN IN (SELECT UKPRN FROM [Valid].[LearningProvider])
GO
