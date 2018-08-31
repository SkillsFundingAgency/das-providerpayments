DECLARE @ukprn bigint = (SELECT DISTINCT [Ukprn] FROM [Valid].[LearningProvider])

EXEC ${ILR_Deds.FQ}.[DataLock].[CleanupDedsDatalocks] @ukprn
