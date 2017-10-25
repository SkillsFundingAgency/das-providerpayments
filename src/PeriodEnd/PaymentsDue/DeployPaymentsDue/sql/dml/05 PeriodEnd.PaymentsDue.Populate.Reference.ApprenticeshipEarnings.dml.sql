TRUNCATE TABLE [Reference].[ApprenticeshipEarnings]
GO

INSERT INTO [Reference].[ApprenticeshipEarnings] (
    [Ukprn],
    [Uln],
    [LearnRefNumber],
    [AimSeqNumber],
    [PriceEpisodeIdentifier],
    [Period],
    [PriceEpisodeEndDate],
    [PriceEpisodeOnProgPayment],
    [PriceEpisodeCompletionPayment],
    [PriceEpisodeBalancePayment],
    [PriceEpisodeFirstEmp1618Pay],
    [PriceEpisodeFirstProv1618Pay],
    [PriceEpisodeSecondEmp1618Pay],
    [PriceEpisodeSecondProv1618Pay],
    [StandardCode],
    [ProgrammeType],
    [FrameworkCode],
    [PathwayCode],
    [ApprenticeshipContractType],
    [PriceEpisodeFundLineType],
    [PriceEpisodeSfaContribPct],
    [PriceEpisodeLevyNonPayInd],
    [PriceEpisodeApplic1618FrameworkUpliftBalancing],
    [PriceEpisodeApplic1618FrameworkUpliftCompletionPayment],
    [PriceEpisodeApplic1618FrameworkUpliftOnProgPayment],
    [PriceEpisodeFirstDisadvantagePayment],
    [PriceEpisodeSecondDisadvantagePayment],
    [LearningSupportPayment],
	[EpisodeStartDate],
	[LearnAimRef] ,
	[LearningStartDate] )
    SELECT
        pe.[Ukprn],
        l.[Uln],
        pe.[LearnRefNumber],
        pe.[PriceEpisodeAimSeqNumber],
        pe.[PriceEpisodeIdentifier],
        pv.[Period],
        COALESCE(pe.[PriceEpisodeActualEndDate],pe.[PriceEpisodePlannedEndDate]),
        ISNULL(pv.[PriceEpisodeOnProgPayment], 0),
        ISNULL(pv.[PriceEpisodeCompletionPayment], 0),
        ISNULL(pv.[PriceEpisodeBalancePayment], 0),
        ISNULL(pv.[PriceEpisodeFirstEmp1618Pay], 0),
        ISNULL(pv.[PriceEpisodeFirstProv1618Pay], 0),
        ISNULL(pv.[PriceEpisodeSecondEmp1618Pay], 0),
        ISNULL(pv.[PriceEpisodeSecondProv1618Pay], 0),
        ld.[StdCode],
        ld.[ProgType],
        ld.[FworkCode],
        ld.[PwayCode],
        Case pe.[PriceEpisodeContractType] When 'Levy Contract' Then 1 Else 2 END,
        pe.[PriceEpisodeFundLineType],
        pv.[PriceEpisodeSFAContribPct],
        pv.[PriceEpisodeLevyNonPayInd],
        ISNULL(pv.[PriceEpisodeApplic1618FrameworkUpliftBalancing], 0),
        ISNULL(pv.[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment], 0),
        ISNULL(pv.[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment], 0),
        ISNULL(pv.[PriceEpisodeFirstDisadvantagePayment], 0),
        ISNULL(pv.[PriceEpisodeSecondDisadvantagePayment], 0),
        ISNULL(pv.[PriceEpisodeLSFCash], 0),
		pe.[EpisodeStartDate],
		ld.LearnAimRef,
		ld.LearnStartDate	
    FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] pe
        JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] pv ON pe.[Ukprn] = pv.[Ukprn]
            AND pe.[LearnRefNumber] = pv.[LearnRefNumber]
            AND pe.[PriceEpisodeIdentifier] = pv.[PriceEpisodeIdentifier]
        JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l ON l.[Ukprn] = pe.[Ukprn]
            AND l.[LearnRefNumber] = pe.[LearnRefNumber]
        JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON pe.[Ukprn] = ld.[Ukprn]
            AND pe.[LearnRefNumber] = ld.[LearnRefNumber]
            AND pe.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
    WHERE pe.[Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
        
GO

TRUNCATE TABLE [Reference].[ApprenticeshipDeliveryEarnings]
GO

INSERT INTO [Reference].[ApprenticeshipDeliveryEarnings] (
    [Ukprn],
    [Uln],
    [LearnRefNumber],
    [AimSeqNumber],
    [Period],
    [MathEngOnProgPayment],
    [MathEngBalPayment],
    [LearningSupportPayment],
    [StandardCode],
    [ProgrammeType],
    [FrameworkCode],
    [PathwayCode],
    [ApprenticeshipContractType],
    [FundingLineType],
    [SfaContributionPercentage],
    [LevyNonPayIndicator],
	[LearnAimRef] ,
	[LearningStartDate] )
    SELECT
        p.[Ukprn],
        l.[ULN],
        p.[LearnRefNumber],
        p.[AimSeqNumber],
        p.[Period],
        ISNULL(p.[MathEngOnProgPayment], 0) AS [MathEngOnProgPayment],
        ISNULL(p.[MathEngBalPayment], 0) AS [MathEngBalPayment],
        ISNULL(p.[LearnSuppFundCash], 0) AS [LearningSupportPayment],
        ld.[StdCode] AS [StandardCode],
        ld.[ProgType] AS [ProgrammeType],
        ld.[FworkCode] AS [FrameworkCode],
        ld.[PwayCode] AS [PathwayCode],
        CASE p.[LearnDelContType] WHEN 'Levy Contract' THEN 1 ELSE 2 END AS [ApprenticeshipContractType],
        p.[FundLineType] AS [FundingLineType],
        p.[LearnDelSFAContribPct] AS [SfaContributionPercentage],
        p.[LearnDelLevyNonPayInd] AS [LevyNonPayIndicator],
		ld.LearnAimRef,
		ld.LearnStartDate	
    FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery_Period] p
        JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l ON l.[Ukprn] = p.[Ukprn]
            AND l.[LearnRefNumber] = p.[LearnRefNumber]
        JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON p.[Ukprn] = ld.[Ukprn]
            AND p.[LearnRefNumber] = ld.[LearnRefNumber]
            AND p.[AimSeqNumber] = ld.[AimSeqNumber]
		
    WHERE ld.[LearnAimRef] != 'ZPROG001'
GO
