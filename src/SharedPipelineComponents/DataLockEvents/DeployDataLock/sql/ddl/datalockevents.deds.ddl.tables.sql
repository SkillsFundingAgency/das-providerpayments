IF NOT EXISTS (SELECT [schema_id] FROM sys.schemas WHERE [name] = 'DataLock')
	BEGIN
		EXEC('CREATE SCHEMA DataLock')
	END
GO

--------------------------------------------------------------------------------------
-- DataLockEvents
--------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.tables WHERE [name] = 'DataLockEvents' AND [schema_id] = SCHEMA_ID('DataLock'))
	BEGIN
		DROP TABLE [DataLock].[DataLockEvents]
	END
GO

CREATE TABLE [DataLock].[DataLockEvents]
(
	Id							bigint				PRIMARY KEY IDENTITY(1,1),
	DataLockEventId				uniqueidentifier	NOT NULL,
	ProcessDateTime				datetime			NOT NULL,
	IlrFileName					nvarchar(50)		NOT NULL,
	SubmittedDateTime		    datetime			NOT NULL,
	AcademicYear				varchar(4)    		NOT NULL,
	UKPRN						bigint				NOT NULL,
	ULN							bigint				NOT NULL,
	LearnRefNumber				varchar(100)		NOT NULL,
    AimSeqNumber				bigint				NOT NULL,
	PriceEpisodeIdentifier		varchar(25)			NOT NULL,
	CommitmentId				bigint				NOT NULL,
	EmployerAccountId			bigint				NOT NULL,
	EventSource					int					NOT NULL,
	HasErrors					bit					NOT NULL,
	IlrStartDate				date				NULL,
	IlrStandardCode				bigint				NULL,
	IlrProgrammeType			int					NULL,
	IlrFrameworkCode			int					NULL,
	IlrPathwayCode				int					NULL,
	IlrTrainingPrice			decimal(12,5)		NULL,
	IlrEndpointAssessorPrice	decimal(12,5)		NULL,
	IlrPriceEffectiveFromDate	date				NULL,
	IlrPriceEffectiveToDate		date				NULL
)
GO

--------------------------------------------------------------------------------------
-- DataLockEventPeriods
--------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.tables WHERE [name] = 'DataLockEventPeriods' AND [schema_id] = SCHEMA_ID('DataLock'))
	BEGIN
		DROP TABLE [DataLock].[DataLockEventPeriods]
	END
GO

CREATE TABLE [DataLock].[DataLockEventPeriods]
(
	DataLockEventId			uniqueidentifier			NOT NULL,
	CollectionPeriodName	varchar(8)		NOT NULL,
	CollectionPeriodMonth	int				NOT NULL,
	CollectionPeriodYear	int				NOT NULL,
	CommitmentVersion		bigint			NOT NULL,
	IsPayable				bit				NOT NULL,
	TransactionType			int				NOT NULL
)
GO

--------------------------------------------------------------------------------------
-- DataLockEventCommitmentVersions
--------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.tables WHERE [name] = 'DataLockEventCommitmentVersions' AND [schema_id] = SCHEMA_ID('DataLock'))
	BEGIN
		DROP TABLE [DataLock].[DataLockEventCommitmentVersions]
	END
GO

CREATE TABLE [DataLock].[DataLockEventCommitmentVersions]
(
	DataLockEventId				uniqueidentifier			NOT NULL,
	CommitmentVersion			bigint			NOT NULL,
	CommitmentStartDate			date			NOT NULL,
	CommitmentStandardCode		bigint			NULL,
	CommitmentProgrammeType		int				NULL,
	CommitmentFrameworkCode		int				NULL,
	CommitmentPathwayCode		int				NULL,
	CommitmentNegotiatedPrice	decimal(12,5)	NOT NULL,
	CommitmentEffectiveDate		date			NOT NULL
)
GO

--------------------------------------------------------------------------------------
-- DataLockEventErrors
--------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.tables WHERE [name] = 'DataLockEventErrors' AND [schema_id] = SCHEMA_ID('DataLock'))
	BEGIN
		DROP TABLE [DataLock].[DataLockEventErrors]
	END
GO

CREATE TABLE [DataLock].[DataLockEventErrors]
(
	DataLockEventId			uniqueidentifier			NOT NULL,
	ErrorCode				varchar(15)		NOT NULL,
	SystemDescription		nvarchar(255)	NOT NULL,
	PRIMARY KEY (DataLockEventId, ErrorCode)
)
GO
