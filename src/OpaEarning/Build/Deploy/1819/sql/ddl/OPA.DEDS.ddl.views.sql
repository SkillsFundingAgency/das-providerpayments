SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Valid].[ApprenticeshipFinancialRecord]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [Valid].[ApprenticeshipFinancialRecord]
AS
	SELECT
		UKPRN
		,LearnRefNumber
		,AimSeqNumber
		,AFinType
		,AFinCode
		,AFinAmount
		,AFinDate
	FROM
		Valid.AppFinRecord
' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Valid].[LearnerDenorm]'))
EXEC dbo.sp_executesql @statement = N'

CREATE VIEW [Valid].[LearnerDenorm]
AS
	SELECT
		l.[UKPRN],
		l.[LearnRefNumber],
		l.[PrevLearnRefNumber],		
		l.[PrevUKPRN],
		l.[PMUKPRN],
		l.[CampId],
		l.[ULN],
		l.[FamilyName],
		l.[GivenNames],
		l.[DateOfBirth],
		l.[Ethnicity],
		l.[Sex],
		l.[LLDDHealthProb],
		l.[NINumber],
		l.[PriorAttain],
		l.[Accom],
		l.[ALSCost],
		l.[PlanLearnHours],
		l.[PlanEEPHours],
		l.[MathGrade],
		l.[EngGrade],
		l.[PostcodePrior],
		l.[Postcode],
		l.[AddLine1],
		l.[AddLine2],
		l.[AddLine3],
		l.[AddLine4],
		l.[TelNo],
		l.[Email],
		l.[OTJHours]
		,LSR.LSR1
		,LSR.LSR2
		,LSR.LSR3
		,LSR.LSR4
		,NLM.NLM1
		,NLM.NLM2
		,PPE.PPE1
		,PPE.PPE2
		,EDF.EDF1
		,EDF.EDF2
		,ehc.LearnFAMCode as [EHC]
		,ecf.LearnFAMCode as [ECF]
		,hns.LearnFAMCode as [HNS]
		,dla.LearnFAMCode as [DLA]
		,mcf.LearnFAMCode as [MCF]
		,sen.LearnFAMCode as [SEN]
		,fme.LearnFAMCode as [FME]
		,ProvSpecMon_A.ProvSpecLearnMon AS ProvSpecLearnMon_A	
		,ProvSpecMon_B.ProvSpecLearnMon AS ProvSpecLearnMon_B
	from
		Valid.Learner as l
		left join
		(
			select
				LearnRefNumber,
				UKPRN,
				max([LSR1]) as [LSR1],
				max([LSR2]) as [LSR2],
				max([LSR3]) as [LSR3],
				max([LSR4]) as [LSR4]
			from
				(
					select
						LearnRefNumber,
						UKPRN,
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 1 then LearnFAMCode else null end  as [LSR1],
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 2 then LearnFAMCode else null end  as [LSR2],
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 3 then LearnFAMCode else null end  as [LSR3],
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 4 then LearnFAMCode else null end  as [LSR4]
					from
						Valid.[LearnerFAM]
					where
						[LearnFAMType]=''LSR''
				) as [LSRs]
			group by
				LearnRefNumber,
				UKPRN
		) as [LSR]
		on [LSR].LearnRefNumber = l.LearnRefNumber
		AND [LSR].UKPRN = L.UKPRN
		left join
		(
			select
				LearnRefNumber,
				UKPRN,
				max([NLM1]) as [NLM1],
				max([NLM2]) as [NLM2]
			from
				(
					select
						LearnRefNumber,
						UKPRN,
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 1 then LearnFAMCode else null end  as [NLM1],
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 2 then LearnFAMCode else null end  as [NLM2]
					from
						Valid.[LearnerFAM]
					where
						[LearnFAMType]=''NLM''
				) as [NLMs]
			group by
				LearnRefNumber,
				UKPRN
		) as [NLM]
			on [NLM].LearnRefNumber = l.LearnRefNumber
			AND [NLM].UKPRN = L.UKPRN
		left join
		(
			select
				LearnRefNumber,
				UKPRN,
				max([PPE1]) as [PPE1],
				max([PPE2]) as [PPE2]
			from
				(
					select
						LearnRefNumber,
						UKPRN,
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 1 then LearnFAMCode else null end  as [PPE1],
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 2 then LearnFAMCode else null end  as [PPE2]
					from
						Valid.[LearnerFAM]
					where
						[LearnFAMType]=''PPE''
				) as [PPEs]
			group by
				LearnRefNumber, [UKPRN]
		) as [PPE]
			on [PPE].LearnRefNumber = l.LearnRefNumber
			AND [PPE].UKPRN = L.UKPRN
		left join
		(
			select
				[LearnRefNumber],
				[UKPRN],
				max([EDF1]) as [EDF1],
				max([EDF2]) as [EDF2]
			from
				(
					select
						[LearnRefNumber],
						[UKPRN],
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 1 then LearnFAMCode else null end  as [EDF1],
						case row_number() over (partition by [LearnRefNumber], [UKPRN] order by [LearnRefNumber]) when 2 then LearnFAMCode else null end  as [EDF2]
					from
						[Valid].[LearnerFAM]
					where
						[LearnFAMType]=''EDF''
				) as [EDFs]
			group by
				[LearnRefNumber]
				,UKPRN
		) as [EDF]
			on [EDF].[LearnRefNumber]=l.LearnRefNumber
			AND [EDF].UKPRN = L.UKPRN
		left join
			Valid.LearnerFAM as ehc
				on ehc.LearnRefNumber = l.LearnRefNumber
				AND ehc.UKPRN = L.UKPRN
				and ehc.LearnFAMType = ''EHC'' 
		left join
			Valid.LearnerFAM as ecf
				on ecf.LearnRefNumber = l.LearnRefNumber
				AND ECF.UKPRN = L.UKPRN
				and ecf.LearnFAMType = ''ECF''
		left join
			Valid.LearnerFAM as hns
				on hns.LearnRefNumber = l.LearnRefNumber
				AND HNS.UKPRN = L.UKPRN
				and hns.LearnFAMType = ''HNS''
		left join
			Valid.LearnerFAM as dla
				on dla.LearnRefNumber = l.LearnRefNumber
				AND DLA.UKPRN = L.UKPRN
				and dla.LearnFAMType = ''DLA''
		left join
			Valid.LearnerFAM as mcf
				on mcf.LearnRefNumber = l.LearnRefNumber
				AND MCF.UKPRN = L.UKPRN
				and mcf.LearnFAMType = ''MCF''
		left join
			Valid.LearnerFAM as sen
				on sen.LearnRefNumber = l.LearnRefNumber
				AND SEN.UKPRN = L.UKPRN
				and sen.LearnFAMType = ''SEN''
		left join
			Valid.LearnerFAM as fme
				on fme.LearnRefNumber = l.LearnRefNumber
				AND FME.UKPRN = L.UKPRN
				and fme.LearnFAMType = ''FME''
		left join Valid.[ProviderSpecLearnerMonitoring] as [ProvSpecMon_A]
			on [ProvSpecMon_A].[LearnRefNumber] = l.LearnRefNumber
			AND [ProvSpecMon_A].UKPRN = L.UKPRN
			and [ProvSpecMon_A].[ProvSpecLearnMonOccur]=''A''
		left join Valid.[ProviderSpecLearnerMonitoring] as [ProvSpecMon_B]
			on [ProvSpecMon_B].LearnRefNumber = l.[LearnRefNumber]
			AND [ProvSpecMon_B].UKPRN = L.UKPRN
			and [ProvSpecMon_B].[ProvSpecLearnMonOccur]=''B''
' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Valid].[LearnerEmploymentStatusDenorm]'))
EXEC dbo.sp_executesql @statement = N'

CREATE VIEW [Valid].[LearnerEmploymentStatusDenorm]
AS
	SELECT 
	les.[UKPRN]
	,les.[LearnRefNumber]
	,les.[EmpStat]
	,les.EmpId
	,les.[DateEmpStatApp]
	,les.[AgreeId]	
	,[EmpStatMon_BSI].ESMCode AS ESMCode_BSI
	,[EmpStatMon_EII].ESMCode AS ESMCode_EII
	,[EmpStatMon_LOE].ESMCode AS ESMCode_LOE
	,[EmpStatMon_LOU].ESMCode AS ESMCode_LOU
	,[EmpStatMon_PEI].ESMCode AS ESMCode_PEI
	,[EmpStatMon_SEI].ESMCode AS ESMCode_SEI
	,[EmpStatMon_SEM].ESMCode AS ESMCode_SEM
FROM 
	Valid.[LearnerEmploymentStatus] as les
	left join Valid.[EmploymentStatusMonitoring] as [EmpStatMon_BSI]
		on [EmpStatMon_BSI].LearnRefNumber=les.LearnRefNumber
		AND [EmpStatMon_BSI].UKPRN = LES.UKPRN
		and [EmpStatMon_BSI].DateEmpStatApp = les.DateEmpStatApp
		and [EmpStatMon_BSI].[ESMType]=''BSI''
	left join Valid.[EmploymentStatusMonitoring] as [EmpStatMon_EII]
		on [EmpStatMon_EII].LearnRefNumber=les.LearnRefNumber
		AND [EmpStatMon_EII].UKPRN = LES.UKPRN
		and [EmpStatMon_EII].DateEmpStatApp = les.DateEmpStatApp
		and [EmpStatMon_EII].[ESMType]=''EII''
	left join Valid.[EmploymentStatusMonitoring] as [EmpStatMon_LOE]
		on [EmpStatMon_LOE].LearnRefNumber=les.LearnRefNumber
		AND [EmpStatMon_LOE].UKPRN = LES.UKPRN
		and [EmpStatMon_LOE].DateEmpStatApp = les.DateEmpStatApp
		and [EmpStatMon_LOE].[ESMType]=''LOE''
	left join Valid.[EmploymentStatusMonitoring] as [EmpStatMon_LOU]
		on [EmpStatMon_LOU].LearnRefNumber=les.LearnRefNumber
		AND [EmpStatMon_LOU].UKPRN = LES.UKPRN
		and [EmpStatMon_LOU].DateEmpStatApp = les.DateEmpStatApp
		and [EmpStatMon_LOU].[ESMType]=''LOU''
	left join Valid.[EmploymentStatusMonitoring] as [EmpStatMon_PEI]
		on [EmpStatMon_PEI].LearnRefNumber=les.LearnRefNumber
		AND [EmpStatMon_PEI].UKPRN = LES.UKPRN
		and [EmpStatMon_PEI].DateEmpStatApp = les.DateEmpStatApp
		and [EmpStatMon_PEI].[ESMType]=''PEI''
	left join Valid.[EmploymentStatusMonitoring] as [EmpStatMon_SEI]
		on [EmpStatMon_SEI].LearnRefNumber=les.LearnRefNumber
		AND [EmpStatMon_SEI].UKPRN = LES.UKPRN
		and [EmpStatMon_SEI].DateEmpStatApp = les.DateEmpStatApp
		and [EmpStatMon_SEI].[ESMType]=''SEI''
	left join Valid.[EmploymentStatusMonitoring] as [EmpStatMon_SEM]
		on [EmpStatMon_SEM].LearnRefNumber=les.LearnRefNumber
		AND [EmpStatMon_SEM].UKPRN = LES.UKPRN
		and [EmpStatMon_SEM].DateEmpStatApp = les.DateEmpStatApp
		and [EmpStatMon_SEM].[ESMType]=''SEM''

' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Valid].[LearningDeliveryDenorm]'))
EXEC dbo.sp_executesql @statement = N'

CREATE VIEW [Valid].[LearningDeliveryDenorm]
AS
SELECT
	ld.[UKPRN]
	,ld.[LearnRefNumber]
	,ld.[LearnAimRef]
	,ld.[AimType]
	,ld.[AimSeqNumber]
	,ld.[LearnStartDate]
	,ld.[OrigLearnStartDate]
	,ld.[LearnPlanEndDate]
	,ld.[FundModel]
	,ld.[ProgType]
	,ld.[FworkCode]
	,ld.[PwayCode]
	,ld.[StdCode]
	,ld.[PartnerUKPRN]
	,ld.[DelLocPostCode]
	,ld.[AddHours]
	,ld.[PriorLearnFundAdj]
	,ld.[OtherFundAdj]
	,ld.[ConRefNumber]
	,ld.[EPAOrgID]
	,ld.[EmpOutcome]
	,ld.[CompStatus]
	,ld.[LearnActEndDate]
	,ld.[WithdrawReason]
	,ld.[Outcome]
	,ld.[AchDate]
	,ld.[OutGrade]
	,ld.[SWSupAimId]
	,HEM.HEM1
	,HEM.HEM2
	,HEM.HEM3
	,HHS.HHS1
	,HHS.HHS2
	,[LDFAM_SOF].LearnDelFAMCode AS [LDFAM_SOF]
	,[LDFAM_EEF].LearnDelFAMCode AS [LDFAM_EEF]
	,[LDFAM_RES].LearnDelFAMCode AS [LDFAM_RES]
	,[LDFAM_ADL].LearnDelFAMCode AS [LDFAM_ADL]
	,[LDFAM_FFI].LearnDelFAMCode AS [LDFAM_FFI]
	,[LDFAM_WPP].LearnDelFAMCode AS [LDFAM_WPP]
	,[LDFAM_POD].LearnDelFAMCode AS [LDFAM_POD]
	,[LDFAM_ASL].LearnDelFAMCode AS [LDFAM_ASL]
	,[LDFAM_FLN].LearnDelFAMCode AS [LDFAM_FLN]
	,[LDFAM_NSA].LearnDelFAMCode AS [LDFAM_NSA]
	,[ProvSpecMon_A].ProvSpecDelMon AS ProvSpecDelMon_A
	,[ProvSpecMon_B].ProvSpecDelMon	AS ProvSpecDelMon_B
	,[ProvSpecMon_C].ProvSpecDelMon	AS ProvSpecDelMon_C
	,[ProvSpecMon_D].ProvSpecDelMon	AS ProvSpecDelMon_D
	,LDM.LDM1
	,LDM.LDM2
	,LDM.LDM3
	,LDM.LDM4

