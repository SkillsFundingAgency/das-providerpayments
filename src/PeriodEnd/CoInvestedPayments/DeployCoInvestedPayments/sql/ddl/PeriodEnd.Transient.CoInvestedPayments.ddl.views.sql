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
	[Id] AS [Period_ID],
	[Name] AS [Collection_Period],
	[CalendarMonth] AS [Period],
	[CalendarYear] AS [Calendar_Year],
	[Open] AS [Collection_Open]
FROM Reference.CollectionPeriods
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
		rp.Ukprn,
		rp.ULN,
		rp.DeliveryMonth,
		rp.DeliveryYear,
		rp.TransactionType,
		(rp.AmountDue - 
			(SELECT COALESCE(SUM(Amount), 0) FROM TransferPayments.Payments WHERE RequiredPaymentId = rp.Id) - 
			(SELECT COALESCE(SUM(Amount), 0) FROM LevyPayments.Payments WHERE RequiredPaymentId = rp.Id)) AS AmountDue,
		rp.SfaContributionPercentage,
		rp.AimSeqNumber As AimSequenceNumber,
		rp.StandardCode,
		rp.ProgrammeType,
		rp.FrameworkCode,
		rp.PathwayCode
	FROM PaymentsDue.RequiredPayments rp
	WHERE (rp.AmountDue - 
		(SELECT COALESCE(SUM(Amount), 0) FROM TransferPayments.Payments WHERE RequiredPaymentId = rp.Id) - 
		(SELECT COALESCE(SUM(Amount), 0) FROM LevyPayments.Payments WHERE RequiredPaymentId = rp.Id)) > 0
	AND rp.Id NOT In (Select RequiredPaymentIdForReversal from Adjustments.ManualAdjustments)	
GO

