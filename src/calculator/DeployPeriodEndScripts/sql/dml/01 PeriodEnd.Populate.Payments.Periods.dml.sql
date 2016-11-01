DELETE ${DAS_PeriodEnd.FQ}.Payments.Periods
FROM ${DAS_PeriodEnd.FQ}.Payments.Periods p
INNER JOIN Reference.CollectionPeriods cp
	ON p.CalendarMonth = cp.CalendarMonth
	AND p.CalendarYear = cp.CalendarYear
GO

INSERT INTO ${DAS_PeriodEnd.FQ}.Payments.Periods
SELECT 
	${ILR_AcademicYear} + '-' + name, 
	CalendarMonth, 
	CalendarYear, 
	(SELECT MAX(ReadDateTime) FROM ${DAS_Accounts.FQ}.dbo.DasAccountsAudit) AccountDataValidAt, 
	(SELECT MAX(ReadDate) FROM ${DAS_Commitments.FQ}.dbo.EventStreamPointer) CommitmentDataValidAt, 
	GETDATE() CompletionDateTime 
FROM Reference.CollectionPeriods
GO