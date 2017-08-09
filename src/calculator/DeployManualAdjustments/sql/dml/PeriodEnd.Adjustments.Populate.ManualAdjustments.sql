TRUNCATE TABLE Adjustments.ManualAdjustments
GO

INSERT INTO Adjustments.ManualAdjustments
(RequiredPaymentIdToReverse,ReasonForReversal,RequestorName,DateUploaded,RequiredPaymentIdForReversal)
SELECT
	RequiredPaymentIdToReverse ,
    ReasonForReversal,
    RequestorName,
    DateUploaded,
    RequiredPaymentIdForReversal
FROM ${DAS_PeriodEnd.FQ}.Adjustments.ManualAdjustments
WHERE RequiredPaymentIdForReversal IS NULL
GO