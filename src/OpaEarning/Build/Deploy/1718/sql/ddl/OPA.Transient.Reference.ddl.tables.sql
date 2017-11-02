if not exists(select schema_id from sys.schemas where name='Reference')	
	exec('create schema [Reference]')
GO

if object_id('[Reference].[ContractAllocation]','u') is not null
	drop table [Reference].[ContractAllocation]
GO

create table [Reference].[ContractAllocation]
(
	[ContractAllocationNumber] varchar(20) not null,
	[LotReference] varchar(100) not null,
	[TenderSpecReference] varchar(100) not null
)
create clustered index IX_ContractAllocation on [Reference].[ContractAllocation]
(
	[ContractAllocationNumber],
	[LotReference],
	[TenderSpecReference]
)
GO

if object_id('[Reference].[EligibilityRule]','u') is not null
	drop table [Reference].[EligibilityRule]
GO

create table [Reference].[EligibilityRule]
(
	[Benefits] bit,
	[LotReference] varchar(100) not null,
	[MaxAge] int,
	[MaxLengthOfUnemployment] int,
	[MaxPriorAttainment] varchar(2),
	[MinAge] int,
	[MinLengthOfUnemployment] int,
	[MinPriorAttainment] varchar(2),
	[TenderSpecificationReference] varchar(100) not null
)
create clustered index IX_EligibilityRule on [Reference].[EligibilityRule]
(
	[LotReference],
	[TenderSpecificationReference]
)
GO

if object_id('[Reference].[EligibilityRuleEmploymentStatus]','u') is not null
	drop table [Reference].[EligibilityRuleEmploymentStatus]
GO

create table [Reference].[EligibilityRuleEmploymentStatus]
(
	[LotReference] varchar(100) not null,
	[TenderSpecificationReference] varchar(100) not null,
	[EmploymentStatusCode] int not null
)
create clustered index IX_EligibilityRuleEmploymentStatus on [Reference].[EligibilityRuleEmploymentStatus]
(
	[LotReference],
	[TenderSpecificationReference]
)
GO

if object_id('[Reference].[EligibilityRuleLocalAuthority]','u') is not null
	drop table [Reference].[EligibilityRuleLocalAuthority]
GO

create table [Reference].[EligibilityRuleLocalAuthority]
(
	[LotReference] varchar(100) not null,
	[TenderSpecificationReference] varchar(100) not null,
	[LocalAuthorityCode] varchar(255) not null
)
create clustered index IX_EligibilityRuleLocalAuthority on [Reference].[EligibilityRuleLocalAuthority]
(
	[LotReference],
	[TenderSpecificationReference]
)
GO

if object_id('[Reference].[EligibilityRuleLocalEnterprisePartnership]','u') is not null
	drop table [Reference].[EligibilityRuleLocalEnterprisePartnership]
GO

create table [Reference].[EligibilityRuleLocalEnterprisePartnership]
(
	[LotReference] varchar(100) not null,
	[TenderSpecificationReference] varchar(100) not null,
	[LocalEnterprisePartnershipCode] varchar(255) not null
)
create clustered index IX_EligibilityRuleLocalEnterprisePartnership on [Reference].[EligibilityRuleLocalEnterprisePartnership]
(
	[LotReference],
	[TenderSpecificationReference]
)
GO

if object_id('[Reference].[EligibilityRuleSectorSubjectAreaLevel]','u') is not null
	drop table [Reference].[EligibilityRuleSectorSubjectAreaLevel]
GO

create table [Reference].[EligibilityRuleSectorSubjectAreaLevel]
(
	[LotReference] varchar(100) not null,
	[TenderSpecificationReference] varchar(100) not null,
	[MaxLevelCode] varchar(1),
	[MinLevelCode] varchar(1),
	[SectorSubjectAreaCode] decimal(5,2)
)
create clustered index IX_EligibilityRuleSectorSubjectAreaLevel on [Reference].[EligibilityRuleSectorSubjectAreaLevel]
(
	[LotReference],
	[TenderSpecificationReference]
)
GO

