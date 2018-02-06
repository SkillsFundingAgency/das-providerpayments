 
	truncate table [Reference].[EFA_PostcodeDisadvantage]
	insert into
		[Reference].[EFA_PostcodeDisadvantage]
	select distinct
		[EFA_PostcodeDisadvantage].[Postcode],
		[EFA_PostcodeDisadvantage].[Uplift],
		[EFA_PostcodeDisadvantage].[EffectiveFrom],
		[EFA_PostcodeDisadvantage].[EffectiveTo]
	from
		[Valid].[Learner]
		inner join [${PostcodeFactorsReferenceData.FullyQualified}].[${PostcodeFactorsReferenceData.schemaname}].[EFA_PostcodeDisadvantage]
			on [EFA_PostcodeDisadvantage].[Postcode]=[Learner].[Postcode]
	go
 
	truncate table [Reference].[LargeEmployers]
	insert into
		[Reference].[LargeEmployers]
	select distinct
		[LargeEmployers].[EffectiveFrom],
		[LargeEmployers].[EffectiveTo],
		[LargeEmployers].[ERN]
	from
		[Valid].[LearnerEmploymentStatus]
		inner join [${Large_Employers_Reference_Data.FullyQualified}].[${Large_Employers_Reference_Data.schemaname}].[LargeEmployers]
			on [LargeEmployers].[ERN]=[LearnerEmploymentStatus].[EmpId]
	go
 
	truncate table [Reference].[LARS_Funding]
	insert into
		[Reference].[LARS_Funding]
	select distinct
		[LARS_Funding].[EffectiveFrom],
		[LARS_Funding].[EffectiveTo],
		[LARS_Funding].[FundingCategory],
		[LARS_Funding].[LearnAimRef],
		[LARS_Funding].[RateUnWeighted],
		[LARS_Funding].[RateWeighted],
		[LARS_Funding].[WeightingFactor]
	from
		[Valid].[LearningDelivery]
		inner join [${UoD.FullyQualified}].[${LARS.schemaname}].[LARS_Funding]
			on [LARS_Funding].[LearnAimRef]=[LearningDelivery].[LearnAimRef]
	go
 
	truncate table [Reference].[LARS_StandardCommonComponent]
	insert into
		[Reference].[LARS_StandardCommonComponent]
	select distinct
		[LARS_StandardCommonComponent].[CommonComponent],
		[LARS_StandardCommonComponent].[EffectiveFrom],
		[LARS_StandardCommonComponent].[EffectiveTo],
		[LARS_StandardCommonComponent].[StandardCode]
	from
		[Valid].[LearningDelivery]
		inner join [${UoD.FullyQualified}].[${LARS.schemaname}].[LARS_StandardCommonComponent]
			on [LARS_StandardCommonComponent].[StandardCode]=[LearningDelivery].[StdCode]
	go
	
	truncate table [Reference].[Org_Funding]
	insert into
		[Reference].[Org_Funding]
	select distinct
		[Org_Funding].[EffectiveFrom],
		[Org_Funding].[EffectiveTo],
		[Org_Funding].[FundingFactor],
		[Org_Funding].[FundingFactorType],
		[Org_Funding].[FundingFactorValue],
		[Org_Funding].[UKPRN]
	from
		[Valid].[LearningProvider]
		inner join [${ORG.FullyQualified}].[${Org.Schemaname}].[Org_Funding]
		on [Org_Funding].[UKPRN]=[LearningProvider].[UKPRN]
	
	go
 
	truncate table [Reference].[SFA_PostcodeAreaCost]
	insert into
		[Reference].[SFA_PostcodeAreaCost]
	select distinct
		[SFA_PostcodeAreaCost].[AreaCostFactor],
		[SFA_PostcodeAreaCost].[EffectiveFrom],
		[SFA_PostcodeAreaCost].[EffectiveTo],
		[SFA_PostcodeAreaCost].[Postcode]
	from
		[Valid].[LearningDelivery]
		inner join [${PostcodeFactorsReferenceData.FullyQualified}].[${PostcodeFactorsReferenceData.schemaname}].[SFA_PostcodeAreaCost]
			on [SFA_PostcodeAreaCost].[Postcode]=[LearningDelivery].[DelLocPostCode]
	go
 
	truncate table [Reference].[SFA_PostcodeDisadvantage]
	insert into
		[Reference].[SFA_PostcodeDisadvantage]
		(
			[Apprenticeship_Uplift],
			[EffectiveFrom],
			[EffectiveTo],
			[Postcode],
			[Uplift]
		)
	select distinct
		[SFA_PostcodeDisadvantage].[Apprenticeship_Uplift],
		[SFA_PostcodeDisadvantage].[EffectiveFrom],
		[SFA_PostcodeDisadvantage].[EffectiveTo],
		[SFA_PostcodeDisadvantage].[Postcode],
		[SFA_PostcodeDisadvantage].[Uplift]
	from
		[Valid].[Learner]
		inner join [${PostcodeFactorsReferenceData.FullyQualified}].[${PostcodeFactorsReferenceData.schemaname}].[SFA_PostcodeDisadvantage]
			on [SFA_PostcodeDisadvantage].[Postcode]=[Learner].[PostcodePrior] 
	go

	truncate table [Reference].[DeliverableCodeMappings]
	insert into
		[Reference].[DeliverableCodeMappings] 
		(
			[ExternalDeliverableCode]
			,[FCSDeliverableCode]
			,[FundingStreamPeriodCode]
			,[DeliverableName]
		)
	select distinct
		[DeliverableCodeMappings].[ExternalDeliverableCode],
		[DeliverableCodeMappings].[FCSDeliverableCode],
		[DeliverableCodeMappings].[FundingStreamPeriodCode],
		[DeliverableCodeMappings].[DeliverableName]
	from
		${FCS-Contracts.FQ}.dbo.[DeliverableCodeMappings]
	inner merge join 
		[${FCS-Contracts.servername}].[${FCS-Contracts.databasename}].[dbo].[vw_ContractDescription]
			on [DeliverableCodeMappings].[FundingStreamPeriodCode]=[vw_ContractDescription].[fundingStreamPeriodCode]
			and [DeliverableCodeMappings].[FCSDeliverableCode]=[vw_ContractDescription].[deliverableCode]
	inner join 
		[Valid].[LearningDelivery]
			on[vw_ContractDescription].[contractAllocationNumber]=[LearningDelivery].[ConRefNumber]
	go

	truncate table [Reference].[vw_ContractDescription]
	insert into
		[Reference].[vw_ContractDescription]
		(
			[contractAllocationNumber]
			,[contractEndDate]
			,[contractStartDate]
			,[deliverableCode]
			,[fundingStreamPeriodCode]
			,[learningRatePremiumFactor]
			,[unitCost]
		)
	select distinct
		[vw_ContractDescription].[contractAllocationNumber],
		[vw_ContractDescription].[contractEndDate],
		[vw_ContractDescription].[contractStartDate],
		[vw_ContractDescription].[deliverableCode],
		[vw_ContractDescription].[fundingStreamPeriodCode],
		[vw_ContractDescription].[learningRatePremiumFactor],
		[vw_ContractDescription].[unitCost]
	from
		${FCS-Contracts.FQ}.dbo.[vw_ContractDescription]
	inner merge join 
		[${FCS-Contracts.servername}].[${FCS-Contracts.databasename}].[dbo].[DeliverableCodeMappings]
			on [DeliverableCodeMappings].[FundingStreamPeriodCode]=[vw_ContractDescription].[fundingStreamPeriodCode]
			and [DeliverableCodeMappings].[FCSDeliverableCode]=[vw_ContractDescription].[deliverableCode]
	inner join 
		[Valid].[LearningDelivery]
			on[vw_ContractDescription].[contractAllocationNumber]=[LearningDelivery].[ConRefNumber]
	go


 

	truncate table [Reference].[Employers]
	insert into
		[Reference].[Employers]
	SELECT
	[FromTables].[URN]
	FROM 
	(
		select
			WorkPlaceEmpId 'URN'
		from
			[Input].[LearningDeliveryWorkPlacement]
		union select
			EmpId
		from
			[Input].[LearnerEmploymentStatus]
	) as [FromTables]
	INNER MERGE JOIN
		[${Employers.servername}].[${Employers.databasename}].[dbo].[Employers]
			ON [Employers].[URN]=[FromTables].[URN]
	go

	truncate table [Reference].[UniqueLearnerNumbers]
	insert into
		[Reference].[UniqueLearnerNumbers]
	select fromtables.ULN FROM
    (
        select
            ULN
        from
            [Input].[LearnerDestinationandProgression]
        union select
            ULN
        from
            [Input].[Learner]
    ) as [FromTables]
	inner merge join
	[${ULN.servername}].[${ULN.databasename}].[dbo].[vwULN] [UniqueLearnerNumbers]
            on [UniqueLearnerNumbers].ULN = [FromTables].[ULN]
	go

	truncate table [Reference].[vw_ContractAllocation]
	insert into
		[Reference].[vw_ContractAllocation]
	select distinct
		[vw_ContractAllocation].[contractAllocationNumber],
		[vw_ContractAllocation].[startDate]
	from
		[Input].[LearningDelivery]
		inner join [${FCS-Contracts.servername}].[${FCS-Contracts.databasename}].[dbo].[vw_ContractAllocation]
			on [vw_ContractAllocation].[contractAllocationNumber]=[LearningDelivery].[ConRefNumber]
	go

	truncate table [Reference].[vw_ContractValidation]
	insert into
		[Reference].[vw_ContractValidation]
	select
	[vw_ContractValidation].[contractAllocationNumber],
	[vw_ContractValidation].[fundingStreamPeriodCode],
	[vw_ContractValidation].[startDate],
	[vw_ContractValidation].[UKPRN]
	from
		[Input].[LearningProvider]
		 inner join [${FCS-Contracts.servername}].[${FCS-Contracts.databasename}].[dbo].[vw_ContractValidation]
		on [vw_ContractValidation].[UKPRN]=[LearningProvider].[UKPRN]
			and [vw_ContractValidation].[fundingStreamPeriodCode] in 
			(
				'16-18APPS1617'
				,'ALLB1617'
				,'ALLBC1617'
				,'AAC1617'
				,'AATO1617'
				,'AEBC1617'
				,'AEBTO-TOL1617'
				,'ESF1420'
				,'OLASS1617'
				,'16-18NLA1617'
				,'ANLA1617'
				,'1618APPS1617'
				,'1618NLA1617'
				,'ANLA1718'
				,'ASC1617'
				,'LEVY1799'
				,'ASTO1617'
				,'16-18APPS1718'
				,'16-18NLA1718'
				,'ALLBC1718'
				,'ALLB1718'
				,'AEBTO-TOL1718'
				,'AEBC1718'
				,'AAPP1718'
				,'AEB-CL1718'
				,'AEB-LS1718'
				,'AEB-TOL1718'
			)
	go

	truncate table Reference.AEC_LatestInYearEarningHistory
	insert into Reference.AEC_LatestInYearEarningHistory
	(
		[AppIdentifier]
		,[AppProgCompletedInTheYearInput]
		,[CollectionYear] 
		,[CollectionReturnCode]
		,[DaysInYear]
		,[FworkCode] 
		,[HistoricEffectiveTNPStartDateInput]
		,HistoricEmpIdEndWithinYear
		,HistoricEmpIdStartWithinYear
		,[HistoricLearner1618StartInput]
		,[HistoricPMRAmount]
		,[HistoricTNP1Input]
		,[HistoricTNP2Input] 
		,[HistoricTNP3Input] 
		,[HistoricTNP4Input] 
		,[HistoricTotal1618UpliftPaymentsInTheYearInput]
		,[HistoricVirtualTNP3EndOfTheYearInput]
		,[HistoricVirtualTNP4EndOfTheYearInput]
		,LatestInYear
		,[LearnRefNumber]
		,[ProgrammeStartDateIgnorePathway]
		,[ProgrammeStartDateMatchPathway]
		,[ProgType]
		,[PwayCode]
		,[STDCode] 
		,[TotalProgAimPaymentsInTheYear]
		,[UptoEndDate]
		,[UKPRN]
		,[ULN]
	)
	select
		 aec.[AppIdentifier]
		,aec.[AppProgCompletedInTheYearInput]
		,aec.[CollectionYear] 
		,aec.[CollectionReturnCode]
		,aec.[DaysInYear]
		,aec.[FworkCode] 
		,aec.[HistoricEffectiveTNPStartDateInput]
		,aec.[HistoricEmpIdEndWithinYear]
		,aec.[HistoricEmpIdStartWithinYear]
		,aec.[HistoricLearner1618StartInput]
		,aec.[HistoricPMRAmount]
		,aec.[HistoricTNP1Input]
		,aec.[HistoricTNP2Input] 
		,aec.[HistoricTNP3Input] 
		,aec.[HistoricTNP4Input] 
		,aec.[HistoricTotal1618UpliftPaymentsInTheYearInput]
		,aec.[HistoricVirtualTNP3EndOfTheYearInput]
		,aec.[HistoricVirtualTNP4EndOfTheYearInput]
		,1
		,aec.[LearnRefNumber]
		,aec.[ProgrammeStartDateIgnorePathway]
		,aec.[ProgrammeStartDateMatchPathway]
		,aec.[ProgType]
		,aec.[PwayCode]
		,aec.[STDCode] 
		,aec.[TotalProgAimPaymentsInTheYear]
		,aec.[UptoEndDate]
		,aec.[UKPRN]
		,aec.[ULN]
	from
		Valid.Learner l
		cross join (select top 1 UKPRN from Valid.LearningProvider) as lp
		join
			[${DAS_EarningsHistoryRD.servername}].[${DAS_EarningsHistoryRD.databasename}].[${DAS_EarningsHistoryRD.schemaname}].AEC_LatestInYearEarningHistory as aec
				on aec.LearnRefNumber = l.LearnRefNumber
				and aec.ULN = l.ULN
				and aec.UKPRN = lp.UKPRN
	go