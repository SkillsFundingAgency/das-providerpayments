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
	[UKPRN] [bigint] NOT NULL,
	PRIMARY KEY CLUSTERED ([UKPRN] ASC)
)


-----------------------------------------------------------------------------------------------------------------------------------------------
-- FileDetails
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='FileDetails' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[FileDetails]
END
GO

CREATE TABLE [dbo].[FileDetails] (
    [ID] [int] IDENTITY(1,1),
    [UKPRN] [bigint] NOT NULL,
    [Filename] [nvarchar](50) NULL,
    [FileSizeKb] [bigint] NULL,
    [TotalLearnersSubmitted] [int] NULL,
    [TotalValidLearnersSubmitted] [int] NULL,
    [TotalInvalidLearnersSubmitted] [int] NULL,
    [TotalErrorCount] [int] NULL,
    [TotalWarningCount] [int] NULL,
    [SubmittedTime] [datetime] NULL,
    [Success] [bit]
    CONSTRAINT [PK_dbo.FileDetails] UNIQUE ([UKPRN], [Filename], [Success] ASC)
)
GO
