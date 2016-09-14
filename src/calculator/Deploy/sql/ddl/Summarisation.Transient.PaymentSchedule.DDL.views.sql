IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentSchedule')
BEGIN
	EXEC('CREATE SCHEMA PaymentSchedule')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CommitmentEarning
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CommitmentEarning' AND [schema_id] = SCHEMA_ID('PaymentSchedule'))
BEGIN
	DROP VIEW PaymentSchedule.vw_CommitmentEarning
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

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CollectionPeriods' AND [schema_id] = SCHEMA_ID('PaymentSchedule'))
BEGIN
	DROP VIEW PaymentSchedule.vw_CollectionPeriods
END
GO

CREATE VIEW PaymentSchedule.vw_CollectionPeriods
AS
SELECT
	cp.Period_ID,
	cp.Period,
	cp.Calendar_Year,
	cp.Collection_Open
FROM ${ILR_Summarisation.FQ}.dbo.Collection_Period_Mapping cp
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Providers' AND [schema_id] = SCHEMA_ID('PaymentSchedule'))
BEGIN
	DROP VIEW PaymentSchedule.vw_Providers
END
GO

CREATE VIEW PaymentSchedule.vw_Providers
AS
SELECT
	p.UKPRN
FROM ${ILR_Current.FQ}.Valid.LearningProvider p
GO