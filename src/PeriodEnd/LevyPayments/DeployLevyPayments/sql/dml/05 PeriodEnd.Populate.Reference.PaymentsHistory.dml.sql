IF  EXISTS (
		SELECT 1
		FROM [sys].[indexes] i
		JOIN sys.objects t ON i.object_id = t.object_id
		WHERE t.name = 'LevyPaymentsHistory'
		AND i.[name] = 'IX_LevyPaymentHistory_RequiredPaymentId'
		)
BEGIN
	DROP INDEX IX_LevyPaymentHistory_RequiredPaymentId ON Reference.LevyPaymentsHistory 
END
GO

TRUNCATE TABLE [Reference].[LevyPaymentsHistory]
GO

INSERT INTO [Reference].[LevyPaymentsHistory]

SELECT

	p.RequiredPaymentId,
	rp.CommitmentId ,
	p.DeliveryMonth ,
	p.DeliveryYear ,
	p.TransactionType ,
	p.FundingSource,
	p.Amount 
    FROM  ${DAS_PeriodEnd.FQ}.Payments.Payments p 
	JOIN  ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments rp on p.RequiredPaymentId = rp.Id
    WHERE rp.Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
	AND IsNull(rp.CommitmentId,0) > 0
GO


IF NOT EXISTS (
		SELECT 1
		FROM [sys].[indexes] i
		JOIN sys.objects t ON i.object_id = t.object_id
		WHERE t.name = 'LevyPaymentsHistory'
		AND i.[name] = 'IX_LevyPaymentHistory_RequiredPaymentId'
		)
BEGIN
	CREATE INDEX IX_LevyPaymentHistory_RequiredPaymentId ON Reference.LevyPaymentsHistory (RequiredPaymentId)
END
GO