if object_id('[Reference].[Employers]','u') is not null
	drop table [Reference].[Employers]
GO

create table [Reference].[Employers]
(
	[URN] int not null
)
create clustered index IX_Employers on [Reference].[Employers]
(
	[URN]
)
GO																		
	
if object_id('[Reference].[LARS_ApprenticeshipFunding]','u') is not null
	drop table [Reference].[LARS_ApprenticeshipFunding]
GO

create table [Reference].[LARS_ApprenticeshipFunding]
(	
	[1618Incentive] decimal(10,5),
	[ApprenticeshipCode] int not null,
	[ApprenticeshipType] varchar(50) not null,
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[FundingCategory] varchar(15) not null,
	[MaxEmployerLevyCap] decimal(10,5),
	[ProgType] int not null,
	[PwayCode] int not null,
	[ReservedValue1] decimal(10,5),
	[ReservedValue2] decimal(10,5),
	ReservedValue3 decimal(10,5),
	[1618ProviderAdditionalPayment] decimal(10,5),
	[1618EmployerAdditionalPayment] decimal(10,5),
	[1618FrameworkUplift] decimal(10,5)
)
create clustered index IX_LARS_ApprenticeshipFunding on [Reference].[LARS_ApprenticeshipFunding]
(
	[ApprenticeshipCode],
	[ApprenticeshipType],
	[EffectiveFrom],
	[FundingCategory],
	[ProgType],
	[PwayCode]
)
GO

if object_id('[Reference].[LARS_AnnualValue]','u') is not null
	drop table [Reference].[LARS_AnnualValue]
GO

create table [Reference].[LARS_AnnualValue]
(
	[BasicSkills] int,
	[BasicSkillsParticipation] int,
	[BasicSkillsType] int,
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[FullLevel2EntitlementCategory] int,
	[FullLevel2Percent] decimal(5,2),
	[FullLevel3EntitlementCategory] int,
	[FullLevel3Percent] decimal(5,2),
	[LearnAimRef] varchar(8) not null,
	[SfaApprovalStatus] int
)
create clustered index IX_LARS_AnnualValue on [Reference].[LARS_AnnualValue]
(
	[LearnAimRef]
)
GO

if object_id('[Reference].[LARS_Current_Version]','u') is not null
	drop table [Reference].[LARS_Current_Version]
GO

create table [Reference].[LARS_Current_Version]
(
	[CurrentVersion] varchar(100)
)
GO

if object_id('[Reference].[LARS_Framework]','u') is not null
	drop table [Reference].[LARS_Framework]
GO

create table [Reference].[LARS_Framework]
(
	[EffectiveTo] date,
	[FworkCode] int not null,
	[ProgType] int not null,
	[PwayCode] int not null
)
create clustered index IX_LARS_Framework on [Reference].[LARS_Framework]
(
	[FworkCode],
	[ProgType],
	[PwayCode]
)
GO

if object_id('[Reference].[LARS_FrameworkAims]','u') is not null
	drop table [Reference].[LARS_FrameworkAims]
GO

create table [Reference].[LARS_FrameworkAims]
(
	[EffectiveTo] date,
	[FrameworkComponentType] int,
	[FworkCode] int not null,
	[LearnAimRef] varchar(8) not null,
	[ProgType] int not null,
	[PwayCode] int not null
)
create clustered index IX_LARS_FrameworkAims on [Reference].[LARS_FrameworkAims]
(
	[FworkCode],
	[LearnAimRef],
	[ProgType],
	[PwayCode]
)
GO

if object_id('[Reference].[LARS_FrameworkCmnComp]','u') is not null
	drop table [Reference].[LARS_FrameworkCmnComp]
GO

create table [Reference].[LARS_FrameworkCmnComp]
(
	[CommonComponent] int not null,
	[FworkCode] int not null,
	[ProgType] int not null,
	[PwayCode] int not null,
	[EffectiveFrom] date not null,
	[EffectiveTo] date
)
create clustered index IX_LARS_FrameworkCmnComp on [Reference].[LARS_FrameworkCmnComp]
(
	[CommonComponent],
	[FworkCode],
	[ProgType],
	[PwayCode]
)
GO

