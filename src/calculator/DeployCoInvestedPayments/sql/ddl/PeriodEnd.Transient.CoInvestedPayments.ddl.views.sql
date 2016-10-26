IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='CoInvestedPayments')
BEGIN
	EXEC('CREATE SCHEMA CoInvestedPayments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CollectionPeriods' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP VIEW CoInvestedPayments.vw_CollectionPeriods
END
GO

CREATE VIEW CoInvestedPayments.vw_CollectionPeriods
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
-- vw_RequiredPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RequiredPayments' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP VIEW CoInvestedPayments.vw_RequiredPayments
END
GO

CREATE VIEW CoInvestedPayments.vw_RequiredPayments
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
		(rp.AmountDue - COALESCE(lp.Amount, 0.00)) AS AmountDue
	FROM PaymentsDue.RequiredPayments rp
		LEFT JOIN LevyPayments.Payments lp ON rp.Id = lp.RequiredPaymentId
	WHERE (rp.AmountDue - COALESCE(lp.Amount, 0.00)) <> 0
GO

