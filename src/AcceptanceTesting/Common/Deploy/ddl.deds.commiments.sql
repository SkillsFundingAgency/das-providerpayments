-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasCommitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasCommitments' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE dbo.DasCommitments
END
GO

CREATE TABLE dbo.DasCommitments
(
	[CommitmentId] varchar(50) PRIMARY KEY,
	[Uln] bigint NOT NULL,
	[Ukprn] bigint NOT NULL,
	[AccountId] bigint NOT NULL,
	[StartDate] date NOT NULL,
	[EndDate] date NOT NULL,
	[AgreedCost] decimal(15, 2) NOT NULL,
	[StandardCode] bigint NULL,
	[ProgrammeType] int NULL,
	[FrameworkCode] int NULL,
	[PathwayCode] int NULL,
	[Priority] int NOT NULL,
	[VersionId] [varchar](50) NOT NULL
)