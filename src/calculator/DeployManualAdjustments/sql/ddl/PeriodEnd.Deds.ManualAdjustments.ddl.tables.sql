IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Adjustments')
BEGIN
    EXEC('CREATE SCHEMA Adjustments')
END
GO

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='CleanupAdjustments' AND [schema_id] = SCHEMA_ID('Adjustments'))
BEGIN
	DROP PROCEDURE [Adjustments].[CleanupAdjustments]
END
GO

CREATE PROCEDURE [Adjustments].[CleanupAdjustments] 
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM Adjustments.ManualAdjustments WHERE RequiredPaymentIdForReversal IS NULL

END
GO