--IF IndexProperty(Object_Id('${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping]'), 'IX_PeriodMapping_YearOfCollection', 'IndexId') IS NULL
--BEGIN
--	CREATE INDEX IX_PeriodMapping_YearOfCollection ON ${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping] (YearOfCollection);
--END
--GO


CREATE INDEX IX_Payments_RequiredPaymentId ON Payments.Payments (
	RequiredPaymentId, 
	CollectionPeriodName, 
	CollectionPeriodMonth, 
	CollectionPeriodYear, 
	FundingSource)
GO


--DROP INDEX PK__Required__3214EC07AA351B24
--CREATE INDEX IX_RequiredPayments_Id_UKPRN_CommitmentId ON PaymentsDue.RequiredPayments (Id, UKPRN, CommitmentId)
--GO


CREATE INDEX IX_RequiredPayments_Ukprn ON PaymentsDue.RequiredPayments (UKPRN)
GO


ALTER TABLE PaymentsDue.RequiredPayments
ALTER COLUMN AccountId bigint
GO


