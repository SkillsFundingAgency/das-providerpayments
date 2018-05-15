TRUNCATE TABLE [Reference].[ApprenticeshipEarnings]
GO

if '${YearOfCollection}' <> '1718' and not exists(select 1 from sys.servers where name = 'SELF')
	raiserror ('This and a few other files cannot run on collection year other than 1718. replace DS_SILR1718_Collection values with relevant ones and adjust this error message accordingly.', 20, 1) with log;

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
    pe.[Uln],
    pe.[LearnRefNumber],
    pe.[PriceEpisodeAimSeqNumber],
    pe.[PriceEpisodeIdentifier],
    pe.[Period],
    pe.[PriceEpisodeActualEndDate],
    pe.[PriceEpisodeOnProgPayment],
    pe.[PriceEpisodeCompletionPayment],
    pe.[PriceEpisodeBalancePayment],
    pe.[PriceEpisodeFirstEmp1618Pay],
    pe.[PriceEpisodeFirstProv1618Pay],
    pe.[PriceEpisodeSecondEmp1618Pay],
    pe.[PriceEpisodeSecondProv1618Pay],
    pe.[StdCode],
    pe.[ProgType],
    pe.[FworkCode],
    pe.[PwayCode],
	pe.[PriceEpisodeContractType],
    pe.[PriceEpisodeFundLineType],
    pe.[PriceEpisodeSfaContribPct],
    pe.[PriceEpisodeLevyNonPayInd],
    pe.[PriceEpisodeApplic1618FrameworkUpliftBalancing],
    pe.[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment],
    pe.[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment],
    pe.[PriceEpisodeFirstDisadvantagePayment],
    pe.[PriceEpisodeSecondDisadvantagePayment],
    pe.[PriceEpisodeLSFCash],
	pe.[EpisodeStartDate],
	pe.[LearnAimRef] ,
	pe.[LearnStartDate],
	pe.[LearnPlanEndDate],
	pe.[LearnActEndDate],
	pe.[CompStatus],
	pe.[PriceEpisodeCompletionElement],
	pe.[PlannedNumOnProgInstalm],
	pe.[PriceEpisodeInstalmentValue],
	pe.[EPAOrgId] 
FROM OPENQUERY(${DS_SILR1718_Collection.servername}, '
		SELECT
			pe.[Ukprn],
			l.[Uln],
			pe.[LearnRefNumber],
			pe.[PriceEpisodeAimSeqNumber],
			pe.[PriceEpisodeIdentifier],
			pv.[Period],
			COALESCE(pe.[PriceEpisodeActualEndDate], pe.[PriceEpisodePlannedEndDate]) PriceEpisodeActualEndDate,
			ISNULL(pv.[PriceEpisodeOnProgPayment], 0) PriceEpisodeOnProgPayment,
			ISNULL(pv.[PriceEpisodeCompletionPayment], 0) PriceEpisodeCompletionPayment,
			ISNULL(pv.[PriceEpisodeBalancePayment], 0) PriceEpisodeBalancePayment,
			ISNULL(pv.[PriceEpisodeFirstEmp1618Pay], 0) PriceEpisodeFirstEmp1618Pay,
			ISNULL(pv.[PriceEpisodeFirstProv1618Pay], 0) PriceEpisodeFirstProv1618Pay,
			ISNULL(pv.[PriceEpisodeSecondEmp1618Pay], 0) PriceEpisodeSecondEmp1618Pay,
			ISNULL(pv.[PriceEpisodeSecondProv1618Pay], 0) PriceEpisodeSecondProv1618Pay,
			ld.[StdCode],
			ld.[ProgType],
			ld.[FworkCode],
			ld.[PwayCode],
			CASE pe.[PriceEpisodeContractType] WHEN ''Levy Contract'' THEN 1 ELSE 2 END PriceEpisodeContractType,
			pe.[PriceEpisodeFundLineType],
			pv.[PriceEpisodeSFAContribPct],
			pv.[PriceEpisodeLevyNonPayInd],
			ISNULL(pv.[PriceEpisodeApplic1618FrameworkUpliftBalancing], 0) PriceEpisodeApplic1618FrameworkUpliftBalancing,
			ISNULL(pv.[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment], 0) PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
			ISNULL(pv.[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment], 0) PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
			ISNULL(pv.[PriceEpisodeFirstDisadvantagePayment], 0) PriceEpisodeFirstDisadvantagePayment,
			ISNULL(pv.[PriceEpisodeSecondDisadvantagePayment], 0) PriceEpisodeSecondDisadvantagePayment,
			ISNULL(pv.[PriceEpisodeLSFCash], 0) PriceEpisodeLSFCash,
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
		FROM
			${DS_SILR1718_Collection.databasename}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] pe
			INNER JOIN ${DS_SILR1718_Collection.databasename}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] pv ON pe.[Ukprn] = pv.[Ukprn]
				AND pe.[LearnRefNumber] = pv.[LearnRefNumber]
				AND pe.[PriceEpisodeIdentifier] = pv.[PriceEpisodeIdentifier]
			INNER JOIN ${DS_SILR1718_Collection.databasename}.[Valid].[Learner] l ON l.[Ukprn] = pe.[Ukprn]
				AND l.[LearnRefNumber] = pe.[LearnRefNumber]
			INNER JOIN ${DS_SILR1718_Collection.databasename}.[Valid].[LearningDelivery] ld ON pe.[Ukprn] = ld.[Ukprn]
				AND pe.[LearnRefNumber] = ld.[LearnRefNumber]
				AND pe.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
			INNER JOIN ${DS_SILR1718_Collection.databasename}.[Rulebase].[AEC_LearningDelivery] aecld ON pe.[Ukprn] = aecld.[Ukprn]
				AND pe.[LearnRefNumber] = aecld.[LearnRefNumber]
				AND pe.[PriceEpisodeAimSeqNumber] = aecld.[AimSeqNumber]') as pe
WHERE pe.[Ukprn] IN (
        SELECT DISTINCT [Ukprn]
        FROM [Reference].[Providers]
        )
        
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
	[LearningStartDate],
	[LearningPlannedEndDate],
	[LearningActualEndDate] ,
	[CompletionStatus],
	[CompletionAmount],
	[TotalInstallments],
	[MonthlyInstallment] ,
	[EndpointAssessorId] )
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
		ld.LearnStartDate,
		ld.LearnPlanEndDate,
		ld.LearnActEndDate,
		ld.CompStatus,
		0,
		aecld.PlannedNumOnProgInstalm,
		1,
		ld.EPAOrgId
    FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery_Period] p
        JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l 
			ON l.[Ukprn] = p.[Ukprn]
            AND l.[LearnRefNumber] = p.[LearnRefNumber]
        JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld 
			ON p.[Ukprn] = ld.[Ukprn]
            AND p.[LearnRefNumber] = ld.[LearnRefNumber]
            AND p.[AimSeqNumber] = ld.[AimSeqNumber]
		JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery] aecld 
			ON p.[Ukprn] = aecld.[Ukprn]
            AND p.[LearnRefNumber] = aecld.[LearnRefNumber]
            AND p.[AimSeqNumber] = aecld.[AimSeqNumber]
		
    WHERE ld.[LearnAimRef] != 'ZPROG001'
GO
