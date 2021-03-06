IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Rulebase')
BEGIN
	EXEC('CREATE SCHEMA Rulebase')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AE_LearningDelivery
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AE_LearningDelivery' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AE_LearningDelivery
END
GO

CREATE TABLE Rulebase.AE_LearningDelivery
(
	[LearnRefNumber] varchar(12) NOT NULL,
	[AimSeqNumber] int NOT NULL,
	[Ukprn] bigint NOT NULL,
	[Uln] bigint NOT NULL,
	[NiNumber] varchar(9) NULL,
	[StdCode] bigint NULL,
	[ProgType] int NULL,
	[FworkCode] int NULL,
	[PwayCode] int NULL,
	[NegotiatedPrice] int NOT NULL,
	[LearnStartDate] date NOT NULL,
	[OrigLearnStartDate] date NULL,
	[LearnPlanEndDate] date NOT NULL,
	[LearnActEndDate] date NULL,
	[MonthlyInstallment] DECIMAL(15,5) NOT NULL,
	[MonthlyInstallmentUncapped] DECIMAL(15,5) NOT NULL,
	[CompletionPayment] DECIMAL(15,5) NOT NULL,
	[CompletionPaymentUncapped] DECIMAL(15,5) NOT NULL,
	CONSTRAINT [PK_AE_LearningDelivery] PRIMARY KEY ([LearnRefNumber], [AimSeqNumber], [Ukprn])
)

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AE_LearningDelivery_PeriodisedValues
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AE_LearningDelivery_PeriodisedValues' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AE_LearningDelivery_PeriodisedValues
END
GO

CREATE TABLE Rulebase.AE_LearningDelivery_PeriodisedValues
(
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12) NOT NULL,
	[AimSeqNumber] int NOT NULL,
	[Period_1] decimal(15,5) NULL DEFAULT 0,
	[Period_2] decimal(15,5) NULL DEFAULT 0,
	[Period_3] decimal(15,5) NULL DEFAULT 0,
	[Period_4] decimal(15,5) NULL DEFAULT 0,
	[Period_5] decimal(15,5) NULL DEFAULT 0,
	[Period_6] decimal(15,5) NULL DEFAULT 0,
	[Period_7] decimal(15,5) NULL DEFAULT 0,
	[Period_8] decimal(15,5) NULL DEFAULT 0,
	[Period_9] decimal(15,5) NULL DEFAULT 0,
	[Period_10] decimal(15,5) NULL DEFAULT 0,
	[Period_11] decimal(15,5) NULL DEFAULT 0,
	[Period_12] decimal(15,5) NULL DEFAULT 0,
	CONSTRAINT [PK_AE_LearningDelivery_PeriodisedValues] PRIMARY KEY ([Ukprn], [LearnRefNumber], [AimSeqNumber])
)
