IF NOT EXISTS (
		SELECT 1
		FROM [sys].[indexes] i
		JOIN sys.objects t ON i.object_id = t.object_id
		WHERE t.name = 'ManualAdjustments'
		AND i.[name] = 'IX_ManualAdjustments'
		)
BEGIN
	CREATE INDEX IX_ManualAdjustments ON Adjustments.ManualAdjustments (UKPRN)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [sys].[indexes] i
		JOIN sys.objects t ON i.object_id = t.object_id
		WHERE t.name = 'ManualAdjustments'
		AND i.[name] = 'IX_ManualAdjustments_RequiredPaymentIdForReversal'
		)
BEGIN
	CREATE INDEX IX_ManualAdjustments_RequiredPaymentIdForReversal ON Adjustments.ManualAdjustments (RequiredPaymentIdForReversal)
END
GO