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

SELECT p.RequiredPaymentId,
    p.CommitmentId,
    p.DeliveryMonth,
    p.DeliveryYear,
    p.TransactionType,
    p.FundingSource,
    p.Amount
FROM OPENQUERY(${DAS_PeriodEnd.servername}, '
		SELECT 
			p.RequiredPaymentId,
			rp.Ukprn ,
			rp.CommitmentId ,
			p.DeliveryMonth ,
			p.DeliveryYear ,
			p.TransactionType ,
			p.FundingSource,
			p.Amount 
		FROM 
			${DAS_PeriodEnd.databasename}.Payments.Payments p 
			JOIN ${DAS_PeriodEnd.databasename}.PaymentsDue.RequiredPayments rp on p.RequiredPaymentId = rp.Id
		WHERE
			IsNull(rp.CommitmentId, 0) > 0'
    ) AS p
WHERE p.Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
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

IF NOT EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'LevyPaymentsHistory'
AND i.name = 'IX_LevyPaymentsHistory_Commitment')
BEGIN 
	CREATE CLUSTERED INDEX [IX_LevyPaymentsHistory_Commitment] ON Reference.LevyPaymentsHistory (DeliveryYear, DeliveryMonth, TransactionType, CommitmentId)
END
