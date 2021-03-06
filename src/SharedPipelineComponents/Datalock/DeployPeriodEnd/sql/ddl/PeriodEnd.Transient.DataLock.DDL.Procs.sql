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
        C.VersionId, 
		C.VersionId [CommitmentVersionId],
        Uln, 
        Ukprn, 
        ProviderUkprn, 
        C.AccountId, 
		A.VersionId [AccountVersionId],
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
		A.IsLevyPayer,
		WithdrawnOnDate,
		PausedOnDate
		FROM DataLock.vw_Commitments C
		LEFT JOIN Reference.DasAccounts A
		ON C.AccountId = A.AccountId
       WHERE ProviderUkprn = @localUkprn

END
GO

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='GetDasAccounts' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP PROCEDURE REference.GetDasAccounts
END
GO

CREATE PROCEDURE Reference.GetDasAccounts
AS

SELECT	AccountId, 
		IsLevyPayer
FROM	Reference.DasAccounts

GO

