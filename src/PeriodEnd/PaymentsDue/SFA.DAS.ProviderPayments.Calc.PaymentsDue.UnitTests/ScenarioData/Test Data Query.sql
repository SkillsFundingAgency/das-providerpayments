declare @ukprn bigint = 100
declare @learnRefNumber varchar(12) = '123'
declare @uln bigint = (SELECT TOP(1)ULN FROM PaymentsDue.RequiredPayments WHERE UKPRN = @UKPRN AND LearnRefNumber = @LearnRefNumber)

SELECT * FROM PaymentsDue.RequiredPayments
--WHERE  UKPRN = @ukprn
-- AND LearnRefNumber = @learnRefNumber
 ORDER BY CollectionPeriodName, DeliveryYear, DeliveryMonth


 SELECT CommitmentId, CommitmentVersionId, AccountId, AccountVersionId, DeliveryMonth, DeliveryYear, CollectionPeriodName, 
 TransactionType, AmountDue, StandardCOde, ProgrammeType, FrameworkCode, PathwayCode, PriceEpisodeIdentifier,
 LearnAimRef, LearningStartDate, ApprenticeshipContractType, SfaContributionPercentage, FundingLineType
  FROM Reference.RequiredPaymentsHistory
-- WHERE  UKPRN = @ukprn
-- AND LearnRefNumber = @learnRefNumber
-- AND TransactionType = 1
 ORDER BY CollectionPeriodName, DeliveryYear, DeliveryMonth


 SELECT  PriceEpisodeIdentifier, EpisodeStartDate, EpisodeEffectiveTNPStartDate, Period, ProgrammeType, FrameworkCode, PathwayCode,
 StandardCode, SfaContributionPercentage, FundingLineType, LearnAimRef, LearningStartDate, TransactionType01, TransactionType02,
 TransactionType03,TransactionType04, TransactionType05, TransactionType06, TransactionType07, TransactionType08, 
 TransactionType09, TransactionType10, TransactionType11, TransactionType12, TransactionType13, TransactionType14, 
 TransactionType15, ApprenticeshipContractType, LearnRefNumber
  FROM Staging.RawEarnings
-- WHERE  UKPRN = @ukprn
-- AND LearnRefNumber = @learnRefNumber
ORDER BY LearnRefNumber

  SELECT  PriceEpisodeIdentifier, EpisodeStartDate, EpisodeEffectiveTNPStartDate, Period, ProgrammeType, FrameworkCode, PathwayCode,
 StandardCode, SfaContributionPercentage, FundingLineType, LearnAimRef, LearningStartDate, TransactionType01, TransactionType02,
 TransactionType03,TransactionType04, TransactionType05, TransactionType06, TransactionType07, TransactionType08, 
 TransactionType09, TransactionType10, TransactionType11, TransactionType12, TransactionType13, TransactionType14, 
 TransactionType15, ApprenticeshipContractType
  FROM Staging.RawEarningsMathsEnglish
-- WHERE  UKPRN = @ukprn
-- AND LearnRefNumber = @learnRefNumber

  SELECT          
	PM.PriceEpisodeIdentifier,
	PM.CommitmentId,
	PM.VersionId,
	PM.[Period],
	CASE WHEN M.IsSuccess = 1 AND PM.Payable = 1 THEN 1 ELSE 0 END AS Payable,
	PM.TransactionTypesFlag,
	PM.LearnRefNumber
FROM [DataLock].[PriceEpisodePeriodMatch] PM
JOIN [DataLock].[PriceEpisodeMatch] M 
	ON M.PriceEpisodeIdentifier = PM.PriceEpisodeIdentifier 
	AND M.UkPrn = PM.UkPrn
WHERE 1=1
AND CONVERT(date, SUBSTRING(PM.PriceEpisodeIdentifier, LEN(PM.PriceEpisodeIdentifier) - 9, 10), 103) < '2018-08-01'
ORDER BY PM.UKPRN, LearnRefNumber
-- AND  UKPRN = @ukprn
-- AND LearnRefNumber = @learnRefNumber

  SELECT PriceEpisodeIdentifier, RuleId
 FROM Datalock.ValidationError
-- WHERE UKPRN = @ukprn
-- AND LearnRefNumber = @learnRefNumber

 SELECT CommitmentId, VersionId, AccountId, StartDate, EndDate, AgreedCost, StandardCode, ProgrammeType, FrameworkCode,
 PathwayCode, PaymentStatus, Priority, EffectiveFrom, EffectiveTo, TransferSendingEmployerAccountId, TransferApprovalDate
  FROM Reference.DasCommitments
-- WHERE UKPRN = @ukprn
-- AND Uln = @uln

