IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
    EXEC('CREATE SCHEMA DataLock')
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
        EffectiveTo,
		WithdrawnOnDate,
		PausedOnDate
		FROM DataLock.vw_Commitments
       WHERE ProviderUkprn = @localUkprn

END
GO

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='GetDasAccounts' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP PROCEDURE Reference.GetDasAccounts
END
GO

CREATE PROCEDURE Reference.GetDasAccounts
AS

SELECT	AccountId, 
		IsLevyPayer
FROM	Reference.DasAccounts

GO

