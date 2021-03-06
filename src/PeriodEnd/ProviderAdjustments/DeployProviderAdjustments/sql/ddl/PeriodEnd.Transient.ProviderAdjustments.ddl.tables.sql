IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='ProviderAdjustments')
BEGIN
    EXEC('CREATE SCHEMA ProviderAdjustments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('ProviderAdjustments'))
BEGIN
	DROP TABLE ProviderAdjustments.TaskLog
END
GO

CREATE TABLE ProviderAdjustments.TaskLog
(
	[TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)
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
