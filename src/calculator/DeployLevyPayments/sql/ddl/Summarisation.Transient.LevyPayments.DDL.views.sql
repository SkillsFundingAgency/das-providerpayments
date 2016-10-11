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
	acc.LevyBalance - ISNULL(stat.LevySpent,0) [Balance]
FROM ${DAS_Accounts.FQ}.dbo.DasAccounts acc
LEFT JOIN LevyPayments.AccountProcessStatus stat
	ON acc.AccountId = stat.AccountId
WHERE stat.HasBeenProcessed IS NULL OR stat.HasBeenProcessed = 0
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
	CommitmentId,
	Uln,
	Ukprn,
	AccountId,
	StartDate,
	EndDate,
	AgreedCost,
	StandardCode,
	ProgrammeType,
	FrameworkCode,
	PathwayCode,
	Priority
FROM ${DAS_Commitments.FQ}.dbo.DasCommitments
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
	cp.Period_ID,
	cp.Period,
	cp.Calendar_Year,
	cp.Collection_Open
FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping cp
GO