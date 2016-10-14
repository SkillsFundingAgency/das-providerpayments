IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
	EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CommitmentEarning
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CommitmentEarning' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP VIEW PaymentsDue.vw_CommitmentEarning
END
GO

CREATE VIEW PaymentsDue.vw_CommitmentEarning
AS
SELECT
	lc.CommitmentId,
	ld.MonthlyInstallment,
	ld.CompletionPayment,
	pv.Ukprn,
	pv.LearnRefNumber,
	pv.AimSeqNumber,
	pv.Period_1,
	pv.Period_2,
	pv.Period_3,
	pv.Period_4,
	pv.Period_5,
	pv.Period_6,
	pv.Period_7,
	pv.Period_8,
	pv.Period_9,
	pv.Period_10,
	pv.Period_11,
	pv.Period_12
FROM ${ILR_Deds.FQ}.Rulebase.AE_LearningDelivery ld
INNER JOIN ${ILR_Deds.FQ}.Rulebase.AE_LearningDelivery_PeriodisedValues pv
	ON ld.Ukprn = pv.Ukprn
	AND ld.LearnRefNumber = pv.LearnRefNumber
	AND ld.AimSeqNumber = pv.AimSeqNumber
INNER JOIN DataLock.DasLearnerCommitment lc
	ON pv.Ukprn = lc.Ukprn
	AND ld.LearnRefNumber = lc.LearnRefNumber
	AND ld.AimSeqNumber = lc.AimSeqNumber
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CollectionPeriods' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP VIEW PaymentsDue.vw_CollectionPeriods
END
GO

CREATE VIEW PaymentsDue.vw_CollectionPeriods
AS
SELECT
	cp.Period_ID,
	cp.Period,
	cp.Calendar_Year,
	cp.Collection_Open,
	cp.Collection_Period
FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping cp
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Providers' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP VIEW PaymentsDue.vw_Providers
END
GO

CREATE VIEW PaymentsDue.vw_Providers
AS
SELECT
	p.UKPRN
FROM ${ILR_Deds.FQ}.Valid.LearningProvider p
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PaymentHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PaymentHistory' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP VIEW PaymentsDue.vw_PaymentHistory
END
GO

CREATE VIEW PaymentsDue.vw_PaymentHistory
AS
SELECT
	CommitmentId,
	LearnRefNumber,
	AimSeqNumber,
	Ukprn,
	DeliveryMonth,
	DeliveryYear,
	CollectionPeriodMonth,
	CollectionPeriodYear,
	AmountDue ,
	TransactionType 
FROM ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_RequiredPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RequiredPayments' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP VIEW PaymentsDue.vw_RequiredPayments
END
GO

CREATE VIEW PaymentsDue.vw_RequiredPayments
AS
SELECT
    Id,
	CommitmentId,
	LearnRefNumber,
	AimSeqNumber,
	Ukprn,
	DeliveryMonth,
	DeliveryYear,
	(SELECT MAX([Collection_Period]) FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1) AS CollectionPeriodName,
	(SELECT MAX([Period]) FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1) AS CollectionPeriodMonth,
	(SELECT MAX([Calendar_Year]) FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1) AS CollectionPeriodYear,
	AmountDue,
	TransactionType 
FROM PaymentsDue.RequiredPayments
GO