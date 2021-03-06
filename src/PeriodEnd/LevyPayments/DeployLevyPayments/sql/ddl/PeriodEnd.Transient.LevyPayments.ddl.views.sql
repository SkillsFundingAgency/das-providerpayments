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
LEFT JOIN LevyPayments.AccountProcessStatus stat 
	ON acc.AccountId = stat.AccountId
LEFT JOIN TransferPayments.vw_TransferedAmountForAccount ta 
	ON acc.AccountId = ta.SendingAccountId
WHERE acc.AccountId IN (SELECT AccountId FROM Reference.DasCommitments)
AND (stat.HasBeenProcessed IS NULL OR stat.HasBeenProcessed = 0)
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
	rp.CommitmentVersionId,
	rp.LearnRefNumber,
	rp.AimSeqNumber,
	rp.Ukprn,
	rp.DeliveryMonth,
	rp.DeliveryYear,
	rp.TransactionType,
	(rp.AmountDue - (SELECT COALESCE(SUM(Amount), 0) FROM TransferPayments.Payments WHERE RequiredPaymentId = rp.Id)) AS AmountDue
FROM PaymentsDue.RequiredPayments rp
WHERE  rp.UseLevyBalance = 1 
AND rp.TransactionType IN (1,2,3) 
AND rp.Id NOT In (Select RequiredPaymentIdForReversal from Adjustments.ManualAdjustments)	
AND (rp.AmountDue - (SELECT COALESCE(SUM(Amount), 0) FROM TransferPayments.Payments WHERE RequiredPaymentId = rp.Id)) > 0
