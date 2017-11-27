IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
    EXEC('CREATE SCHEMA DataLock')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ValidationError
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(select 1 from sys.procedures where name = 'CleanupDeds')
BEGIN
    DROP PROCEDURE DataLock.CleanupDeds
END
GO


CREATE PROCEDURE DataLock.CleanupDeds
	@Ukprn bigint NULL	
AS

DECLARE @UkprnsToDelete TABLE (UKPRN bigint)

IF @Ukprn Is NULL
BEGIN
	INSERT INTO @UkprnsToDelete
	SELECT DISTINCT lp.[Ukprn] FROM ${ILR_Deds.FQ}.[Valid].[LearningProvider] lp
END
ELSE
BEGIN
	INSERT INTO @UkprnsToDelete (UKPRN) values (@ukprn)
END

DELETE FROM [DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT UKPRN from @UkprnsToDelete)

DELETE FROM [DataLock].[PriceEpisodeMatch]
    WHERE [Ukprn] IN (SELECT UKPRN from @UkprnsToDelete)

DELETE FROM [DataLock].[PriceEpisodePeriodMatch]
    WHERE [Ukprn] IN (SELECT UKPRN from @UkprnsToDelete)

GO
