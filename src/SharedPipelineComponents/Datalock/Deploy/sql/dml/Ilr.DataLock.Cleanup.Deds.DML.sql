DECLARE @ukprn int = (SELECT DISTINCT [Ukprn] FROM [Valid].[LearningProvider])

EXEC ${ILR_Deds.FQ}.[DataLock].[CleanUpDatalock] @ukprn