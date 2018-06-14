declare @ukprn bigint = 100
declare @learnRefNumber varchar(12) = '123'
declare @uln bigint = (SELECT TOP(1)ULN FROM PaymentsDue.RequiredPayments WHERE UKPRN = @UKPRN AND LearnRefNumber = @LearnRefNumber)

SELECT * FROM PaymentsDue.RequiredPayments
WHERE  UKPRN = @ukprn
 AND LearnRefNumber = @learnRefNumber
 ORDER BY CollectionPeriodName, DeliveryYear, DeliveryMonth


 SELECT CommitmentId, CommitmentVersionId, AccountId, AccountVersionId, DeliveryMonth, DeliveryYear, CollectionPeriodName, 
 TransactionType, AmountDue, StandardCOde, ProgrammeType, FrameworkCode, PathwayCode, PriceEpisodeIdentifier,
 LearnAimRef, LearningStartDate, ApprenticeshipContractType, SfaContributionPercentage, FundingLineType
  FROM Reference.RequiredPaymentsHistory
 WHERE  UKPRN = @ukprn
 AND LearnRefNumber = @learnRefNumber
-- AND TransactionType = 1
 ORDER BY CollectionPeriodName, DeliveryYear, DeliveryMonth


 SELECT  PriceEpisodeIdentifier, EpisodeStartDate, EpisodeEffectiveTNPStartDate, Period, ProgrammeType, FrameworkCode, PathwayCode,
 StandardCode, SfaContributionPercentage, FundingLineType, LearnAimRef, LearningStartDate, TransactionType01, TransactionType02,
 TransactionType03,TransactionType04, TransactionType05, TransactionType06, TransactionType07, TransactionType08, 
 TransactionType09, TransactionType10, TransactionType11, TransactionType12, TransactionType13, TransactionType14, 
 TransactionType15, ApprenticeshipContractType
  FROM Staging.RawEarnings
 WHERE  UKPRN = @ukprn
 AND LearnRefNumber = @learnRefNumber

  SELECT  PriceEpisodeIdentifier, EpisodeStartDate, EpisodeEffectiveTNPStartDate, Period, ProgrammeType, FrameworkCode, PathwayCode,
 StandardCode, SfaContributionPercentage, FundingLineType, LearnAimRef, LearningStartDate, TransactionType01, TransactionType02,
 TransactionType03,TransactionType04, TransactionType05, TransactionType06, TransactionType07, TransactionType08, 
 TransactionType09, TransactionType10, TransactionType11, TransactionType12, TransactionType13, TransactionType14, 
 TransactionType15, ApprenticeshipContractType
  FROM Staging.RawEarningsMathsEnglish
 WHERE  UKPRN = @ukprn
 AND LearnRefNumber = @learnRefNumber

 SELECT PriceEpisodeIdentifier, CommitmentId, VersionId, Period, Payable
  FROM Datalock.PriceEpisodePeriodMatch
 WHERE  UKPRN = @ukprn
 AND LearnRefNumber = @learnRefNumber

 SELECT CommitmentId, VersionId, AccountId, StartDate, EndDate, AgreedCost, StandardCode, ProgrammeType, FrameworkCode,
 PathwayCode, PaymentStatus, Priority, EffectiveFrom, EffectiveTo, TransferSendingEmployerAccountId, TransferApprovalDate
  FROM Reference.DasCommitments
 WHERE UKPRN = @ukprn
 AND Uln = @uln