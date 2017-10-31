if not exists(select schema_id from sys.schemas where name='Rulebase')
	exec('create schema Rulebase')
GO

if object_id('[Rulebase].[vw_AEC_Cases]','v') is not null
	drop view [Rulebase].[vw_AEC_Cases]
GO

create view [Rulebase].[vw_AEC_Cases]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[CaseData]
FROM [Rulebase].[AEC_Cases]
GO


if object_id('[Rulebase].[vw_AEC_global]','v') is not null
	drop view [Rulebase].[vw_AEC_global]
GO

create view [Rulebase].[vw_AEC_global]
AS SELECT '' AS [Nothing]
    ,[UKPRN]
    ,[LARSVersion]
    ,[RulebaseVersion]
    ,[Year]
FROM [Rulebase].[AEC_global]
GO


if object_id('[Rulebase].[vw_AEC_LearningDelivery]','v') is not null
	drop view [Rulebase].[vw_AEC_LearningDelivery]
GO

create view [Rulebase].[vw_AEC_LearningDelivery]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[AimSeqNumber]
    ,[ActualDaysIL]
    ,[ActualNumInstalm]
    ,[AdjStartDate]
    ,[AgeAtProgStart]
    ,[AppAdjLearnStartDate]
    ,[AppAdjLearnStartDateMatchPathway]
    ,[ApplicCompDate]
    ,[CombinedAdjProp]
    ,[Completed]
    ,[FirstIncentiveThresholdDate]
    ,[FundStart]
    ,[LDApplic1618FrameworkUpliftBalancingValue]
    ,[LDApplic1618FrameworkUpliftCompElement]
    ,[LDApplic1618FRameworkUpliftCompletionValue]
    ,[LDApplic1618FrameworkUpliftMonthInstalVal]
    ,[LDApplic1618FrameworkUpliftPrevEarnings]
    ,[LDApplic1618FrameworkUpliftPrevEarningsStage1]
    ,[LDApplic1618FrameworkUpliftRemainingAmount]
    ,[LDApplic1618FrameworkUpliftTotalActEarnings]
    ,[LearnAimRef]
    ,[LearnDel1618AtStart]
    ,[LearnDelAppAccDaysIL]
    ,[LearnDelApplicDisadvAmount]
    ,[LearnDelApplicEmp1618Incentive]
    ,[LearnDelApplicEmpDate]
    ,[LearnDelApplicProv1618FrameworkUplift]
    ,[LearnDelApplicProv1618Incentive]
    ,[LearnDelAppPrevAccDaysIL]
    ,[LearnDelDaysIL]
    ,[LearnDelDisadAmount]
    ,[LearnDelEligDisadvPayment]
    ,[LearnDelEmpIdFirstAdditionalPaymentThreshold]
    ,[LearnDelEmpIdSecondAdditionalPaymentThreshold]
    ,[LearnDelHistDaysThisApp]
    ,[LearnDelHistProgEarnings]
    ,[LearnDelInitialFundLineType]
    ,[LearnDelMathEng]
    ,[MathEngAimValue]
    ,[OutstandNumOnProgInstalm]
    ,[PlannedNumOnProgInstalm]
    ,[PlannedTotalDaysIL]
    ,[SecondIncentiveThresholdDate]
    ,[ThresholdDays]
FROM [Rulebase].[AEC_LearningDelivery]
GO


if object_id('[Rulebase].[vw_AEC_LearningDelivery_Period]','v') is not null
	drop view [Rulebase].[vw_AEC_LearningDelivery_Period]
GO

create view [Rulebase].[vw_AEC_LearningDelivery_Period]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[AimSeqNumber]
    ,[Period]
    ,[DisadvFirstPayment]
    ,[DisadvSecondPayment]
    ,[FundLineType]
    ,[InstPerPeriod]
    ,[LDApplic1618FrameworkUpliftBalancingPayment]
    ,[LDApplic1618FrameworkUpliftCompletionPayment]
    ,[LDApplic1618FrameworkUpliftOnProgPayment]
    ,[LearnDelContType]
    ,[LearnDelFirstEmp1618Pay]
    ,[LearnDelFirstProv1618Pay]
    ,[LearnDelLevyNonPayInd]
    ,[LearnDelSecondEmp1618Pay]
    ,[LearnDelSecondProv1618Pay]
    ,[LearnDelSEMContWaiver]
    ,[LearnDelSFAContribPct]
    ,[LearnSuppFund]
    ,[LearnSuppFundCash]
    ,[MathEngBalPayment]
    ,[MathEngBalPct]
    ,[MathEngOnProgPayment]
    ,[MathEngOnProgPct]
    ,[ProgrammeAimBalPayment]
    ,[ProgrammeAimCompletionPayment]
    ,[ProgrammeAimOnProgPayment]
    ,[ProgrammeAimProgFundIndMaxEmpCont]
    ,[ProgrammeAimProgFundIndMinCoInvest]
    ,[ProgrammeAimTotProgFund]
