-----------------------------------------------------------------------------------------------------------------------------------------------
-- Collection_Period_Mapping
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Collection_Period_Mapping' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE dbo.Collection_Period_Mapping
END
GO

CREATE TABLE [dbo].[Collection_Period_Mapping] (
	[Period_ID] [int] NOT NULL,
	[Collection_Period] [varchar](3) NOT NULL,
	[Period] [int] NOT NULL,
	[Calendar_Year] [int] NOT NULL,
	[Collection_Open] [bit] NOT NULL,
	[ActualsSchemaPeriod] [int] NOT NULL,
	CONSTRAINT [PK_Collection_Period_Mapping] PRIMARY KEY CLUSTERED ([Period_ID] ASC)
)


IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Payments')
BEGIN
	EXEC('CREATE SCHEMA Payments')
END
GO
-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('Payments'))
BEGIN
	DROP TABLE Payments.Payments
END
GO

CREATE TABLE Payments.Payments
(
	PaymentId uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
	RequiredPaymentId uniqueidentifier NOT NULL,
	DeliveryMonth int NOT NULL,
	DeliveryYear int NOT NULL,
	CollectionPeriodName varchar(8) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	FundingSource int NOT NULL,
	TransactionType int NOT NULL,
	Amount decimal(15,5)
)
GO