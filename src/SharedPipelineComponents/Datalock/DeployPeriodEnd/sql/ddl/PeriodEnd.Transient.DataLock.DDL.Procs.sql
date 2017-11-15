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

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='GetCommitmentsForProvider' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP PROCEDURE DataLock.GetCommitmentsForProvider
END
GO

CREATE PROC DataLock.GetCommitmentsForProvider (@ukprn BIGINT)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @localUkprn BIGINT = @ukprn

	SELECT 
		CommitmentId, 
        VersionId, 
        Uln, 
        Ukprn, 
        ProviderUkprn, 
        AccountId, 
        StartDate, 
        EndDate, 
        AgreedCost, 
        StandardCode, 
        ProgrammeType, 
        FrameworkCode, 
        PathwayCode, 
        PaymentStatus, 
        PaymentStatusDescription, 
        Priority, 
        EffectiveFrom, 
        EffectiveTo
		FROM DataLock.vw_Commitments
       WHERE ProviderUkprn = @localUkprn

END