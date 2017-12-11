IF IndexProperty(Object_Id('${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping]'), 'IX_PeriodMapping_YearOfCollection', 'IndexId') IS NULL
BEGIN
	CREATE INDEX IX_PeriodMapping_YearOfCollection ON ${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping] (YearOfCollection);
END
GO



-- Think this already exists
--CREATE INDEX IX_LearningProvider_UKPRN ON ${ILR_Deds.FQ}.[Valid].[LearningProvider] (UKPRN)


CREATE INDEX IX_FileDetails_UKPRN ON ${ILR_Deds.FQ}.[dbo].[FileDetails] (Id);
GO


-- Make this more sequential. Not perfect, but should help with inserts
DROP INDEX PK__Payments__9B556A387D159181
CREATE CLUSTERED INDEX PK_Payments ON ${DAS_PeriodEnd.FQ}.Payments.Payments (DeliveryYear, DeliveryMonth, PaymentId)


CREATE INDEX IX_Payments_RequiredPayment ON ${DAS_PeriodEnd.FQ}.Payments.Payments (RequiredPaymentId, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, FundingSource)

-- Making a random key more sequential
DROP INDEX 
CREATE CLUSTERED INDEX PK_RequiredPayments ON ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments (IlrSubmissionDateTime, Id)

CREATE INDEX IX_RequiredPayments_Ukprn ON ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments (UKPRN, FundingSource)



