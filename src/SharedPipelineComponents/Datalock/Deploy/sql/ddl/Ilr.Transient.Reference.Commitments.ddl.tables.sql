IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
    EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasCommitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasCommitments' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
    DROP TABLE Reference.DasCommitments
END
GO

CREATE TABLE [Reference].[DasCommitments] (
    [CommitmentId] bigint NOT NULL,
    [VersionId] varchar(25) NOT NULL,
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
    [PaymentStatus] int NOT NULL,
    [PaymentStatusDescription] varchar(50) NOT NULL,
    [Priority] int NOT NULL,
	[EffectiveFrom] date NOT NULL,
	[EffectiveTo] date NULL,
	[LegalEntityName] [nvarchar](100) NULL,
	[TransferSendingEmployerAccountId] bigint null,
	[TransferApprovalDate] datetime null,
  	[PausedOnDate] datetime2 null,
	[WithdrawnOnDate] datetime2 null,
  PRIMARY KEY CLUSTERED (
        CommitmentId ASC,
		VersionId ASC
    )
)
GO

