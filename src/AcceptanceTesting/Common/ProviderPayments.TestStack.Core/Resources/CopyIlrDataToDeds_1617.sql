-- Clean Deds
DELETE FROM ${ILR_Deds.FQ}.Valid.LearningProvider
WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Valid.TrailblazerApprenticeshipFinancialRecord
WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDeliveryFAM
WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDelivery
WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Valid.Learner
WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

-- Valid.LearningProvider
INSERT INTO ${ILR_Deds.FQ}.Valid.LearningProvider
	SELECT
		[Ukprn]
	FROM [Valid].[LearningProvider]
GO

-- Valid.Learner
INSERT INTO ${ILR_Deds.FQ}.Valid.Learner
	SELECT
		(SELECT TOP 1 Ukprn FROM [Valid].[LearningProvider]),
		[LearnRefNumber],
		[PrevLearnRefNumber],
		[PrevUKPRN],
		[ULN],
		[FamilyName],
		[GivenNames],
		[DateOfBirth],
		[Ethnicity],
		[Sex],
		[LLDDHealthProb],
		[NINumber],
		[PriorAttain],
		[Accom],
		[ALSCost],
		[PlanLearnHours],
		[PlanEEPHours],
		[MathGrade],
		[EngGrade],
		[HomePostcode],
		[CurrentPostcode],
		[LrnFAM_DLA],
		[LrnFAM_ECF],
		[LrnFAM_EDF1],
		[LrnFAM_EDF2],
		[LrnFAM_EHC],
		[LrnFAM_FME],
		[LrnFAM_HNS],
		[LrnFAM_LDA],
		[LrnFAM_LSR1],
		[LrnFAM_LSR2],
		[LrnFAM_LSR3],
		[LrnFAM_LSR4],
		[LrnFAM_MCF],
		[LrnFAM_NLM1],
		[LrnFAM_NLM2],
		[LrnFAM_PPE1],
		[LrnFAM_PPE2],
		[LrnFAM_SEN],
		[ProvSpecMon_A],
		[ProvSpecMon_B]
	FROM [Valid].[Learner]

-- Valid.LearningDelivery
INSERT INTO ${ILR_Deds.FQ}.Valid.LearningDelivery
	SELECT 
		(SELECT TOP 1 Ukprn FROM [Valid].[LearningProvider]),
		[LearnRefNumber],
		[LearnAimRef],
		[AimType],
		[AimSeqNumber],
		[LearnStartDate],
		[OrigLearnStartDate],
		[LearnPlanEndDate],
		[FundModel],
		[ProgType],
		[FworkCode],
		[PwayCode],
		[StdCode],
		[PartnerUKPRN],
		[DelLocPostCode],
		[AddHours],
		[PriorLearnFundAdj],
		[OtherFundAdj],
		[ConRefNumber],
		[EmpOutcome],
		[CompStatus],
		[LearnActEndDate],
		[WithdrawReason],
		[Outcome],
		[AchDate],
		[OutGrade],
		[SWSupAimId],
		[LrnDelFAM_ADL],
		[LrnDelFAM_ASL],
		[LrnDelFAM_EEF],
		[LrnDelFAM_FFI],
		[LrnDelFAM_FLN],
		[LrnDelFAM_HEM1],
		[LrnDelFAM_HEM2],
		[LrnDelFAM_HEM3],
		[LrnDelFAM_HHS1],
		[LrnDelFAM_HHS2],
		[LrnDelFAM_LDM1],
		[LrnDelFAM_LDM2],
		[LrnDelFAM_LDM3],
		[LrnDelFAM_LDM4],
		[LrnDelFAM_NSA],
		[LrnDelFAM_POD],
		[LrnDelFAM_RES],
		[LrnDelFAM_SOF],
		[LrnDelFAM_SPP],
		[LrnDelFAM_WPP],
		[ProvSpecMon_A],
		[ProvSpecMon_B],
		[ProvSpecMon_C],
		[ProvSpecMon_D]
	FROM [Valid].[LearningDelivery]

-- Valid.LearningDeliveryFAM
INSERT INTO ${ILR_Deds.FQ}.Valid.LearningDeliveryFAM
	SELECT 
		(SELECT TOP 1 Ukprn FROM [Valid].[LearningProvider]),
		[LearnRefNumber],
		[AimSeqNumber],
		[LearnDelFAMType],
		[LearnDelFAMCode],
		[LearnDelFAMDateFrom],
		[LearnDelFAMDateTo]
	FROM [Valid].[LearningDeliveryFAM]

-- Valid.TrailblazerApprenticeshipFinancialRecord
INSERT INTO ${ILR_Deds.FQ}.Valid.TrailblazerApprenticeshipFinancialRecord
SELECT 
	(SELECT TOP 1 Ukprn FROM [Valid].[LearningProvider]),
	[LearnRefNumber],
	[AimSeqNumber],
	[TBFinType],
	[TBFinCode],
	[TBFinDate],
	[TBFinAmount]
FROM [Valid].[TrailblazerApprenticeshipFinancialRecord]


-- dbo.FileDetails
INSERT INTO ${ILR_Deds.FQ}.dbo.FileDetails
(UKPRN, Filename, SubmittedTime, Success)
SELECT
	lp.UKPRN,
	fd.Filename,
	fd.SubmittedTime,
	1
FROM Valid.LearningProvider lp
INNER JOIN dbo.FileDetails fd
	ON lp.UKPRN = fd.UKPRN
