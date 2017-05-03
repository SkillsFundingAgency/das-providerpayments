DELETE ${DAS_PeriodEnd.FQ}.Payments.Periods
FROM ${DAS_PeriodEnd.FQ}.Payments.Periods p
INNER JOIN Reference.CollectionPeriods cp
	ON p.PeriodName = '${YearOfCollection}-' + cp.Name
	WHERE cp.[Open]=1
GO

DECLARE @completionDateTime DATETIME = GETDATE()

INSERT INTO ${DAS_PeriodEnd.FQ}.Payments.Periods
SELECT 
    '${YearOfCollection}-' + name, 
    CalendarMonth, 
    CalendarYear, 
    (SELECT MAX(ReadDateTime) FROM ${DAS_Accounts.FQ}.dbo.DasAccountsAudit) AccountDataValidAt, 
    (SELECT MAX(ReadDate) FROM ${DAS_Commitments.FQ}.dbo.EventStreamPointer) CommitmentDataValidAt, 
    @completionDateTime CompletionDateTime 
FROM Reference.CollectionPeriods
WHERE [Open]=1
GO

