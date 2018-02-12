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
	[LearningStartDate],
	[LearningPlannedEndDate],
	[LearningActualEndDate] ,
	[CompletionStatus],
	[CompletionAmount],
	[TotalInstallments],
	[MonthlyInstallment],
	[EndpointAssessorId] 
	)
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
		ld.LearnStartDate,
		ld.LearnPlanEndDate,
		ld.LearnActEndDate,
		ld.CompStatus,
		pe.PriceEpisodeCompletionElement,
		aecld.PlannedNumOnProgInstalm,
		pe.PriceEpisodeInstalmentValue,
		ld.EPAOrgId
			
    FROM Reference.[Deds_AEC_ApprenticeshipPriceEpisode] pe
        JOIN Reference.[Deds_AEC_ApprenticeshipPriceEpisode_Period] pv ON pe.[Ukprn] = pv.[Ukprn]
            AND pe.[LearnRefNumber] = pv.[LearnRefNumber]
            AND pe.[PriceEpisodeIdentifier] = pv.[PriceEpisodeIdentifier]
        JOIN Reference.[Deds_Valid_Learner] l ON l.[Ukprn] = pe.[Ukprn]
            AND l.[LearnRefNumber] = pe.[LearnRefNumber]
        JOIN Reference.[Deds_Valid_LearningDelivery] ld ON pe.[Ukprn] = ld.[Ukprn]
            AND pe.[LearnRefNumber] = ld.[LearnRefNumber]
            AND pe.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
		JOIN Reference.[Deds_AEC_LearningDelivery] aecld ON pe.[Ukprn] = aecld.[Ukprn]
            AND pe.[LearnRefNumber] = aecld.[LearnRefNumber]
            AND pe.[PriceEpisodeAimSeqNumber] = aecld.[AimSeqNumber]
    WHERE pe.[Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
        
GO

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', 
	'05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 
	'[Reference].[ApprenticeshipEarnings] populated')
