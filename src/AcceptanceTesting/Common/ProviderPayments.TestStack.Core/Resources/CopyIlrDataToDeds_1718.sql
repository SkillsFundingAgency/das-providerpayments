-- Clean Deds
DELETE FROM ${ILR_Deds.FQ}.Valid.LearningProvider
WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO


DELETE FROM ${ILR_Deds.FQ}.Valid.AppFinRecord
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

DELETE FROM ${ILR_Deds.FQ}.Valid.EmploymentStatusMonitoring
WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[EmploymentStatusMonitoring])
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
		[LearnRefNumber]
      ,[PrevLearnRefNumber]
      ,[PrevUKPRN]
      ,[PMUKPRN]
      ,[ULN]
      ,[FamilyName]
      ,[GivenNames]
      ,[DateOfBirth]
      ,[Ethnicity]
      ,[Sex]
      ,[LLDDHealthProb]
      ,[NINumber]
      ,[PriorAttain]
      ,[Accom]
      ,[ALSCost]
      ,[PlanLearnHours]
      ,[PlanEEPHours]
      ,[MathGrade]
      ,[EngGrade]
      ,[PostcodePrior]
      ,[Postcode]
      ,[AddLine1]
      ,[AddLine2]
      ,[AddLine3]
      ,[AddLine4]
      ,[TelNo]
      ,[Email]
	FROM [Valid].[Learner]

-- Valid.LearningDelivery
INSERT INTO ${ILR_Deds.FQ}.Valid.LearningDelivery
	SELECT 
		(SELECT TOP 1 Ukprn FROM [Valid].[LearningProvider]),
		[LearnRefNumber]
      ,[LearnAimRef]
      ,[AimType]
      ,[AimSeqNumber]
      ,[LearnStartDate]
      ,[OrigLearnStartDate]
      ,[LearnPlanEndDate]
      ,[FundModel]
      ,[ProgType]
      ,[FworkCode]
      ,[PwayCode]
      ,[StdCode]
      ,[PartnerUKPRN]
      ,[DelLocPostCode]
      ,[AddHours]
      ,[PriorLearnFundAdj]
      ,[OtherFundAdj]
      ,[ConRefNumber]
      ,[EPAOrgID]
      ,[EmpOutcome]
      ,[CompStatus]
      ,[LearnActEndDate]
      ,[WithdrawReason]
      ,[Outcome]
      ,[AchDate]
      ,[OutGrade]
      ,[SWSupAimId]
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


	INSERT INTO ${ILR_Deds.FQ}.Valid.AppFinRecord
	SELECT 
		(SELECT TOP 1 Ukprn FROM [Valid].[LearningProvider]),
		[LearnRefNumber],
		[AimSeqNumber],
		[AFinType],
		[AFinCode],
		[AFinDate],
		[AFinAmount]
	FROM [Valid].[AppFinRecord]
GO
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

GO
--Valid.EmploymentStatusMonitoring
INSERT INTO ${ILR_Deds.FQ}.[Valid].[EmploymentStatusMonitoring]
           ([UKPRN], [LearnRefNumber],[DateEmpStatApp],[ESMType],[ESMCode])
SELECT
	(SELECT TOP 1 Ukprn FROM [Valid].[LearningProvider]),
	esm.LearnRefNumber,
	esm.DateEmpStatApp,
	esm.ESMType,
	esm.ESMCode
FROM Valid.EmploymentStatusMonitoring esm