if object_id('[Reference].[LARS_LearningDelivery]','u') is not null
	drop table [Reference].[LARS_LearningDelivery]
GO

create table [Reference].[LARS_LearningDelivery]
(
	[AwardOrgAimRef] varchar(50),
	[AwardOrgCode] varchar(20),
	[CreditBasedFwkType] int,
	[EFACOFType] int,
	[EnglandFEHEStatus] char(1),
	[EnglPrscID] int,
	[FrameworkCommonComponent] int,
	[LearnAimRef] varchar(8) not null,
	[LearnAimRefTitle] varchar(254),
	[LearnAimRefType] varchar(4),
	[LearnDirectClassSystemCode1] varchar(12),
	[LearnDirectClassSystemCode2] varchar(12),
	[LearnDirectClassSystemCode3] varchar(12),
	[LearningDeliveryGenre] varchar(3),
	[NotionalNVQLevel] char(1),
	[NotionalNVQLevelv2] varchar(5),
	[RegulatedCreditValue] int,
	[SectorSubjectAreaTier1] decimal(5,2),
	[SectorSubjectAreaTier2] decimal(5,2),
	[UnemployedOnly] int,
	[UnitType] varchar(50),
	[EffectiveFrom] date not null,
	[EffectiveTo] date
)
create clustered index IX_LARS_LearningDelivery on [Reference].[LARS_LearningDelivery]
(
	[LearnAimRef]
)
GO

if object_id('[Reference].[LARS_LearningDeliveryCategory]','u') is not null
	drop table [Reference].[LARS_LearningDeliveryCategory]
GO

create table [Reference].[LARS_LearningDeliveryCategory]
(
	[CategoryRef] int not null,
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[LearnAimRef] varchar(8) not null
)
create clustered index IX_LARS_LearningDeliveryCategory on [Reference].[LARS_LearningDeliveryCategory]
(
	[CategoryRef],
	[EffectiveFrom],
	[LearnAimRef]
)
GO

if object_id('[Reference].[LARS_LearningDeliveryCategory_Children]','u') is not null
	drop table [Reference].[LARS_LearningDeliveryCategory_Children]
GO

create table [Reference].[LARS_LearningDeliveryCategory_Children]
(
	[CategoryRef] int,
	[ParentCategoryRef] int,
	[RootCategoryRef] int
)
create clustered index IX_LARS_LearningDeliveryCategory_Children on [Reference].[LARS_LearningDeliveryCategory_Children]
(
	[CategoryRef]
)
GO

if object_id('[Reference].[LARS_LearningDeliveryCategory_TopMostCategory]','u') is not null
	drop table [Reference].[LARS_LearningDeliveryCategory_TopMostCategory]
GO

create table [Reference].[LARS_LearningDeliveryCategory_TopMostCategory]
(
	[CategoryRef] int,
	[LearnAimRef] varchar(8)
)
create clustered index IX_LARS_LearningDeliveryCategory_TopMostCategory on [Reference].[LARS_LearningDeliveryCategory_TopMostCategory]
(
	[CategoryRef],
	[LearnAimRef]
)
GO

if object_id('[Reference].[LARS_Section96]','u') is not null
	drop table [Reference].[LARS_Section96]
GO

create table [Reference].[LARS_Section96]
(
	[LearnAimRef] varchar(8) not null,
	[Section96ApprovalStatus] int,
	[Section96ReviewDate] date,
	[Section96Valid16to18] int
)
create clustered index IX_LARS_Section96 on [Reference].[LARS_Section96]
(
	[LearnAimRef]
)
GO

if object_id('[Reference].[LARS_Standard]','u') is not null
	drop table [Reference].[LARS_Standard]
GO

create table [Reference].[LARS_Standard]
(
	[EffectiveTo] date,
	[StandardCode] int not null,
	[NotionalEndLevel] varchar(5)
)
create clustered index IX_LARS_Standard on [Reference].[LARS_Standard]
(
	[StandardCode]
)
GO

