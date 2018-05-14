DECLARE @CollectionPeriodMonth int = (SELECT [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1)
DECLARE @CollectionPeriodYear int = (SELECT [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)

DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationError]
    WHERE 
        [CollectionPeriodMonth] = @CollectionPeriodMonth
        AND [CollectionPeriodYear] = @CollectionPeriodYear


DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[PriceEpisodeMatch]
    WHERE 
		[CollectionPeriodMonth] = @CollectionPeriodMonth
        AND [CollectionPeriodYear] = @CollectionPeriodYear


DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[PriceEpisodePeriodMatch]
    WHERE 
		[CollectionPeriodMonth] = @CollectionPeriodMonth
        AND [CollectionPeriodYear] = @CollectionPeriodYear
