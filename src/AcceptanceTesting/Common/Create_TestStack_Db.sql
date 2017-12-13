CREATE SCHEMA Reference
GO
CREATE SCHEMA TestStack
GO

CREATE TABLE TestStack.Logs
(
	LogId uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
	Logger varchar(255), 
	LogLevel varchar(10), 
	EntryDate datetime, 
	[Message] nvarchar(max), 
	ErrorDetails nvarchar(max)
)
GO

CREATE TABLE TestStack.Component
(
	ComponentType int PRIMARY KEY,
	VersionNumber varchar(15) NOT NULL,
	ArchiveData varbinary(max) NOT NULL
)
GO

CREATE TABLE TestStack.ProcessStatus
(
	ProcessId varchar(50) NOT NULL,
	StepId varchar(125) NOT NULL,
	StepIndex int NOT NULL,
	StepDescription varchar(255) NOT NULL,
	ExecutionStartTime datetime NULL,
	ExecutionEndTime datetime NULL,
	ErrorMessage varchar(max) NULL,
	CONSTRAINT [PK_TESTSTACK_PROCESSSTATUS] PRIMARY KEY (ProcessId, StepId)
)
GO

CREATE TABLE TestStack.Learner
(
	Uln bigint PRIMARY KEY,
	LearnerName varchar(125) NOT NULL
)
GO

CREATE TABLE TestStack.Provider
(
	Ukprn int PRIMARY KEY,
	ProviderName varchar(50) NULL
)
GO

CREATE TABLE TestStack.TrainingFramework
(
	PathwayCode int NOT NULL,
	FrameworkCode int NOT NULL,
	ProgrammeType int NOT NULL,
	DisplayName varchar(50) NULL,
	CONSTRAINT [PK_TESTSTACK_TRAININGFRAMEWORK] PRIMARY KEY CLUSTERED 
	(
		PathwayCode,
		FrameworkCode,
		ProgrammeType
	)
)
GO

CREATE TABLE TestStack.TrainingStandard
(
	StandardCode bigint PRIMARY KEY,
	DisplayName varchar(50) NULL
)
GO

CREATE TABLE Reference.DasAccounts
(
	AccountId bigint PRIMARY KEY,
	AccountName varchar(125) NOT NULL,
	LevyBalance decimal(15, 2) NULL
)
GO

CREATE TABLE Reference.DataLockCommitments
(
	CommitmentId bigint PRIMARY KEY,
	Uln bigint NOT NULL,
	Ukprn bigint NOT NULL,
	AccountId bigint NOT NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL,
	AgreedCost decimal(15, 2) NOT NULL,
	StandardCode bigint NULL,
	ProgrammeType int NULL,
	FrameworkCode int NULL,
	PathwayCode int NULL,
	CONSTRAINT [FK_LEARNER] FOREIGN KEY(Uln) REFERENCES TestStack.Learner(Uln),
	CONSTRAINT [FK_PROVIDER] FOREIGN KEY(Ukprn) REFERENCES TestStack.Provider(Ukprn),
	CONSTRAINT [FK_ACCOUNT] FOREIGN KEY(AccountId) REFERENCES Reference.DasAccounts(AccountId),
	CONSTRAINT [FK_STANDARD] FOREIGN KEY (StandardCode) REFERENCES TestStack.TrainingStandard(StandardCode),
	CONSTRAINT [FK_FRAMEWORK] FOREIGN KEY(PathwayCode,FrameworkCode,ProgrammeType) REFERENCES TestStack.TrainingFramework(PathwayCode,FrameworkCode,ProgrammeType)
)
GO

CREATE PROCEDURE TestStack.ClearTransientTables
AS
	DECLARE @schemaname varchar(125), 
			@tablename varchar(125)

	DECLARE c CURSOR FOR
		SELECT s.name, t.name 
		FROM sys.tables t
		INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
		WHERE s.name NOT IN ('TestStack','Reference')

	OPEN c

	FETCH NEXT FROM c INTO @schemaname, @tablename
	WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC('DELETE FROM ' + @schemaname + '.' + @tablename)
			FETCH NEXT FROM c INTO @schemaname, @tablename
		END

	CLOSE c
	DEALLOCATE c
GO