if object_id('[Reference].[LARS_StandardFunding]','u') is not null
	drop table [Reference].[LARS_StandardFunding]
GO

create table [Reference].[LARS_StandardFunding]
(
	[1618Incentive] decimal(10,5),
	[AchievementIncentive] decimal(10,5),
	[CoreGovContributionCap] decimal(10,5),
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[FundableWithoutEmployer] varchar(50),
	[FundingCategory] varchar(15) not null,
	[SmallBusinessIncentive] decimal(10,5),
	[StandardCode] int not null
)
create clustered index IX_LARS_StandardFunding on [Reference].[LARS_StandardFunding]
(
	[EffectiveFrom],
	[FundingCategory],
	[StandardCode]
)
GO

if object_id('[Reference].[LARS_Validity]','u') is not null
	drop table [Reference].[LARS_Validity]
GO

create table [Reference].[LARS_Validity]
(
	[EndDate] date,
	[LastNewStartDate] date,
	[LearnAimRef] varchar(8) not null,
	[StartDate] date not null,
	[ValidityCategory] varchar(50) not null
)
create clustered index IX_LARS_Validity on [Reference].[LARS_Validity]
(
	[LearnAimRef],
	[StartDate],
	[ValidityCategory]
)
GO

if object_id('[Reference].[ONS_Postcode]','u') is not null
	drop table [Reference].[ONS_Postcode]
GO

create table [Reference].[ONS_Postcode]
(
	[doterm] varchar(6),
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[lep1] varchar(9),
	[lep2] varchar(9),
	[oslaua] varchar(9),
	[pcds] varchar(8) not null
)
create clustered index IX_ONS_Postcode on [Reference].[ONS_Postcode]
(
	[EffectiveFrom],
	[pcds]
)	
GO
					
if object_id('[Reference].[Org_Current_Version]','u') is not null
	drop table [Reference].[Org_Current_Version]
GO

create table [Reference].[Org_Current_Version]
(
	[CurrentVersion] varchar(100)
)
GO

if object_id('[Reference].[Org_Details]','u') is not null
	drop table [Reference].[Org_Details]
GO

create table [Reference].[Org_Details]
(
	[LegalOrgType] varchar(50),
	[ThirdSector] int,
	[UKPRN] bigint not null
)
create clustered index IX_Org_Details on [Reference].[Org_Details]
(
	[UKPRN]
)
GO

if object_id('[Reference].[Org_HMPP_PostCode]','u') is not null
	drop table [Reference].[Org_HMPP_PostCode]
GO

create table [Reference].[Org_HMPP_PostCode]
(
	[EffectiveFrom] date not null,
	[HMPPPostCode] varchar(15) not null,
	[UKPRN] bigint not null
)
create clustered index IX_Org_HMPP_PostCode on [Reference].[Org_HMPP_PostCode]
(
	[EffectiveFrom],
	[HMPPPostCode],
	[UKPRN]
)
GO

if object_id('[Reference].[Org_PartnerUKPRN]','u') is not null
	drop table [Reference].[Org_PartnerUKPRN]
GO

create table [Reference].[Org_PartnerUKPRN]
(
	[UKPRN] bigint not null
)
create clustered index IX_Org_PartnerUKPRN on [Reference].[Org_PartnerUKPRN]
(
	[UKPRN]
)	
GO

if object_id('[Reference].[Postcodes]','u') is not null
	drop table [Reference].[Postcodes]
GO

create table [Reference].[Postcodes]
(
	[Postcode] nvarchar(8) not null
)
create clustered index IX_Postcodes on [Reference].[Postcodes]
(
	[Postcode]
)
GO

if object_id('[Reference].[UniqueLearnerNumbers]','u') is not null
	drop table [Reference].[UniqueLearnerNumbers]
GO

create table [Reference].[UniqueLearnerNumbers]
(
	[ULN] bigint primary key
)
GO

if object_id('[Reference].[vw_ContractAllocation]','u') is not null
	drop table [Reference].[vw_ContractAllocation]
GO