FROM [Rulebase].[AEC_LearningDelivery_Period]
GO


if object_id('[Rulebase].[vw_AEC_LearningDelivery_PeriodisedValues]','v') is not null
	drop view [Rulebase].[vw_AEC_LearningDelivery_PeriodisedValues]
GO

create view [Rulebase].[vw_AEC_LearningDelivery_PeriodisedValues]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[AimSeqNumber]
    ,[AttributeName]
    ,[Period_1]
    ,[Period_2]
    ,[Period_3]
    ,[Period_4]
    ,[Period_5]
    ,[Period_6]
    ,[Period_7]
    ,[Period_8]
    ,[Period_9]
    ,[Period_10]
    ,[Period_11]
    ,[Period_12]
FROM [Rulebase].[AEC_LearningDelivery_PeriodisedValues]
GO


if object_id('[Rulebase].[vw_AEC_LearningDelivery_PeriodisedTextValues]','v') is not null
	drop view [Rulebase].[vw_AEC_LearningDelivery_PeriodisedTextValues]
GO

create view [Rulebase].[vw_AEC_LearningDelivery_PeriodisedTextValues]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[AimSeqNumber]
    ,[AttributeName]
    ,[Period_1]
    ,[Period_2]
    ,[Period_3]
    ,[Period_4]
    ,[Period_5]
    ,[Period_6]
    ,[Period_7]
    ,[Period_8]
    ,[Period_9]
    ,[Period_10]
    ,[Period_11]
    ,[Period_12]
FROM [Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]
GO


if object_id('[Rulebase].[vw_AEC_HistoricEarningOutput]','v') is not null
	drop view [Rulebase].[vw_AEC_HistoricEarningOutput]
GO

create view [Rulebase].[vw_AEC_HistoricEarningOutput]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[AppIdentifierOutput]
    ,[AppProgCompletedInTheYearOutput]
    ,[HistoricDaysInYearOutput]
    ,[HistoricEffectiveTNPStartDateOutput]
    ,[HistoricEmpIdEndWithinYearOutput]
    ,[HistoricEmpIdStartWithinYearOutput]
    ,[HistoricFworkCodeOutput]
    ,[HistoricLearner1618AtStartOutput]
    ,[HistoricPMRAmountOutput]
    ,[HistoricProgrammeStartDateIgnorePathwayOutput]
    ,[HistoricProgrammeStartDateMatchPathwayOutput]
    ,[HistoricProgTypeOutput]
    ,[HistoricPwayCodeOutput]
    ,[HistoricSTDCodeOutput]
    ,[HistoricTNP1Output]
    ,[HistoricTNP2Output]
    ,[HistoricTNP3Output]
    ,[HistoricTNP4Output]
    ,[HistoricTotal1618UpliftPaymentsInTheYear]
    ,[HistoricTotalProgAimPaymentsInTheYear]
    ,[HistoricULNOutput]
    ,[HistoricUptoEndDateOutput]
    ,[HistoricVirtualTNP3EndofThisYearOutput]
    ,[HistoricVirtualTNP4EndofThisYearOutput]
FROM [Rulebase].[AEC_HistoricEarningOutput]
GO


if object_id('[Rulebase].[vw_AEC_ApprenticeshipPriceEpisode]','v') is not null
	drop view [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode]
GO

create view [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[PriceEpisodeIdentifier]
    ,[EpisodeEffectiveTNPStartDate]
    ,[EpisodeStartDate]
    ,[PriceEpisodeActualEndDate]
    ,[PriceEpisodeActualInstalments]
    ,[PriceEpisodeAimSeqNumber]
    ,[PriceEpisodeCappedRemainingTNPAmount]
    ,[PriceEpisodeCompExemCode]
    ,[PriceEpisodeCompleted]
    ,[PriceEpisodeCompletionElement]
    ,[PriceEpisodeContractType]
    ,[PriceEpisodeCumulativePMRs]
    ,[PriceEpisodeExpectedTotalMonthlyValue]
    ,[PriceEpisodeFirstAdditionalPaymentThresholdDate]
    ,[PriceEpisodeFundLineType]
    ,[PriceEpisodeInstalmentValue]
    ,[PriceEpisodePlannedEndDate]
    ,[PriceEpisodePlannedInstalments]
    ,[PriceEpisodePreviousEarnings]
    ,[PriceEpisodePreviousEarningsSameProvider]
    ,[PriceEpisodeRemainingAmountWithinUpperLimit]
    ,[PriceEpisodeRemainingTNPAmount]
    ,[PriceEpisodeSecondAdditionalPaymentThresholdDate]
    ,[PriceEpisodeTotalEarnings]
    ,[PriceEpisodeTotalPMRs]
    ,[PriceEpisodeTotalTNPPrice]
    ,[PriceEpisodeUpperBandLimit]
    ,[PriceEpisodeUpperLimitAdjustment]
    ,[TNP1]
    ,[TNP2]
    ,[TNP3]
    ,[TNP4]
FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode]
GO


