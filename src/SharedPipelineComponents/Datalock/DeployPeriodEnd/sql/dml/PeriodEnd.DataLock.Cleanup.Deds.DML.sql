DECLARE @CollectionPeriodName varchar(8) = (SELECT [AcademicYear] FROM Reference.CollectionPeriods WHERE [Open] = 1) + '-' + (SELECT [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1)


DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationError]
    WHERE 
        [CollectionPeriodName] = @CollectionPeriodName


DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationErrorByPeriod]
    WHERE 
		[CollectionPeriodName] = @CollectionPeriodName
        

DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[PriceEpisodeMatch]
    WHERE 
		[CollectionPeriodName] = @CollectionPeriodName
        

DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[PriceEpisodePeriodMatch]
    WHERE 
		[CollectionPeriodName] = @CollectionPeriodName
        