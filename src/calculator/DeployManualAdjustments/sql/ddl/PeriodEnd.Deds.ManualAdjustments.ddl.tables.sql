IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Adjustments')
BEGIN
    EXEC('CREATE SCHEMA Adjustments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ManualAdjustments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ManualAdjustments' AND [schema_id] = SCHEMA_ID('Adjustments'))
BEGIN
    DROP TABLE Adjustments.ManualAdjustments
END
GO
CREATE TABLE Adjustments.ManualAdjustments
(
    RequiredPaymentIdToReverse uniqueidentifier NOT NULL PRIMARY KEY,
    ReasonForReversal nvarchar(max) NOT NULL,
    RequestorName nvarchar(255) NOT NULL,
    DateUploaded datetime NOT NULL DEFAULT(GETDATE()),
    RequiredPaymentIdForReversal uniqueidentifier NULL,
)