--====================================================================================
-- Component tables
--====================================================================================
IF NOT EXISTS (SELECT [schema_id] FROM sys.schemas WHERE [name] = 'Submissions')
	BEGIN
		EXEC('CREATE SCHEMA Submissions')
	END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('Submissions'))
BEGIN
    DROP TABLE Submissions.TaskLog
END
GO

CREATE TABLE Submissions.TaskLog
(
    [TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
    [DateTime] datetime NOT NULL DEFAULT(GETDATE()),
    [Level] nvarchar(10) NOT NULL,
    [Logger] nvarchar(512) NOT NULL,
    [Message] nvarchar(1024) NOT NULL,
    [Exception] nvarchar(max) NULL
)
GO



--------------------------------------------------------------------------------------
-- SubmissionEvents
--------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.tables WHERE [name] = 'SubmissionEvents' AND [schema_id] = SCHEMA_ID('Submissions'))
	BEGIN
		DROP TABLE [Submissions].[SubmissionEvents]
	END
GO

CREATE TABLE [Submissions].[SubmissionEvents]
(
	Id						bigint			PRIMARY KEY IDENTITY(1,1),
	IlrFileName				nvarchar(50)	NOT NULL,
	FileDateTime			datetime		NOT NULL,
	SubmittedDateTime		datetime		NOT NULL,
	ComponentVersionNumber	int				NOT NULL,
	UKPRN					bigint			NOT NULL,
	ULN						bigint			NOT NULL,
	LearnRefNumber			varchar(12)		NOT NULL,
    AimSeqNumber			INT				NOT NULL,
	PriceEpisodeIdentifier	varchar(25)		NOT NULL,
	StandardCode			bigint			NULL,
	ProgrammeType			int				NULL,
	FrameworkCode			int				NULL,
	PathwayCode				int				NULL,
	ActualStartDate			date			NULL,
	PlannedEndDate			date			NULL,
	ActualEndDate			date			NULL,
	OnProgrammeTotalPrice	decimal(15,5)	NULL,
	CompletionTotalPrice	decimal(15,5)	NULL,
	NINumber				varchar(9)		NULL,
	CommitmentId			bigint			NULL,
	AcademicYear			varchar(4)    	NOT NULL,
	EmployerReferenceNumber int             NULL,
	EPAOrgId				VARCHAR(7)		NULL,
	GivenNames				VARCHAR(100)	NULL,
	FamilyName				VARCHAR(100)	NULL,
	CompStatus				int				NULL,
	FundingModel			int			    NULL,
    DelLocPostCode          VARCHAR(50)     NULL,
    LearnActEndDate			Date			NULL,
    WithdrawReason          int				NULL,
    Outcome					int				NULL,
    AimType					int				NULL
)
GO

--------------------------------------------------------------------------------------
-- LastSeenVersion
--------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.tables WHERE [name] = 'LastSeenVersion' AND [schema_id] = SCHEMA_ID('Submissions'))
	BEGIN
		DROP TABLE [Submissions].[LastSeenVersion]
	END
GO

CREATE TABLE [Submissions].[LastSeenVersion]
(
	IlrFileName				nvarchar(50)	NOT NULL,
	FileDateTime			datetime		NOT NULL,
	SubmittedDateTime		datetime		NOT NULL,
	ComponentVersionNumber	int				NOT NULL,
	UKPRN					bigint			NOT NULL,
	ULN						bigint			NOT NULL,
	LearnRefNumber			varchar(12)		NOT NULL,
    AimSeqNumber			INT				NOT NULL,
	PriceEpisodeIdentifier	varchar(25)		NOT NULL,
	StandardCode			bigint			NULL,
	ProgrammeType			int				NULL,
	FrameworkCode			int				NULL,
	PathwayCode				int				NULL,
	ActualStartDate			date			NULL,
	PlannedEndDate			date			NULL,
	ActualEndDate			date			NULL,
	OnProgrammeTotalPrice	decimal(15,5)	NULL,
	CompletionTotalPrice	decimal(15,5)	NULL,
	NINumber				varchar(9)		NULL,
	CommitmentId			bigint			NULL,
	AcademicYear			varchar(4)    	NOT NULL,
	EmployerReferenceNumber int             NULL,
	EPAOrgId				VARCHAR(7)		NULL,
	GivenNames				VARCHAR(100)	NULL,
	FamilyName				VARCHAR(100)	NULL,
	CompStatus				int				NULL,
    FundingModel			int			    NULL,
    DelLocPostCode          VARCHAR(50)     NULL,
    LearnActEndDate			Date			NULL,
    WithdrawReason          int				NULL,
    Outcome					int				NULL,
    AimType					int				NULL
)
GO
