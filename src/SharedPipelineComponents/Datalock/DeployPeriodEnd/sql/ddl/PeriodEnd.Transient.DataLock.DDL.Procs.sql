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

	DELETE pepm 
FROM DataLock.PriceEpisodePeriodMatch pepm
		INNER JOIN [Reference].[ApprenticeshipPriceEpisode_Period] pe 
  		  ON 
				pepm.Ukprn = pe.Ukprn AND
				pepm.[LearnRefNumber] = pe.[LearnRefNumber] AND
				pe.[Period] = pepm.[Period] AND
				pe.[PriceEpisodeIdentifier] = pepm.[PriceEpisodeIdentifier]  
	 
	WHERE pepm.[Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers]) AND
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