if object_id('[Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_Period]','v') is not null
	drop view [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_Period]
GO

create view [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_Period]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[PriceEpisodeIdentifier]
    ,[Period]
    ,[PriceEpisodeApplic1618FrameworkUpliftBalancing]
    ,[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment]
    ,[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment]
    ,[PriceEpisodeBalancePayment]
    ,[PriceEpisodeBalanceValue]
    ,[PriceEpisodeCompletionPayment]
    ,[PriceEpisodeFirstDisadvantagePayment]
    ,[PriceEpisodeFirstEmp1618Pay]
    ,[PriceEpisodeFirstProv1618Pay]
    ,[PriceEpisodeInstalmentsThisPeriod]
    ,[PriceEpisodeLevyNonPayInd]
    ,[PriceEpisodeLSFCash]
    ,[PriceEpisodeOnProgPayment]
    ,[PriceEpisodeProgFundIndMaxEmpCont]
    ,[PriceEpisodeProgFundIndMinCoInvest]
    ,[PriceEpisodeSecondDisadvantagePayment]
    ,[PriceEpisodeSecondEmp1618Pay]
    ,[PriceEpisodeSecondProv1618Pay]
    ,[PriceEpisodeSFAContribPct]
    ,[PriceEpisodeTotProgFunding]
FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
GO


if object_id('[Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_PeriodisedValues]','v') is not null
	drop view [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
GO

create view [Rulebase].[vw_AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
AS SELECT (SELECT Ukprn FROM Valid.LearningProvider) AS [Ukprn]
    ,[LearnRefNumber]
    ,[PriceEpisodeIdentifier]
    ,[AttributeName]
    ,[Period_1]
    ,[Period_2]
    ,[Period_3]
    ,[Period_4]
    ,[Period_5]
    ,[Period_6]
    ,[Period_7]
    ,[Period_8]
    ,[Period_9]
    ,[Period_10]
    ,[Period_11]
    ,[Period_12]
FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
GO


IF EXISTS (SELECT [object_id] FROM sys.views WHERE [name] = 'vw_AEC_EarningHistory' and [schema_id] = SCHEMA_ID('Rulebase')) 
BEGIN 
	DROP VIEW Rulebase.vw_AEC_EarningHistory 
END
GO

CREATE VIEW [Rulebase].[vw_AEC_EarningHistory] 
AS SELECT 
	[AppIdentifierOutput] [AppIdentifier], 
	[AppProgCompletedInTheYearOutput][AppProgCompletedInTheYearInput], 
	(SELECT[Name] FROM[Reference].[CollectionPeriods]) AS[CollectionReturnCode], 
	'${YearOfCollection}' AS[CollectionYear], 
	[HistoricDaysInYearOutput] [DaysInYear], 
	[HistoricFworkCodeOutput] [FworkCode], 
	[HistoricEffectiveTNPStartDateOutput] [HistoricEffectiveTNPStartDateInput], 
	[HistoricLearner1618AtStartOutput] [HistoricLearner1618StartInput],
	[HistoricTNP1Output] [HistoricTNP1Input], 
	[HistoricTNP2Output] [HistoricTNP2Input],
	[HistoricTNP3Output] [HistoricTNP3Input], 
	[HistoricTNP4Output] [HistoricTNP4Input], 
	[HistoricTotal1618UpliftPaymentsInTheYear] 
	[HistoricTotal1618UpliftPaymentsInTheYearInput],
	[HistoricVirtualTNP3EndofThisYearOutput] [HistoricVirtualTNP3EndOfTheYearInput],
	[HistoricVirtualTNP4EndofThisYearOutput] [HistoricVirtualTNP4EndOfTheYearInput], 
	1 AS[LatestInYear],
	[LearnRefNumber],
	[HistoricProgrammeStartDateIgnorePathwayOutput] [ProgrammeStartDateIgnorePathway], 
	[HistoricProgrammeStartDateMatchPathwayOutput] [ProgrammeStartDateMatchPathway], 
	[HistoricProgTypeOutput] [ProgType], 
	[HistoricPwayCodeOutput] [PwayCode], 
	[HistoricSTDCodeOutput] [STDCode], 
	[HistoricTotalProgAimPaymentsInTheYear] [TotalProgAimPaymentsInTheYear], 
	(SELECT Ukprn FROM Valid.LearningProvider) [UKPRN],
	[HistoricULNOutput] [ULN],
	[HistoricUptoEndDateOutput] [UptoEndDate], 
	0.00 [BalancingProgAimPaymentsInTheYear], 
	0.00 [CompletionProgAimPaymentsInTheYear], 
	0.00 [OnProgProgAimPaymentsInTheYear]
	
FROM [Rulebase].[AEC_HistoricEarningOutput]
GO
