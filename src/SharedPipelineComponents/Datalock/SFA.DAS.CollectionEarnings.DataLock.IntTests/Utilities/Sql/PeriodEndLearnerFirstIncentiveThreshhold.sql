-- Learning provider
INSERT [Valid].[LearningProvider] ([UKPRN]) VALUES (10007459)
INSERT [dbo].[FileDetails] ([UKPRN],[SubmittedTime]) VALUES (10007459, GETDATE())

-- Learner
INSERT [Valid].[Learner] ([UKPRN], [LearnRefNumber], [PrevLearnRefNumber], [PrevUKPRN], [ULN], [FamilyName], [GivenNames], [DateOfBirth], [Ethnicity], [Sex], [LLDDHealthProb], [NINumber], [PriorAttain], [Accom], [ALSCost], [PlanLearnHours], [PlanEEPHours], [MathGrade], [EngGrade], [HomePostcode], [CurrentPostcode], [LrnFAM_DLA], [LrnFAM_ECF], [LrnFAM_EDF1], [LrnFAM_EDF2], [LrnFAM_EHC], [LrnFAM_FME], [LrnFAM_HNS], [LrnFAM_LDA], [LrnFAM_LSR1], [LrnFAM_LSR2], [LrnFAM_LSR3], [LrnFAM_LSR4], [LrnFAM_MCF], [LrnFAM_NLM1], [LrnFAM_NLM2], [LrnFAM_PPE1], [LrnFAM_PPE2], [LrnFAM_SEN], [ProvSpecMon_A], [ProvSpecMon_B]) VALUES (10007459, N'2', NULL, NULL, 1000000019, N'FamilyName', N'GivenNames', CAST(N'1985-10-10' AS Date), 98, N'F', 2, N'AB123456C', 1, NULL, NULL, NULL, NULL, NULL, NULL, N'OX17 1EZ', N'OX17 1EZ', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)


-- Learning Delivery
INSERT [Valid].[LearningDelivery] ([UKPRN], [LearnRefNumber], [LearnAimRef], [AimType], [AimSeqNumber], [LearnStartDate], [OrigLearnStartDate], [LearnPlanEndDate], [FundModel], [ProgType], [FworkCode], [PwayCode], [StdCode], [PartnerUKPRN], [DelLocPostCode], [AddHours], [PriorLearnFundAdj], [OtherFundAdj], [ConRefNumber], [EmpOutcome], [CompStatus], [LearnActEndDate], [WithdrawReason], [Outcome], [AchDate], [OutGrade], [SWSupAimId], [LrnDelFAM_ADL], [LrnDelFAM_ASL], [LrnDelFAM_EEF], [LrnDelFAM_FFI], [LrnDelFAM_FLN], [LrnDelFAM_HEM1], [LrnDelFAM_HEM2], [LrnDelFAM_HEM3], [LrnDelFAM_HHS1], [LrnDelFAM_HHS2], [LrnDelFAM_LDM1], [LrnDelFAM_LDM2], [LrnDelFAM_LDM3], [LrnDelFAM_LDM4], [LrnDelFAM_NSA], [LrnDelFAM_POD], [LrnDelFAM_RES], [LrnDelFAM_SOF], [LrnDelFAM_SPP], [LrnDelFAM_WPP], [ProvSpecMon_A], [ProvSpecMon_B], [ProvSpecMon_C], [ProvSpecMon_D]) VALUES (10007459, N'2', N'ZPROG001', 1, 1, CAST(N'2017-08-01' AS Date), NULL, CAST(N'2018-08-08' AS Date), 36, 25, NULL, NULL, 27, NULL, N'OX17 1EZ', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, N'83282eb2aa2230439a9964374c163b9c', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'105', NULL, NULL, NULL, NULL, NULL, NULL)

-- LearningDeliveryFAM
INSERT INTO [Valid].[LearningDeliveryFAM] ([UKPRN], [LearnRefNumber], [AimSeqNumber], [LearnDelFAMType], [LearnDelFAMCode], [LearnDelFAMDateFrom], [LearnDelFAMDateTo]) VALUES (10007459, N'2', 1, N'ACT', N'1', CAST(N'2017-08-01' AS Date), CAST(N'2018-08-08' AS Date))

-- Earnings price episodes
INSERT [Rulebase].[AEC_ApprenticeshipPriceEpisode] ([UKPRN], [LearnRefNumber], [PriceEpisodeIdentifier], [EpisodeEffectiveTNPStartDate], [EpisodeStartDate], [PriceEpisodeActualEndDate], [PriceEpisodeActualInstalments], [PriceEpisodeAimSeqNumber], [PriceEpisodeCappedRemainingTNPAmount], [PriceEpisodeCompleted], [PriceEpisodeCompletionElement], [PriceEpisodeExpectedTotalMonthlyValue], [PriceEpisodeInstalmentValue], [PriceEpisodePlannedEndDate], [PriceEpisodePlannedInstalments], [PriceEpisodePreviousEarnings], [PriceEpisodeRemainingAmountWithinUpperLimit], [PriceEpisodeRemainingTNPAmount], [PriceEpisodeTotalEarnings], [PriceEpisodeTotalTNPPrice], [PriceEpisodeUpperBandLimit], [PriceEpisodeUpperLimitAdjustment], [TNP1], [TNP2], [TNP3], [TNP4], [PriceEpisodeFirstAdditionalPaymentThresholdDate], [PriceEpisodeSecondAdditionalPaymentThresholdDate] , [PriceEpisodeContractType]) VALUES (10007459, N'2', N'25-98765-01/08/2017', CAST(N'2017-08-01' AS Date), CAST(N'2017-08-01' AS Date), NULL, 0, 1, CAST(7500.00000 AS Decimal(10, 5)), 0, CAST(1500.00000 AS Decimal(10, 5)), CAST(6000.00000 AS Decimal(10, 5)), CAST(500.00000 AS Decimal(10, 5)), CAST(N'2018-08-08' AS Date), 12, CAST(0.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(6000.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(6000.00000 AS Decimal(10, 5)), CAST(1500.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)),'2017-11-04',NULL,'Levy Contract')

INSERT INTO [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
           ([Ukprn]
           ,[LearnRefNumber]
           ,[PriceEpisodeIdentifier]
           ,[Period]
           ,[PriceEpisodeApplic1618FrameworkUpliftBalancing]
           ,[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment]
           ,[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment]
           ,[PriceEpisodeBalancePayment]
           ,[PriceEpisodeBalanceValue]
           ,[PriceEpisodeCompletionPayment]
           ,[PriceEpisodeFirstDisadvantagePayment]
           ,[PriceEpisodeFirstEmp1618Pay]
           ,[PriceEpisodeFirstProv1618Pay]
           ,[PriceEpisodeFundLineType]
           ,[PriceEpisodeInstalmentsThisPeriod]
           ,[PriceEpisodeLSFCash]
           ,[PriceEpisodeOnProgPayment]
           ,[PriceEpisodeSecondDisadvantagePayment]
           ,[PriceEpisodeSecondEmp1618Pay]
           ,[PriceEpisodeSecondProv1618Pay])
     VALUES
           (10007459,'2',N'25-98765-01/08/2017',4,0,0,0,0,0,0,0,500,500,'16-18 Levy funding line',12,0,1000,0,0,0)
           