create table [Reference].[vw_ContractAllocation]
(
	[contractAllocationNumber] nvarchar(100),
	[startDate] nvarchar(100)
)
create clustered index IX_vw_ContractAllocation on [Reference].[vw_ContractAllocation]
(
	[contractAllocationNumber]
)
GO

if object_id('[Reference].[vw_ContractValidation]','u') is not null
	drop table [Reference].[vw_ContractValidation]
GO

create table [Reference].[vw_ContractValidation]
(
	[contractAllocationNumber] nvarchar(100),
	[fundingStreamPeriodCode] nvarchar(100),
	[startDate] nvarchar(100),
	[UKPRN] int
)
create clustered index IX_vw_ContractValidation on [Reference].[vw_ContractValidation]
(
	[contractAllocationNumber],
	[fundingStreamPeriodCode],
	[startDate],
	[UKPRN]
)
GO

if object_id('[Reference].[DeliverableCodeMappings]','u') is not null
	drop table [Reference].[DeliverableCodeMappings]
GO

create table [Reference].[DeliverableCodeMappings]
(
	[ExternalDeliverableCode] nvarchar(5),
	[FCSDeliverableCode] bigint,
	[FundingStreamPeriodCode] nvarchar(20)
)
create clustered index IX_DeliverableCodeMappings on [Reference].[DeliverableCodeMappings]
(
	[FCSDeliverableCode],
	[FundingStreamPeriodCode]
)
GO

if object_id('[Reference].[EFA_PostcodeDisadvantage]','u') is not null
	drop table [Reference].[EFA_PostcodeDisadvantage]
GO

create table [Reference].[EFA_PostcodeDisadvantage]
(
	[Postcode] varchar(10) not null,
	[Uplift] decimal(10,5) not null
)
create clustered index IX_EFA_PostcodeDisadvantage on [Reference].[EFA_PostcodeDisadvantage]
(
	[Postcode]
)
GO

if object_id('[Reference].[LargeEmployers]','u') is not null
	drop table [Reference].[LargeEmployers]
GO

create table [Reference].[LargeEmployers]
(
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[ERN] int not null
)
create clustered index IX_LargeEmployers on [Reference].[LargeEmployers]
(
	[EffectiveFrom],
	[ERN]
)
GO

if object_id('[Reference].[LARS_Funding]','u') is not null
	drop table [Reference].[LARS_Funding]
GO

create table [Reference].[LARS_Funding]
(
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[FundingCategory] varchar(15) not null,
	[LearnAimRef] varchar(8) not null,
	[RateUnWeighted] decimal(10,5),
	[RateWeighted] decimal(10,5),
	[WeightingFactor] varchar(1) not null
)
create clustered index IX_LARS_Funding on [Reference].[LARS_Funding]
(
	[EffectiveFrom],
	[FundingCategory],
	[LearnAimRef]
)
GO

if object_id('[Reference].[LARS_StandardCommonComponent]','u') is not null
	drop table [Reference].[LARS_StandardCommonComponent]
GO

create table [Reference].[LARS_StandardCommonComponent]
(
	[CommonComponent] int not null,
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[StandardCode] int not null
)
create clustered index IX_LARS_StandardCommonComponent on [Reference].[LARS_StandardCommonComponent]
(
	[StandardCode]
)
GO

if object_id('[Reference].[Lot]','u') is not null
	drop table [Reference].[Lot]
GO

create table [Reference].[Lot]
(
	[CalcMethod] int,
	[LotReference] varchar(100) not null,
	[TenderSpecificationReference] varchar(100) not null
)
create clustered index IX_Lot on [Reference].[Lot]
(
	[LotReference],
	[TenderSpecificationReference]
)
GO

if object_id('[Reference].[Org_Funding]','u') is not null
	drop table [Reference].[Org_Funding]
GO

create table [Reference].[Org_Funding]
(
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[FundingFactor] varchar(250) not null,
	[FundingFactorType] varchar(100) not null,
	[FundingFactorValue] varchar(250) not null,
	[UKPRN] bigint not null
)
create clustered index IX_Org_Funding on [Reference].[Org_Funding]
(
	[EffectiveFrom],
	[FundingFactor],
	[FundingFactorType],
	[UKPRN]
)
GO

