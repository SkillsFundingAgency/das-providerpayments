IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='LevyPayments')
BEGIN
	EXEC('CREATE SCHEMA LevyPayments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Accounts requiring processing
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.views WHERE [name] = 'vw_AccountsRequiringProcessing' and [schema_id] = SCHEMA_ID('LevyPayments'))
	BEGIN
		DROP VIEW LevyPayments.vw_AccountsRequiringProcessing
	END
GO

CREATE VIEW LevyPayments.vw_AccountsRequiringProcessing
AS
SELECT
	acc.AccountId,
	acc.AccountName,
	acc.Balance - ISNULL(stat.LevySpent,0) [Balance]
FROM Reference.DasAccounts acc
LEFT JOIN LevyPayments.AccountProcessStatus stat
	ON acc.AccountId = stat.AccountId
WHERE acc.AccountId IN (SELECT AccountId FROM Reference.DasCommitments)
AND (stat.HasBeenProcessed IS NULL OR stat.HasBeenProcessed = 0)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Account commitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.views WHERE [name] = 'vw_AccountCommitments' and [schema_id] = SCHEMA_ID('LevyPayments'))
	BEGIN
		DROP VIEW LevyPayments.vw_AccountCommitments
	END
GO

CREATE VIEW LevyPayments.vw_AccountCommitments
AS
SELECT
	[CommitmentId],
	[Uln],
	[Ukprn],
	[AccountId],
	[StartDate],
	[EndDate],
	[AgreedCost],
	[StandardCode],
	[ProgrammeType],
	[FrameworkCode],
	[PathwayCode],
	[Priority],
	[VersionId],
	RANK() OVER (PARTITION BY [CommitmentId] ORDER BY [VersionId] DESC) AS [Rank]
FROM Reference.DasCommitments
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CollectionPeriods' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP VIEW LevyPayments.vw_CollectionPeriods
END
GO

CREATE VIEW LevyPayments.vw_CollectionPeriods
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
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PaymentsDue' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP VIEW LevyPayments.vw_PaymentsDue
END
GO

CREATE VIEW LevyPayments.vw_PaymentsDue
AS
SELECT
	Id,
	CommitmentId,
	LearnRefNumber,
	AimSeqNumber,
	Ukprn,
	DeliveryMonth,
	DeliveryYear,
	TransactionType,
	AmountDue
FROM PaymentsDue.RequiredPayments
WHERE  UseLevyBalance = 1 AND TransactionType Not IN (4,5,6,7,8,9,10,11,12,13,14,15)
	And Id NOT In (Select RequiredPaymentIdForReversal from Adjustments.ManualAdjustments)	
GO