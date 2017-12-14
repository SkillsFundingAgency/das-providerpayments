


--DROP INDEX PK__ManualAd__3735DF3407A6ABE8
--CREATE INDEX PK_ManualAdjustments ON Adjustments.ManualAdjustments (RequiredPaymentIdToReverse)

CREATE INDEX IX_ManualAdjustments ON Adjustments.ManualAdjustments (UKPRN)


CREATE INDEX IX_ManualAdjustments_RequiredPaymentIdForReversal ON Adjustments.ManualAdjustments (RequiredPaymentIdForReversal)

