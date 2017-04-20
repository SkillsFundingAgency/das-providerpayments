TRUNCATE TABLE Staging.ApprenticeshipEarnings2
GO

INSERT INTO Staging.ApprenticeshipEarnings2
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

FROM Staging.ApprenticeshipEarnings ae
    JOIN Staging.LearnerPriceEpisodePerPeriod pae
        ON ae.LearnRefNumber = pae.LearnRefNumber
        AND ae.AimSeqNumber = pae.AimSeqNumber
        AND ae.Period = pae.Period
        AND ae.EpisodeStartDate = pae.MaxEpisodeStartDate
    JOIN Reference.ApprenticeshipDeliveryEarnings ade ON ae.Ukprn = ade.Ukprn
        AND ae.LearnRefNumber = ade.LearnRefNumber
        AND ISNULL(ae.StandardCode, -1) = ISNULL(ade.StandardCode, -1)
        AND ISNULL(ae.FrameworkCode, -1) = ISNULL(ade.FrameworkCode, -1)
        AND ISNULL(ae.ProgrammeType, -1) = ISNULL(ade.ProgrammeType, -1)
        AND ISNULL(ae.PathwayCode, -1) = ISNULL(ade.PathwayCode, -1)
        AND ae.Period = ade.Period
WHERE ae.EpisodeStartDate >= (
    Select
    Case WHEN  [Name] = 'R01' OR [Name] = 'R02' OR [Name] = 'R03' OR [Name] = 'R04' OR [Name] = 'R05'  THEN CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear as varchar) , 101) 
        ELSE CONVERT(VARCHAR(10), '08/01/' +  Cast(CalendarYear -1  as varchar) , 101) END
        From  Reference.CollectionPeriods Where [Open] = 1)
    AND
        ae.EpisodeStartDate <= ( Select 
        Case WHEN  [Name] = 'R01' OR [Name] = 'R02' OR [Name] = 'R03' OR [Name] = 'R04' OR [Name] = 'R05'  THEN CONVERT(VARCHAR(10), '07/31/' +  Cast(CalendarYear +1 as varchar) , 101) 
        ELSE CONVERT(VARCHAR(10), '07/31/' +  Cast(CalendarYear as varchar) , 101) END
        From  Reference.CollectionPeriods Where [Open] = 1)
And   ae.TransactionType In ( 13,14,15)
GO