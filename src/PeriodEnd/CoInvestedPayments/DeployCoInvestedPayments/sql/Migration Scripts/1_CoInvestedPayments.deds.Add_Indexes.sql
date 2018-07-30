IF EXISTS (SELECT 1 FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'Collection_Period_Mapping'
AND i.name = 'IX_PeriodMapping_YearOfCollection')
BEGIN
	DROP INDEX IX_PeriodMapping_YearOfCollection ON [dbo].[Collection_Period_Mapping]
END
GO
CREATE INDEX IX_PeriodMapping_YearOfCollection ON [dbo].[Collection_Period_Mapping] (Collection_Year);
GO

-- Think this already exists
--CREATE INDEX IX_LearningProvider_UKPRN ON ${ILR_Deds.FQ}.[Valid].[LearningProvider] (UKPRN)

-- Replace clustered index on Payments.Payments
--DROP INDEX PK__Payments__9B556A387D159181
--CREATE INDEX PK_Payments ON Payments.Payments (PaymentId)

IF EXISTS (SELECT 1 FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'Payments'
AND i.name = 'IX_Payments_RequiredPayment')
BEGIN
	DROP INDEX IX_Payments_RequiredPayment ON Payments.Payments 
END
GO
CREATE INDEX IX_Payments_RequiredPayment ON Payments.Payments (RequiredPaymentId, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, FundingSource);
GO

-- Replace clustered index on random
--CREATE INDEX PK_RequiredPayments ON PaymentsDue.RequiredPayments (Id)
IF EXISTS (SELECT 1 FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'RequiredPayments'
AND i.name = 'IX_RequiredPayments_Ukprn')
BEGIN
	DROP INDEX IX_RequiredPayments_Ukprn ON PaymentsDue.RequiredPayments
END
GO
CREATE INDEX IX_RequiredPayments_Ukprn ON PaymentsDue.RequiredPayments (UKPRN, FundingLineType)
