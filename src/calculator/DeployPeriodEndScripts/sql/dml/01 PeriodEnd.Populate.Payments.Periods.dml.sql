DELETE ${DAS_PeriodEnd.FQ}.Payments.Periods
FROM ${DAS_PeriodEnd.FQ}.Payments.Periods p
INNER JOIN Reference.CollectionPeriods cp
	ON p.PeriodName = '${YearOfCollection}-' + cp.Name
GO

DECLARE @now DATETIME = GETDATE()
DECLARE @completionDateTime DATETIME

SET @completionDateTime = (SELECT DATEADD(MONTH, 1, DATEFROMPARTS(q.CalendarYear, q.CalendarMonth, 1)) FROM Reference.CollectionPeriods q WHERE [OPEN]=1)
SET @completionDateTime = DATEADD(DAY, DAY(@now) - 1, @completionDateTime)

SET @completionDateTime = DATEADD(HOUR, DATEPART(HOUR, @now), @completionDateTime)
SET @completionDateTime = DATEADD(MINUTE, DATEPART(MINUTE, @now), @completionDateTime)
SET @completionDateTime = DATEADD(SECOND, DATEPART(SECOND, @now), @completionDateTime)
SET @completionDateTime = DATEADD(MILLISECOND, DATEPART(MILLISECOND, @now), @completionDateTime)

INSERT INTO ${DAS_PeriodEnd.FQ}.Payments.Periods
SELECT 
    '${YearOfCollection}-' + name, 
    CalendarMonth, 
    CalendarYear, 
    (SELECT MAX(ReadDateTime) FROM ${DAS_Accounts.FQ}.dbo.DasAccountsAudit) AccountDataValidAt, 
    (SELECT MAX(ReadDate) FROM ${DAS_Commitments.FQ}.dbo.EventStreamPointer) CommitmentDataValidAt, 
    @completionDateTime CompletionDateTime 
FROM Reference.CollectionPeriods
GO

