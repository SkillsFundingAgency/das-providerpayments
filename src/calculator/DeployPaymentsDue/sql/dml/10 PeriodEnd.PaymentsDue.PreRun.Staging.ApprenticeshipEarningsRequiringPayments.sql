TRUNCATE TABLE Staging.ApprenticeshipEarningsRequiringPayments
GO

INSERT INTO Staging.ApprenticeshipEarningsRequiringPayments
SELECT
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
	ae.[EpisodeStartDate],
	ae.PriceEpisodeOnProgPayment,
	ae.PriceEpisodeCompletionPayment,
	ae.PriceEpisodeBalancePayment,
	ae.PriceEpisodeFirstEmp1618Pay,
	ae.PriceEpisodeFirstProv1618Pay,
	ae.PriceEpisodeSecondEmp1618Pay,
	ae.PriceEpisodeSecondProv1618Pay,
	ae.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
	ae.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
	ae.PriceEpisodeApplic1618FrameworkUpliftBalancing,
	ae.PriceEpisodeFirstDisadvantagePayment,
	ae.PriceEpisodeSecondDisadvantagePayment,
	ae.LearningSupportPayment
FROM Reference.ApprenticeshipEarnings AE
JOIN Staging.CollectionPeriods cp
              ON ae.Period = cp.PeriodNumber
LEFT JOIN Reference.RequiredPaymentsHistory ph
              ON ae.Ukprn = ph.Ukprn
              AND ae.Uln = ph.Uln
              --AND ae.LearnRefNumber = ph.LearnRefNumber
              --AND ae.AimSeqNumber = ph.AimSeqNumber
              AND ae.StandardCode = ph.StandardCode
              AND (ISNULL(ae.StandardCode,0) > 0 OR ISNULL(ae.ProgrammeType,0) = ISNULL(ph.ProgrammeType,0))
              AND ISNULL(ae.FrameworkCode,0) = ISNULL(ph.FrameworkCode,0)
              AND ISNULL(ae.PathwayCode,0) = ISNULL(ph.PathwayCode,0)
              AND case 
					when PriceEpisodeOnProgPayment>0 then 1
					when PriceEpisodeCompletionPayment > 0 then 2
					when PriceEpisodeBalancePayment > 0 then 3
					when PriceEpisodeFirstEmp1618Pay > 0 then 4
					when PriceEpisodeFirstProv1618Pay > 0 then 5
					when PriceEpisodeSecondEmp1618Pay > 0 then 6
					when PriceEpisodeSecondProv1618Pay > 0 then 7
					when PriceEpisodeApplic1618FrameworkUpliftOnProgPayment > 0 then 8
					when PriceEpisodeApplic1618FrameworkUpliftCompletionPayment > 0 then 9
					when PriceEpisodeApplic1618FrameworkUpliftBalancing > 0 then 10
					when PriceEpisodeFirstDisadvantagePayment > 0 then 11
					when PriceEpisodeSecondDisadvantagePayment > 0 then 12
					when LearningSupportPayment > 0 then 15
					else ph.TransactionType
                  end
              = ph.TransactionType
              AND cp.CalendarMonth = ph.DeliveryMonth
              AND cp.CalendarYear = ph.DeliveryYear
WHERE
PriceEpisodeOnProgPayment > 0
OR PriceEpisodeCompletionPayment > 0
OR PriceEpisodeBalancePayment > 0
OR PriceEpisodeFirstEmp1618Pay > 0
OR PriceEpisodeFirstProv1618Pay > 0
OR PriceEpisodeSecondEmp1618Pay > 0
OR PriceEpisodeSecondProv1618Pay > 0
OR PriceEpisodeApplic1618FrameworkUpliftOnProgPayment > 0
OR PriceEpisodeApplic1618FrameworkUpliftCompletionPayment > 0
OR PriceEpisodeApplic1618FrameworkUpliftBalancing > 0
OR PriceEpisodeFirstDisadvantagePayment > 0
OR PriceEpisodeSecondDisadvantagePayment > 0
OR LearningSupportPayment > 0
OR ph.AmountDue > 0