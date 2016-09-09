IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentSchedule')
BEGIN
	EXEC('CREATE SCHEMA PaymentSchedule')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CommitmentEarning' AND [schema_id] = SCHEMA_ID('PaymentSchedule'))
BEGIN
	DROP TABLE PaymentSchedule.vw_CommitmentEarning
END
GO

IF EXISTS (SELECT [object_id] FROM sys.views WHERE [name] = 'vw_CommitmentEarning' and [schema_id] = SCHEMA_ID('PaymentSchedule'))
	BEGIN
		DROP VIEW LevyPayments.vw_CommitmentEarning
	END
GO

CREATE VIEW PaymentSchedule.vw_CommitmentEarning
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
INNER JOIN ${DAS_Commitments.FQ}.dbo.DasCommitments c
	ON ld.Ukprn = c.Ukprn
	AND ld.Uln = c.Uln
	AND (
			(ld.StdCode IS NOT NULL AND ld.StdCode = c.StandardCode)
			OR
			(ld.StdCode IS NULL AND ld.FworkCode = c.FrameworkCode AND ld.ProgType = c.ProgrammeType AND ld.PwayCode = c.PathwayCode)
		)
	AND ld.NegotiatedPrice = c.AgreedCost
GO