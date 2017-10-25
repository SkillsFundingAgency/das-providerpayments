IF NOT EXISTS (SELECT [schema_id] FROM sys.schemas WHERE [name] = 'TestStack')
BEGIN
	EXEC('CREATE SCHEMA TestStack')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Logs
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Logs' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP TABLE TestStack.Logs
END
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

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Component
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Component' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP TABLE TestStack.Component
END
GO

CREATE TABLE TestStack.Component
(
	ComponentType int PRIMARY KEY,
	VersionNumber varchar(15) NOT NULL,
	ArchiveData varbinary(max) NOT NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ProcessStatus
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ProcessStatus' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP TABLE TestStack.ProcessStatus
END
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

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Learner
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Learner' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP TABLE TestStack.Learner
END
GO

CREATE TABLE TestStack.Learner
(
	Uln bigint PRIMARY KEY,
	LearnerName varchar(125) NOT NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Provider
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Provider' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP TABLE TestStack.Provider
END
GO

CREATE TABLE TestStack.Provider
(
	Ukprn int PRIMARY KEY,
	ProviderName varchar(50) NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TrainingFramework
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TrainingFramework' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP TABLE TestStack.TrainingFramework
END
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

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TrainingStandard
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TrainingStandard' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP TABLE TestStack.TrainingStandard
END
GO

CREATE TABLE TestStack.TrainingStandard
(
	StandardCode bigint PRIMARY KEY,
	DisplayName varchar(50) NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ClearTransientTables
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='ClearTransientTables' AND [schema_id] = SCHEMA_ID('TestStack'))
BEGIN
	DROP PROCEDURE TestStack.ClearTransientTables
END
GO

CREATE PROCEDURE TestStack.ClearTransientTables
AS
	DECLARE @schemaname varchar(125), 
			@tablename varchar(125)

	DECLARE c CURSOR FOR
		SELECT s.name, t.name 
		FROM sys.tables t
		INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
		WHERE s.name NOT IN ('TestStack')

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