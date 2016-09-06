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
	acc.LevyBalance
FROM Reference.DasAccounts acc
LEFT JOIN LevyPayments.AccountProcessStatus stat
	ON acc.AccountId = stat.AccountId
WHERE stat.HasBeenProcessed IS NULL OR stat.HasBeenProcessed = 0