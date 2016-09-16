IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Valid')
BEGIN
	EXEC('CREATE SCHEMA Valid')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- LearningProvider
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='LearningProvider' AND [schema_id] = SCHEMA_ID('Valid'))
BEGIN
	DROP TABLE Valid.LearningProvider
END
GO

create table [Valid].[LearningProvider] (
	[UKPRN] [int] NOT NULL,
	PRIMARY KEY CLUSTERED ([UKPRN] ASC)
)