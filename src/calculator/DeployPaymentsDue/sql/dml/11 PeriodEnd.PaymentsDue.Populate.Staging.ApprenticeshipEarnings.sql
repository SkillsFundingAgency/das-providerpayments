TRUNCATE TABLE Staging.ApprenticeshipEarnings
GO

INSERT INTO Staging.ApprenticeshipEarnings
SELECT
	pepm.CommitmentId,
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
	LearningSupportPayment
FROM Staging.ApprenticeshipEarningsRequiringPayments ae
    LEFT JOIN DataLock.PriceEpisodeMatch pem ON ae.Ukprn = pem.Ukprn
        AND ae.PriceEpisodeIdentifier = pem.PriceEpisodeIdentifier
        AND ae.LearnRefNumber = pem.LearnRefNumber
        AND ae.AimSeqNumber = pem.AimSeqNumber
    LEFT JOIN DataLock.PriceEpisodePeriodMatch pepm ON ae.Ukprn = pepm.Ukprn
        AND ae.PriceEpisodeIdentifier = pepm.PriceEpisodeIdentifier
        AND ae.LearnRefNumber = pepm.LearnRefNumber
        AND ae.AimSeqNumber = pepm.AimSeqNumber
        AND ae.Period = pepm.Period
    LEFT JOIN Reference.DasCommitments c ON c.CommitmentId = pepm.CommitmentId
        AND c.VersionId = pepm.VersionId
    LEFT JOIN Reference.DasAccounts a ON c.AccountId = a.AccountId
	LEFT JOIN Staging.NonDasTransactionTypes ndtt ON ndtt.ApprenticeshipContractType = ae.ApprenticeshipContractType
GO