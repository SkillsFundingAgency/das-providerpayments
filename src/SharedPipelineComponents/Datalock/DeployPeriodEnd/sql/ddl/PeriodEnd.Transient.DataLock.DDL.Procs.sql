IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
    EXEC('CREATE SCHEMA DataLock')
END
GO

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='DeleteExtraPriceEpisodeperiodMatches' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP PROCEDURE [DataLock].[DeleteExtraPriceEpisodeperiodMatches]
END
GO

CREATE PROCEDURE [DataLock].[DeleteExtraPriceEpisodeperiodMatches] 
AS
BEGIN
	SET NOCOUNT ON;

END
GO


