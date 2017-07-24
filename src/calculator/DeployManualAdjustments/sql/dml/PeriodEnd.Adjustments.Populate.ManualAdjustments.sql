TRUNCATE TABLE Adjustments.ManualAdjustments
GO

INSERT INTO Adjustments.ManualAdjustments
(RequiredPaymentIdToReverse, RequiredPaymentIdForReversal)
SELECT
	RequiredPaymentIdToReverse,
	RequiredPaymentIdForReversal
FROM ${DAS_PeriodEnd.FQ}.Adjustments.ManualAdjustments
WHERE RequiredPaymentIdForReversal IS NULL
GO