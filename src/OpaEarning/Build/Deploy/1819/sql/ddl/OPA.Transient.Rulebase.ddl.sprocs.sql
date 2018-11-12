if object_id('Rulebase.AEC_Get_Cases','p') is not null
begin
	drop procedure Rulebase.AEC_Get_Cases
end
GO

create procedure Rulebase.AEC_Get_Cases as
begin
	set nocount on
	select	CaseData
	from	Rulebase.AEC_Cases
end
GO

if object_id('Rulebase.AEC_Insert_Cases','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_Cases
end
GO

create procedure Rulebase.AEC_Insert_Cases as
begin
	set nocount on

	insert into	Rulebase.AEC_Cases (
		LearnRefNumber,
		CaseData
	)
	select	ControllingTable.[LearnRefNumber],
			convert(xml, (select	LARS_Current_Version.CurrentVersion as [@LARSVersion],
									lp.UKPRN as [@UKPRN],
									'${YearOfCollection}' as [@Year],
									(select l.DateOfBirth as [@DateOfBirth],
											l.LearnRefNumber as [@LearnRefNumber],
											l.PrevUKPRN as [@PrevUKPRN],
											l.PMUKPRN as [@PMUKPRN],
											l.ULN as [@ULN],
											(select	ld.AimSeqNumber as [@AimSeqNumber],
													ld.AimType as [@AimType],
													ld.CompStatus as [@CompStatus],
													lars_ld.FrameworkCommonComponent as [@FrameworkCommonComponent],
													ld.FworkCode as [@FworkCode],
													ld.LearnActEndDate as [@LearnActEndDate],
													ld.LearnAimRef as [@LearnAimRef],
													ld.LearnPlanEndDate as [@LearnPlanEndDate],
													ld.LearnStartDate as [@LearnStartDate],
													ldd.LDFAM_EEF as [@LrnDelFAM_EEF],
													ldd.LDM1 as [@LrnDelFAM_LDM1],
													ldd.LDM2 as [@LrnDelFAM_LDM2],
													ldd.LDM3 as [@LrnDelFAM_LDM3],
													ldd.LDM4 as [@LrnDelFAM_LDM4],
													ld.OrigLearnStartDate as [@OrigLearnStartDate],
													ld.OtherFundAdj as [@OtherFundAdj],
													ld.PriorLearnFundAdj as [@PriorLearnFundAdj],
													ld.ProgType as [@ProgType],
													ld.PwayCode as [@PwayCode],
													ld.StdCode as [@STDCode],
													(select	AFinAmount as [@AFinAmount],
															AFinCode as [@AFinCode],
															AFinDate as [@AFinDate],
															AFinType as [@AFinType]
													from	Valid.AppFinRecord
													where	LearnRefNumber = ld.LearnRefNumber
													and		AimSeqNumber = ld.AimSeqNumber
													for xml path ('ApprenticeshipFinancialRecord'), type),
													(select	LearnDelFAMCode as [@LearnDelFAMCode],
															LearnDelFAMDateFrom as [@LearnDelFAMDateFrom],
															LearnDelFAMDateTo as [@LearnDelFAMDateTo],
															LearnDelFAMType as [@LearnDelFAMType]
													from	Valid.LearningDeliveryFAM
													where	LearnRefNumber = ld.LearnRefNumber
													and		AimSeqNumber = ld.AimSeqNumber
													for xml path ('LearningDeliveryFAM'), type),
													(select	[1618EmployerAdditionalPayment] as [@StandardAF1618EmployerAdditionalPayment],
															[1618FrameworkUplift] as [@StandardAF1618FrameworkUplift],
															[1618ProviderAdditionalPayment] as [@StandardAF1618ProviderAdditionalPayment],
															CareLeaverAdditionalPayment as [@StandardAFCareLeaverAdditionalPayment],
															EffectiveFrom as [@StandardAFEffectiveFrom],
															EffectiveTo as [@StandardAFEffectiveTo],
															FundingCategory as [@StandardAFFundingCategory],
															MaxEmployerLevyCap as [@StandardAFMaxEmployerLevyCap],
															ReservedValue2 as [@StandardAFReservedValue2],
															ReservedValue3 as [@StandardAFReservedValue3]
													from	Reference.LARS_ApprenticeshipFunding
													where	ApprenticeshipType = 'STD'
													and		ApprenticeshipCode = ld.StdCode
													and		ProgType = ld.ProgType
													and		PwayCode = 0
													for xml path ('Standard_LARS_ApprenticshipFunding'), type),
													(select	[1618EmployerAdditionalPayment] as [@FrameworkAF1618EmployerAdditionalPayment],
															[1618FrameworkUplift] as [@FrameworkAF1618FrameworkUplift],
															[1618ProviderAdditionalPayment] as [@FrameworkAF1618ProviderAdditionalPayment],
															CareLeaverAdditionalPayment as [@FrameworkAFCareLeaverAdditionalPayment],
															EffectiveFrom as [@FrameworkAFEffectiveFrom],
															EffectiveTo as [@FrameworkAFEffectiveTo],
															FundingCategory as [@FrameworkAFFundingCategory],
															MaxEmployerLevyCap as [@FrameworkAFMaxEmployerLevyCap],
															ReservedValue2 as [@FrameworkAFReservedValue2],
															ReservedValue3 as [@FrameworkAFReservedValue3]
													from	Reference.LARS_ApprenticeshipFunding
													where	ApprenticeshipType = 'FWK'
													and		ApprenticeshipCode = ld.FworkCode
													and		ProgType = ld.ProgType
													and		PwayCode = ld.PwayCode
													for xml path ('Framework_LARS_ApprenticshipFunding'), type),
													(select	ld_lars_fcc.CommonComponent as [@LARSFrameworkCommonComponentCode],
															ld_lars_fcc.EffectiveFrom as [@LARSFrameworkCommonComponentEffectiveFrom],
															ld_lars_fcc.EffectiveTo as [@LARSFrameworkCommonComponentEffectiveTo]
													from	(select	lars_fcc.CommonComponent,
																	lars_fcc.EffectiveFrom,
																	lars_fcc.EffectiveTo,
																	lars_ld.LearnAimRef,
																	lars_fcc.FworkCode,
																	lars_fcc.ProgType,
																	lars_fcc.PwayCode
															from	Reference.LARS_FrameworkCmnComp as lars_fcc
																		inner join Reference.LARS_LearningDelivery as lars_ld
																			on lars_fcc.CommonComponent = lars_ld.FrameworkCommonComponent
															)  as ld_lars_fcc
													where	ld_lars_fcc.LearnAimRef = ld.LearnAimRef
													and		ld_lars_fcc.FworkCode = ld.FworkCode
													and		ld_lars_fcc.ProgType = ld.ProgType
													and		ld_lars_fcc.PwayCode = ld.PwayCode
													for xml path ('LARS_FrameworkCmnComp'), type),
													(select	CommonComponent as [@LARSStandardCommonComponentCode],
															EffectiveFrom as [@LARSStandardCommonComponentEffectiveFrom],
															EffectiveTo as [@LARSStandardCommonComponentEffectiveTo]
													from	Reference.LARS_StandardCommonComponent
													where	StandardCode = ld.StdCode
													for xml path ('LARS_StandardCommonComponent'), type),
													(select	FundingCategory as [@LARSFundCategory],
															EffectiveFrom as [@LARSFundEffectiveFrom],
															EffectiveTo as [@LARSFundEffectiveTo],
															RateWeighted as [@LARSFundWeightedRate]
													from	Reference.LARS_Funding
													where	LearnAimRef = ld.LearnAimRef
													for xml path ('LearningDeliveryLARS_Funding'), type)
											from	Valid.LearningDelivery as ld
														left join Reference.LARS_LearningDelivery as lars_ld
															on lars_ld.LearnAimRef = ld.LearnAimRef
														join Valid.LearningDeliveryDenorm as ldd
															on ldd.LearnRefNumber = ld.LearnRefNumber
															and ldd.AimSeqNumber = ld.AimSeqNumber
											where	ld.LearnRefNumber = l.LearnRefNumber
											and		ld.FundModel = 36
											for xml path ('LearningDelivery'), type),
											(select	les.DateEmpStatApp as [@DateEmpStatApp],
													les.EmpId as [@EmpId],
													les.EmpStat as [@EMPStat],
													les.AgreeId as [@AgreeId],
													lesd.ESMCode_SEM as [@EmpStatMon_SEM]
											from	Valid.LearnerEmploymentStatus as les
														join Valid.LearnerEmploymentStatusDenorm as lesd
															on lesd.LearnRefNumber = les.LearnRefNumber
															and lesd.DateEmpStatApp = les.DateEmpStatApp
											where	les.LearnRefNumber = l.LearnRefNumber
											for xml path ('LearnerEmploymentStatus'), type),
											(select	EffectiveFrom as [@DisUpEffectiveFrom],
													EffectiveTo as [@DisUpEffectiveTo],
													Apprenticeship_Uplift as [@DisApprenticeshipUplift]
											from	Reference.SFA_PostcodeDisadvantage
											where	Postcode = l.PostcodePrior
											for xml path ('SFA_PostcodeDisadvantage'), type),
											(select	AppIdentifier as [@AppIdentifierInput],
													case AppProgCompletedInTheYearInput 
														when 1 then 'true' 
														when 0 then 'false' 
													end as [@AppProgCompletedInTheYearInput],
													CollectionReturnCode as [@HistoricCollectionReturnInput],
													CollectionYear as [@HistoricCollectionYearInput],
													DaysInYear as [@HistoricDaysInYearInput],
													HistoricEffectiveTNPStartDateInput as [@HistoricEffectiveTNPStartDateInput],
													HistoricEmpIdEndWithinYear as [@HistoricEmpIdEndWithinYearInput],
													HistoricEmpIdStartWithinYear as [@HistoricEmpIdStartWithinYear],
													FworkCode as [@HistoricFworkCodeInput],
													case HistoricLearner1618StartInput 
														when 1 then 'true' 
														when 0 then 'false' 
													end as [@HistoricLearner1618AtStartInput],
													LearnRefNumber as [@HistoricLearnRefNumberInput],
													HistoricPMRAmount as [@HistoricPMRAmountInput],
													ProgrammeStartDateIgnorePathway as [@HistoricProgrammeStartDateIgnorePathwayInput],
													ProgrammeStartDateMatchPathway as [@HistoricProgrammeStartDateMatchPathwayInput],
													ProgType as [@HistoricProgTypeInput],
													PwayCode as [@HistoricPwayCodeInput],
													STDCode as [@HistoricSTDCodeInput],
													HistoricTNP1Input as [@HistoricTNP1Input],
													HistoricTNP2Input as [@HistoricTNP2Input],
													HistoricTNP3Input as [@HistoricTNP3Input],
													HistoricTNP4Input as [@HistoricTNP4Input],
													HistoricTotal1618UpliftPaymentsInTheYearInput as [@HistoricTotal1618UpliftPaymentsInTheYearInput],
													TotalProgAimPaymentsInTheYear as [@HistoricTotalProgAimPaymentsInTheYearInput],
													UKPRN as [@HistoricUKPRNInput],
													ULN as [@HistoricULNInput],
													UptoEndDate as [@HistoricUptoEndDateInput],
													HistoricVirtualTNP3EndOfTheYearInput as [@HistoricVirtualTNP3EndofTheYearInput],
													HistoricVirtualTNP4EndOfTheYearInput as [@HistoricVirtualTNP4EndofTheYearInput],
													HistoricLearnDelProgEarliestACT2DateInput as [@HistoricLearnDelProgEarliestACT2DateInput]
											from	Reference.AEC_LatestInYearEarningHistory
											where	ULN = l.ULN
											for xml path ('HistoricEarningInput'), type)
									from	Valid.Learner as l
									where	l.LearnRefNumber = globalLearner.LearnRefNumber
									for xml path ('Learner'), type)
							from	Valid.Learner as globalLearner
										cross join (select top 1 UKPRN from Valid.LearningProvider) as lp
										cross join Reference.LARS_Current_Version
							where	globalLearner.LearnRefNumber = ControllingTable.LearnRefNumber
							for xml path ('global'), type))
					from	(select	distinct
									LearnRefNumber
							from	Valid.LearningDelivery
							where	FundModel = 36
					) as ControllingTable
end
GO

if object_id('Rulebase.AEC_Insert_global','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_global
end
GO

create procedure Rulebase.AEC_Insert_global (
	@LARSVersion varchar(100),
	@RulebaseVersion varchar(10),
	@UKPRN int,
	@Year varchar(4)
) as
begin
	set nocount on

	insert into Rulebase.AEC_global (
		UKPRN,
		LARSVersion,
		RulebaseVersion,
		[Year]
	) values (
		@UKPRN,
		@LARSVersion,
		@RulebaseVersion,
		@Year
	)
end
GO

if object_id('Rulebase.AEC_Insert_Learner','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_Learner
end
GO

create procedure Rulebase.AEC_Insert_Learner (
	@LearnRefNumber varchar(12)
) as
begin
	set nocount on
end
GO

if object_id('Rulebase.AEC_Insert_LearningDelivery','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_LearningDelivery
end
GO

create procedure Rulebase.AEC_Insert_LearningDelivery (
	@LearnRefNumber varchar(12),
	@ActualDaysIL int = null,
	@ActualNumInstalm int = null,
	@AdjStartDate date = null,
	@AgeAtProgStart int = null,
	@AimSeqNumber int,
	@AppAdjLearnStartDate date = null,
	@AppAdjLearnStartDateMatchPathway date = null,
	@ApplicCompDate date = null,
	@CombinedAdjProp decimal(10,5) = null,
	@Completed bit = null,
	@DisadvFirstPayment decimal(10,5) = null,
	@DisadvSecondPayment decimal(10,5) = null,
	@DisUpFactAdj decimal(10,4) = null,
	@FirstIncentiveThresholdDate date = null,
	@FundLineType varchar(60) = null,
	@FundStart bit = null,
	@InstPerPeriod int = null,
	@LDApplic1618FrameworkUpliftBalancingPayment decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftBalancingValue decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftCompElement decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftCompletionPayment decimal(10,5) = null,
	@LDApplic1618FRameworkUpliftCompletionValue decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftMonthInstalVal decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftOnProgPayment decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftPrevEarnings decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftPrevEarningsStage1 decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftRemainingAmount decimal(10,5) = null,
	@LDApplic1618FrameworkUpliftTotalActEarnings decimal(10,5) = null,
	@LearnAimRef varchar(8),
	@LearnDel1618AtStart bit = null,
	@LearnDelAppAccDaysIL int = null,
	@LearnDelApplicDisadvAmount decimal(10,5) = null,
	@LearnDelApplicEmp1618Incentive decimal(10,5) = null,
	@LearnDelApplicEmpDate date = null,
	@LearnDelApplicProv1618FrameworkUplift decimal(10,5) = null,
	@LearnDelApplicProv1618Incentive decimal(10,5) = null,
	@LearnDelApplicTot1618Incentive decimal(10,5) = null,
	@LearnDelAppPrevAccDaysIL int = null,
	@LearnDelContType varchar(50) = null,
	@LearnDelDaysIL int = null,
	@LearnDelDisadAmount decimal(10,5) = null,
	@LearnDelEligDisadvPayment bit = null,
	@LearnDelEmpIdFirstAdditionalPaymentThreshold int = null,
	@LearnDelEmpIdSecondAdditionalPaymentThreshold int = null,
	@LearnDelFirstEmp1618Pay decimal(10,5) = null,
	@LearnDelFirstProv1618Pay decimal(10,5) = null,
	@LearnDelHistDaysThisApp int = null,
	@LearnDelHistProgEarnings decimal(10,5) = null,
	@LearnDelInitialFundLineType varchar(100) = null,
	@LearnDelLevyNonPayInd int = null,
	@LearnDelSecondEmp1618Pay decimal(10,5) = null,
	@LearnDelSecondProv1618Pay decimal(10,5) = null,
	@LearnDelSEMContWaiver bit = null,
	@LearnDelSFAContribPct decimal(10,5) = null,
	@LearnSuppFund bit = null,
	@LearnSuppFundCash decimal(10,5) = null,
	@MathEngAimValue decimal(10,5) = null,
	@MathEngBalPayment decimal(10,5) = null,
	@MathEngBalPct decimal(8,5) = null,
	@MathEngOnProgPayment decimal(10,5) = null,
	@MathEngOnProgPct decimal(8,5) = null,
	@OutstandNumOnProgInstalm int = null,
	@PlannedNumOnProgInstalm int = null,
	@PlannedTotalDaysIL int = null,
	@ProgrammeAimBalPayment decimal(10,5) = null,
	@ProgrammeAimCompletionPayment decimal(10,5) = null,
	@ProgrammeAimOnProgPayment decimal(10,5) = null,
	@SecondIncentiveThresholdDate date = null,
	@ThresholdDays int = null,
	@LearnDelMathEng bit = null,
	@LearnDelProgEarliestACT2Date date = null,
	@LearnDelNonLevyProcured bit = null,
	@ProgrammeAimTotProgFund decimal(12,5) = null,
	@ProgrammeAimProgFundIndMinCoInvest decimal(12, 5) = null,
	@ProgrammeAimProgFundIndMaxEmpCont decimal(12, 5) = null,
	@LearnDelApplicCareLeaverIncentive decimal(12, 5) = null,
	@LearnDelHistDaysCareLeavers int = null,
	@LearnDelAccDaysILCareLeavers int = null,
	@LearnDelPrevAccDaysILCareLeavers int = null,
	@LearnDelLearnerAddPayThresholdDate	date = null,
	@LearnDelLearnAddPayment decimal(12, 5)	= null, --'periodised'
	@LearnDelRedCode int = null,
	@LearnDelRedStartDate date = null
) as
begin
	set nocount on

	insert into Rulebase.AEC_LearningDelivery (
		LearnRefNumber,
		AimSeqNumber,
		ActualDaysIL,
		ActualNumInstalm,
		AdjStartDate,
		AgeAtProgStart,
		AppAdjLearnStartDate,
		AppAdjLearnStartDateMatchPathway,
		ApplicCompDate,
		CombinedAdjProp,
		Completed,
		FirstIncentiveThresholdDate,
		FundStart,
		LDApplic1618FrameworkUpliftBalancingValue,
		LDApplic1618FrameworkUpliftCompElement,
		LDApplic1618FRameworkUpliftCompletionValue,
		LDApplic1618FrameworkUpliftMonthInstalVal,
		LDApplic1618FrameworkUpliftPrevEarnings,
		LDApplic1618FrameworkUpliftPrevEarningsStage1, 
		LDApplic1618FrameworkUpliftRemainingAmount,
		LDApplic1618FrameworkUpliftTotalActEarnings,
		LearnAimRef,
		LearnDel1618AtStart,
		LearnDelAppAccDaysIL,
		LearnDelApplicDisadvAmount,
		LearnDelApplicEmp1618Incentive,
		LearnDelApplicEmpDate,
		LearnDelApplicProv1618FrameworkUplift,
		LearnDelApplicProv1618Incentive,
		LearnDelAppPrevAccDaysIL,
		LearnDelDaysIL,
		LearnDelDisadAmount,
		LearnDelEligDisadvPayment,
		LearnDelEmpIdFirstAdditionalPaymentThreshold,
		LearnDelEmpIdSecondAdditionalPaymentThreshold,
		LearnDelHistDaysThisApp,
		LearnDelHistProgEarnings,
		LearnDelInitialFundLineType,
		LearnDelMathEng,
		LearnDelProgEarliestACT2Date,
		LearnDelNonLevyProcured,
		MathEngAimValue,
		OutstandNumOnProgInstalm,
		PlannedNumOnProgInstalm,
		PlannedTotalDaysIL,
		SecondIncentiveThresholdDate,
		ThresholdDays,
		LearnDelApplicCareLeaverIncentive,
		LearnDelHistDaysCareLeavers,
		LearnDelAccDaysILCareLeavers,
		LearnDelPrevAccDaysILCareLeavers,
		LearnDelLearnerAddPayThresholdDate,
		LearnDelRedCode,
		LearnDelRedStartDate
	) values (
		@LearnRefNumber,
		@AimSeqNumber,
		@ActualDaysIL,
		@ActualNumInstalm, 
		@AdjStartDate,
		@AgeAtProgStart,
		@AppAdjLearnStartDate,
		@AppAdjLearnStartDateMatchPathway,
		@ApplicCompDate,
		@CombinedAdjProp,
		@Completed,
		@FirstIncentiveThresholdDate,
		@FundStart,
		@LDApplic1618FrameworkUpliftBalancingValue,
		@LDApplic1618FrameworkUpliftCompElement,
		@LDApplic1618FRameworkUpliftCompletionValue,
		@LDApplic1618FrameworkUpliftMonthInstalVal,
		@LDApplic1618FrameworkUpliftPrevEarnings,
		@LDApplic1618FrameworkUpliftPrevEarningsStage1,
		@LDApplic1618FrameworkUpliftRemainingAmount,
		@LDApplic1618FrameworkUpliftTotalActEarnings,
		@LearnAimRef,
		@LearnDel1618AtStart,
		@LearnDelAppAccDaysIL,
		@LearnDelApplicDisadvAmount,
		@LearnDelApplicEmp1618Incentive,
		@LearnDelApplicEmpDate,
		@LearnDelApplicProv1618FrameworkUplift,
		@LearnDelApplicProv1618Incentive,
		@LearnDelAppPrevAccDaysIL,
		@LearnDelDaysIL,
		@LearnDelDisadAmount,
		@LearnDelEligDisadvPayment,
		@LearnDelEmpIdFirstAdditionalPaymentThreshold,
		@LearnDelEmpIdSecondAdditionalPaymentThreshold,
		@LearnDelHistDaysThisApp,
		@LearnDelHistProgEarnings,
		@LearnDelInitialFundLineType,
		@LearnDelMathEng,
		@LearnDelProgEarliestACT2Date,
		@LearnDelNonLevyProcured,
		@MathEngAimValue,
		@OutstandNumOnProgInstalm,
		@PlannedNumOnProgInstalm,
		@PlannedTotalDaysIL,
		@SecondIncentiveThresholdDate,
		@ThresholdDays,
		@LearnDelApplicCareLeaverIncentive,
		@LearnDelHistDaysCareLeavers,
		@LearnDelAccDaysILCareLeavers,
		@LearnDelPrevAccDaysILCareLeavers,
		@LearnDelLearnerAddPayThresholdDate,
		@LearnDelRedCode,
		@LearnDelRedStartDate
	)
end
GO

if object_id('Rulebase.AEC_Insert_LearningDelivery_PeriodisedValues','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_LearningDelivery_PeriodisedValues
end
GO

create procedure Rulebase.AEC_Insert_LearningDelivery_PeriodisedValues (
	@LearnRefNumber varchar(12),
	@AimSeqNumber int,
	@AttributeName varchar(100),
	@Period_1 decimal(15,5),
	@Period_2 decimal(15,5),
	@Period_3 decimal(15,5),
	@Period_4 decimal(15,5),
	@Period_5 decimal(15,5),
	@Period_6 decimal(15,5),
	@Period_7 decimal(15,5),
	@Period_8 decimal(15,5),
	@Period_9 decimal(15,5),
	@Period_10 decimal(15,5),
	@Period_11 decimal(15,5),
	@Period_12 decimal(15,5)
) as
begin
	set nocount on

	insert into Rulebase.AEC_LearningDelivery_PeriodisedValues (
		LearnRefNumber,
		AimSeqNumber,
		AttributeName,
		Period_1,
		Period_2,
		Period_3,
		Period_4,
		Period_5,
		Period_6,
		Period_7,
		Period_8,
		Period_9,
		Period_10,
		Period_11,
		Period_12
	) values (
		@LearnRefNumber,
		@AimSeqNumber,
		@AttributeName,
		@Period_1,
		@Period_2,
		@Period_3,
		@Period_4,
		@Period_5,
		@Period_6,
		@Period_7,
		@Period_8,
		@Period_9,
		@Period_10,
		@Period_11,
		@Period_12
	)
end
GO

if object_id('Rulebase.AEC_Insert_LearningDelivery_PeriodisedTextValues','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_LearningDelivery_PeriodisedTextValues
end
GO

create procedure Rulebase.AEC_Insert_LearningDelivery_PeriodisedTextValues (
	@LearnRefNumber varchar(12),
	@AimSeqNumber int,
	@AttributeName varchar(100),
	@Period_1 varchar(255)	= null,
	@Period_2 varchar(255)	= null,
	@Period_3 varchar(255)	= null,
	@Period_4 varchar(255)	= null,
	@Period_5 varchar(255)	= null,
	@Period_6 varchar(255)	= null,
	@Period_7 varchar(255)	= null,
	@Period_8 varchar(255)	= null,
	@Period_9 varchar(255)	= null,
	@Period_10 varchar(255) = null,
	@Period_11 varchar(255) = null,
	@Period_12 varchar(255) = null
) as
begin
	set nocount on

	insert into Rulebase.AEC_LearningDelivery_PeriodisedTextValues (
		LearnRefNumber,
		AimSeqNumber,
		AttributeName,
		Period_1,
		Period_2,
		Period_3,
		Period_4,
		Period_5,
		Period_6,
		Period_7,
		Period_8,
		Period_9,
		Period_10,
		Period_11,
		Period_12
	) values (
		@LearnRefNumber,
		@AimSeqNumber,
		@AttributeName,
		case @Period_1 when 'Unknown' then null else @Period_1 end,
		case @Period_2 when 'Unknown' then null else @Period_2 end,
		case @Period_3 when 'Unknown' then null else @Period_3 end,
		case @Period_4 when 'Unknown' then null else @Period_4 end,
		case @Period_5 when 'Unknown' then null else @Period_5 end,
		case @Period_6 when 'Unknown' then null else @Period_6 end,
		case @Period_7 when 'Unknown' then null else @Period_7 end,
		case @Period_8 when 'Unknown' then null else @Period_8 end,
		case @Period_9 when 'Unknown' then null else @Period_9 end,
		case @Period_10 when 'Unknown' then null else @Period_10 end,
		case @Period_11 when 'Unknown' then null else @Period_11 end,
		case @Period_12 when 'Unknown' then null else @Period_12 end
	)
end
GO

if object_id('Rulebase.AEC_PivotTemporals_LearningDelivery','p') is not null
begin
	drop procedure Rulebase.AEC_PivotTemporals_LearningDelivery
end
GO

create procedure Rulebase.AEC_PivotTemporals_LearningDelivery as
begin
	-- why is there a truncate here???
	-- truncate table [Rulebase].[AEC_LearningDelivery_Period]

	insert into Rulebase.AEC_LearningDelivery_Period (
		LearnRefNumber,
		AimSeqNumber,
		[Period],
		FundLineType,
		LearnDelContType,
		DisadvFirstPayment,
		DisadvSecondPayment,
		InstPerPeriod,
		LDApplic1618FrameworkUpliftBalancingPayment,
		LDApplic1618FrameworkUpliftCompletionPayment,
		LDApplic1618FrameworkUpliftOnProgPayment,
		LearnDelFirstEmp1618Pay,
		LearnDelFirstProv1618Pay,
		LearnDelLevyNonPayInd,
		LearnDelSecondEmp1618Pay,
		LearnDelSecondProv1618Pay,
		LearnDelSFAContribPct,
		LearnSuppFund,
		LearnSuppFundCash,
		MathEngBalPayment,
		MathEngBalPct,
		MathEngOnProgPayment,
		MathEngOnProgPct,
		ProgrammeAimBalPayment,
		ProgrammeAimCompletionPayment,
		ProgrammeAimOnProgPayment,
		ProgrammeAimProgFundIndMaxEmpCont,
		ProgrammeAimProgFundIndMinCoInvest,
		ProgrammeAimTotProgFund,
		LearnDelLearnAddPayment
	)
	select	LearnRefNumber,
			AimSeqNumber,
			[Period],
			null as FundLineType,
			null as LearnDelContType,
			max(case AttributeName when 'DisadvFirstPayment' then Value else null end) as DisadvFirstPayment,
			max(case AttributeName when 'DisadvSecondPayment' then Value else null end) as DisadvSecondPayment,
			max(case AttributeName when 'InstPerPeriod' then Value else null end) as InstPerPeriod,
			max(case AttributeName when 'LDApplic1618FrameworkUpliftBalancingPayment' then Value else null end) as LDApplic1618FrameworkUpliftBalancingPayment,
			max(case AttributeName when 'LDApplic1618FrameworkUpliftCompletionPayment' then Value else null end) as LDApplic1618FrameworkUpliftCompletionPayment,
			max(case AttributeName when 'LDApplic1618FrameworkUpliftOnProgPayment' then Value else null end) as LDApplic1618FrameworkUpliftOnProgPayment,
			max(case AttributeName when 'LearnDelFirstEmp1618Pay' then Value else null end) as LearnDelFirstEmp1618Pay,
			max(case AttributeName when 'LearnDelFirstProv1618Pay' then Value else null end) as LearnDelFirstProv1618Pay,
			max(case AttributeName when 'LearnDelLevyNonPayInd' then Value else null end) as LearnDelLevyNonPayInd,
			max(case AttributeName when 'LearnDelSecondEmp1618Pay' then Value else null end) as LearnDelSecondEmp1618Pay,
			max(case AttributeName when 'LearnDelSecondProv1618Pay' then Value else null end) as LearnDelSecondProv1618Pay,
			max(case AttributeName when 'LearnDelSFAContribPct' then Value else null end) as LearnDelSFAContribPct,
			max(case AttributeName when 'LearnSuppFund' then Value else null end) as LearnSuppFund,
			max(case AttributeName when 'LearnSuppFundCash' then Value else null end) as LearnSuppFundCash,
			max(case AttributeName when 'MathEngBalPayment' then Value else null end) as MathEngBalPayment,
			max(case AttributeName when 'MathEngBalPct' then Value else null end) as MathEngBalPct,
			max(case AttributeName when 'MathEngOnProgPayment' then Value else null end) as MathEngOnProgPayment,
			max(case AttributeName when 'MathEngOnProgPct' then Value else null end) as MathEngOnProgPct,
			max(case AttributeName when 'ProgrammeAimBalPayment' then Value else null end) as ProgrammeAimBalPayment,
			max(case AttributeName when 'ProgrammeAimCompletionPayment' then Value else null end) as ProgrammeAimCompletionPayment,
			max(case AttributeName when 'ProgrammeAimOnProgPayment' then Value else null end) as ProgrammeAimOnProgPayment,
			max(case AttributeName when 'ProgrammeAimProgFundIndMaxEmpCont' then Value else null end) as ProgrammeAimProgFundIndMaxEmpCont,
			max(case AttributeName when 'ProgrammeAimProgFundIndMinCoInvest' then Value else null end) as ProgrammeAimProgFundIndMinCoInvest,
			max(case AttributeName when 'ProgrammeAimTotProgFund' then Value else null end) as ProgrammeAimTotProgFund,
			max(case AttributeName when 'LearnDelLearnAddPayment' then Value else null end) as LearnDelLearnAddPayment
	from	(select	LearnRefNumber,
					AimSeqNumber,
					AttributeName,
					cast(substring([PeriodValue].[Period], 8, 2) as int) as [Period],
					[PeriodValue].[Value]
			from	Rulebase.AEC_LearningDelivery_PeriodisedValues
						unpivot ([Value] for [Period] in (Period_1, Period_2, Period_3, Period_4, Period_5, Period_6, Period_7, Period_8, Period_9, Period_10, Period_11, Period_12)) as PeriodValue
			) as UnrequiredAlias
	group by
			LearnRefNumber,
			AimSeqNumber,
			[Period]

	update	Rulebase.AEC_LearningDelivery_Period
	set		FundLineType = Vic.FundLineType,
			LearnDelContType = Vic.LearnDelContType
	from	Rulebase.AEC_LearningDelivery_Period
				inner join (select	LearnRefNumber,
									AimSeqNumber,
									[Period],
									max(case AttributeName when 'FundLineType' then [Value] else null end) as FundLineType,
									max(case AttributeName when 'LearnDelContType' then [Value] else null end) as LearnDelContType
							from	(select	LearnRefNumber,
											AimSeqNumber,
											AttributeName,
											cast(substring(PeriodValue.[Period], 8, 2) as int) as [Period],
											PeriodValue.[Value]
									from	Rulebase.AEC_LearningDelivery_PeriodisedTextValues
												unpivot ([Value] for [Period] in (Period_1, Period_2, Period_3, Period_4, Period_5, Period_6, Period_7, Period_8, Period_9, Period_10, Period_11, Period_12)) as PeriodValue
									) as UnrequiredAlias
							group by
									LearnRefNumber,
									AimSeqNumber,
									[Period]
							) as Vic
					on AEC_LearningDelivery_Period.LearnRefNumber = Vic.LearnRefNumber
					and AEC_LearningDelivery_Period.AimSeqNumber = Vic.AimSeqNumber
					and AEC_LearningDelivery_Period.[Period] = Vic.[Period]
end
GO

if object_id('Rulebase.AEC_Insert_HistoricEarningOutput','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_HistoricEarningOutput
end
GO

create procedure Rulebase.AEC_Insert_HistoricEarningOutput (
	@LearnRefNumber varchar(12),
	@AppIdentifierOutput varchar(10),
	@AppProgCompletedInTheYearOutput bit,
	@HistoricDaysInYearOutput int,
	@HistoricEffectiveTNPStartDateOutput date,
	@HistoricEmpIdEndWithinYearOutput bigint,
	@HistoricEmpIdStartWithinYearOutput bigint,
	@HistoricFworkCodeOutput int,
	@HistoricLearner1618AtStartOutput bit,
	@HistoricPMRAmountOutput decimal(12,5),
	@HistoricProgrammeStartDateIgnorePathwayOutput date,
	@HistoricProgrammeStartDateMatchPathwayOutput date,
	@HistoricProgTypeOutput int,
	@HistoricPwayCodeOutput int,
	@HistoricSTDCodeOutput int,
	@HistoricTNP1Output decimal(12,5),
	@HistoricTNP2Output decimal(12,5),
	@HistoricTNP3Output decimal(12,5),
	@HistoricTNP4Output decimal(12,5),
	@HistoricTotal1618UpliftPaymentsInTheYear decimal(10,5),
	@HistoricTotalProgAimPaymentsInTheYear decimal(10,5),
	@HistoricULNOutput bigint,
	@HistoricUptoEndDateOutput date,
	@HistoricVirtualTNP3EndofThisYearOutput decimal(12,5),
	@HistoricVirtualTNP4EndofThisYearOutput decimal(12,5),
	@HistoricLearnDelProgEarliestACT2DateOutput date
) as
begin
	set nocount on

	insert into Rulebase.AEC_HistoricEarningOutput (
		LearnRefNumber,
		AppIdentifierOutput,
		AppProgCompletedInTheYearOutput ,
		HistoricDaysInYearOutput,
		HistoricEffectiveTNPStartDateOutput,
		HistoricEmpIdEndWithinYearOutput ,
		HistoricEmpIdStartWithinYearOutput ,
		HistoricFworkCodeOutput,
		HistoricLearner1618AtStartOutput,
		HistoricPMRAmountOutput,
		HistoricProgrammeStartDateIgnorePathwayOutput,
		HistoricProgrammeStartDateMatchPathwayOutput,
		HistoricProgTypeOutput,
		HistoricPwayCodeOutput,
		HistoricSTDCodeOutput,
		HistoricTNP1Output,
		HistoricTNP2Output,
		HistoricTNP3Output,
		HistoricTNP4Output,
		HistoricTotal1618UpliftPaymentsInTheYear,
		HistoricTotalProgAimPaymentsInTheYear ,
		HistoricULNOutput,
		HistoricUptoEndDateOutput,
		HistoricVirtualTNP3EndofThisYearOutput,
		HistoricVirtualTNP4EndofThisYearOutput,
		HistoricLearnDelProgEarliestACT2DateOutput
	) values (
		@LearnRefNumber,
		@AppIdentifierOutput,
		@AppProgCompletedInTheYearOutput ,
		@HistoricDaysInYearOutput,
		@HistoricEffectiveTNPStartDateOutput,
		@HistoricEmpIdEndWithinYearOutput ,
		@HistoricEmpIdStartWithinYearOutput ,
		@HistoricFworkCodeOutput,
		@HistoricLearner1618AtStartOutput,
		@HistoricPMRAmountOutput,
		@HistoricProgrammeStartDateIgnorePathwayOutput,
		@HistoricProgrammeStartDateMatchPathwayOutput,
		@HistoricProgTypeOutput,
		@HistoricPwayCodeOutput,
		@HistoricSTDCodeOutput,
		@HistoricTNP1Output,
		@HistoricTNP2Output,
		@HistoricTNP3Output,
		@HistoricTNP4Output,
		@HistoricTotal1618UpliftPaymentsInTheYear,
		@HistoricTotalProgAimPaymentsInTheYear ,
		@HistoricULNOutput,
		@HistoricUptoEndDateOutput,
		@HistoricVirtualTNP3EndofThisYearOutput,
		@HistoricVirtualTNP4EndofThisYearOutput,
		@HistoricLearnDelProgEarliestACT2DateOutput
	)
end
GO

if object_id('Rulebase.AEC_Insert_ApprenticeshipPriceEpisode','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_ApprenticeshipPriceEpisode
end
GO

create procedure Rulebase.AEC_Insert_ApprenticeshipPriceEpisode (
	@LearnRefNumber varchar(12),
	@PriceEpisodeIdentifier varchar(25),
	@TNP4 decimal(12,5) = null,
	@TNP1 decimal(12,5) = null,
	@EpisodeStartDate date = null,
	@TNP2 decimal(12, 5) = null,
	@TNP3 decimal(12, 5) = null,
	@PriceEpisodeUpperBandLimit decimal(12, 5) = null,
	@PriceEpisodePlannedEndDate date = null,
	@PriceEpisodeActualEndDate date= null,
	@PriceEpisodeTotalTNPPrice decimal(12, 5) = null,
	@PriceEpisodeUpperLimitAdjustment decimal(12, 5) = null,
	@PriceEpisodePlannedInstalments int = null,
	@PriceEpisodeActualInstalments int = null,
	@PriceEpisodeInstalmentsThisPeriod int = null,
	@PriceEpisodeCompletionElement decimal(12, 5) = null,
	@PriceEpisodePreviousEarnings decimal(12, 5) = null,
	@PriceEpisodeInstalmentValue decimal(12, 5) = null,
	@PriceEpisodeOnProgPayment decimal(12, 5) = null,
	@PriceEpisodeTotalEarnings decimal(12, 5) = null,
	@PriceEpisodeBalanceValue decimal(12, 5) = null,
	@PriceEpisodeBalancePayment decimal(12, 5) = null,
	@PriceEpisodeCompleted bit = null,
	@PriceEpisodeCompletionPayment decimal(12, 5) = null,
	@PriceEpisodeRemainingTNPAmount decimal(12, 5) = null,
	@PriceEpisodeRemainingAmountWithinUpperLimit decimal(12, 5) = null,
	@PriceEpisodeCappedRemainingTNPAmount decimal(12, 5) = null,
	@PriceEpisodeExpectedTotalMonthlyValue decimal(12, 5) = null,
	@PriceEpisodeAimSeqNumber bigint = null,
	@PriceEpisodeFirstDisadvantagePayment decimal(12, 5) = null,
	@PriceEpisodeSecondDisadvantagePayment decimal(12, 5) = null,
	@PriceEpisodeApplic1618FrameworkUpliftBalancing decimal(12, 5) = null,
	@PriceEpisodeApplic1618FrameworkUpliftCompletionPayment decimal(12, 5) = null,
	@PriceEpisodeApplic1618FrameworkUpliftOnProgPayment decimal(12, 5) = null,
	@PriceEpisodeSecondProv1618Pay decimal(12, 5) = null,
	@PriceEpisodeFirstEmp1618Pay decimal(12, 5) = null,
	@PriceEpisodeSecondEmp1618Pay decimal(12, 5) = null,
	@PriceEpisodeFirstProv1618Pay decimal(12, 5) = null,
	@PriceEpisodeLSFCash decimal(12, 5) = null,
	@PriceEpisodeFundLineType varchar(100) = null,
	@PriceEpisodeSFAContribPct decimal(10, 5) = null,
	@PriceEpisodeLevyNonPayInd int = null,
	@EpisodeEffectiveTNPStartDate date = null,
	@PriceEpisodeFirstAdditionalPaymentThresholdDate date = null,
	@PriceEpisodeSecondAdditionalPaymentThresholdDate date = null,
	@PriceEpisodeContractType varchar(50) = null,
	@PriceEpisodePreviousEarningsSameProvider decimal(12, 5) = null,
	@PriceEpisodeTotProgFunding decimal(12, 5) = null,
	@PriceEpisodeProgFundIndMinCoInvest decimal(12, 5) = null,
	@PriceEpisodeProgFundIndMaxEmpCont decimal(12, 5) = null,
	@PriceEpisodeTotalPMRs decimal(12, 5) = null,
	@PriceEpisodeCumulativePMRs decimal(12, 5) = null,
	@PriceEpisodeCompExemCode int = null,
	@PriceEpisodeLearnerAdditionalPaymentThresholdDate date = null,
	@PriceEpisodeLearnerAdditionalPayment decimal(12, 5), --'periodised'
	@PriceEpisodeAgreeId varchar(6) = null,
	@PriceEpisodeRedStartDate date = null,
	@PriceEpisodeRedStatusCode int = null
) as
begin
	set nocount on

	insert into Rulebase.AEC_ApprenticeshipPriceEpisode (
		LearnRefNumber,
		PriceEpisodeIdentifier,
		TNP4,
		TNP1,
		EpisodeStartDate,
		TNP2,
		TNP3,
		PriceEpisodeUpperBandLimit,
		PriceEpisodePlannedEndDate,
		PriceEpisodeActualEndDate,
		PriceEpisodeTotalTNPPrice,
		PriceEpisodeUpperLimitAdjustment,
		PriceEpisodePlannedInstalments,
		PriceEpisodeActualInstalments,
		PriceEpisodeInstalmentsThisPeriod,
		PriceEpisodeCompletionElement,
		PriceEpisodePreviousEarnings,
		PriceEpisodeInstalmentValue,
		PriceEpisodeOnProgPayment,
		PriceEpisodeTotalEarnings,
		PriceEpisodeBalanceValue,
		PriceEpisodeBalancePayment,
		PriceEpisodeCompleted,
		PriceEpisodeCompletionPayment,
		PriceEpisodeRemainingTNPAmount,
		PriceEpisodeRemainingAmountWithinUpperLimit,
		PriceEpisodeCappedRemainingTNPAmount,
		PriceEpisodeExpectedTotalMonthlyValue,
		PriceEpisodeAimSeqNumber,
		PriceEpisodeFirstDisadvantagePayment,
		PriceEpisodeSecondDisadvantagePayment,
		PriceEpisodeApplic1618FrameworkUpliftBalancing,
		PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
		PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
		PriceEpisodeSecondProv1618Pay,
		PriceEpisodeFirstEmp1618Pay,
		PriceEpisodeSecondEmp1618Pay,
		PriceEpisodeFirstProv1618Pay,
		PriceEpisodeLSFCash,
		PriceEpisodeFundLineType,
		PriceEpisodeSFAContribPct,
		PriceEpisodeLevyNonPayInd,
		EpisodeEffectiveTNPStartDate,
		PriceEpisodeFirstAdditionalPaymentThresholdDate,
		PriceEpisodeSecondAdditionalPaymentThresholdDate,
		PriceEpisodeContractType,
		PriceEpisodePreviousEarningsSameProvider,
		PriceEpisodeTotProgFunding,
		PriceEpisodeProgFundIndMinCoInvest,
		PriceEpisodeProgFundIndMaxEmpCont,
		PriceEpisodeTotalPMRs,
		PriceEpisodeCumulativePMRs,
		PriceEpisodeCompExemCode,
		PriceEpisodeLearnerAdditionalPaymentThresholdDate,
		PriceEpisodeAgreeId,
		PriceEpisodeRedStartDate,
		PriceEpisodeRedStatusCode
	) values (
		@LearnRefNumber,
		@PriceEpisodeIdentifier,
		@TNP4,
		@TNP1,
		@EpisodeStartDate,
		@TNP2,
		@TNP3,
		@PriceEpisodeUpperBandLimit,
		@PriceEpisodePlannedEndDate,
		@PriceEpisodeActualEndDate,
		@PriceEpisodeTotalTNPPrice,
		@PriceEpisodeUpperLimitAdjustment,
		@PriceEpisodePlannedInstalments,
		@PriceEpisodeActualInstalments,
		@PriceEpisodeInstalmentsThisPeriod,
		@PriceEpisodeCompletionElement,
		@PriceEpisodePreviousEarnings,
		@PriceEpisodeInstalmentValue,
		@PriceEpisodeOnProgPayment,
		@PriceEpisodeTotalEarnings,
		@PriceEpisodeBalanceValue,
		@PriceEpisodeBalancePayment,
		@PriceEpisodeCompleted,
		@PriceEpisodeCompletionPayment,
		@PriceEpisodeRemainingTNPAmount,
		@PriceEpisodeRemainingAmountWithinUpperLimit,
		@PriceEpisodeCappedRemainingTNPAmount,
		@PriceEpisodeExpectedTotalMonthlyValue,
		@PriceEpisodeAimSeqNumber,
		@PriceEpisodeFirstDisadvantagePayment,
		@PriceEpisodeSecondDisadvantagePayment,
		@PriceEpisodeApplic1618FrameworkUpliftBalancing,
		@PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
		@PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
		@PriceEpisodeSecondProv1618Pay,
		@PriceEpisodeFirstEmp1618Pay,
		@PriceEpisodeSecondEmp1618Pay,
		@PriceEpisodeFirstProv1618Pay,
		@PriceEpisodeLSFCash,
		@PriceEpisodeFundLineType,
		@PriceEpisodeSFAContribPct,
		@PriceEpisodeLevyNonPayInd,
		@EpisodeEffectiveTNPStartDate,
		@PriceEpisodeFirstAdditionalPaymentThresholdDate,
		@PriceEpisodeSecondAdditionalPaymentThresholdDate,
		@PriceEpisodeContractType,
		@PriceEpisodePreviousEarningsSameProvider,
		@PriceEpisodeTotProgFunding,
		@PriceEpisodeProgFundIndMinCoInvest,
		@PriceEpisodeProgFundIndMaxEmpCont,
		@PriceEpisodeTotalPMRs,
		@PriceEpisodeCumulativePMRs,
		@PriceEpisodeCompExemCode,
		@PriceEpisodeLearnerAdditionalPaymentThresholdDate,
		@PriceEpisodeAgreeId,
		@PriceEpisodeRedStartDate,
		@PriceEpisodeRedStatusCode
	)
end
GO


if object_id('Rulebase.AEC_Insert_ApprenticeshipPriceEpisode_PeriodisedValues','p') is not null
begin
	drop procedure Rulebase.AEC_Insert_ApprenticeshipPriceEpisode_PeriodisedValues
end
GO

create procedure Rulebase.AEC_Insert_ApprenticeshipPriceEpisode_PeriodisedValues (
	@LearnRefNumber varchar(12),
	@PriceEpisodeIdentifier varchar(25),
	@AttributeName varchar(100),
	@Period_1 decimal(15,5),
	@Period_2 decimal(15,5),
	@Period_3 decimal(15,5),
	@Period_4 decimal(15,5),
	@Period_5 decimal(15,5),
	@Period_6 decimal(15,5),
	@Period_7 decimal(15,5),
	@Period_8 decimal(15,5),
	@Period_9 decimal(15,5),
	@Period_10 decimal(15,5),
	@Period_11 decimal(15,5),
	@Period_12 decimal(15,5)
) as
begin
	set nocount on

	insert into Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedValues (
		LearnRefNumber,
		PriceEpisodeIdentifier,
		AttributeName,
		Period_1,
		Period_2,
		Period_3,
		Period_4,
		Period_5,
		Period_6,
		Period_7,
		Period_8,
		Period_9,
		Period_10,
		Period_11,
		Period_12
	) values (
		@LearnRefNumber,
		@PriceEpisodeIdentifier,
		@AttributeName,
		@Period_1,
		@Period_2,
		@Period_3,
		@Period_4,
		@Period_5,
		@Period_6,
		@Period_7,
		@Period_8,
		@Period_9,
		@Period_10,
		@Period_11,
		@Period_12
	)
end
GO

if object_id('Rulebase.AEC_PivotTemporals_ApprenticeshipPriceEpisode','p') is not null
begin
	drop procedure Rulebase.AEC_PivotTemporals_ApprenticeshipPriceEpisode
end
GO

create procedure Rulebase.AEC_PivotTemporals_ApprenticeshipPriceEpisode as
begin
	-- why is there a truncate here???
	-- truncate table Rulebase.AEC_ApprenticeshipPriceEpisode_Period

	insert into Rulebase.AEC_ApprenticeshipPriceEpisode_Period (
		LearnRefNumber,
		PriceEpisodeIdentifier,
		[Period],
		PriceEpisodeApplic1618FrameworkUpliftBalancing,
		PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
		PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
		PriceEpisodeBalancePayment,
		PriceEpisodeBalanceValue,
		PriceEpisodeCompletionPayment,
		PriceEpisodeFirstDisadvantagePayment,
		PriceEpisodeFirstEmp1618Pay,
		PriceEpisodeFirstProv1618Pay,
		PriceEpisodeInstalmentsThisPeriod,
		PriceEpisodeLevyNonPayInd,
		PriceEpisodeLSFCash,
		PriceEpisodeOnProgPayment,
		PriceEpisodeSecondDisadvantagePayment,
		PriceEpisodeSecondEmp1618Pay,
		PriceEpisodeSecondProv1618Pay,
		PriceEpisodeSFAContribPct,
		PriceEpisodeProgFundIndMaxEmpCont,
		PriceEpisodeProgFundIndMinCoInvest,
		PriceEpisodeTotProgFunding,
		PriceEpisodeLearnerAdditionalPayment
	)
	select	LearnRefNumber,
			PriceEpisodeIdentifier,
			[Period],
			max(case AttributeName when 'PriceEpisodeApplic1618FrameworkUpliftBalancing' then Value else null end) as PriceEpisodeApplic1618FrameworkUpliftBalancing,
			max(case AttributeName when 'PriceEpisodeApplic1618FrameworkUpliftCompletionPayment' then Value else null end) as PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
			max(case AttributeName when 'PriceEpisodeApplic1618FrameworkUpliftOnProgPayment' then Value else null end) as PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
			max(case AttributeName when 'PriceEpisodeBalancePayment' then Value else null end) as PriceEpisodeBalancePayment,
			max(case AttributeName when 'PriceEpisodeBalanceValue' then Value else null end) as PriceEpisodeBalanceValue,
			max(case AttributeName when 'PriceEpisodeCompletionPayment' then Value else null end) as PriceEpisodeCompletionPayment,
			max(case AttributeName when 'PriceEpisodeFirstDisadvantagePayment' then Value else null end) as PriceEpisodeFirstDisadvantagePayment,
			max(case AttributeName when 'PriceEpisodeFirstEmp1618Pay' then Value else null end) as PriceEpisodeFirstEmp1618Pay,
			max(case AttributeName when 'PriceEpisodeFirstProv1618Pay' then Value else null end) as PriceEpisodeFirstProv1618Pay,
			max(case AttributeName when 'PriceEpisodeInstalmentsThisPeriod' then Value else null end) as PriceEpisodeInstalmentsThisPeriod,
			max(case AttributeName when 'PriceEpisodeLevyNonPayInd' then Value else null end) as PriceEpisodeLevyNonPayInd,
			max(case AttributeName when 'PriceEpisodeLSFCash' then Value else null end) as PriceEpisodeLSFCash,
			max(case AttributeName when 'PriceEpisodeOnProgPayment' then Value else null end) as PriceEpisodeOnProgPayment,
			max(case AttributeName when 'PriceEpisodeSecondDisadvantagePayment' then Value else null end) as PriceEpisodeSecondDisadvantagePayment,
			max(case AttributeName when 'PriceEpisodeSecondEmp1618Pay' then Value else null end) as PriceEpisodeSecondEmp1618Pay,
			max(case AttributeName when 'PriceEpisodeSecondProv1618Pay' then Value else null end) as PriceEpisodeSecondProv1618Pay,
			max(case AttributeName when 'PriceEpisodeSFAContribPct' then Value else null end) as PriceEpisodeSFAContribPct,
			max(case AttributeName when 'PriceEpisodeProgFundIndMaxEmpCont' then Value else null end) as PriceEpisodeProgFundIndMaxEmpCont,
			max(case AttributeName when 'PriceEpisodeProgFundIndMinCoInvest' then Value else null end) as PriceEpisodeProgFundIndMinCoInvest,
			max(case AttributeName when 'PriceEpisodeTotProgFunding' then Value else null end) as PriceEpisodeTotProgFunding,
			max(case AttributeName when 'PriceEpisodeLearnerAdditionalPayment' then Value else null end) as PriceEpisodeLearnerAdditionalPayment
	from	(select	LearnRefNumber,
					PriceEpisodeIdentifier,
					AttributeName,
					cast(substring([PeriodValue].[Period], 8, 2) as int) as [Period],
					[PeriodValue].[Value]
			from	Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedValues
						unpivot ([Value] for [Period] in (Period_1, Period_2, Period_3, Period_4, Period_5, Period_6, Period_7, Period_8, Period_9, Period_10, Period_11, Period_12)) as PeriodValue
			) as UnrequiredAlias
	group by
			LearnRefNumber,
			PriceEpisodeIdentifier,
			[Period]
end
GO
