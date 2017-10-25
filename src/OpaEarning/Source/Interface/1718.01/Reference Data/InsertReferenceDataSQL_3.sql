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
			and [vw_ContractValidation].[fundingStreamPeriodCode] in ('16-18APPS1617','ALLB1617','ALLBC1617','AAC1617','AATO1617','AEBC1617','AEBTO-TOL1617','ESF1420','OLASS1617', '16-18NLA1617', 'ANLA1617', '1618APPS1617', '1618NLA1617', 'ANLA1718', 'ASC1617', 'LEVY1799', 'ASTO1617')

