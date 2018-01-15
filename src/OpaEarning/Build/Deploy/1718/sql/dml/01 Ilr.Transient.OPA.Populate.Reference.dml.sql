DELETE FROM [Reference].[LARS_ApprenticeshipFunding]
GO

DELETE FROM [Reference].[LARS_Current_Version]
GO

DELETE FROM [Reference].[LARS_LearningDelivery]
GO

DELETE FROM [Reference].[LARS_FrameworkCmnComp]
GO

DELETE FROM [Reference].[LARS_StandardCommonComponent]
GO

DELETE FROM [Reference].[AEC_LatestInYearEarningHistory]
GO

DELETE FROM [Reference].[LARS_Funding]
GO

DECLARE @ReferenceCap decimal(12, 5) = (SELECT x.[Value] FROM ${ILR_Deds.FQ}.[AT].[ReferenceData] x WHERE x.[Type] = 'FundingBandCap' AND x.[Key] = 'Cap')
DECLARE @FrameworkCap decimal(12, 5) = (
	SELECT TOP 1 SUM(ISNULL(tafr.[AFinAmount], 0)) 
	FROM [Input].[AppFinRecord] tafr
		JOIN [Input].[LearningDelivery] ld ON tafr.[LearnRefNumber] = ld.[LearnRefNumber]
			AND tafr.[AimSeqNumber] = ld.[AimSeqNumber]
	WHERE ld.StdCode IS NULL
		AND tafr.[AFinType] = 'TNP'
	GROUP BY tafr.LearningDelivery_Id
)

DECLARE @StandardCap decimal(12, 5) = (
	SELECT TOP 1 SUM(ISNULL(tafr.[AFinAmount], 0)) 
	FROM [Input].[AppFinRecord] tafr
		JOIN [Input].[LearningDelivery] ld ON tafr.[LearnRefNumber] = ld.[LearnRefNumber]
			AND tafr.[AimSeqNumber] = ld.[AimSeqNumber]
	WHERE ld.StdCode IS NOT NULL
		AND tafr.[AFinType] = 'TNP'
	GROUP BY tafr.LearningDelivery_Id
)

INSERT INTO [Reference].[LARS_ApprenticeshipFunding]
	SELECT
		x.[1618Incentive],
		x.[ApprenticeshipCode],
		x.[ApprenticeshipType],
		x.[EffectiveFrom],
		x.[EffectiveTo],
		x.[FundingCategory],
		MAX(x.[MaxEmployerLevyCap]),
		x.[ProgType],
		x.[PwayCode],
		x.[ReservedValue1],
		x.[ReservedValue2],
		x.[ReservedValue3],
		x.[1618ProviderAdditionalPayment],
		x.[1618EmployerAdditionalPayment],
		x.[1618FrameworkUplift]


	FROM (
		SELECT
			2000.00 + ISNULL(@ReferenceCap, @FrameworkCap) * 0.2 AS [1618Incentive],
			ld.[FworkCode] AS [ApprenticeshipCode],
			'FWK' AS [ApprenticeshipType],
			'2010-01-01' AS [EffectiveFrom],
			NULL AS [EffectiveTo],
			'App_May_2017' AS [FundingCategory],
			ISNULL(@ReferenceCap, @FrameworkCap) AS [MaxEmployerLevyCap],
			ld.[ProgType] AS [ProgType],
			ld.[PwayCode] AS [PwayCode],
			12.00 AS [ReservedValue1],
			0 AS [ReservedValue2],
			0 AS [ReservedValue3],
			1000 AS [1618ProviderAdditionalPayment],
			1000 AS [1618EmployerAdditionalPayment],
			ISNULL(@ReferenceCap, @FrameworkCap) * 0.2 AS [1618FrameworkUplift]
		FROM [Input].[LearningDelivery] ld
		WHERE ld.StdCode IS NULL
		UNION ALL
		SELECT
			2000.00 AS [1618Incentive],
			ld.[StdCode] AS [ApprenticeshipCode],
			'STD' AS [ApprenticeshipType],
			'2010-01-01' AS [EffectiveFrom],
			NULL AS [EffectiveTo],
			'App_May_2017' AS [FundingCategory],
			ISNULL(@ReferenceCap, @StandardCap) AS [MaxEmployerLevyCap],
			ld.[ProgType] AS [ProgType],
			0 AS [PwayCode],
			12.00 AS [ReservedValue1],
			0 AS [ReservedValue2],
			0 AS [ReservedValue3],
			1000 AS [1618ProviderAdditionalPayment],
			1000 AS [1618EmployerAdditionalPayment],
			0 AS [1618FrameworkUplift]
		FROM [Input].[LearningDelivery] ld
		WHERE ld.StdCode IS NOT NULL
	) x
	GROUP BY
		x.[1618Incentive],
		x.[ApprenticeshipCode],
		x.[ApprenticeshipType],
		x.[EffectiveFrom],
		x.[EffectiveTo],
		x.[FundingCategory],
		x.[ProgType],
		x.[PwayCode],
		x.[ReservedValue1],
		x.[ReservedValue2],
		x.[ReservedValue3],
		x.[1618ProviderAdditionalPayment],
		x.[1618EmployerAdditionalPayment],
		x.[1618FrameworkUplift]
