IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- PaymentsHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='PaymentsHistory' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.PaymentsHistory
END
GO

CREATE TABLE Reference.PaymentsHistory
(
	[PaymentId] [uniqueidentifier] NOT NULL,
	[RequiredPaymentId] [uniqueidentifier] NOT NULL,
	[DeliveryMonth] [int] NOT NULL,
	[DeliveryYear] [int] NOT NULL,
	[CollectionPeriodName] [varchar](8) NOT NULL,
	[CollectionPeriodMonth] [int] NOT NULL,
	[CollectionPeriodYear] [int] NOT NULL,
	[FundingSource] [int] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[Amount] [decimal](15, 5) NULL,
	[ApprenticeshipContractType] [int] NULL,
	[Ukprn] [bigint] NULL,
	[AccountId] [bigint] NULL,
	[LearnRefNumber] [varchar](12) NULL,
	[FundingLineType] varchar(100) NOT NULL
	,[StandardCode] int
	,[FrameworkCode] int
	,[ProgrammeType] int
	,[PathwayCode] int
	,[SfaContributionPercentage] decimal(15,5)

)

CREATE INDEX IX_Reference_PaymentsHistory_Ukrpn ON Reference.PaymentsHistory (Ukprn, FundingSource)

