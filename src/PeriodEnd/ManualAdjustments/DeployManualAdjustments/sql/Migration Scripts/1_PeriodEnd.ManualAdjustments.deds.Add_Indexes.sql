--IF IndexProperty(Object_Id('${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping]'), 'IX_PeriodMapping_YearOfCollection', 'IndexId') IS NULL
--BEGIN
--	CREATE INDEX IX_PeriodMapping_YearOfCollection ON ${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping] (YearOfCollection);
--END
--GO


DROP INDEX PK__ManualAd__3735DF3407A6ABE8
CREATE INDEX PK_ManualAdjustments ON Adjustments.ManualAdjustments (RequiredPaymentIdToReverse)

CREATE INDEX IX_ManualAdjustments ON Adjustments.ManualAdjustments (UKPRN)


CREATE INDEX IX_ManualAdjustments_RequiredPaymentIdForReversal ON Adjustments.ManualAdjustments (RequiredPaymentIdForReversal)

