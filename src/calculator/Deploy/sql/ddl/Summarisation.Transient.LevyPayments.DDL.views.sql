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
FROM ${DAS_Accounts.FQ}.Reference.DasAccounts acc
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
	PathwayCode
FROM ${DAS_Commitments.FQ}.Reference.DataLockCommitments
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Commitment earning
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT [object_id] FROM sys.views WHERE [name] = 'vw_CommitmentEarning' and [schema_id] = SCHEMA_ID('LevyPayments'))
	BEGIN
		DROP VIEW LevyPayments.vw_CommitmentEarning
	END
GO

CREATE VIEW LevyPayments.vw_CommitmentEarning
AS
SELECT
	c.CommitmentId,
	ld.LearnRefNumber,
	ld.AimSeqNumber,
	ld.Ukprn,
	ld.Uln,
	ld.StdCode,
	ld.FworkCode,
	ld.ProgType,
	ld.PwayCode,
	ld.MonthlyInstallment,
	ld.MonthlyInstallmentUncapped,
	ld.CompletionPayment,
	ld.CompletionPaymentUncapped,
	ld.CurrentPeriod,
	ld.NumberOfPeriods,
	ld.LearnStartDate,
	ld.LearnPlanEndDate,
	ld.LearnActEndDate
FROM ${ILR_Current.FQ}.Rulebase.AE_LearningDelivery ld
INNER JOIN ${DAS_Commitments.FQ}.Reference.DataLockCommitments c
	ON ld.Ukprn = c.Ukprn
	AND ld.Uln = c.Uln
	AND (
			(ld.StdCode IS NOT NULL AND ld.StdCode = c.StandardCode)
			OR
			(ld.StdCode IS NULL AND ld.FworkCode = c.FrameworkCode AND ld.ProgType = c.ProgrammeType AND ld.PwayCode = c.PathwayCode)
		)
GO