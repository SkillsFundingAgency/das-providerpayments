TRUNCATE TABLE Staging.ApprenticeshipEarnings
GO

INSERT INTO Staging.ApprenticeshipEarnings
SELECT
	distinct pepm.CommitmentId,
    pepm.VersionId CommitmentVersionId,
    a.AccountId,
    a.VersionId AccountVersionId,
    ae.Ukprn,
    ae.Uln,
    ae.LearnRefNumber,
    ae.AimSeqNumber,
    ae.Period,
	
    ae.PriceEpisodeEndDate,
    ae.StandardCode,
    (CASE WHEN ae.StandardCode IS NULL THEN ae.ProgrammeType ELSE NULL END) ProgrammeType,
    ae.FrameworkCode,
    ae.PathwayCode,
    ae.ApprenticeshipContractType,
    ae.PriceEpisodeIdentifier,
    ae.PriceEpisodeFundLineType,
    ae.PriceEpisodeSfaContribPct,
    ae.PriceEpisodeLevyNonPayInd,
    COALESCE(pepm.TransactionType, ndtt.TransactionType) AS TransactionType,
    ae.[EpisodeStartDate],
    IsNull(pem.IsSuccess,0) As IsSuccess, 
    IsNull(pepm.Payable,0) As Payable,
	PriceEpisodeOnProgPayment,
	PriceEpisodeCompletionPayment,
	PriceEpisodeBalancePayment,
	PriceEpisodeFirstEmp1618Pay,
	PriceEpisodeFirstProv1618Pay,
	PriceEpisodeSecondEmp1618Pay,
	PriceEpisodeSecondProv1618Pay,
	PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
	PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
	PriceEpisodeApplic1618FrameworkUpliftBalancing,
	PriceEpisodeFirstDisadvantagePayment,
	PriceEpisodeSecondDisadvantagePayment,
	LearningSupportPayment,
	ae.LearnAimref,
	ae.LearningStartDate
FROM Staging.ApprenticeshipEarningsRequiringPayments ae
	JOIN Staging.CollectionPeriods cp
              ON ae.Period = cp.PeriodNumber
    LEFT JOIN DataLock.PriceEpisodeMatch pem ON ae.Ukprn = pem.Ukprn
        AND ae.PriceEpisodeIdentifier = pem.PriceEpisodeIdentifier
        AND ae.LearnRefNumber = pem.LearnRefNumber
        AND ae.AimSeqNumber = pem.AimSeqNumber
    LEFT JOIN DataLock.PriceEpisodePeriodMatch pepm ON ae.Ukprn = pepm.Ukprn
        AND ae.PriceEpisodeIdentifier = pepm.PriceEpisodeIdentifier
        AND ae.LearnRefNumber = pepm.LearnRefNumber
        AND ae.AimSeqNumber = pepm.AimSeqNumber

    LEFT JOIN Reference.DasCommitments c ON c.CommitmentId = pepm.CommitmentId
        AND c.VersionId = pepm.VersionId
    LEFT JOIN Reference.DasAccounts a ON c.AccountId = a.AccountId
	LEFT JOIN Staging.NonDasTransactionTypes ndtt ON ndtt.ApprenticeshipContractType = ae.ApprenticeshipContractType
	LEFT JOIN Reference.RequiredPaymentsHistory ph 
		ON	ae.Ukprn = ph.Ukprn
              AND ae.LearnRefNumber = ph.LearnRefNumber
              AND ae.StandardCode = ph.StandardCode
              AND ISNULL(ae.ProgrammeType,0) = ISNULL(ph.ProgrammeType,0)
              AND ISNULL(ae.FrameworkCode,0) = ISNULL(ph.FrameworkCode,0)
              AND ISNULL(ae.PathwayCode,0) = ISNULL(ph.PathwayCode,0)
              AND COALESCE(pepm.TransactionType, ndtt.TransactionType) = ph.TransactionType
              AND cp.CalendarMonth = ph.DeliveryMonth
              AND cp.CalendarYear = ph.DeliveryYear
	WHERE 
	(
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 1 AND PriceEpisodeOnProgPayment <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 2 AND PriceEpisodeCompletionPayment <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 3 AND PriceEpisodeBalancePayment <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 4 AND PriceEpisodeFirstEmp1618Pay <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 5 AND PriceEpisodeFirstProv1618Pay <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 6 AND PriceEpisodeSecondEmp1618Pay <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 7 AND PriceEpisodeSecondProv1618Pay <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 8 AND PriceEpisodeApplic1618FrameworkUpliftOnProgPayment <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 9 AND PriceEpisodeApplic1618FrameworkUpliftCompletionPayment <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 10 AND PriceEpisodeApplic1618FrameworkUpliftBalancing <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 11 AND PriceEpisodeFirstDisadvantagePayment <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 12 AND PriceEpisodeSecondDisadvantagePayment <> 0 ) OR
	(COALESCE(pepm.TransactionType, ndtt.TransactionType) = 15 AND LearningSupportPayment <> 0 ) 
	OR ph.AmountDue> 0
	)
	AND (pepm.Period Is Null OR ae.Period = pepm.Period OR ph.AmountDue >0)