FROM
	Valid.LearningDelivery as ld
	left join
	(
		select
			[LearnRefNumber],
			[AimSeqNumber],
			UKPRN,
			max([HEM1]) as [HEM1],
			max([HEM2]) as [HEM2],
			max([HEM3]) as [HEM3]
		from
			(
				select
					[LearnRefNumber],
					[AimSeqNumber],
					UKPRN,
					case row_number() over (partition by LearnRefNumber, AimSeqNumber, UKPRN order by LearnRefNumber, AimSeqNumber) when 1 then LearnDelFAMCode else null end  as [HEM1],
					case row_number() over (partition by LearnRefNumber, AimSeqNumber, UKPRN order by LearnRefNumber, AimSeqNumber) when 2 then LearnDelFAMCode else null end  as [HEM2],
					case row_number() over (partition by LearnRefNumber, AimSeqNumber, UKPRN order by LearnRefNumber, AimSeqNumber) when 3 then LearnDelFAMCode else null end  as [HEM3]
				from
					Valid.[LearningDeliveryFAM]
				where
					[LearnDelFAMType]=''HEM''
			) as [HEMs]
		group by
			[LearnRefNumber]
			,[AimSeqNumber]
			,UKPRN
	) as [HEM]
	on [HEM].[LearnRefNumber]=ld.[LearnRefNumber]
	and [HEM].[AimSeqNumber]=ld.[AimSeqNumber]
	AND [HEM].UKPRN = LD.UKPRN
	left join
	(
		select
			LearnRefNumber, 
			AimSeqNumber,
			UKPRN,
			max([HHS1]) as [HHS1],
			max([HHS2]) as [HHS2]
		from
			(
				select
					LearnRefNumber,
					AimSeqNumber,
					UKPRN,
					case row_number() over (partition by LearnRefNumber, AimSeqNumber, UKPRN order by LearnRefNumber, AimSeqNumber) when 1 then LearnDelFAMCode else null end  as [HHS1],
					case row_number() over (partition by LearnRefNumber, AimSeqNumber, UKPRN order by LearnRefNumber, AimSeqNumber) when 2 then LearnDelFAMCode else null end  as [HHS2]
				from
					Valid.[LearningDeliveryFAM]
				where
					[LearnDelFAMType]=''HHS''
			) as [HHSs]
		group by
			[LearnRefNumber]
			,[AimSeqNumber]
			,UKPRN
	) as [HHS]
	on [HHS].[LearnRefNumber]=ld.[LearnRefNumber]
	and [HHS].AimSeqNumber=ld.AimSeqNumber
	AND [HHS].UKPRN = LD.UKPRN

	LEFT JOIN
		[Valid].[LearningDeliveryFAM] AS [LDFAM_SOF] 
			ON ld.[LearnRefNumber] = [LDFAM_SOF].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_SOF].[AimSeqNumber]
			AND ld.UKPRN = [LDFAM_SOF].UKPRN
			AND [LDFAM_SOF].[LearnDelFAMType] = ''SOF''
	LEFT JOIN
		[Valid].[LearningDeliveryFAM] AS [LDFAM_EEF] 
			ON ld.[LearnRefNumber] = [LDFAM_EEF].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_EEF].[AimSeqNumber]
			AND ld.UKPRN = [LDFAM_EEF].UKPRN
			AND [LDFAM_EEF].[LearnDelFAMType] = ''EEF''
	LEFT JOIN
		[Valid].[LearningDeliveryFAM] AS [LDFAM_RES] 
			ON ld.[LearnRefNumber] = [LDFAM_RES].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_RES].[AimSeqNumber]
			AND ld.UKPRN = [LDFAM_RES].UKPRN
			AND [LDFAM_RES].[LearnDelFAMType] = ''RES''
	LEFT JOIN            
		[Valid].[LearningDeliveryFAM] AS [LDFAM_ADL] 
			ON  ld.[LearnRefNumber] = [LDFAM_ADL].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_ADL].[AimSeqNumber]
			AND ld.UKPRN = [LDFAM_ADL].UKPRN
			AND [LDFAM_ADL].[LearnDelFAMType] = ''ADL''
	LEFT JOIN
        [Valid].[LearningDeliveryFAM] AS [LDFAM_FFI] 
			ON  ld.[LearnRefNumber] = [LDFAM_FFI].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_FFI].[AimSeqNumber]
			AND [LDFAM_FFI].[LearnDelFAMType] = ''FFI''
			AND LD.UKPRN = [LDFAM_FFI].UKPRN
	LEFT JOIN 
		[Valid].[LearningDeliveryFAM] AS [LDFAM_WPP] 
			ON ld.[LearnRefNumber] = [LDFAM_WPP].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_WPP].[AimSeqNumber]
			AND LD.UKPRN = [LDFAM_WPP].UKPRN
			AND [LDFAM_WPP].[LearnDelFAMType] = ''WPP''
	LEFT JOIN 
		[Valid].[LearningDeliveryFAM] AS [LDFAM_POD] 
			ON ld.[LearnRefNumber] = [LDFAM_POD].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_POD].[AimSeqNumber]
			AND LD.UKPRN = [LDFAM_POD].UKPRN
			AND [LDFAM_POD].[LearnDelFAMType] = ''POD''
	LEFT JOIN 
		[Valid].[LearningDeliveryFAM] AS [LDFAM_ASL] 
			ON ld.[LearnRefNumber] = [LDFAM_ASL].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_ASL].[AimSeqNumber]
			AND LD.UKPRN = [LDFAM_ASL].UKPRN
			AND [LDFAM_ASL].[LearnDelFAMType] = ''ASL''
	LEFT JOIN 
		[Valid].[LearningDeliveryFAM] AS [LDFAM_FLN] 
			ON ld.[LearnRefNumber] = [LDFAM_FLN].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_FLN].[AimSeqNumber]
			AND LD.UKPRN = [LDFAM_FLN].UKPRN
			AND [LDFAM_FLN].[LearnDelFAMType] = ''FLN''
	LEFT JOIN 
		[Valid].[LearningDeliveryFAM] AS [LDFAM_NSA] 
			ON ld.[LearnRefNumber] = [LDFAM_NSA].[LearnRefNumber]
			AND ld.[AimSeqNumber] = [LDFAM_NSA].[AimSeqNumber]
			AND LD.UKPRN = [LDFAM_NSA].UKPRN
			AND [LDFAM_NSA].[LearnDelFAMType] = ''NSA''

	left join Valid.[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_A]
		on [ProvSpecMon_A].[LearnRefNumber]=ld.[LearnRefNumber]
		and [ProvSpecMon_A].[AimSeqNumber]=ld.[AimSeqNumber]
		AND [ProvSpecMon_A].UKPRN = LD.UKPRN
		and [ProvSpecMon_A].[ProvSpecDelMonOccur]=''A''

	left join Valid.[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_B]
		on [ProvSpecMon_B].[LearnRefNumber]=ld.[LearnRefNumber]
		and [ProvSpecMon_B].[AimSeqNumber]=ld.[AimSeqNumber]
		AND ProvSpecMon_B.UKPRN = LD.UKPRN
		and [ProvSpecMon_B].[ProvSpecDelMonOccur]=''B''

	left join Valid.[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_C]
		on [ProvSpecMon_C].[LearnRefNumber]=ld.[LearnRefNumber]
		and [ProvSpecMon_C].[AimSeqNumber]=ld.[AimSeqNumber]
		AND [ProvSpecMon_C].UKPRN = LD.UKPRN
		and [ProvSpecMon_C].[ProvSpecDelMonOccur]=''C''

	left join Valid.[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_D]
		on [ProvSpecMon_D].[LearnRefNumber]=ld.[LearnRefNumber]
		and [ProvSpecMon_D].[AimSeqNumber]=ld.[AimSeqNumber]
		AND [ProvSpecMon_D].UKPRN = LD.UKPRN
		and [ProvSpecMon_D].[ProvSpecDelMonOccur]=''D''
	left join
	(
		select
			[LearnRefNumber],
			[AimSeqNumber],
			[UKPRN],
			max([LDM1]) as [LDM1],
			max([LDM2]) as [LDM2],
			max([LDM3]) as [LDM3],
			max([LDM4]) as [LDM4]
		from
		(
			select
				[LearnRefNumber],
				[AimSeqNumber],
				UKPRN,
				case row_number() over (partition by [LearnRefNumber], AimSeqNumber, UKPRN order by [LearnRefNumber]) when 1 then LearnDelFAMCode else null end  as [LDM1],
				case row_number() over (partition by [LearnRefNumber], AimSeqNumber, UKPRN order by [LearnRefNumber]) when 2 then LearnDelFAMCode else null end  as [LDM2],
				case row_number() over (partition by [LearnRefNumber], AimSeqNumber, UKPRN order by [LearnRefNumber]) when 3 then LearnDelFAMCode else null end  as [LDM3],
				case row_number() over (partition by [LearnRefNumber], AimSeqNumber, UKPRN order by [LearnRefNumber]) when 4 then LearnDelFAMCode else null end  as [LDM4]
			from
				[Valid].[LearningDeliveryFAM]
			where
				[LearnDelFAMType]=''LDM''
		) as [LDMs]
		group by
			[LearnRefNumber],
			[AimSeqNumber],
			[UKPRN]
	) as [LDM]
	on [LDM].[LearnRefNumber]=ld.[LearnRefNumber]
	and LDM.AimSeqNumber = ld.AimSeqNumber
	AND LDM.UKPRN = LD.UKPRN

' 
GO
