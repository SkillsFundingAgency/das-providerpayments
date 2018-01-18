CREATE INDEX IX_Payments_RequiredPaymentId ON Payments.Payments (
	RequiredPaymentId, 
	CollectionPeriodName, 
	CollectionPeriodMonth, 
	CollectionPeriodYear, 
	FundingSource)
GO






