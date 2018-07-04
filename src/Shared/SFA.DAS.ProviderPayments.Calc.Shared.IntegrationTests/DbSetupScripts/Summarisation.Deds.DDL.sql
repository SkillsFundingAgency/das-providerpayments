-----------------------------------------------------------------------------------------------------------------------------------------------
-- Collection_Period_Mapping
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Collection_Period_Mapping' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE dbo.Collection_Period_Mapping
END
GO

CREATE TABLE [dbo].[Collection_Period_Mapping](
       [Collection_Year] [int] NOT NULL,
       [Period_ID] [int] NOT NULL,
       [Return_Code] [varchar](10) NOT NULL,
       [Collection_Period_Name] [nvarchar](20) NOT NULL,
       [Collection_ReturnCode] [varchar](10) NOT NULL,
       [Calendar_Month] [int] NOT NULL,
       [Calendar_Year] [int] NOT NULL,
       [Collection_Open] [bit] NOT NULL,
       [ActualsSchemaPeriod] [int] NOT NULL
       
CONSTRAINT [PK_Collection_Period_Mapping] PRIMARY KEY CLUSTERED 
(
       [Collection_Year],[Period_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

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