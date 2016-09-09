IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentSchedule')
BEGIN
	EXEC('CREATE SCHEMA PaymentSchedule')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('PaymentSchedule'))
BEGIN
	DROP TABLE PaymentSchedule.TaskLog
END
GO

CREATE TABLE PaymentSchedule.TaskLog
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
-- RequiredPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='RequiredPayments' AND [schema_id] = SCHEMA_ID('PaymentSchedule'))
BEGIN
	DROP TABLE PaymentSchedule.RequiredPayments
END
GO

CREATE TABLE PaymentSchedule.RequiredPayments
(
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Ukprn bigint,
	DeliveryMonth int,
	DeliveryYear int,
	TransactionType int,
	AmountDue decimal(15,2),
	CONSTRAINT [PK_RequiredPayments] PRIMARY KEY (LearnRefNumber,AimSeqNumber,Ukprn)
)