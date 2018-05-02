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
	(acc.Balance - ISNULL(stat.LevySpent,0) - ISNULL(ta.Amount,0)) [Balance]
FROM Reference.DasAccounts acc
LEFT JOIN LevyPayments.AccountProcessStatus stat ON acc.AccountId = stat.AccountId
LEFT JOIN TransferPayments.vw_TransferedAmountForAccount ta ON acc.AccountId = ta.FundingAccountId
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
	rp.Id,
	rp.CommitmentId,
	rp.LearnRefNumber,
	rp.AimSeqNumber,
	rp.Ukprn,
	rp.DeliveryMonth,
	rp.DeliveryYear,
	rp.TransactionType,
	(rp.AmountDue - COALESCE(tp.Amount, 0.00)) AS AmountDue
FROM PaymentsDue.RequiredPayments rp
	LEFT JOIN TransferPayments.Payments tp ON rp.Id = tp.RequiredPaymentId
WHERE  rp.UseLevyBalance = 1 AND rp.TransactionType IN (1,2,3) 
	AND rp.Id NOT In (Select RequiredPaymentIdForReversal from Adjustments.ManualAdjustments)	
