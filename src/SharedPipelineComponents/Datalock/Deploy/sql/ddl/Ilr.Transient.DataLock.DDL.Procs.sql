IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
    EXEC('CREATE SCHEMA DataLock')
END
GO

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='DeleteExtraPriceEpisodeperiodMatches' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP PROCEDURE DataLock.DeleteExtraPriceEpisodeperiodMatches
END
GO

CREATE PROCEDURE DataLock.DeleteExtraPriceEpisodeperiodMatches
AS
BEGIN
	SET NOCOUNT ON

		DELETE pepm 
FROM DataLock.PriceEpisodePeriodMatch pepm
		INNER JOIN [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] pe 
  		  ON 
				pepm.[LearnRefNumber] = pe.[LearnRefNumber] AND
				pe.[Period] = pepm.[Period] AND
				pe.[PriceEpisodeIdentifier] = pepm.[PriceEpisodeIdentifier]  
	 
	WHERE 
	((pepm.TransactionType = 1 and IsNull(PriceEpisodeOnProgPayment,0) = 0 )
	OR (pepm.TransactionType = 2 and IsNull(PriceEpisodeCompletionPayment,0) = 0 )
	OR (pepm.TransactionType = 3 and IsNull(PriceEpisodeBalancePayment,0) = 0 )
	OR (pepm.TransactionType = 4 and IsNull(PriceEpisodeFirstEmp1618Pay,0) = 0 )
	OR (pepm.TransactionType = 5 and IsNull(PriceEpisodeFirstProv1618Pay,0) = 0 )
	OR (pepm.TransactionType = 6 and IsNull(PriceEpisodeSecondEmp1618Pay,0) = 0 )
	OR (pepm.TransactionType = 7 and IsNull(PriceEpisodeSecondProv1618Pay,0) = 0 )
	OR (pepm.TransactionType = 8 and IsNull(PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,0) = 0 )
	OR (pepm.TransactionType = 9 and IsNull(PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,0) = 0 )
	OR (pepm.TransactionType = 10 and IsNull(PriceEpisodeApplic1618FrameworkUpliftBalancing,0) = 0 )
	OR (pepm.TransactionType = 11 and IsNull(PriceEpisodeFirstDisadvantagePayment,0) = 0 )
	OR (pepm.TransactionType = 12 and IsNull(PriceEpisodeSecondDisadvantagePayment,0) = 0 ))


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

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='GetPriceEpisodesByUkprn' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
	DROP PROCEDURE DataLock.GetPriceEpisodesByUkprn
END
GO

CREATE PROCEDURE DataLock.GetPriceEpisodesByUkprn
	@ukprn BIGINT
AS
DECLARE @localUkprn BIGINT = @ukprn

SELECT 
	Ukprn, 
	LearnRefNumber, 
	Uln, 
	NiNumber, 
	AimSeqNumber, 
	StandardCode, 
	ProgrammeType, 
	FrameworkCode, 
	PathwayCode, 
	StartDate, 
	NegotiatedPrice, 
	PriceEpisodeIdentifier, 
	EndDate, 
	FirstAdditionalPaymentThresholdDate, 
	SecondAdditionalPaymentThresholdDate
FROM  DataLock.vw_PriceEpisode
WHERE Ukprn = @localUkprn