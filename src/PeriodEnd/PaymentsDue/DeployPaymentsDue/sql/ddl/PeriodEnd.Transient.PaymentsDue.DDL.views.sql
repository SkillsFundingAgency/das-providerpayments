IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
    EXEC('CREATE SCHEMA PaymentsDue')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CollectionPeriods' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_CollectionPeriods
END
GO

CREATE VIEW PaymentsDue.vw_CollectionPeriods
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
-- vw_EarningsForPayment
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_EarningsForPayment' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_EarningsForPayment
END
GO

CREATE VIEW PaymentsDue.vw_EarningsForPayment
AS
SELECT 
	R.Id [RequiredPaymentId],
	Instalment [MonthlyInstallment],
	StartDate,
	PlannedEndDate,
	ActualEndDate,
	CompletionStatus,
	CompletionPayment [CompletionAmount],
	PlannedInstalments [TotalInstallments],
	EndpointAssessorId [EndpointAssessorId]

FROM PaymentsDue.RequiredPayments R
LEFT JOIN Reference.IlrBreakdown I
	ON R.Ukprn = I.Ukprn
	AND R.LearnRefNumber = I.LearnRefNumber
	AND R.PriceEpisodeIdentifier = I.PriceEpisodeIdentifier

GO