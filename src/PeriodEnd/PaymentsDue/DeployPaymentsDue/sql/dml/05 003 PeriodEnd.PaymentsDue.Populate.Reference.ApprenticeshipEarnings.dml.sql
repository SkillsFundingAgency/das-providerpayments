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
    FROM Reference.[Deds_AEC_LearningDelivery_Period] p
        JOIN Reference.[Deds_Valid_Learner] l ON l.[Ukprn] = p.[Ukprn]
            AND l.[LearnRefNumber] = p.[LearnRefNumber]
        JOIN Reference.[Deds_Valid_LearningDelivery] ld ON p.[Ukprn] = ld.[Ukprn]
            AND p.[LearnRefNumber] = ld.[LearnRefNumber]
            AND p.[AimSeqNumber] = ld.[AimSeqNumber]
		JOIN Reference.[Deds_AEC_LearningDelivery] aecld ON p.[Ukprn] = aecld.[Ukprn]
            AND p.[LearnRefNumber] = aecld.[LearnRefNumber]
            AND p.[AimSeqNumber] = aecld.[AimSeqNumber]
		
    WHERE ld.[LearnAimRef] != 'ZPROG001'
GO

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', 
	'05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 
	'[Reference].[ApprenticeshipDeliveryEarnings] populated')

INSERT INTO [PaymentsDue].[TaskLog] ([Level], [Logger], [Message]) VALUES ('Info', 
	'05 PeriodEnd.PaymentsDue.Populate.Reference.ApprenticeshipEarnings.dml.sql', 
	'Finished')

