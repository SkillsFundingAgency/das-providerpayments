TRUNCATE TABLE Staging.ApprenticeshipEarnings3
GO

INSERT INTO Staging.ApprenticeshipEarnings3
SELECT
	ae.CommitmentId,
	ae.CommitmentVersionId,
	ae.AccountId,
	ae.AccountVersionId,
	ade.[Ukprn],
	ade.[Uln],
	ade.[LearnRefNumber],
	ade.[AimSeqNumber],
	ade.[Period],
	ae.PriceEpisodeEndDate,
	ade.StandardCode,
	(CASE WHEN ade.StandardCode IS NULL THEN ade.ProgrammeType ELSE NULL END) ProgrammeType,
	ade.FrameworkCode,
	ade.PathwayCode,
	ade.ApprenticeshipContractType,
	ae.PriceEpisodeIdentifier,
	ade.FundingLineType AS PriceEpisodeFundLineType,
	ade.SfaContributionPercentage AS PriceEpisodeSfaContribPct,
	ade.LevyNonPayIndicator AS PriceEpisodeLevyNonPayInd,
	ae.TransactionType AS TransactionType,
	(CASE ae.TransactionType
	WHEN 13 THEN ade.MathEngOnProgPayment
	WHEN 14 THEN ade.MathEngBalPayment
	WHEN 15 THEN ade.LearningSupportPayment
	END) AS EarningAmount,
	ae.[EpisodeStartDate],
	IsNull(ae.IsSuccess,0) As IsSuccess, 
	IsNull(ae.Payable,0) As Payable 
FROM 
	(SELECT MAX(PriceEpisodeEndDate) as LatestPriceEpisodeEndDate, Ukprn, LearnRefNumber,StandardCode,FrameworkCode,ProgrammeType,PathwayCode
	FROM Reference.ApprenticeshipEarnings 
	GROUP BY Ukprn, LearnRefNumber,StandardCode,FrameworkCode,ProgrammeType,PathwayCode) ae1
JOIN Staging.ApprenticeshipEarnings ae 
	ON ae1.LatestPriceEpisodeEndDate = ae.PriceEpisodeEndDate
	AND ISNULL(ae.StandardCode, -1) = ISNULL(ae1.StandardCode, -1)
	AND ISNULL(ae.FrameworkCode, -1) = ISNULL(ae1.FrameworkCode, -1)
	AND ISNULL(ae.ProgrammeType, -1) = ISNULL(ae1.ProgrammeType, -1)
	AND ISNULL(ae.PathwayCode, -1) = ISNULL(ae1.PathwayCode, -1)
	AND ae1.LearnRefNumber = ae.LearnRefNumber
	AND ae1.Ukprn = ae.Ukprn
JOIN Reference.ApprenticeshipDeliveryEarnings ade 
	ON ae.Ukprn = ade.Ukprn
	AND ae.LearnRefNumber = ade.LearnRefNumber
	AND ISNULL(ae.StandardCode, -1) = ISNULL(ade.StandardCode, -1)
	AND ISNULL(ae.FrameworkCode, -1) = ISNULL(ade.FrameworkCode, -1)
	AND ISNULL(ae.ProgrammeType, -1) = ISNULL(ade.ProgrammeType, -1)
	AND ISNULL(ae.PathwayCode, -1) = ISNULL(ade.PathwayCode, -1)

WHERE 
(Select
Case  ade.Period 
WHEN 1 THEN CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear as varchar) , 101) 
WHEN 2 THEN CONVERT(VARCHAR(10), '09/01/' +  Cast(CalendarYear as varchar) , 101) 
WHEN 3 THEN CONVERT(VARCHAR(10), '10/01/' +  Cast(CalendarYear as varchar) , 101) 
WHEN 4 THEN CONVERT(VARCHAR(10), '11/01/' +  Cast(CalendarYear as varchar) , 101) 
WHEN 5 THEN CONVERT(VARCHAR(10), '12/01/' +  Cast(CalendarYear as varchar) , 101) 
WHEN 6 THEN CONVERT(VARCHAR(10), '01/01/' +  Cast(CalendarYear  as varchar) , 101) 
WHEN 7 THEN CONVERT(VARCHAR(10), '02/01/' +  Cast(CalendarYear  as varchar) , 101) 
WHEN 8 THEN CONVERT(VARCHAR(10), '03/01/' +  Cast(CalendarYear  as varchar) , 101) 
WHEN 9 THEN CONVERT(VARCHAR(10), '04/01/' +  Cast(CalendarYear  as varchar) , 101) 
WHEN 10 THEN CONVERT(VARCHAR(10), '05/01/' +  Cast(CalendarYear  as varchar) , 101) 
WHEN 11 THEN CONVERT(VARCHAR(10), '06/01/' +  Cast(CalendarYear  as varchar) , 101) 
WHEN 12 THEN CONVERT(VARCHAR(10), '07/01/' +  Cast(CalendarYear  as varchar) , 101) 
END FROM  Reference.CollectionPeriods Where [Open] = 1) > ae.PriceEpisodeEndDate 

AND   ae.TransactionType In ( 13,14,15)
GO