IF IndexProperty(Object_Id('[dbo].[Collection_Period_Mapping]'), 'IX_PeriodMapping_YearOfCollection', 'IndexId') IS NULL
BEGIN
	CREATE INDEX IX_PeriodMapping_YearOfCollection ON [dbo].[Collection_Period_Mapping] (YearOfCollection);
END
GO



-- Think this already exists
--CREATE INDEX IX_LearningProvider_UKPRN ON ${ILR_Deds.FQ}.[Valid].[LearningProvider] (UKPRN)


-- Replace clustered index on Payments.Payments
--DROP INDEX PK__Payments__9B556A387D159181
--CREATE INDEX PK_Payments ON Payments.Payments (PaymentId)


CREATE INDEX IX_Payments_RequiredPayment ON Payments.Payments (RequiredPaymentId, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, FundingSource)


-- Replace clustered index on random
--DROP INDEX PK__Required__3214EC07132E501E
--CREATE INDEX PK_RequiredPayments ON PaymentsDue.RequiredPayments (Id)

CREATE INDEX IX_RequiredPayments_Ukprn ON PaymentsDue.RequiredPayments (UKPRN, FundingSource)



