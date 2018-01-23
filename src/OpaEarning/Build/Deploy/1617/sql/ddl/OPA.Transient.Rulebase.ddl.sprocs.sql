if object_id('[Rulebase].[AEC_Get_Cases]','p') is not null
	drop procedure [Rulebase].[AEC_Get_Cases]
GO
create procedure [Rulebase].[AEC_Get_Cases] as

	begin
		set nocount on
		select
			CaseData
		from
			[Rulebase].[AEC_Cases]
	end
GO
if object_id('[Rulebase].[AEC_Insert_Cases]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_Cases]
GO
create procedure [Rulebase].[AEC_Insert_Cases] as

	begin
		set nocount on

		insert into
			[Rulebase].[AEC_Cases]
			(
				[LearnRefNumber],
				CaseData
			)
		select
			ControllingTable.[LearnRefNumber],
			convert(xml,
				(
					select
						[LARS_Current_Version].[CurrentVersion] as [@LARSVersion],
						[LearningProvider].[UKPRN] as [@UKPRN],
						'${YearOfCollection}' as [@Year],
						(
							select
								[Learner].[DateOfBirth] as [@DateOfBirth],
								[Learner].[LearnRefNumber] as [@LearnRefNumber],
								[Learner].[PrevUKPRN] as [@PrevUKPRN],
								[Learner].[ULN] as [@ULN],
								(
									select
										[LearningDelivery].[AimSeqNumber] as [@AimSeqNumber],
										[LearningDelivery].[AimType] as [@AimType],
										[LearningDelivery].[CompStatus] as [@CompStatus],
										[LARS_LearningDelivery].[FrameworkCommonComponent] as [@FrameworkCommonComponent],
										[LearningDelivery].[FworkCode] as [@FworkCode],
										[LearningDelivery].[LearnActEndDate] as [@LearnActEndDate],
										[LearningDelivery].[LearnAimRef] as [@LearnAimRef],
										[LearningDelivery].[LearnPlanEndDate] as [@LearnPlanEndDate],
										[LearningDelivery].[LearnStartDate] as [@LearnStartDate],
										[LearningDelivery].[LrnDelFAM_EEF] as [@LrnDelFAM_EEF],
										[LearningDelivery].[OrigLearnStartDate] as [@OrigLearnStartDate],
										[LearningDelivery].[OtherFundAdj] as [@OtherFundAdj],
										[LearningDelivery].[PriorLearnFundAdj] as [@PriorLearnFundAdj],
										[LearningDelivery].[ProgType] as [@ProgType],
										[LearningDelivery].[PwayCode] as [@PwayCode],
										[LearningDelivery].[StdCode] as [@STDCode],
										(
											select
												[TrailblazerApprenticeshipFinancialRecord].[TBFinAmount] as [@TBFinAmount],
												[TrailblazerApprenticeshipFinancialRecord].[TBFinCode] as [@TBFinCode],
												[TrailblazerApprenticeshipFinancialRecord].[TBFinDate] as [@TBFinDate],
												[TrailblazerApprenticeshipFinancialRecord].[TBFinType] as [@TBFinType]
											from
												[Valid].[TrailblazerApprenticeshipFinancialRecord]
											where
												[TrailblazerApprenticeshipFinancialRecord].[LearnRefNumber] = [LearningDelivery].[LearnRefNumber]
												and [TrailblazerApprenticeshipFinancialRecord].[AimSeqNumber] = [LearningDelivery].[AimSeqNumber]
											for xml path ('TrailblazerApprenticeshipFinancialRecord'), type
										),
										(
											select
												[LearningDeliveryFAM].[LearnDelFAMCode] as [@LearnDelFAMCode],
												[LearningDeliveryFAM].[LearnDelFAMDateFrom] as [@LearnDelFAMDateFrom],
												[LearningDeliveryFAM].[LearnDelFAMDateTo] as [@LearnDelFAMDateTo],
												[LearningDeliveryFAM].[LearnDelFAMType] as [@LearnDelFAMType]
											from
												[Valid].[LearningDeliveryFAM]
											where
												[LearningDeliveryFAM].[LearnRefNumber] = [LearningDelivery].[LearnRefNumber]
												and [LearningDeliveryFAM].[AimSeqNumber] = [LearningDelivery].[AimSeqNumber]
											for xml path ('LearningDeliveryFAM'), type
										),
										(
											select
												[LARS_ApprenticeshipFunding].[1618Incentive] as [@StandardAF1618Incentive],
												[LARS_ApprenticeshipFunding].[EffectiveFrom] as [@StandardAFEffectiveFrom],
												[LARS_ApprenticeshipFunding].[EffectiveTo] as [@StandardAFEffectiveTo],
												[LARS_ApprenticeshipFunding].[FundingCategory] as [@StandardAFFundingCategory],
												[LARS_ApprenticeshipFunding].[MaxEmployerLevyCap] as [@StandardAFMaxEmployerLevyCap],
												[LARS_ApprenticeshipFunding].[ReservedValue1] as [@StandardAFReservedValue1],
												[LARS_ApprenticeshipFunding].[ReservedValue2] as [@StandardAFReservedValue2]
											from
												[Reference].[LARS_ApprenticeshipFunding]
											where
												[LARS_ApprenticeshipFunding].[ApprenticeshipType]='STD'
												and [LARS_ApprenticeshipFunding].[ApprenticeshipCode]=[LearningDelivery].[StdCode]
												and [LARS_ApprenticeshipFunding].[ProgType]=[LearningDelivery].[ProgType]
												and [LARS_ApprenticeshipFunding].[PwayCode]=0
											for xml path ('Standard_LARS_ApprenticshipFunding'), type
										),
										(
											select
												[LARS_ApprenticeshipFunding].[1618Incentive] as [@FrameworkAF1618Incentive],
												[LARS_ApprenticeshipFunding].[EffectiveFrom] as [@FrameworkAFEffectiveFrom],
												[LARS_ApprenticeshipFunding].[EffectiveTo] as [@FrameworkAFEffectiveTo],
												[LARS_ApprenticeshipFunding].[FundingCategory] as [@FrameworkAFFundingCategory],
												[LARS_ApprenticeshipFunding].[MaxEmployerLevyCap] as [@FrameworkAFMaxEmployerLevyCap],
												[LARS_ApprenticeshipFunding].[ReservedValue1] as [@FrameworkAFReservedValue1],
												[LARS_ApprenticeshipFunding].[ReservedValue2] as [@FrameworkAFReservedValue2]
											from
												[Reference].[LARS_ApprenticeshipFunding]
											where
												[LARS_ApprenticeshipFunding].[ApprenticeshipType]='FWK'
												and [LARS_ApprenticeshipFunding].[ApprenticeshipCode]=[LearningDelivery].[FworkCode]
												and [LARS_ApprenticeshipFunding].[ProgType]=[LearningDelivery].[ProgType]
												and [LARS_ApprenticeshipFunding].[PwayCode]=[LearningDelivery].[PwayCode]
											for xml path ('Framework_LARS_ApprenticshipFunding'), type
										),
										(
											select
												[LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp].[CommonComponent] as [@LARSFrameworkCommonComponentCode],
												[LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp].[EffectiveFrom] as [@LARSFrameworkCommonComponentEffectiveFrom],
												[LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp].[EffectiveTo] as [@LARSFrameworkCommonComponentEffectiveTo]
											from
												(
													select
														[LARS_FrameworkCmnComp].[CommonComponent],
														[LARS_FrameworkCmnComp].[EffectiveFrom],
														[LARS_FrameworkCmnComp].[EffectiveTo],
														[LARS_LearningDelivery].[LearnAimRef],
														[LARS_FrameworkCmnComp].[FworkCode],
														[LARS_FrameworkCmnComp].[ProgType],
														[LARS_FrameworkCmnComp].[PwayCode]
													from
														[Reference].[LARS_FrameworkCmnComp]
														inner join [Reference].[LARS_LearningDelivery]
															on [LARS_FrameworkCmnComp].[CommonComponent]=[LARS_LearningDelivery].[FrameworkCommonComponent]
												)  as LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp
											where
												[LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp].[LearnAimRef]=[LearningDelivery].[LearnAimRef]
												and [LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp].[FworkCode]=[LearningDelivery].[FworkCode]
												and [LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp].[ProgType]=[LearningDelivery].[ProgType]
												and [LearningDelivery_LARS_LearningDelivery_LARS_FrameworkCmnComp].[PwayCode]=[LearningDelivery].[PwayCode]
											for xml path ('LARS_FrameworkCmnComp'), type
										),
										(
											select
												[LARS_StandardCommonComponent].[CommonComponent] as [@LARSStandardCommonComponentCode],
												[LARS_StandardCommonComponent].[EffectiveFrom] as [@LARSStandardCommonComponentEffectiveFrom],
												[LARS_StandardCommonComponent].[EffectiveTo] as [@LARSStandardCommonComponentEffectiveTo]
											from
												[Reference].[LARS_StandardCommonComponent]
											where
												[LARS_StandardCommonComponent].[StandardCode]=[LearningDelivery].[StdCode]
											for xml path ('LARS_StandardCommonComponent'), type
										),
										(
											select
												[LARS_Funding].[FundingCategory] as [@LARSFundCategory],
												[LARS_Funding].[EffectiveFrom] as [@LARSFundEffectiveFrom],
												[LARS_Funding].[EffectiveTo] as [@LARSFundEffectiveTo],
												[LARS_Funding].[RateWeighted] as [@LARSFundWeightedRate]
											from
												[Reference].[LARS_Funding]
											for xml path ('LearningDeliveryLARS_Funding'), type
										)
									from
										[Valid].[LearningDelivery]
										left join [Reference].[LARS_LearningDelivery]
											on [LARS_LearningDelivery].[LearnAimRef]=[LearningDelivery].[LearnAimRef]
									where
										[LearningDelivery].[LearnRefNumber] = [Learner].[LearnRefNumber]
										and [LearningDelivery].[FundModel]=36
									for xml path ('LearningDelivery'), type
								),
								(
									select
										[LearnerEmploymentStatus].[DateEmpStatApp] as [@DateEmpStatApp],
										[LearnerEmploymentStatus].[EmpId] as [@EmpId],
										[LearnerEmploymentStatus].[EmpStat] as [@EMPStat],
										[LearnerEmploymentStatus].[EmpStatMon_EII] as [@EmpStatMon_EII],
										[LearnerEmploymentStatus].[EmpStatMon_SEM] as [@EmpStatMon_SEM]
									from
										[Valid].[LearnerEmploymentStatus]
									where
										[LearnerEmploymentStatus].[LearnRefNumber] = [Learner].[LearnRefNumber]
									for xml path ('LearnerEmploymentStatus'), type
								),
								(
									select
										[SFA_PostcodeDisadvantage].[EffectiveFrom] as [@DisUpEffectiveFrom],
										[SFA_PostcodeDisadvantage].[EffectiveTo] as [@DisUpEffectiveTo],
										[SFA_PostcodeDisadvantage].[Uplift] as [@DisUplift]
									from
										[Reference].[SFA_PostcodeDisadvantage]
									where
										[SFA_PostcodeDisadvantage].[Postcode]=[Learner].[HomePostcode]
									for xml path ('SFA_PostcodeDisadvantage'), type
								),
								(
									select
										[AEC_LatestInYearEarningHistory].[AppIdentifier] as [@AppIdentifierInput],
										case [AEC_LatestInYearEarningHistory].[AppProgCompletedInTheYearInput] when 1 then 'true' when 0 then 'false' end as [@AppProgCompletedInTheYearInput],
										[AEC_LatestInYearEarningHistory].[CollectionReturnCode] as [@HistoricCollectionReturnInput],
										[AEC_LatestInYearEarningHistory].[CollectionYear] as [@HistoricCollectionYearInput],
										[AEC_LatestInYearEarningHistory].[DaysInYear] as [@HistoricDaysInYearInput],
										[AEC_LatestInYearEarningHistory].[HistoricEffectiveTNPStartDateInput] as [@HistoricEffectiveTNPStartDateInput],
										[AEC_LatestInYearEarningHistory].[FworkCode] as [@HistoricFworkCodeInput],
										case [AEC_LatestInYearEarningHistory].[HistoricLearner1618StartInput] when 1 then 'true' when 0 then 'false' end as [@HistoricLearner1618AtStartInput],
										[AEC_LatestInYearEarningHistory].[LearnRefNumber] as [@HistoricLearnRefNumberInput],
										[AEC_LatestInYearEarningHistory].[ProgrammeStartDateIgnorePathway] as [@HistoricProgrammeStartDateIgnorePathwayInput],
										[AEC_LatestInYearEarningHistory].[ProgrammeStartDateMatchPathway] as [@HistoricProgrammeStartDateMatchPathwayInput],
										[AEC_LatestInYearEarningHistory].[ProgType] as [@HistoricProgTypeInput],
										[AEC_LatestInYearEarningHistory].[PwayCode] as [@HistoricPwayCodeInput],
										[AEC_LatestInYearEarningHistory].[STDCode] as [@HistoricSTDCodeInput],
										[AEC_LatestInYearEarningHistory].[HistoricTNP1Input] as [@HistoricTNP1Input],
										[AEC_LatestInYearEarningHistory].[HistoricTNP2Input] as [@HistoricTNP2Input],
										[AEC_LatestInYearEarningHistory].[HistoricTNP3Input] as [@HistoricTNP3Input],
										[AEC_LatestInYearEarningHistory].[HistoricTNP4Input] as [@HistoricTNP4Input],
										[AEC_LatestInYearEarningHistory].[HistoricTotal1618UpliftPaymentsInTheYearInput] as [@HistoricTotal1618UpliftPaymentsInTheYearInput],
										[AEC_LatestInYearEarningHistory].[TotalProgAimPaymentsInTheYear] as [@HistoricTotalProgAimPaymentsInTheYearInput],
										[AEC_LatestInYearEarningHistory].[UKPRN] as [@HistoricUKPRNInput],
										[AEC_LatestInYearEarningHistory].[ULN] as [@HistoricULNInput],
										[AEC_LatestInYearEarningHistory].[UptoEndDate] as [@HistoricUptoEndDateInput],
										[AEC_LatestInYearEarningHistory].[HistoricVirtualTNP3EndOfTheYearInput] as [@HistoricVirtualTNP3EndofTheYearInput],
										[AEC_LatestInYearEarningHistory].[HistoricVirtualTNP4EndOfTheYearInput] as [@HistoricVirtualTNP4EndofTheYearInput]
									from
										[Reference].[AEC_LatestInYearEarningHistory]
									where 
										[AEC_LatestInYearEarningHistory].[ULN] = [Learner].[ULN]
									for xml path ('HistoricEarningInput'), type
								)
							from
								[Valid].[Learner]
							where
								[Learner].[LearnRefNumber] = [ControllingTable].[LearnRefNumber]
							for xml path ('Learner'), type
						)
					from
						[Valid].[LearningProvider]
cross join [Reference].[LARS_Current_Version]					for xml path ('global'), type
				)
			)
		from
			[Valid].[Learner] ControllingTable
			inner join
				(
					select distinct
						[LearningDelivery].[LearnRefNumber]
					from
						[Valid].[LearningDelivery]
					where
						[LearningDelivery].[FundModel]=36
				) [Filter_LearningDelivery]
				on [Filter_LearningDelivery].[LearnRefNumber]=[ControllingTable].[LearnRefNumber]
	end
GO
if object_id('[Rulebase].[AEC_Insert_global]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_global]
GO

create procedure [Rulebase].[AEC_Insert_global]
	(
		@LARSVersion varchar(100),
		@RulebaseVersion varchar(10),
		@UKPRN int,
		@Year varchar(4)
	)
as
	begin
		set nocount on
		insert into
			[Rulebase].[AEC_global]
		values (
			@UKPRN,
			@LARSVersion,
			@RulebaseVersion,
			@Year
		)
	end
GO
if object_id('[Rulebase].[AEC_Insert_Learner]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_Learner]
GO

create procedure [Rulebase].[AEC_Insert_Learner]
	(
		@LearnRefNumber varchar(12)
	)
as
	begin
		set nocount on
	end
GO
if object_id('[Rulebase].[AEC_Insert_LearningDelivery]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_LearningDelivery]
GO

create procedure [Rulebase].[AEC_Insert_LearningDelivery]
	(
		@LearnRefNumber varchar(12),
		@ActualDaysIL int,
		@ActualNumInstalm int,
		@AdjStartDate date,
		@AgeAtProgStart int,
		@AimSeqNumber int,
		@AppAdjLearnStartDate date,
		@AppAdjLearnStartDateMatchPathway date,
		@ApplicCompDate date,
		@CombinedAdjProp decimal(10,5),
		@Completed bit,
		@DisadvFirstPayment decimal(10,5),
		@DisadvSecondPayment decimal(10,5),
		@DisUpFactAdj decimal(10,4),
		@FirstIncentiveThresholdDate date,
		@FundLineType varchar(60),
		@FundStart bit,
		@InstPerPeriod int,
		@LDApplic1618FrameworkUpliftBalancingPayment decimal(10,5),
		@LDApplic1618FrameworkUpliftBalancingValue decimal(10,5),
		@LDApplic1618FrameworkUpliftCompElement decimal(10,5),
		@LDApplic1618FrameworkUpliftCompletionPayment decimal(10,5),
		@LDApplic1618FRameworkUpliftCompletionValue decimal(10,5),
		@LDApplic1618FrameworkUpliftMonthInstalVal decimal(10,5),
		@LDApplic1618FrameworkUpliftOnProgPayment decimal(10,5),
		@LDApplic1618FrameworkUpliftPrevEarnings decimal(10,5),
		@LDApplic1618FrameworkUpliftPrevEarningsStage1 decimal(10,5),
		@LDApplic1618FrameworkUpliftRemainingAmount decimal(10,5),
		@LDApplic1618FrameworkUpliftTotalActEarnings decimal(10,5),
		@LearnAimRef varchar(8),
		@LearnDel1618AtStart bit,
		@LearnDelAppAccDaysIL int,
		@LearnDelApplicDisadvAmount decimal(10,5),
		@LearnDelApplicEmp1618Incentive decimal(10,5),
		@LearnDelApplicEmpDate date,
		@LearnDelApplicProv1618FrameworkUplift decimal(10,5),
		@LearnDelApplicProv1618Incentive decimal(10,5),
		@LearnDelApplicTot1618Incentive decimal(10,5),
		@LearnDelAppPrevAccDaysIL int,
		@LearnDelContType varchar(50),
		@LearnDelDaysIL int,
		@LearnDelDisadAmount decimal(10,5),
		@LearnDelEligDisadvPayment bit,
		@LearnDelEmpIdFirstAdditionalPaymentThreshold int,
		@LearnDelEmpIdSecondAdditionalPaymentThreshold int,
		@LearnDelFirstEmp1618Pay decimal(10,5),
		@LearnDelFirstProv1618Pay decimal(10,5),
		@LearnDelHistDaysThisApp int,
		@LearnDelHistProgEarnings decimal(10,5),
		@LearnDelInitialFundLineType varchar(60),
		@LearnDelLevyNonPayInd int,
		@LearnDelSecondEmp1618Pay decimal(10,5),
		@LearnDelSecondProv1618Pay decimal(10,5),
		@LearnDelSEMContWaiver bit,
		@LearnDelSFAContribPct decimal(10,5),
		@LearnSuppFund bit,
		@LearnSuppFundCash decimal(10,5),
		@MathEngAimValue decimal(10,5),
		@MathEngBalPayment decimal(10,5),
		@MathEngBalPct decimal(8,5),
		@MathEngOnProgPayment decimal(10,5),
		@MathEngOnProgPct decimal(8,5),
		@OutstandNumOnProgInstalm int,
		@PlannedNumOnProgInstalm int,
		@PlannedTotalDaysIL int,
		@ProgrammeAimBalPayment decimal(10,5),
		@ProgrammeAimCompletionPayment decimal(10,5),
		@ProgrammeAimOnProgPayment decimal(10,5),
		@SecondIncentiveThresholdDate date,
		@ThresholdDays int
	)
as
	begin
		set nocount on
		insert into
			[Rulebase].[AEC_LearningDelivery]
		values (
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
			@DisUpFactAdj,
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
			@LearnDelApplicTot1618Incentive,
			@LearnDelAppPrevAccDaysIL,
			@LearnDelDaysIL,
			@LearnDelDisadAmount,
			@LearnDelEligDisadvPayment,
			@LearnDelEmpIdFirstAdditionalPaymentThreshold,
			@LearnDelEmpIdSecondAdditionalPaymentThreshold,
			@LearnDelHistDaysThisApp,
			@LearnDelHistProgEarnings,
			@LearnDelInitialFundLineType,
			@LearnDelSEMContWaiver,
			@MathEngAimValue,
			@OutstandNumOnProgInstalm,
			@PlannedNumOnProgInstalm,
			@PlannedTotalDaysIL,
			@SecondIncentiveThresholdDate,
			@ThresholdDays
		)
	end
GO
if object_id('[Rulebase].[AEC_Insert_LearningDelivery_PeriodisedValues]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_LearningDelivery_PeriodisedValues]
GO
create procedure [Rulebase].[AEC_Insert_LearningDelivery_PeriodisedValues]
	(
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
	)
as
	begin
		set nocount on
		insert into
			[Rulebase].[AEC_LearningDelivery_PeriodisedValues]
				(
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
				)
		values
			(
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
if object_id('[Rulebase].[AEC_Insert_LearningDelivery_PeriodisedTextValues]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_LearningDelivery_PeriodisedTextValues]
GO
create procedure [Rulebase].[AEC_Insert_LearningDelivery_PeriodisedTextValues]
	(
		@LearnRefNumber varchar(12),
		@AimSeqNumber int,
		@AttributeName varchar(100),
		@Period_1 varchar(255)=null,
		@Period_2 varchar(255)=null,
		@Period_3 varchar(255)=null,
		@Period_4 varchar(255)=null,
		@Period_5 varchar(255)=null,
		@Period_6 varchar(255)=null,
		@Period_7 varchar(255)=null,
		@Period_8 varchar(255)=null,
		@Period_9 varchar(255)=null,
		@Period_10 varchar(255)=null,
		@Period_11 varchar(255)=null,
		@Period_12 varchar(255)=null
	)
as
	begin
		set nocount on
		insert into
			[Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]
				(
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
				)
		values
			(
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
if object_id('[Rulebase].[AEC_PivotTemporals_LearningDelivery]','p') is not null
	drop procedure [Rulebase].[AEC_PivotTemporals_LearningDelivery]
GO
create procedure [Rulebase].[AEC_PivotTemporals_LearningDelivery] as
	begin
		truncate table [Rulebase].[AEC_LearningDelivery_Period]
		insert into
			[Rulebase].[AEC_LearningDelivery_Period]
		select
			LearnRefNumber,
			AimSeqNumber,
			Period,
			max(case AttributeName when 'DisadvFirstPayment' then Value else null end) DisadvFirstPayment,
			max(case AttributeName when 'DisadvSecondPayment' then Value else null end) DisadvSecondPayment,
			null FundLineType,
			max(case AttributeName when 'InstPerPeriod' then Value else null end) InstPerPeriod,
			max(case AttributeName when 'LDApplic1618FrameworkUpliftBalancingPayment' then Value else null end) LDApplic1618FrameworkUpliftBalancingPayment,
			max(case AttributeName when 'LDApplic1618FrameworkUpliftCompletionPayment' then Value else null end) LDApplic1618FrameworkUpliftCompletionPayment,
			max(case AttributeName when 'LDApplic1618FrameworkUpliftOnProgPayment' then Value else null end) LDApplic1618FrameworkUpliftOnProgPayment,
			null LearnDelContType,
			max(case AttributeName when 'LearnDelFirstEmp1618Pay' then Value else null end) LearnDelFirstEmp1618Pay,
			max(case AttributeName when 'LearnDelFirstProv1618Pay' then Value else null end) LearnDelFirstProv1618Pay,
			max(case AttributeName when 'LearnDelLevyNonPayInd' then Value else null end) LearnDelLevyNonPayInd,
			max(case AttributeName when 'LearnDelSecondEmp1618Pay' then Value else null end) LearnDelSecondEmp1618Pay,
			max(case AttributeName when 'LearnDelSecondProv1618Pay' then Value else null end) LearnDelSecondProv1618Pay,
			max(case AttributeName when 'LearnDelSFAContribPct' then Value else null end) LearnDelSFAContribPct,
			max(case AttributeName when 'LearnSuppFund' then Value else null end) LearnSuppFund,
			max(case AttributeName when 'LearnSuppFundCash' then Value else null end) LearnSuppFundCash,
			max(case AttributeName when 'MathEngBalPayment' then Value else null end) MathEngBalPayment,
			max(case AttributeName when 'MathEngBalPct' then Value else null end) MathEngBalPct,
			max(case AttributeName when 'MathEngOnProgPayment' then Value else null end) MathEngOnProgPayment,
			max(case AttributeName when 'MathEngOnProgPct' then Value else null end) MathEngOnProgPct,
			max(case AttributeName when 'ProgrammeAimBalPayment' then Value else null end) ProgrammeAimBalPayment,
			max(case AttributeName when 'ProgrammeAimCompletionPayment' then Value else null end) ProgrammeAimCompletionPayment,
			max(case AttributeName when 'ProgrammeAimOnProgPayment' then Value else null end) ProgrammeAimOnProgPayment
		from
			(
				select
					LearnRefNumber,
					AimSeqNumber,
					AttributeName,
					cast(substring(PeriodValue.Period,8,2) as int) Period,
					PeriodValue.Value
				from
					[Rulebase].[AEC_LearningDelivery_PeriodisedValues]
					unpivot (Value for Period in (Period_1,Period_2,Period_3,Period_4,Period_5,Period_6,Period_7,Period_8,Period_9,Period_10,Period_11,Period_12)) as PeriodValue
			) Bob
		group by
			LearnRefNumber,
			AimSeqNumber,
			Period
		update
			[Rulebase].[AEC_LearningDelivery_Period]
		set
			FundLineType=Vic.FundLineType,
			LearnDelContType=Vic.LearnDelContType
		from
			[Rulebase].[AEC_LearningDelivery_Period]
			inner join
				(
					select
						LearnRefNumber,
						AimSeqNumber,
						Period,
						max(case AttributeName when 'FundLineType' then Value else null end) FundLineType,
						max(case AttributeName when 'LearnDelContType' then Value else null end) LearnDelContType
					from
						(
							select
								LearnRefNumber,
								AimSeqNumber,
								AttributeName,
								cast(substring(PeriodValue.Period,8,2) as int) Period,
								PeriodValue.Value
							from
								[Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]
								unpivot (Value for Period in (Period_1,Period_2,Period_3,Period_4,Period_5,Period_6,Period_7,Period_8,Period_9,Period_10,Period_11,Period_12)) as PeriodValue
						) Bob
					group by
						LearnRefNumber,
						AimSeqNumber,
						Period
				) Vic
					on [AEC_LearningDelivery_Period].LearnRefNumber=Vic.LearnRefNumber
					and [AEC_LearningDelivery_Period].AimSeqNumber=Vic.AimSeqNumber
					and [AEC_LearningDelivery_Period].Period=Vic.Period
	end
GO
if object_id('[Rulebase].[AEC_Insert_HistoricEarningOutput]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_HistoricEarningOutput]
GO

create procedure [Rulebase].[AEC_Insert_HistoricEarningOutput]
	(
		@LearnRefNumber varchar(12),
		@AppIdentifierOutput varchar(10),
		@AppProgCompletedInTheYearOutput bit,
		@HistoricDaysInYearOutput int,
		@HistoricEffectiveTNPStartDateOutput date,
		@HistoricFworkCodeOutput int,
		@HistoricLearner1618AtStartOutput bit,
		@HistoricProgrammeStartDateIgnorePathwayOutput date,
		@HistoricProgrammeStartDateMatchPathwayOutput date,
		@HistoricProgTypeOutput int,
		@HistoricPwayCodeOutput int,
		@HistoricSTDCodeOutput int,
		@HistoricTNP1Output decimal(10,5),
		@HistoricTNP2Output decimal(10,5),
		@HistoricTNP3Output decimal(10,5),
		@HistoricTNP4Output decimal(10,5),
		@HistoricTotal1618UpliftPaymentsInTheYear decimal(10,5),
		@HistoricTotalProgAimPaymentsInTheYear decimal(10,5),
		@HistoricULNOutput bigint,
		@HistoricUptoEndDateOutput date,
		@HistoricVirtualTNP3EndofThisYearOutput decimal(10,5),
		@HistoricVirtualTNP4EndofThisYearOutput decimal(10,5)
	)
as
	begin
		set nocount on
		insert into
			[Rulebase].[AEC_HistoricEarningOutput]
		values (
			@LearnRefNumber,
			@AppIdentifierOutput,
			@AppProgCompletedInTheYearOutput,
			@HistoricDaysInYearOutput,
			@HistoricEffectiveTNPStartDateOutput,
			@HistoricFworkCodeOutput,
			@HistoricLearner1618AtStartOutput,
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
			@HistoricTotalProgAimPaymentsInTheYear,
			@HistoricULNOutput,
			@HistoricUptoEndDateOutput,
			@HistoricVirtualTNP3EndofThisYearOutput,
			@HistoricVirtualTNP4EndofThisYearOutput
		)
	end
GO
if object_id('[Rulebase].[AEC_Insert_ApprenticeshipPriceEpisode]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_ApprenticeshipPriceEpisode]
GO

create procedure [Rulebase].[AEC_Insert_ApprenticeshipPriceEpisode]
	(
		@LearnRefNumber varchar(12),
		@EpisodeEffectiveTNPStartDate date,
		@EpisodeStartDate date,
		@PriceEpisodeActualEndDate date,
		@PriceEpisodeActualInstalments int,
		@PriceEpisodeAimSeqNumber int,
		@PriceEpisodeApplic1618FrameworkUpliftBalancing decimal(10,5),
		@PriceEpisodeApplic1618FrameworkUpliftCompletionPayment decimal(10,5),
		@PriceEpisodeApplic1618FrameworkUpliftOnProgPayment decimal(10,5),
		@PriceEpisodeBalancePayment decimal(10,5),
		@PriceEpisodeBalanceValue decimal(10,5),
		@PriceEpisodeCappedRemainingTNPAmount decimal(10,5),
		@PriceEpisodeCompleted bit,
		@PriceEpisodeCompletionElement decimal(10,5),
		@PriceEpisodeCompletionPayment decimal(10,5),
		@PriceEpisodeContractType varchar(50),
		@PriceEpisodeExpectedTotalMonthlyValue decimal(10,5),
		@PriceEpisodeFirstAdditionalPaymentThresholdDate date,
		@PriceEpisodeFirstDisadvantagePayment decimal(10,5),
		@PriceEpisodeFirstEmp1618Pay decimal(10,5),
		@PriceEpisodeFirstProv1618Pay decimal(10,5),
		@PriceEpisodeFundLineType varchar(60),
		@PriceEpisodeIdentifier varchar(25),
		@PriceEpisodeInstalmentsThisPeriod int,
		@PriceEpisodeInstalmentValue decimal(10,5),
		@PriceEpisodeLevyNonPayInd int,
		@PriceEpisodeLSFCash decimal(10,5),
		@PriceEpisodeOnProgPayment decimal(10,5),
		@PriceEpisodePlannedEndDate date,
		@PriceEpisodePlannedInstalments int,
		@PriceEpisodePreviousEarnings decimal(10,5),
		@PriceEpisodePreviousEarningsSameProvider decimal(10,5),
		@PriceEpisodeRemainingAmountWithinUpperLimit decimal(10,5),
		@PriceEpisodeRemainingTNPAmount decimal(10,5),
		@PriceEpisodeSecondAdditionalPaymentThresholdDate date,
		@PriceEpisodeSecondDisadvantagePayment decimal(10,5),
		@PriceEpisodeSecondEmp1618Pay decimal(10,5),
		@PriceEpisodeSecondProv1618Pay decimal(10,5),
		@PriceEpisodeSFAContribPct decimal(10,5),
		@PriceEpisodeTotalEarnings decimal(10,5),
		@PriceEpisodeTotalTNPPrice decimal(10,5),
		@PriceEpisodeUpperBandLimit decimal(10,5),
		@PriceEpisodeUpperLimitAdjustment decimal(10,5),
		@TNP1 decimal(10,5),
		@TNP2 decimal(10,5),
		@TNP3 decimal(10,5),
		@TNP4 decimal(10,5)
	)
as
	begin
		set nocount on
		insert into
			[Rulebase].[AEC_ApprenticeshipPriceEpisode]
		values (
			@LearnRefNumber,
			@PriceEpisodeIdentifier,
			@EpisodeEffectiveTNPStartDate,
			@EpisodeStartDate,
			@PriceEpisodeActualEndDate,
			@PriceEpisodeActualInstalments,
			@PriceEpisodeAimSeqNumber,
			@PriceEpisodeCappedRemainingTNPAmount,
			@PriceEpisodeCompleted,
			@PriceEpisodeCompletionElement,
			@PriceEpisodeContractType,
			@PriceEpisodeExpectedTotalMonthlyValue,
			@PriceEpisodeFirstAdditionalPaymentThresholdDate,
			@PriceEpisodeFundLineType,
			@PriceEpisodeInstalmentValue,
			@PriceEpisodePlannedEndDate,
			@PriceEpisodePlannedInstalments,
			@PriceEpisodePreviousEarnings,
			@PriceEpisodePreviousEarningsSameProvider,
			@PriceEpisodeRemainingAmountWithinUpperLimit,
			@PriceEpisodeRemainingTNPAmount,
			@PriceEpisodeSecondAdditionalPaymentThresholdDate,
			@PriceEpisodeTotalEarnings,
			@PriceEpisodeTotalTNPPrice,
			@PriceEpisodeUpperBandLimit,
			@PriceEpisodeUpperLimitAdjustment,
			@TNP1,
			@TNP2,
			@TNP3,
			@TNP4
		)
	end
GO
if object_id('[Rulebase].[AEC_Insert_ApprenticeshipPriceEpisode_PeriodisedValues]','p') is not null
	drop procedure [Rulebase].[AEC_Insert_ApprenticeshipPriceEpisode_PeriodisedValues]
GO
create procedure [Rulebase].[AEC_Insert_ApprenticeshipPriceEpisode_PeriodisedValues]
	(
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
	)
as
	begin
		set nocount on
		insert into
			[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
				(
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
				)
		values
			(
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
if object_id('[Rulebase].[AEC_PivotTemporals_ApprenticeshipPriceEpisode]','p') is not null
	drop procedure [Rulebase].[AEC_PivotTemporals_ApprenticeshipPriceEpisode]
GO
create procedure [Rulebase].[AEC_PivotTemporals_ApprenticeshipPriceEpisode] as
	begin
		truncate table [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
		insert into
			[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
		select
			LearnRefNumber,
			PriceEpisodeIdentifier,
			Period,
			max(case AttributeName when 'PriceEpisodeApplic1618FrameworkUpliftBalancing' then Value else null end) PriceEpisodeApplic1618FrameworkUpliftBalancing,
			max(case AttributeName when 'PriceEpisodeApplic1618FrameworkUpliftCompletionPayment' then Value else null end) PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
			max(case AttributeName when 'PriceEpisodeApplic1618FrameworkUpliftOnProgPayment' then Value else null end) PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
			max(case AttributeName when 'PriceEpisodeBalancePayment' then Value else null end) PriceEpisodeBalancePayment,
			max(case AttributeName when 'PriceEpisodeBalanceValue' then Value else null end) PriceEpisodeBalanceValue,
			max(case AttributeName when 'PriceEpisodeCompletionPayment' then Value else null end) PriceEpisodeCompletionPayment,
			max(case AttributeName when 'PriceEpisodeFirstDisadvantagePayment' then Value else null end) PriceEpisodeFirstDisadvantagePayment,
			max(case AttributeName when 'PriceEpisodeFirstEmp1618Pay' then Value else null end) PriceEpisodeFirstEmp1618Pay,
			max(case AttributeName when 'PriceEpisodeFirstProv1618Pay' then Value else null end) PriceEpisodeFirstProv1618Pay,
			max(case AttributeName when 'PriceEpisodeInstalmentsThisPeriod' then Value else null end) PriceEpisodeInstalmentsThisPeriod,
			max(case AttributeName when 'PriceEpisodeLevyNonPayInd' then Value else null end) PriceEpisodeLevyNonPayInd,
			max(case AttributeName when 'PriceEpisodeLSFCash' then Value else null end) PriceEpisodeLSFCash,
			max(case AttributeName when 'PriceEpisodeOnProgPayment' then Value else null end) PriceEpisodeOnProgPayment,
			max(case AttributeName when 'PriceEpisodeSecondDisadvantagePayment' then Value else null end) PriceEpisodeSecondDisadvantagePayment,
			max(case AttributeName when 'PriceEpisodeSecondEmp1618Pay' then Value else null end) PriceEpisodeSecondEmp1618Pay,
			max(case AttributeName when 'PriceEpisodeSecondProv1618Pay' then Value else null end) PriceEpisodeSecondProv1618Pay,
			max(case AttributeName when 'PriceEpisodeSFAContribPct' then Value else null end) PriceEpisodeSFAContribPct
		from
			(
				select
					LearnRefNumber,
					PriceEpisodeIdentifier,
					AttributeName,
					cast(substring(PeriodValue.Period,8,2) as int) Period,
					PeriodValue.Value
				from
					[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
					unpivot (Value for Period in (Period_1,Period_2,Period_3,Period_4,Period_5,Period_6,Period_7,Period_8,Period_9,Period_10,Period_11,Period_12)) as PeriodValue
			) Bob
		group by
			LearnRefNumber,
			PriceEpisodeIdentifier,
			Period
	end
GO
