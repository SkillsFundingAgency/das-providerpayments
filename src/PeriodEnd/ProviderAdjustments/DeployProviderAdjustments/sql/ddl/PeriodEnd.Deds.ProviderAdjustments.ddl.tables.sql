IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='ProviderAdjustments')
BEGIN
    EXEC('CREATE SCHEMA ProviderAdjustments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('ProviderAdjustments'))
BEGIN
    DROP TABLE ProviderAdjustments.Payments
END
GO

CREATE TABLE ProviderAdjustments.Payments
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
    CONSTRAINT [PK_ProviderAdjustmentsPayments] PRIMARY KEY CLUSTERED
    (
        [Ukprn],
        [SubmissionId],
        [SubmissionCollectionPeriod],
        [SubmissionAcademicYear],
        [PaymentType]
    )
)
GO

