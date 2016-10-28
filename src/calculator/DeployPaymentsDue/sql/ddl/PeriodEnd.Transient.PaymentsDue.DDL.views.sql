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
		c.VersionId CommitmentVersionId,
		a.AccountId,
		a.VersionId AccountVersionId,
		ae.MonthlyInstallment,
		ae.CompletionPayment,
		ae.Ukprn,
		ae.Uln,
		ae.LearnRefNumber,
		ae.AimSeqNumber,
		ae.Period_1,
		ae.Period_2,
		ae.Period_3,
		ae.Period_4,
		ae.Period_5,
		ae.Period_6,
		ae.Period_7,
		ae.Period_8,
		ae.Period_9,
		ae.Period_10,
		ae.Period_11,
		ae.Period_12
	FROM Reference.ApprenticeshipEarnings ae
	INNER JOIN DataLock.DasLearnerCommitment lc 
		ON ae.Ukprn = lc.Ukprn
		AND ae.LearnRefNumber = lc.LearnRefNumber
		AND ae.AimSeqNumber = lc.AimSeqNumber
	INNER JOIN Reference.DasCommitments c
		ON c.CommitmentId = lc.CommitmentId
	INNER JOIN Reference.DasAccounts a
		ON c.AccountId = a.AccountId
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
	[Id] AS [Period_ID],
	[Name] AS [Collection_Period],
	[CalendarMonth] AS [Period],
	[CalendarYear] AS [Calendar_Year],
	[Open] AS [Collection_Open]
FROM Reference.CollectionPeriods
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
	p.Ukprn
FROM Reference.Providers p
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
	AmountDue,
	TransactionType 
FROM Reference.RequiredPaymentsHistory
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
	(SELECT MAX([Name]) FROM [Reference].[CollectionPeriods] WHERE [Open] = 1) AS CollectionPeriodName,
	(SELECT MAX([CalendarMonth]) FROM [Reference].[CollectionPeriods] WHERE [Open] = 1) AS CollectionPeriodMonth,
	(SELECT MAX([CalendarYear]) FROM [Reference].[CollectionPeriods] WHERE [Open] = 1) AS CollectionPeriodYear,
	AmountDue,
	TransactionType 
FROM PaymentsDue.RequiredPayments
GO