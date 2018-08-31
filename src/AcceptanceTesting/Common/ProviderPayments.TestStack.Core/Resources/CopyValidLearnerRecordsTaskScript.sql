/****** Object:  View [dbo].[ValidLearners]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ValidLearners]'))
DROP VIEW [dbo].[ValidLearners]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_SourceFile]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_SourceFile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_SourceFile]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_Source]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_Source]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_Source]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LLDDandHealthProblem]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LLDDandHealthProblem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LLDDandHealthProblem]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningProvider]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningProvider]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearningProvider]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryWorkPlacement]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDeliveryWorkPlacement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearningDeliveryWorkPlacement]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryHE]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDeliveryHE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearningDeliveryHE]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryFAM]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDeliveryFAM]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearningDeliveryFAM]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDelivery]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDelivery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearningDelivery]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerHEFinancialSupport]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerHEFinancialSupport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearnerHEFinancialSupport]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerHE]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerHE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearnerHE]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerEmploymentStatus]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerEmploymentStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearnerEmploymentStatus]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerContact]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerContact]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_LearnerContact]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_Learner]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_Learner]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_Learner]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_ContactPreference]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_ContactPreference]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_ContactPreference]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_CollectionDetails]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_CollectionDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid_CollectionDetails]
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid]    Script Date: 12/09/2016 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TransformInputToValid]
GO
/****** Object:  View [dbo].[ValidLearners]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ValidLearners]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[ValidLearners] 
AS
SELECT L.Learner_Id
FROM [Input].[Learner] L
WHERE L.LearnRefNumber IS NOT NULL
' 
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_CollectionDetails]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_CollectionDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_CollectionDetails] as
	begin
	IF OBJECT_ID(''tempdb..#CollectionDetails'', ''U'') IS NOT NULL
	DROP TABLE #CollectionDetails;
		
		select
			[CollectionDetails].[Collection],
			[CollectionDetails].[Year],
			[CollectionDetails].[FilePreparationDate]
		INTO #CollectionDetails
		from
			[Input].[CollectionDetails]

		insert into 
			[Valid].[CollectionDetails]
				(
					[Collection],
					[Year],
					[FilePreparationDate]
				)
	   select * from #CollectionDetails

	IF OBJECT_ID(''tempdb..#CollectionDetails'', ''U'') IS NOT NULL
	DROP TABLE #CollectionDetails;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_ContactPreference]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_ContactPreference]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_ContactPreference] as
	begin
	IF OBJECT_ID(''tempdb..#ContactPreference'', ''U'') IS NOT NULL
	DROP TABLE #ContactPreference;

		select
			[Learner].[LearnRefNumber],
			[ContactPreference].[ContPrefType],
			[ContactPreference].[ContPrefCode]
		into #ContactPreference
		from
			[Input].[ContactPreference]
			inner join [Input].[Learner]
				on [ContactPreference].[Learner_Id]=[Learner].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [ContactPreference].[Learner_Id]=[ValidLearners].[Learner_Id]

	    
		insert into 
			[Valid].[ContactPreference]
				(
					[LearnRefNumber],
					[ContPrefType],
					[ContPrefCode]
				)
	    select * from #ContactPreference

	IF OBJECT_ID(''tempdb..#ContactPreference'', ''U'') IS NOT NULL
	DROP TABLE #ContactPreference;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_Learner]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_Learner]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_Learner] as
	begin
	IF OBJECT_ID(''tempdb..#Learner'', ''U'') IS NOT NULL
	DROP TABLE #Learner;

    	select
			[Learner].[LearnRefNumber],
			[Learner].[PrevLearnRefNumber],
			[Learner].[PrevUKPRN],
			[Learner].[ULN],
			[Learner].[FamilyName],
			[Learner].[GivenNames],
			[Learner].[DateOfBirth],
			[Learner].[Ethnicity],
			[Learner].[Sex],
			[Learner].[LLDDHealthProb],
			[Learner].[NINumber],
			[Learner].[PriorAttain],
			[Learner].[Accom],
			[Learner].[ALSCost],
			[Learner].[PlanLearnHours],
			[Learner].[PlanEEPHours],
			[Learner].[MathGrade],
			[Learner].[EngGrade],
			HP.Postcode as [HomePostcode],
			CP.Postcode as [CurrentPostcode],
			[LrnFAM_DLA].[LearnFAMCode] as [LrnFAM_DLA],
			[LrnFAM_ECF].[LearnFAMCode] as [LrnFAM_ECF],
			[EDF].[EDF1] as [LrnFAM_EDF1],
			[EDF].[EDF2] as [LrnFAM_EDF2],
			[LrnFAM_EHC].[LearnFAMCode] as[LrnFAM_EHC],
			[LrnFAM_FME].[LearnFAMCode] as [LrnFAM_FME],
			[LrnFAM_HNS].[LearnFAMCode] as [LrnFAM_HNS],
			[LrnFAM_LDA].[LearnFAMCode] as [LrnFAM_LDA],
			[LSR].[LSR1] as [LrnFAM_LSR1],
			[LSR].[LSR2] as [LrnFAM_LSR2],
			[LSR].[LSR3] as [LrnFAM_LSR3],
			[LSR].[LSR4] as [LrnFAM_LSR4],
			[LrnFAM_MCF].[LearnFAMCode] as [LrnFAM_MCF],
			[NLM].[NLM1] as [LrnFAM_NLM1],
			[NLM].[NLM2] as [LrnFAM_NLM2],
			[PPE].[PPE1] as [LrnFAM_PPE1],
			[PPE].[PPE2] as [LrnFAM_PPE2],
			[LrnFAM_SEN].[LearnFAMCode] as [LrnFAM_SEN],
			[ProvSpecMon_A].[ProvSpecLearnMon] as [ProvSpecMon_A],
			[ProvSpecMon_B].[ProvSpecLearnMon] as [ProvSpecMon_B]
		into #Learner
		from
			[Input].[Learner]
			left join [Input].LearnerContact HP
				on Learner.Learner_Id=HP.Learner_Id
				and HP.LocType=2
				and HP.ContType=1
			left join [Input].LearnerContact CP
				on Learner.Learner_Id=CP.Learner_Id
				and CP.LocType=2
				and CP.ContType=2
			left join [Input].[LearnerFAM] as [LrnFAM_DLA]
				on [LrnFAM_DLA].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_DLA].[LearnFAMType]=''DLA''
			left join [Input].[LearnerFAM] as [LrnFAM_ECF]
				on [LrnFAM_ECF].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_ECF].[LearnFAMType]=''ECF''
			left join
				(
					select
						[Learner_Id],
						max([EDF1]) as [EDF1],
						max([EDF2]) as [EDF2]
					from
						(
							select
								[Learner_Id],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 1 then LearnFAMCode else null end  as [EDF1],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 2 then LearnFAMCode else null end  as [EDF2]
							from
								[Input].[LearnerFAM]
							where
								[LearnFAMType]=''EDF''
						) as [EDFs]
					group by
						[Learner_Id]
				) as [EDF]
				on [EDF].[Learner_Id]=[Learner].[Learner_Id]
			left join [Input].[LearnerFAM] as [LrnFAM_EHC]
				on [LrnFAM_EHC].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_EHC].[LearnFAMType]=''EHC''
			left join [Input].[LearnerFAM] as [LrnFAM_FME]
				on [LrnFAM_FME].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_FME].[LearnFAMType]=''FME''
			left join [Input].[LearnerFAM] as [LrnFAM_HNS]
				on [LrnFAM_HNS].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_HNS].[LearnFAMType]=''HNS''
			left join [Input].[LearnerFAM] as [LrnFAM_LDA]
				on [LrnFAM_LDA].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_LDA].[LearnFAMType]=''LDA''
			left join
				(
					select
						[Learner_Id],
						max([LSR1]) as [LSR1],
						max([LSR2]) as [LSR2],
						max([LSR3]) as [LSR3],
						max([LSR4]) as [LSR4]
					from
						(
							select
								[Learner_Id],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 1 then LearnFAMCode else null end  as [LSR1],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 2 then LearnFAMCode else null end  as [LSR2],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 3 then LearnFAMCode else null end  as [LSR3],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 4 then LearnFAMCode else null end  as [LSR4]
							from
								[Input].[LearnerFAM]
							where
								[LearnFAMType]=''LSR''
						) as [LSRs]
					group by
						[Learner_Id]
				) as [LSR]
				on [LSR].[Learner_Id]=[Learner].[Learner_Id]
			left join [Input].[LearnerFAM] as [LrnFAM_MCF]
				on [LrnFAM_MCF].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_MCF].[LearnFAMType]=''MCF''
			left join
				(
					select
						[Learner_Id],
						max([NLM1]) as [NLM1],
						max([NLM2]) as [NLM2]
					from
						(
							select
								[Learner_Id],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 1 then LearnFAMCode else null end  as [NLM1],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 2 then LearnFAMCode else null end  as [NLM2]
							from
								[Input].[LearnerFAM]
							where
								[LearnFAMType]=''NLM''
						) as [NLMs]
					group by
						[Learner_Id]
				) as [NLM]
				on [NLM].[Learner_Id]=[Learner].[Learner_Id]
			left join
				(
					select
						[Learner_Id],
						max([PPE1]) as [PPE1],
						max([PPE2]) as [PPE2]
					from
						(
							select
								[Learner_Id],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 1 then LearnFAMCode else null end  as [PPE1],
								case row_number() over (partition by [Learner_Id] order by [Learner_Id]) when 2 then LearnFAMCode else null end  as [PPE2]
							from
								[Input].[LearnerFAM]
							where
								[LearnFAMType]=''PPE''
						) as [PPEs]
					group by
						[Learner_Id]
				) as [PPE]
				on [PPE].[Learner_Id]=[Learner].[Learner_Id]
			left join [Input].[LearnerFAM] as [LrnFAM_SEN]
				on [LrnFAM_SEN].[Learner_Id]=[Learner].[Learner_Id]
				and [LrnFAM_SEN].[LearnFAMType]=''SEN''
			left join [Input].[ProviderSpecLearnerMonitoring] as [ProvSpecMon_A]
				on [ProvSpecMon_A].[Learner_Id]=[Learner].[Learner_Id]
				and [ProvSpecMon_A].[ProvSpecLearnMonOccur]=''A''
			left join [Input].[ProviderSpecLearnerMonitoring] as [ProvSpecMon_B]
				on [ProvSpecMon_B].[Learner_Id]=[Learner].[Learner_Id]
				and [ProvSpecMon_B].[ProvSpecLearnMonOccur]=''B''
			inner join [dbo].[ValidLearners]
				on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]

			insert into 
			[Valid].[Learner]
				(
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
				)
				select * from #Learner

	IF OBJECT_ID(''tempdb..#Learner'', ''U'') IS NOT NULL
	DROP TABLE #Learner;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerContact]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerContact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearnerContact] as
	begin
	IF OBJECT_ID(''tempdb..#LearnerContact'', ''U'') IS NOT NULL
	DROP TABLE #LearnerContact;

		select
			[Learner].[LearnRefNumber],
			[ConjoinedLearnerContact].[HomePostcode],
			[ConjoinedLearnerContact].[CurrentPostcode],
			[ConjoinedLearnerContact].[TelNumber],
			[ConjoinedLearnerContact].[Email],
			[ConjoinedLearnerContact].[AddLine1],
			[ConjoinedLearnerContact].[AddLine2],
			[ConjoinedLearnerContact].[AddLine3],
			[ConjoinedLearnerContact].[AddLine4]
		into #LearnerContact
		from
			(
				select
					coalesce(CurrentContact.Learner_Id,PreviousContact.Learner_Id) Learner_Id,
					max(PostAdd.AddLine1) AddLine1,
					max(PostAdd.AddLine2) AddLine2,
					max(PostAdd.AddLine3) AddLine3,
					max(PostAdd.AddLine4) AddLine4,
					max(CurrentContact.Postcode) CurrentPostcode,
					max(PreviousContact.Postcode) HomePostcode,
					max(CurrentContact.Email) Email,
					max(CurrentContact.TelNumber) TelNumber
				from
					(select * from [Input].LearnerContact where ContType=2) CurrentContact
					full outer join (select * from [Input].LearnerContact where ContType=1) PreviousContact
						on CurrentContact.Learner_Id=PreviousContact.Learner_Id
						--and PreviousContact.ContType=1
					full outer join [Input].PostAdd 
						on CurrentContact.LearnerContact_Id=PostAdd.LearnerContact_Id
				group by
					coalesce(CurrentContact.Learner_Id,PreviousContact.Learner_Id)			
			) as [ConjoinedLearnerContact]
			inner join [Input].[Learner]
				on [ConjoinedLearnerContact].[Learner_Id]=[Learner].[Learner_Id]
				inner join [dbo].[ValidLearners]
				on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]

		insert into 
			[Valid].[LearnerContact]
				(
					[LearnRefNumber],
					[HomePostcode],
					[CurrentPostcode],
					[TelNumber],
					[Email],
					[AddLine1],
					[AddLine2],
					[AddLine3],
					[AddLine4]
				)
        select * from #LearnerContact

	IF OBJECT_ID(''tempdb..#LearnerContact'', ''U'') IS NOT NULL
	DROP TABLE #LearnerContact;

	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerEmploymentStatus]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerEmploymentStatus]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearnerEmploymentStatus] as
	begin

	IF OBJECT_ID(''tempdb..#LearnerEmploymentStatus'', ''U'') IS NOT NULL
	DROP TABLE #LearnerEmploymentStatus;

		select
			[Learner].[LearnRefNumber],
			[LearnerEmploymentStatus].[EmpStat],
			[LearnerEmploymentStatus].[DateEmpStatApp],
			[LearnerEmploymentStatus].[EmpId],
			[EmpStatMon_BSI].[ESMCode] as [EmpStatMon_BSI],
			[EmpStatMon_EII].[ESMCode] as [EmpStatMon_EII],
			[EmpStatMon_LOE].[ESMCode] as [EmpStatMon_LOE],
			[EmpStatMon_LOU].[ESMCode] as [EmpStatMon_LOU],
			[EmpStatMon_PEI].[ESMCode] as [EmpStatMon_PEI],
			[EmpStatMon_SEI].[ESMCode] as [EmpStatMon_SEI],
			[EmpStatMon_SEM].[ESMCode] as [EmpStatMon_SEM]
		into #LearnerEmploymentStatus
		from
			[Input].[LearnerEmploymentStatus]
			inner join [Input].[Learner]
				on [LearnerEmploymentStatus].[Learner_Id]=[Learner].[Learner_Id]
			left join [Input].[EmploymentStatusMonitoring] as [EmpStatMon_BSI]
				on [EmpStatMon_BSI].[LearnerEmploymentStatus_Id]=[LearnerEmploymentStatus].[LearnerEmploymentStatus_Id]
				and [EmpStatMon_BSI].[ESMType]=''BSI''
			left join [Input].[EmploymentStatusMonitoring] as [EmpStatMon_EII]
				on [EmpStatMon_EII].[LearnerEmploymentStatus_Id]=[LearnerEmploymentStatus].[LearnerEmploymentStatus_Id]
				and [EmpStatMon_EII].[ESMType]=''EII''
			left join [Input].[EmploymentStatusMonitoring] as [EmpStatMon_LOE]
				on [EmpStatMon_LOE].[LearnerEmploymentStatus_Id]=[LearnerEmploymentStatus].[LearnerEmploymentStatus_Id]
				and [EmpStatMon_LOE].[ESMType]=''LOE''
			left join [Input].[EmploymentStatusMonitoring] as [EmpStatMon_LOU]
				on [EmpStatMon_LOU].[LearnerEmploymentStatus_Id]=[LearnerEmploymentStatus].[LearnerEmploymentStatus_Id]
				and [EmpStatMon_LOU].[ESMType]=''LOU''
			left join [Input].[EmploymentStatusMonitoring] as [EmpStatMon_PEI]
				on [EmpStatMon_PEI].[LearnerEmploymentStatus_Id]=[LearnerEmploymentStatus].[LearnerEmploymentStatus_Id]
				and [EmpStatMon_PEI].[ESMType]=''PEI''
			left join [Input].[EmploymentStatusMonitoring] as [EmpStatMon_SEI]
				on [EmpStatMon_SEI].[LearnerEmploymentStatus_Id]=[LearnerEmploymentStatus].[LearnerEmploymentStatus_Id]
				and [EmpStatMon_SEI].[ESMType]=''SEI''
			left join [Input].[EmploymentStatusMonitoring] as [EmpStatMon_SEM]
				on [EmpStatMon_SEM].[LearnerEmploymentStatus_Id]=[LearnerEmploymentStatus].[LearnerEmploymentStatus_Id]
				and [EmpStatMon_SEM].[ESMType]=''SEM''
			inner join [dbo].[ValidLearners]
				on [LearnerEmploymentStatus].[Learner_Id]=[ValidLearners].[Learner_Id]

				
		insert into 
			[Valid].[LearnerEmploymentStatus]
				(
					[LearnRefNumber],
					[EmpStat],
					[DateEmpStatApp],
					[EmpId],
					[EmpStatMon_BSI],
					[EmpStatMon_EII],
					[EmpStatMon_LOE],
					[EmpStatMon_LOU],
					[EmpStatMon_PEI],
					[EmpStatMon_SEI],
					[EmpStatMon_SEM]
				)
        select * from #LearnerEmploymentStatus

	IF OBJECT_ID(''tempdb..#LearnerEmploymentStatus'', ''U'') IS NOT NULL
	DROP TABLE #LearnerEmploymentStatus;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerHE]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerHE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearnerHE] as
	begin
	IF OBJECT_ID(''tempdb..#LearnerHE'', ''U'') IS NOT NULL
	DROP TABLE #LearnerHE;

		select
			[Learner].[LearnRefNumber],
			[LearnerHE].[UCASPERID],
			[LearnerHE].[TTACCOM]
		into #LearnerHE
		from
			[Input].[LearnerHE]
			inner join [Input].[Learner]
				on [LearnerHE].[Learner_Id]=[Learner].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [LearnerHE].[Learner_Id]=[ValidLearners].[Learner_Id]

		insert into 
			[Valid].[LearnerHE]
				(
					[LearnRefNumber],
					[UCASPERID],
					[TTACCOM]
				)
        select * from #LearnerHE

	IF OBJECT_ID(''tempdb..#LearnerHE'', ''U'') IS NOT NULL
	DROP TABLE #LearnerHE;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearnerHEFinancialSupport]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearnerHEFinancialSupport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearnerHEFinancialSupport] as
	begin
	IF OBJECT_ID(''tempdb..#LearnerHEFinancialSupport'', ''U'') IS NOT NULL
	DROP TABLE #LearnerHEFinancialSupport;

		select
			[Learner].[LearnRefNumber],
			[LearnerHEFinancialSupport].[FINTYPE],
			[LearnerHEFinancialSupport].[FINAMOUNT]
		into #LearnerHEFinancialSupport
		from
			[Input].[LearnerHEFinancialSupport]
			inner join [Input].[LearnerHE]
				on [LearnerHEFinancialSupport].[LearnerHE_Id]=[LearnerHE].[LearnerHE_Id]
			inner join [Input].[Learner]
				on [Learner].[Learner_Id]=[LearnerHE].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [LearnerHE].[Learner_Id]=[ValidLearners].[Learner_Id]

		insert into 
			[Valid].[LearnerHEFinancialSupport]
				(
					[LearnRefNumber],
					[FINTYPE],
					[FINAMOUNT]
				)
        select * from #LearnerHEFinancialSupport

	IF OBJECT_ID(''tempdb..#LearnerHEFinancialSupport'', ''U'') IS NOT NULL
	DROP TABLE #LearnerHEFinancialSupport;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDelivery]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearningDelivery] as
	begin
	IF OBJECT_ID(''tempdb..#LearningDelivery'', ''U'') IS NOT NULL
	DROP TABLE #LearningDelivery;

		select
			[Learner].[LearnRefNumber],
			[LearningDelivery].[LearnAimRef],
			[LearningDelivery].[AimType],
			[LearningDelivery].[AimSeqNumber],
			[LearningDelivery].[LearnStartDate],
			[LearningDelivery].[OrigLearnStartDate],
			[LearningDelivery].[LearnPlanEndDate],
			[LearningDelivery].[FundModel],
			[LearningDelivery].[ProgType],
			[LearningDelivery].[FworkCode],
			[LearningDelivery].[PwayCode],
			[LearningDelivery].[StdCode],
			[LearningDelivery].[PartnerUKPRN],
			[LearningDelivery].[DelLocPostCode],
			[LearningDelivery].[AddHours],
			[LearningDelivery].[PriorLearnFundAdj],
			[LearningDelivery].[OtherFundAdj],
			[LearningDelivery].[ConRefNumber],
			[LearningDelivery].[EPAOrgID],
			[LearningDelivery].[EmpOutcome],
			[LearningDelivery].[CompStatus],
			[LearningDelivery].[LearnActEndDate],
			[LearningDelivery].[WithdrawReason],
			[LearningDelivery].[Outcome],
			[LearningDelivery].[AchDate],
			[LearningDelivery].[OutGrade],
			[LearningDelivery].[SWSupAimId],
			[LrnDelFAM_ADL].[LearnDelFAMCode] as [LrnDelFAM_ADL],
			[LrnDelFAM_ASL].[LearnDelFAMCode] as [LrnDelFAM_ASL],
			[LrnDelFAM_EEF].[LearnDelFAMCode] as [LrnDelFAM_EEF],
			[LrnDelFAM_FFI].[LearnDelFAMCode] as [LrnDelFAM_FFI],
			[LrnDelFAM_FLN].[LearnDelFAMCode] as [LrnDelFAM_FLN],
			[HEM].[HEM1] as [LrnDelFAM_HEM1],
			[HEM].[HEM2] as [LrnDelFAM_HEM2],
			[HEM].[HEM3] as [LrnDelFAM_HEM3],
			[HHS].[HHS1] as [LrnDelFAM_HHS1],
			[HHS].[HHS2] as [LrnDelFAM_HHS2],
			[LDM].[LDM1] as [LrnDelFAM_LDM1],
			[LDM].[LDM2] as [LrnDelFAM_LDM2],
			[LDM].[LDM3] as [LrnDelFAM_LDM3],
			[LDM].[LDM4] as [LrnDelFAM_LDM4],
			[LrnDelFAM_NSA].[LearnDelFAMCode] as [LrnDelFAM_NSA],
			[LrnDelFAM_POD].[LearnDelFAMCode] as [LrnDelFAM_POD],
			[LrnDelFAM_RES].[LearnDelFAMCode] as [LrnDelFAM_RES],
			[LrnDelFAM_SOF].[LearnDelFAMCode] as [LrnDelFAM_SOF],
			[LrnDelFAM_SPP].[LearnDelFAMCode] as [LrnDelFAM_SPP],
			[LrnDelFAM_WPP].[LearnDelFAMCode] as [LrnDelFAM_WPP],
			[ProvSpecMon_A].[ProvSpecDelMon] as [ProvSpecMon_A],
			[ProvSpecMon_B].[ProvSpecDelMon] as [ProvSpecMon_B],
			[ProvSpecMon_C].[ProvSpecDelMon] as [ProvSpecMon_C],
			[ProvSpecMon_D].[ProvSpecDelMon] as [ProvSpecMon_D]
			into #LearningDelivery
		from
			[Input].[LearningDelivery]
			inner join [Input].[Learner]
				on [LearningDelivery].[Learner_Id]=[Learner].[Learner_Id]
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_ADL]
				on [LrnDelFAM_ADL].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_ADL].[LearnDelFAMType]=''ADL''
			left join
				(
					select
						[LearningDelivery_Id]
					from
						(
							select
								[LearningDelivery_Id]
							from
								[Input].[LearningDeliveryFAM]
							where
								[LearnDelFAMType]=''ALB''
						) as [ALBs]
					group by
						[LearningDelivery_Id]
				) as [ALB]
				on [ALB].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_ASL]
				on [LrnDelFAM_ASL].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_ASL].[LearnDelFAMType]=''ASL''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_EEF]
				on [LrnDelFAM_EEF].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_EEF].[LearnDelFAMType]=''EEF''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_FFI]
				on [LrnDelFAM_FFI].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_FFI].[LearnDelFAMType]=''FFI''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_FLN]
				on [LrnDelFAM_FLN].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_FLN].[LearnDelFAMType]=''FLN''
			left join
				(
					select
						[LearningDelivery_Id],
						max([HEM1]) as [HEM1],
						max([HEM2]) as [HEM2],
						max([HEM3]) as [HEM3]
					from
						(
							select
								[LearningDelivery_Id],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 1 then LearnDelFAMCode else null end  as [HEM1],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 2 then LearnDelFAMCode else null end  as [HEM2],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 3 then LearnDelFAMCode else null end  as [HEM3]
							from
								[Input].[LearningDeliveryFAM]
							where
								[LearnDelFAMType]=''HEM''
						) as [HEMs]
					group by
						[LearningDelivery_Id]
				) as [HEM]
				on [HEM].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			left join
				(
					select
						[LearningDelivery_Id],
						max([HHS1]) as [HHS1],
						max([HHS2]) as [HHS2]
					from
						(
							select
								[LearningDelivery_Id],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 1 then LearnDelFAMCode else null end  as [HHS1],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 2 then LearnDelFAMCode else null end  as [HHS2]
							from
								[Input].[LearningDeliveryFAM]
							where
								[LearnDelFAMType]=''HHS''
						) as [HHSs]
					group by
						[LearningDelivery_Id]
				) as [HHS]
				on [HHS].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			left join
				(
					select
						[LearningDelivery_Id],
						max([LDM1]) as [LDM1],
						max([LDM2]) as [LDM2],
						max([LDM3]) as [LDM3],
						max([LDM4]) as [LDM4]
					from
						(
							select
								[LearningDelivery_Id],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 1 then LearnDelFAMCode else null end  as [LDM1],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 2 then LearnDelFAMCode else null end  as [LDM2],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 3 then LearnDelFAMCode else null end  as [LDM3],
								case row_number() over (partition by [LearningDelivery_Id] order by [LearningDelivery_Id]) when 4 then LearnDelFAMCode else null end  as [LDM4]
							from
								[Input].[LearningDeliveryFAM]
							where
								[LearnDelFAMType]=''LDM''
						) as [LDMs]
					group by
						[LearningDelivery_Id]
				) as [LDM]
				on [LDM].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			left join
				(
					select
						[LearningDelivery_Id]
					from
						(
							select
								[LearningDelivery_Id]
							from
								[Input].[LearningDeliveryFAM]
							where
								[LearnDelFAMType]=''LSF''
						) as [LSFs]
					group by
						[LearningDelivery_Id]
				) as [LSF]
				on [LSF].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_NSA]
				on [LrnDelFAM_NSA].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_NSA].[LearnDelFAMType]=''NSA''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_POD]
				on [LrnDelFAM_POD].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_POD].[LearnDelFAMType]=''POD''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_RES]
				on [LrnDelFAM_RES].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_RES].[LearnDelFAMType]=''RES''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_SOF]
				on [LrnDelFAM_SOF].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_SOF].[LearnDelFAMType]=''SOF''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_SPP]
				on [LrnDelFAM_SPP].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_SPP].[LearnDelFAMType]=''SPP''
			left join [Input].[LearningDeliveryFAM] as [LrnDelFAM_WPP]
				on [LrnDelFAM_WPP].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [LrnDelFAM_WPP].[LearnDelFAMType]=''WPP''
			left join [Input].[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_A]
				on [ProvSpecMon_A].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [ProvSpecMon_A].[ProvSpecDelMonOccur]=''A''
			left join [Input].[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_B]
				on [ProvSpecMon_B].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [ProvSpecMon_B].[ProvSpecDelMonOccur]=''B''
			left join [Input].[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_C]
				on [ProvSpecMon_C].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [ProvSpecMon_C].[ProvSpecDelMonOccur]=''C''
			left join [Input].[ProviderSpecDeliveryMonitoring] as [ProvSpecMon_D]
				on [ProvSpecMon_D].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
				and [ProvSpecMon_D].[ProvSpecDelMonOccur]=''D''
			inner join [dbo].[ValidLearners]
				on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]
    		insert into 
			[Valid].[LearningDelivery]
				(
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
					[EPAOrgID],
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
				)
	select * from #LearningDelivery

	IF OBJECT_ID(''tempdb..#LearningDelivery'', ''U'') IS NOT NULL
	DROP TABLE #LearningDelivery;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryFAM]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDeliveryFAM]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearningDeliveryFAM] as
	begin
	IF OBJECT_ID(''tempdb..#LearningDeliveryFAM'', ''U'') IS NOT NULL
	DROP TABLE #LearningDeliveryFAM;

		select
			[Learner].[LearnRefNumber],
			[LearningDelivery].[AimSeqNumber],
			[LearningDeliveryFAM].[LearnDelFAMType],
			[LearningDeliveryFAM].[LearnDelFAMCode],
			[LearningDeliveryFAM].[LearnDelFAMDateFrom],
			[LearningDeliveryFAM].[LearnDelFAMDateTo]
		into #LearningDeliveryFAM
		from
			[Input].[LearningDeliveryFAM]
			inner join [Input].[LearningDelivery]
				on [LearningDeliveryFAM].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			inner join [Input].[Learner]
				on [LearningDelivery].[Learner_Id]=[Learner].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]
		where 
			[LearningDeliveryFAM].[LearnDelFAMType] in (''LSF'',''ALB'', ''ACT'')
        
		insert into 
			[Valid].[LearningDeliveryFAM]
				(
					[LearnRefNumber],
					[AimSeqNumber],
					[LearnDelFAMType],
					[LearnDelFAMCode],
					[LearnDelFAMDateFrom],
					[LearnDelFAMDateTo]
				)
		select * from #LearningDeliveryFAM

	IF OBJECT_ID(''tempdb..#LearningDeliveryFAM'', ''U'') IS NOT NULL
	DROP TABLE #LearningDeliveryFAM;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryHE]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDeliveryHE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearningDeliveryHE] as
	begin
	IF OBJECT_ID(''tempdb..#LearningDeliveryHE'', ''U'') IS NOT NULL
	DROP TABLE #LearningDeliveryHE;

		
		select
			[Learner].[LearnRefNumber],
			[LearningDelivery].[AimSeqNumber],
			[LearningDeliveryHE].[NUMHUS],
			[LearningDeliveryHE].[SSN],
			[LearningDeliveryHE].[QUALENT3],
			[LearningDeliveryHE].[SOC2000],
			[LearningDeliveryHE].[SEC],
			[LearningDeliveryHE].[UCASAPPID],
			[LearningDeliveryHE].[TYPEYR],
			[LearningDeliveryHE].[MODESTUD],
			[LearningDeliveryHE].[FUNDLEV],
			[LearningDeliveryHE].[FUNDCOMP],
			[LearningDeliveryHE].[STULOAD],
			[LearningDeliveryHE].[YEARSTU],
			[LearningDeliveryHE].[MSTUFEE],
			[LearningDeliveryHE].[PCOLAB],
			[LearningDeliveryHE].[PCFLDCS],
			[LearningDeliveryHE].[PCSLDCS],
			[LearningDeliveryHE].[PCTLDCS],
			[LearningDeliveryHE].[SPECFEE],
			[LearningDeliveryHE].[NETFEE],
			[LearningDeliveryHE].[GROSSFEE],
			[LearningDeliveryHE].[DOMICILE],
			[LearningDeliveryHE].[ELQ],
			[LearningDeliveryHE].[HEPostCode]
			into #LearningDeliveryHE
		from
			[Input].[LearningDeliveryHE]
			inner join [Input].[LearningDelivery]
				on [LearningDeliveryHE].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			inner join [Input].[Learner]
				on [LearningDelivery].[Learner_Id]=[Learner].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]
				
			insert into 
			[Valid].[LearningDeliveryHE]
				(
					[LearnRefNumber],
					[AimSeqNumber],
					[NUMHUS],
					[SSN],
					[QUALENT3],
					[SOC2000],
					[SEC],
					[UCASAPPID],
					[TYPEYR],
					[MODESTUD],
					[FUNDLEV],
					[FUNDCOMP],
					[STULOAD],
					[YEARSTU],
					[MSTUFEE],
					[PCOLAB],
					[PCFLDCS],
					[PCSLDCS],
					[PCTLDCS],
					[SPECFEE],
					[NETFEE],
					[GROSSFEE],
					[DOMICILE],
					[ELQ],
					[HEPostCode]
				)
				select * from #LearningDeliveryHE

	IF OBJECT_ID(''tempdb..#LearningDeliveryHE'', ''U'') IS NOT NULL
	DROP TABLE #LearningDeliveryHE;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningDeliveryWorkPlacement]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningDeliveryWorkPlacement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LearningDeliveryWorkPlacement] as
	begin
	IF OBJECT_ID(''tempdb..#LearningDeliveryWorkPlacement'', ''U'') IS NOT NULL
	DROP TABLE #LearningDeliveryWorkPlacement;

	  select
			[Learner].[LearnRefNumber],
			[LearningDelivery].[AimSeqNumber],
			[LearningDeliveryWorkPlacement].[WorkPlaceStartDate],
			[LearningDeliveryWorkPlacement].[WorkPlaceEndDate],
			[LearningDeliveryWorkPlacement].[WorkPlaceMode],
			[LearningDeliveryWorkPlacement].[WorkPlaceEmpId]
			into #LearningDeliveryWorkPlacement
		from
			[Input].[LearningDeliveryWorkPlacement]
			inner join [Input].[LearningDelivery]
				on [LearningDeliveryWorkPlacement].[LearningDelivery_Id]=[LearningDelivery].[LearningDelivery_Id]
			inner join [Input].[Learner]
				on [LearningDelivery].[Learner_Id]=[Learner].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]

	   insert into 
			[Valid].[LearningDeliveryWorkPlacement]
				(
					[LearnRefNumber],
					[AimSeqNumber],
					[WorkPlaceStartDate],
					[WorkPlaceEndDate],
					[WorkPlaceMode],
					[WorkPlaceEmpId]
				)
		select * from #LearningDeliveryWorkPlacement

	IF OBJECT_ID(''tempdb..#LearningDeliveryWorkPlacement'', ''U'') IS NOT NULL
	DROP TABLE #LearningDeliveryWorkPlacement;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LearningProvider]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LearningProvider]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[TransformInputToValid_LearningProvider] as
	begin
		insert into 
			[Valid].[LearningProvider]
				(
					[UKPRN]
				)
		select
			[LearningProvider].[UKPRN]
		from
			[Input].[LearningProvider]
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_LLDDandHealthProblem]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_LLDDandHealthProblem]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_LLDDandHealthProblem] as
	begin
	IF OBJECT_ID(''tempdb..#LLDDandHealthProblem'', ''U'') IS NOT NULL
	DROP TABLE #LLDDandHealthProblem;

		
		select
			[Learner].[LearnRefNumber],
			[LLDDandHealthProblem].[LLDDCat],
			[LLDDandHealthProblem].[PrimaryLLDD]
			into  #LLDDandHealthProblem
		from
			[Input].[LLDDandHealthProblem]
			inner join [Input].[Learner]
				on [LLDDandHealthProblem].[Learner_Id]=[Learner].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [LLDDandHealthProblem].[Learner_Id]=[ValidLearners].[Learner_Id]
    insert into 
			[Valid].[LLDDandHealthProblem]
				(
					[LearnRefNumber],
					[LLDDCat],
					[PrimaryLLDD]
				)
    select * from #LLDDandHealthProblem
	IF OBJECT_ID(''tempdb..#LLDDandHealthProblem'', ''U'') IS NOT NULL
	DROP TABLE #LLDDandHealthProblem;

	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_Source]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_Source]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_Source] as
	begin
	IF OBJECT_ID(''tempdb..#Source'', ''U'') IS NOT NULL
	DROP TABLE #Source;

	 select
			[Source].[ProtectiveMarking],
			[Source].[UKPRN],
			[Source].[SoftwareSupplier],
			[Source].[SoftwarePackage],
			[Source].[Release],
			[Source].[SerialNo],
			[Source].[DateTime],
			[Source].[ReferenceData],
			[Source].[ComponentSetVersion]
		into #Source
		from
			[Input].[Source]

		insert into 
			[Valid].[Source]
				(
					[ProtectiveMarking],
					[UKPRN],
					[SoftwareSupplier],
					[SoftwarePackage],
					[Release],
					[SerialNo],
					[DateTime],
					[ReferenceData],
					[ComponentSetVersion]
				)
        select * from #Source
	IF OBJECT_ID(''tempdb..#Source'', ''U'') IS NOT NULL
	DROP TABLE #Source;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_SourceFile]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_SourceFile]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_SourceFile] as
	begin
	IF OBJECT_ID(''tempdb..#SourceFile'', ''U'') IS NOT NULL
	DROP TABLE #SourceFile;
		
		select
			[SourceFile].[SourceFileName],
			[SourceFile].[FilePreparationDate],
			[SourceFile].[SoftwareSupplier],
			[SourceFile].[SoftwarePackage],
			[SourceFile].[Release],
			[SourceFile].[SerialNo],
			[SourceFile].[DateTime]
		into #SourceFile
		from
			[Input].[SourceFile]
		insert into 
			[Valid].[SourceFile]
				(
					[SourceFileName],
					[FilePreparationDate],
					[SoftwareSupplier],
					[SoftwarePackage],
					[Release],
					[SerialNo],
					[DateTime]
				)
        select * from #SourceFile
	IF OBJECT_ID(''tempdb..#SourceFile'', ''U'') IS NOT NULL
	DROP TABLE #SourceFile;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord] as
	begin
	   IF OBJECT_ID(''tempdb..#TrailblazerApprenticeshipFinancialRecord'', ''U'') IS NOT NULL
	   DROP TABLE #TrailblazerApprenticeshipFinancialRecord;
		
		select
			[Learner].[LearnRefNumber],
			[LearningDelivery].[AimSeqNumber],
			[TrailblazerApprenticeshipFinancialRecord].[TBFinType],
			[TrailblazerApprenticeshipFinancialRecord].[TBFinCode],
			[TrailblazerApprenticeshipFinancialRecord].[TBFinDate],
			[TrailblazerApprenticeshipFinancialRecord].[TBFinAmount]
		into #TrailblazerApprenticeshipFinancialRecord
		from
			[Input].[TrailblazerApprenticeshipFinancialRecord]
			inner join [Input].[LearningDelivery]
				on [LearningDelivery].[LearningDelivery_Id]=[TrailblazerApprenticeshipFinancialRecord].[LearningDelivery_Id]
			inner join [Input].[Learner]
				on [Learner].[Learner_Id]=[LearningDelivery].[Learner_Id]
			inner join [dbo].[ValidLearners]
				on [Learner].[Learner_Id]=[ValidLearners].[Learner_Id]

		insert into 
			[Valid].[TrailblazerApprenticeshipFinancialRecord]
				(
					[LearnRefNumber],
					[AimSeqNumber],
					[TBFinType],
					[TBFinCode],
					[TBFinDate],
					[TBFinAmount]
				)
		select * from #TrailblazerApprenticeshipFinancialRecord
	   IF OBJECT_ID(''tempdb..#TrailblazerApprenticeshipFinancialRecord'', ''U'') IS NOT NULL
	   DROP TABLE #TrailblazerApprenticeshipFinancialRecord;
	end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[TransformInputToValid]    Script Date: 12/09/2016 09:23:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransformInputToValid]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create procedure [dbo].[TransformInputToValid] as
	begin

		exec dbo.TransformInputToValid_CollectionDetails
		exec dbo.TransformInputToValid_Source
		exec dbo.TransformInputToValid_SourceFile
		exec dbo.TransformInputToValid_LearningProvider
		exec dbo.TransformInputToValid_Learner
		exec dbo.TransformInputToValid_ContactPreference
		exec dbo.TransformInputToValid_LLDDandHealthProblem
		exec dbo.TransformInputToValid_LearnerEmploymentStatus
		exec dbo.TransformInputToValid_LearnerHE
		exec dbo.TransformInputToValid_LearnerHEFinancialSupport
		exec dbo.TransformInputToValid_LearningDelivery
		exec dbo.TransformInputToValid_LearningDeliveryFAM
		exec dbo.TransformInputToValid_LearningDeliveryWorkPlacement
		exec dbo.TransformInputToValid_TrailblazerApprenticeshipFinancialRecord
		exec dbo.TransformInputToValid_LearningDeliveryHE
		exec dbo.TransformInputToValid_LearnerContact
	end

' 
END
GO

/****** Transform Input to Valid ******/
EXEC [dbo].[TransformInputToValid]
GO
