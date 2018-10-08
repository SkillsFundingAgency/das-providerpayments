IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- ProviderAdjustmentsCurrent
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ProviderAdjustmentsCurrent' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
    DROP TABLE Reference.ProviderAdjustmentsCurrent
END
GO

CREATE TABLE Reference.ProviderAdjustmentsCurrent
(
    [Ukprn] bigint NOT NULL,
    [SubmissionId] uniqueidentifier NOT NULL,
    [SubmissionCollectionPeriod] int NOT NULL,
    [PaymentType] int NOT NULL,
    [PaymentTypeName] nvarchar(250) NOT NULL,
    [Amount] decimal(15,5) NOT NULL,
    CONSTRAINT [PK_ProviderAdjustmentsCurrent] PRIMARY KEY CLUSTERED
    (
        [Ukprn],
        [SubmissionId],
        [SubmissionCollectionPeriod],
        [PaymentType]
    )
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ProviderAdjustmentsHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ProviderAdjustmentsHistory' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
    DROP TABLE Reference.ProviderAdjustmentsHistory
END
GO

CREATE TABLE Reference.ProviderAdjustmentsHistory
(
    [Ukprn] bigint NOT NULL,
    [SubmissionId] uniqueidentifier NOT NULL,
    [SubmissionCollectionPeriod] int NOT NULL,
    [SubmissionAcademicYear] int NOT NULL,
    [PaymentType] int NOT NULL,
    [PaymentTypeName] nvarchar(250) NOT NULL,
    [Amount] decimal(15,5) NOT NULL,
    [CollectionPeriodName] varchar(8) NOT NULL,
    [CollectionPeriodMonth] int NOT NULL,
    [CollectionPeriodYear] int NOT NULL,
    CONSTRAINT [PK_ProviderAdjustmentsHistory] PRIMARY KEY CLUSTERED
    (
        [Ukprn],
        [SubmissionId],
        [SubmissionCollectionPeriod],
        [SubmissionAcademicYear],
        [PaymentType]
    )
)
GO
