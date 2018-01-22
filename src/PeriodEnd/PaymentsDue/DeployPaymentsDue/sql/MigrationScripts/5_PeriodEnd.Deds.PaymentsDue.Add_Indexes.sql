

IF NOT EXISTS (SELECT 1 FROM sys.indexes i
				JOIN sys.objects t ON i.object_id = t.object_id
				WHERE t.name = 'Payments'
				AND i.name = 'IX_Payments_RequiredPayment')
BEGIN
	CREATE INDEX IX_Payments_RequiredPayment ON Payments.Payments (RequiredPaymentId, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, FundingSource);
END
GO


--DROP INDEX PK__Required__3214EC07AA351B24
--CREATE INDEX IX_RequiredPayments_Id_UKPRN_CommitmentId ON PaymentsDue.RequiredPayments (Id, UKPRN, CommitmentId)
--GO

IF NOT EXISTS (SELECT NULL FROM sys.indexes WHERE name = 'IX_RequiredPayments_Ukprn')
BEGIN
	CREATE INDEX IX_RequiredPayments_Ukprn ON PaymentsDue.RequiredPayments (UKPRN)
END
GO


ALTER TABLE PaymentsDue.RequiredPayments
ALTER COLUMN AccountId bigint
GO