if object_id('[Reference].[SFA_PostcodeAreaCost]','u') is not null
	drop table [Reference].[SFA_PostcodeAreaCost]
GO

create table [Reference].[SFA_PostcodeAreaCost]
(
	[AreaCostFactor] decimal(10,5) not null,
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[Postcode] varchar(10) not null
)
create clustered index IX_SFA_PostcodeAreaCost on [Reference].[SFA_PostcodeAreaCost]
(
	[EffectiveFrom],
	[Postcode]
)
GO

if object_id('[Reference].[SFA_PostcodeDisadvantage]','u') is not null
	drop table [Reference].[SFA_PostcodeDisadvantage]
GO

create table [Reference].[SFA_PostcodeDisadvantage]
(
	[EffectiveFrom] date not null,
	[EffectiveTo] date,
	[Postcode] varchar(10) not null,
	[Uplift] decimal(10,5) not null,
	[Apprenticeship_Uplift] decimal(7,2) null
)
create clustered index IX_SFA_PostcodeDisadvantage on [Reference].[SFA_PostcodeDisadvantage]
(
	[EffectiveFrom],
	[Postcode]
)
GO

if object_id('[Reference].[vw_ContractDescription]','u') is not null
	drop table [Reference].[vw_ContractDescription]
GO

create table [Reference].[vw_ContractDescription]
(
	[contractAllocationNumber] nvarchar(100),
	[contractEndDate] nvarchar(100),
	[contractStartDate] nvarchar(100),
	[deliverableCode] int,
	[fundingStreamPeriodCode] nvarchar(100),
	[learningRatePremiumFactor] decimal(13,2),
	[unitCost] decimal(13,2)
)
create clustered index IX_vw_ContractDescription on [Reference].[vw_ContractDescription]
(
	[contractAllocationNumber],
	[deliverableCode],
	[fundingStreamPeriodCode]
)
GO

if object_id ('Reference.[AEC_LatestInYearEarningHistory]', 'u') is not null
begin
	drop table Reference.AEC_LatestInYearEarningHistory
end
GO

create table Reference.AEC_LatestInYearEarningHistory
(
	[AppIdentifier] [varchar](50) NOT NULL,
	[AppProgCompletedInTheYearInput] [bit] NULL,
	[CollectionYear] [varchar](4) NOT NULL,
	[CollectionReturnCode] [varchar](3) NOT NULL,
	[DaysInYear] [int] NULL,		
	[FworkCode] [int] NULL,
	[HistoricEffectiveTNPStartDateInput] [date] NULL,
	HistoricEmpIdEndWithinYear bigint null,
	HistoricEmpIdStartWithinYear bigint null,
	[HistoricLearner1618StartInput] bit null,
	[HistoricPMRAmount] decimal(12, 5) null,
	[HistoricTNP1Input] decimal(12, 5) null,
	[HistoricTNP2Input] decimal(12,5) null,
	[HistoricTNP3Input] decimal(12,5) null,
	[HistoricTNP4Input] decimal(12,5) null,
	[HistoricTotal1618UpliftPaymentsInTheYearInput] decimal(10,5) null,
	[HistoricVirtualTNP3EndOfTheYearInput] decimal(12, 5) null,
	[HistoricVirtualTNP4EndOfTheYearInput] decimal(12, 5) null,
	LatestInYear bit not null,
	[LearnRefNumber] varchar(12) not null,
	[ProgrammeStartDateIgnorePathway] date null,
	[ProgrammeStartDateMatchPathway] date null,
	[ProgType] int null,
	[PwayCode] int null,	 	
	[STDCode] int null,		
	[TotalProgAimPaymentsInTheYear] decimal(10,5),
	[UptoEndDate] date null,
	[UKPRN] int not null,
	[ULN] bigint not null,
	primary key
	(
		[LatestInYear] DESC,
		[LearnRefNumber] ASC,
		[UKPRN] ASC,
		[CollectionYear] ASC,
		[CollectionReturnCode] ASC,
		[AppIdentifier] ASC
	)
)
GO