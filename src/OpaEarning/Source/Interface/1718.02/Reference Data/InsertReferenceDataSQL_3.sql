	truncate table [Reference].[UniqueLearnerNumbers]
	insert into
		[Reference].[UniqueLearnerNumbers]
	select fromtables.ULN FROM
    (
            --LearnerDestinationandProgression - UniqueLearnerNumbers
            select
                ULN
            from
                [Input].[LearnerDestinationandProgression]
            --Learner - UniqueLearnerNumbers
            union select
                ULN
            from
                [Input].[Learner]
    ) [FromTables]
	inner merge join
	[${ULN.servername}].[${ULN.databasename}].[dbo].[UniqueLearnerNumbers]
            on [UniqueLearnerNumbers].ULN=CAST([FromTables].[ULN] AS VARCHAR(100))


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
		inner join [${FCS-Contracts.servername}].[${FCS-Contracts.databasename}].[dbo].[vw_ContractValidation]
              on vw_ContractAllocation.contractAllocationNumber=vw_ContractValidation.contractAllocationNumber
		inner join [Input].[LearningProvider]
              on LearningProvider.UKPRN=vw_ContractValidation.UKPRN


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
			and [vw_ContractValidation].[fundingStreamPeriodCode] in ('16-18APPS1718','ALLB1718','ALLBC1718','AEBC1718','AEBTO-TOL1718','ESF1420','16-18NLA1718','ANLA1718','LEVY1799','AAPP1718','16-18TRN1718','AEB-CL1718','AEB-LS1718','AEB-TOL1718')

	GO
			
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


	truncate table [Reference].[Postcodes]
	insert into
		[Reference].[Postcodes]
	 select
		[Postcodes].[Postcode]
	from
		[Input].[PostcodesList]
		inner merge join [${Postcodes.servername}].[${Postcodes.databasename}].[dbo].[Postcodes]
		on [Postcodes].[Postcode]=[PostcodesList].[PostCode]
	go
	
	truncate table [Reference].[Lot]
	insert into
		[Reference].[Lot]
		(
			[CalcMethod]
			,[LotReference]
			,[TenderSpecificationReference]
		)
	select distinct
		[Lot].[CalcMethod],
		[Lot].[LotReference],
		[Lot].[TenderSpecificationReference]
	from
		${ESF_Contract_Reference_Data.FQ}.dbo.[Lot]
		inner merge join 
			[${ESF_Contract_Reference_Data.servername}].[${ESF_Contract_Reference_Data.databasename}].[dbo].[ContractAllocation]
				on [Lot].[TenderSpecificationReference]=[ContractAllocation].[TenderSpecReference]
				and [Lot].[LotReference]=[ContractAllocation].[LotReference]
		inner join 
			[Valid].[LearningDelivery]
				on[ContractAllocation].[ContractAllocationNumber]=[LearningDelivery].[ConRefNumber]
	go

		truncate table [Reference].[ContractAllocation]
	insert into
		[Reference].[ContractAllocation]
	select distinct
		[ContractAllocation].[ContractAllocationNumber],
		[ContractAllocation].[LotReference],
		[ContractAllocation].[TenderSpecReference]
	from
		${ESF_Contract_Reference_Data.FQ}.dbo.[ContractAllocation]
	inner merge join 
		[${ESF_Contract_Reference_Data.servername}].[${ESF_Contract_Reference_Data.databasename}].[dbo].[EligibilityRule]
			on [EligibilityRule].[TenderSpecificationReference]=[ContractAllocation].[TenderSpecReference]
			and [EligibilityRule].[LotReference]=[ContractAllocation].[LotReference]
	inner join
		[Input].[LearningDelivery]
			on[ContractAllocation].[ContractAllocationNumber]=[LearningDelivery].[ConRefNumber]
	go

	truncate table [Reference].[EligibilityRule]
	insert into
		[Reference].[EligibilityRule]
	select distinct
		[EligibilityRule].[Benefits],
		[EligibilityRule].[LotReference],
		[EligibilityRule].[MaxAge],
		[EligibilityRule].[MaxLengthOfUnemployment],
		[EligibilityRule].[MaxPriorAttainment],
		[EligibilityRule].[MinAge],
		[EligibilityRule].[MinLengthOfUnemployment],
		[EligibilityRule].[MinPriorAttainment],
		[EligibilityRule].[TenderSpecificationReference]
	from
		${ESF_Contract_Reference_Data.FQ}.dbo.[EligibilityRule]
	inner merge join 
		[${ESF_Contract_Reference_Data.servername}].[${ESF_Contract_Reference_Data.databasename}].[dbo].[ContractAllocation]
			on [EligibilityRule].[TenderSpecificationReference]=[ContractAllocation].[TenderSpecReference]
			and [EligibilityRule].[LotReference]=[ContractAllocation].[LotReference]
	inner join 
		[Input].[LearningDelivery]
			on[ContractAllocation].[ContractAllocationNumber]=[LearningDelivery].[ConRefNumber]
	go
 
	truncate table [Reference].[EligibilityRuleEmploymentStatus]
	insert into
		[Reference].[EligibilityRuleEmploymentStatus]
	select distinct
		[LotReference],
		[TenderSpecificationReference],
		[EligibilityRuleEmploymentStatus].[EmploymentStatusCode]
	from
		${ESF_Contract_Reference_Data.FQ}.dbo.[EligibilityRuleEmploymentStatus]
	go
 
	truncate table [Reference].[EligibilityRuleLocalAuthority]
	insert into
		[Reference].[EligibilityRuleLocalAuthority]
	select distinct
		[LotReference],
		[TenderSpecificationReference],
		[EligibilityRuleLocalAuthority].[LocalAuthorityCode]
	from
		${ESF_Contract_Reference_Data.FQ}.dbo.[EligibilityRuleLocalAuthority]
	go
 
	truncate table [Reference].[EligibilityRuleLocalEnterprisePartnership]
	insert into
		[Reference].[EligibilityRuleLocalEnterprisePartnership]
	select distinct
		[LotReference],
		[TenderSpecificationReference],
		[EligibilityRuleLocalEnterprisePartnership].[LocalEnterprisePartnershipCode]
	from
		${ESF_Contract_Reference_Data.FQ}.dbo.[EligibilityRuleLocalEnterprisePartnership]
	go
 
	truncate table [Reference].[EligibilityRuleSectorSubjectAreaLevel]
	insert into
		[Reference].[EligibilityRuleSectorSubjectAreaLevel]
	select distinct
		[LotReference],
		[TenderSpecificationReference],
		[EligibilityRuleSectorSubjectAreaLevel].[MaxLevelCode],
		[EligibilityRuleSectorSubjectAreaLevel].[MinLevelCode],
		[EligibilityRuleSectorSubjectAreaLevel].[SectorSubjectAreaCode]
	from
		${ESF_Contract_Reference_Data.FQ}.dbo.[EligibilityRuleSectorSubjectAreaLevel]
	go

	truncate table [Reference].[ONS_Postcode]
	insert into
		[Reference].[ONS_Postcode]
	select distinct
		[ONS_Postcode].[doterm],
		[ONS_Postcode].[EffectiveFrom],
		[ONS_Postcode].[EffectiveTo],
		[ONS_Postcode].[lep1],
		[ONS_Postcode].[lep2],
		[ONS_Postcode].[oslaua],
		[ONS_Postcode].[pcds]
	from
		[Input].[LearningDelivery]
		inner merge join 
			[${ONS_Postcode_Directory.servername}].[${ONS_Postcode_Directory.databasename}].[dbo].[ONS_Postcode]
				on [ONS_Postcode].[pcds]=[LearningDelivery].[DelLocPostCode]
	go
