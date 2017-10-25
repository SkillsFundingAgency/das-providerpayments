truncate table [Reference].[EFA_PostcodeDisadvantage]
	insert into
		[Reference].[EFA_PostcodeDisadvantage]
	select distinct
		[EFA_PostcodeDisadvantage].[Postcode],
		[EFA_PostcodeDisadvantage].[Uplift]
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
		and [Org_Funding].[FundingFactorType]='Adult Skills'
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
		--[SFA_PostcodeDisadvantage].[Apprenticeship_Uplift],
		1 as [Apprenticeship_Uplift],
		[SFA_PostcodeDisadvantage].[EffectiveFrom],
		[SFA_PostcodeDisadvantage].[EffectiveTo],
		[SFA_PostcodeDisadvantage].[Postcode],
		[SFA_PostcodeDisadvantage].[Uplift]
	from
		[Valid].[Learner]
		inner join [${PostcodeFactorsReferenceData.FullyQualified}].[${PostcodeFactorsReferenceData.schemaname}].[SFA_PostcodeDisadvantage]
			on [SFA_PostcodeDisadvantage].[Postcode]=[Learner].[Postcode] 
	go