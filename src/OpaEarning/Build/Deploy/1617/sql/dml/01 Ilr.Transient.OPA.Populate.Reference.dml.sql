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

DECLARE @ReferenceCap decimal(10, 5) = (SELECT x.[Value] FROM ${ILR_Deds.FQ}.[AT].[ReferenceData] x WHERE x.[Type] = 'FundingBandCap' AND x.[Key] = 'Cap')
DECLARE @FrameworkCap decimal(10, 5) = (
	SELECT TOP 1 SUM(ISNULL(tafr.[TBFinAmount], 0)) 
	FROM [Input].[TrailblazerApprenticeshipFinancialRecord] tafr
		JOIN [Input].[LearningDelivery] ld ON tafr.[LearnRefNumber] = ld.[LearnRefNumber]
			AND tafr.[AimSeqNumber] = ld.[AimSeqNumber]
	WHERE ld.StdCode IS NULL
		AND tafr.[TBFinType] = 'TNP'
	GROUP BY tafr.LearningDelivery_Id
)

DECLARE @StandardCap decimal(10, 5) = (
	SELECT TOP 1 SUM(ISNULL(tafr.[TBFinAmount], 0)) 
	FROM [Input].[TrailblazerApprenticeshipFinancialRecord] tafr
		JOIN [Input].[LearningDelivery] ld ON tafr.[LearnRefNumber] = ld.[LearnRefNumber]
			AND tafr.[AimSeqNumber] = ld.[AimSeqNumber]
	WHERE ld.StdCode IS NOT NULL
		AND tafr.[TBFinType] = 'TNP'
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
		x.[ReservedValue2]
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
			0 AS [ReservedValue2]
		FROM [Input].[LearningDelivery] ld
		WHERE ld.StdCode IS NULL
		UNION
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
			0 AS [ReservedValue2]
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
		x.[ReservedValue2]
GO

INSERT INTO [Reference].[LARS_Current_Version] (CurrentVersion) VALUES ('LARS-TestStack-v1')
GO

INSERT INTO [Reference].[LARS_LearningDelivery] (
	[FrameworkCommonComponent],
	[LearnAimRef]
) VALUES (
	-2,
	'ZPROG001'
)
GO

INSERT INTO [Reference].[LARS_LearningDelivery] (
	[FrameworkCommonComponent],
	[LearnAimRef])
    SELECT DISTINCT
        11,
        [LearnAimRef]
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
		[LearnStartDate],
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
        [LearnStartDate],
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
    [ULN])
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
        [ULN]
    FROM ${ILR_Deds.FQ}.[Version_001].[AEC_LatestInYearEarningHistory]
    WHERE [ULN] IN (SELECT [ULN] FROM [Valid].[Learner])
GO


INSERT INTO [Reference].[SFA_PostcodeDisadvantage]
	([EffectiveFrom],
	[EffectiveTo],
	[Postcode],
	[Uplift])
	SELECT
		DISTINCT
		'2015-10-10',
		NULL,
		l.[HomePostcode],
		CASE d.[Value] When '1-10%' THEN 1.15 WHEN '11-20%' THEN 1.11 WHEN '20-27%' THEN 1.01 ELSE 0 END
	FROM
	${ILR_Deds.FQ}.[AT].[ReferenceData] d JOIN
	[VALID].[Learner] l on d.[Key] = l.[HomePostCode]
	WHERE d.[Type] = 'PostCode'
GO


INSERT INTO [Reference].[LARS_Funding] (
    [FundingCategory],
    [RateWeighted],
    [EffectiveFrom],
    [EffectiveTo]
) VALUES (
    'APP_ACT_COST',
    0.00000,
    '2013-08-01',
    '2017-07-31'
)
GO

INSERT INTO [Reference].[LARS_Funding] (
    [FundingCategory],
    [RateWeighted],
    [EffectiveFrom],
    [EffectiveTo]
) VALUES (
    'Matrix',
    0.00000,
    '2013-08-01',
    '2100-01-01'
)
GO

-- English or maths
declare @mathsAim varchar(100) = (select top 1 LearnAimRef from Valid.LearningDelivery WHERE LearnAimRef = '50086832') -- where LearnAimRef != 'ZPROG001' AND LearnAimRef != 'Z0001875' AND LearnAimRef!='60051255') --Z0001875 is used as a normal component aim in ILR generator

if (@mathsAim is not null)
INSERT INTO [Reference].[LARS_Funding] (
    [FundingCategory],
    [RateWeighted],
    [EffectiveFrom],
    [EffectiveTo]
) VALUES (
    'APP_May_2017_EM',
    471.00000,
    '2013-08-01',
    NULL
)

GO
