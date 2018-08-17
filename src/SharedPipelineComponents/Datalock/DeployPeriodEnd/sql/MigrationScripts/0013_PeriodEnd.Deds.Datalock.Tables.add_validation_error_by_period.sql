-----------------------------------------------------------------------------------------------------------------------------------------------
-- ValidationErrorByPeriod
-----------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ValidationErrorByPeriod' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    CREATE TABLE DataLock.ValidationErrorByPeriod
    (
        [Ukprn] bigint,
        [LearnRefNumber] varchar(12),
        [AimSeqNumber] int,
        [RuleId] varchar(50),
        [PriceEpisodeIdentifier] varchar(25) NOT NULL,
        [Period] int,
		[CollectionPeriodName] varchar(8) NOT NULL,
		[CollectionPeriodMonth] int NOT NULL,
		[CollectionPeriodYear] int NOT NULL
    )
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Cleanup Deds
-----------------------------------------------------------------------------------------------------------------------------------------------


IF EXISTS(select 1 from sys.procedures where name = 'CleanupDedsDatalocks')
BEGIN
    DROP PROCEDURE DataLock.[CleanupDedsDatalocks]
END
GO

CREATE PROCEDURE DataLock.[CleanupDedsDatalocks]
	@Ukprn bigint NULL	
AS

DECLARE @UkprnsToDelete TABLE (UKPRN bigint)

IF @Ukprn Is NULL
BEGIN
	INSERT INTO @UkprnsToDelete
	SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp
END
ELSE
BEGIN
	INSERT INTO @UkprnsToDelete (UKPRN) values (@ukprn)
END

DELETE FROM [DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT UKPRN from @UkprnsToDelete)

DELETE FROM [DataLock].[ValidationErrorByPeriod]
    WHERE [Ukprn] IN (SELECT UKPRN from @UkprnsToDelete)

DELETE FROM [DataLock].[PriceEpisodeMatch]
    WHERE [Ukprn] IN (SELECT UKPRN from @UkprnsToDelete)

DELETE FROM [DataLock].[PriceEpisodePeriodMatch]
    WHERE [Ukprn] IN (SELECT UKPRN from @UkprnsToDelete)

GO