GO

INSERT INTO [Reference].[LARS_Current_Version] (CurrentVersion) VALUES ('LARS-TestStack-v1')
GO

INSERT INTO [Reference].[LARS_LearningDelivery] (
	[FrameworkCommonComponent],
	[LearnAimRef],
	[EffectiveFrom]
) VALUES (
	-2,
	'ZPROG001',
	GetDate()
)
GO

INSERT INTO [Reference].[LARS_LearningDelivery] (
	[FrameworkCommonComponent],
	[LearnAimRef],
	[EffectiveFrom])
    SELECT DISTINCT
        11,
        [LearnAimRef],
		'01-05-2017'
    FROM [Input].[LearningDelivery]
    WHERE [LearnAimRef] != 'ZPROG001'
GO
INSERT INTO [Reference].[LARS_FrameworkCmnComp] (
    [CommonComponent],
    [FworkCode],
    [ProgType],
    [PwayCode],
	[EffectiveFrom],
	[EffectiveTo])
    SELECT DISTINCT
        11,
        [FworkCode],
        [ProgType],
        [PwayCode],
		'01-05-2017',
		COALESCE([LearnActEndDate],[LearnPlanEndDate])
    FROM [Input].[LearningDelivery]
    WHERE [FworkCode] IS NOT NULL
        AND [LearnAimRef] != 'ZPROG001'
GO

INSERT INTO [Reference].[LARS_StandardCommonComponent] (
    [CommonComponent],
    [StandardCode],
    [EffectiveFrom],
	[EffectiveTo])
    SELECT DISTINCT
        11,
        [StdCode],
        '01-05-2017',
		COALESCE([LearnActEndDate],[LearnPlanEndDate])
    FROM [Input].[LearningDelivery]
    WHERE [StdCode] IS NOT NULL
        AND [LearnAimRef] != 'ZPROG001'
GO

