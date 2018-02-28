
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
FROM OPENQUERY(${DAS_ProviderEvents.servername}, '
		select 
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
		from 
			${DAS_ProviderEvents.databasename}.Submissions.LastSeenVersion'
    ) lv
WHERE UKPRN IN (
        SELECT UKPRN
        FROM [Valid].[LearningProvider]
        )
GO
