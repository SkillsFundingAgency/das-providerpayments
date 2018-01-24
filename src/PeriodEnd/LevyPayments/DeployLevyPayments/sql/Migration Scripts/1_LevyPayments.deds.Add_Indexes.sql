IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'Payments'
AND i.name = 'IX_Payments_RequiredPaymentId')
BEGIN
	DROP INDEX IX_Payments_RequiredPaymentId ON Payments.Payments
END
GO

CREATE INDEX IX_Payments_RequiredPaymentId ON Payments.Payments (
	RequiredPaymentId, 
	CollectionPeriodName, 
	CollectionPeriodMonth, 
	CollectionPeriodYear, 
	FundingSource)
GO