INSERT INTO [Reference].[AEC_LatestInYearEarningHistory] (
    [AppIdentifier],
    [AppProgCompletedInTheYearInput],
    [CollectionYear],
    [CollectionReturnCode],
    [DaysInYear],
    [FworkCode],
    [HistoricEffectiveTNPStartDateInput],
    [HistoricLearner1618StartInput],
    [HistoricTNP1Input],
    [HistoricTNP2Input],
    [HistoricTNP3Input],
    [HistoricTNP4Input],
    [HistoricTotal1618UpliftPaymentsInTheYearInput],
    [HistoricVirtualTNP3EndOfTheYearInput],
    [HistoricVirtualTNP4EndOfTheYearInput],
    [LearnRefNumber],
    [ProgrammeStartDateIgnorePathway],
    [ProgrammeStartDateMatchPathway],
    [ProgType],
    [PwayCode],
    [STDCode],
    [TotalProgAimPaymentsInTheYear],
    [UptoEndDate],
    [UKPRN],
    [ULN],
	[LatestInYear])
    SELECT
        [AppIdentifier],
        [AppProgCompletedInTheYearInput],
        [CollectionYear],
        [CollectionReturnCode],
        [DaysInYear],
        [FworkCode],
        [HistoricEffectiveTNPStartDateInput],
        [HistoricLearner1618StartInput],
        [HistoricTNP1Input],
        [HistoricTNP2Input],
        [HistoricTNP3Input],
        [HistoricTNP4Input],
        [HistoricTotal1618UpliftPaymentsInTheYearInput],
        [HistoricVirtualTNP3EndOfTheYearInput],
        [HistoricVirtualTNP4EndOfTheYearInput],
        [LearnRefNumber],
        [ProgrammeStartDateIgnorePathway],
        [ProgrammeStartDateMatchPathway],
        [ProgType],
        [PwayCode],
        [STDCode],
        [TotalProgAimPaymentsInTheYear],
        [UptoEndDate],
        [UKPRN],
        [ULN],
		[LatestInYear]
    FROM ${ILR_Deds.FQ}.[Version_001].[AEC_LatestInYearEarningHistory]
    WHERE [ULN] IN (SELECT [ULN] FROM [Valid].[Learner])
GO


INSERT INTO [Reference].[SFA_PostcodeDisadvantage]
	([EffectiveFrom],
	[EffectiveTo],
	[Postcode],
	[Uplift],
	[Apprenticeship_Uplift])
	SELECT
		DISTINCT
		'2015-10-10',
		NULL,
		l.[PostcodePrior],
		CASE d.[Value] When '1-10%' THEN 1.15 WHEN '11-20%' THEN 1.11 WHEN '20-27%' THEN 1.01 ELSE 0 END,
		CASE d.[Value] When '1-10%' THEN 600 WHEN '11-20%' THEN 300 WHEN '20-27%' THEN 200 ELSE 0 END
	FROM
	${ILR_Deds.FQ}.[AT].[ReferenceData] d JOIN
	[VALID].[Learner] l on d.[Key] = l.[PostcodePrior]
	WHERE d.[Type] = 'PostCode'
GO


INSERT INTO [Reference].[LARS_Funding] (
    [FundingCategory],
	[LearnAimRef],
    [RateWeighted],
    [EffectiveFrom],
    [EffectiveTo],
	[WeightingFactor]
) VALUES (
    'APP_ACT_COST',
	'ZPROG001',
    0.00000,
    '2013-08-01',
    '2017-07-31',
	'A'
)
GO

INSERT INTO [Reference].[LARS_Funding] (
    [FundingCategory],
	[LearnAimRef],
    [RateWeighted],
    [EffectiveFrom],
    [EffectiveTo],
	[WeightingFactor]
) VALUES (
    'Matrix',
	'ZPROG001',
    0.00000,
    '2013-08-01',
    '2100-01-01',
	'A'
)
GO

-- English or maths
declare @mathsAim varchar(100) = (select top 1 LearnAimRef from Valid.LearningDelivery WHERE LearnAimRef = '50086832') -- where LearnAimRef != 'ZPROG001' AND LearnAimRef != 'Z0001875' AND LearnAimRef!='60051255') --Z0001875 is used as a normal component aim in ILR generator

if (@mathsAim is not null)
INSERT INTO [Reference].[LARS_Funding] (
    [FundingCategory],
	[LearnAimRef],
    [RateWeighted],
    [EffectiveFrom],
    [EffectiveTo],
	[WeightingFactor]
) VALUES (
    'APP_May_2017_EM',
	'50086832',
    471.00000,
    '2013-08-01',
    NULL,
	'A'
)

GO
