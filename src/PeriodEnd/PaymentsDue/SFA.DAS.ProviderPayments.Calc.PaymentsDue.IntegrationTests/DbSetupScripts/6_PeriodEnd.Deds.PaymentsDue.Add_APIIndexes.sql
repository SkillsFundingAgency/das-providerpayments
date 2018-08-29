IF NOT EXISTS(SELECT NULL FROM sys.indexes WHERE name='IX_RequiredPayments_AccountId')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_RequiredPayments_AccountId]
		ON [PaymentsDue].[RequiredPayments] ([AccountId])
END
GO

IF NOT EXISTS(SELECT NULL FROM sys.indexes WHERE name='IX_Payments_CollectionPeriod')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Payments_CollectionPeriod]
		ON [Payments].[Payments] ([CollectionPeriodYear], [CollectionPeriodMonth])
END
GO

IF NOT EXISTS(SELECT NULL FROM sys.indexes WHERE name='IX_Earnings_RequiredPaymentId')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_Earnings_RequiredPaymentId]
		ON [PaymentsDue].[Earnings] ([RequiredPaymentId])
END
GO
