﻿-- Learning provider
INSERT [Valid].[LearningProvider] ([UKPRN]) VALUES (10007459)
INSERT [Input].[LearningProvider] ([LearningProvider_Id], [UKPRN]) VALUES (1, 10007459)

-- Learner
INSERT [Valid].[Learner] ([LearnRefNumber], [PrevLearnRefNumber], [PrevUKPRN], [ULN], [FamilyName], [GivenNames], [DateOfBirth], [Ethnicity], [Sex], [LLDDHealthProb], [NINumber], [PriorAttain], [Accom], [ALSCost], [PlanLearnHours], [PlanEEPHours], [MathGrade], [EngGrade], [HomePostcode], [CurrentPostcode], [LrnFAM_DLA], [LrnFAM_ECF], [LrnFAM_EDF1], [LrnFAM_EDF2], [LrnFAM_EHC], [LrnFAM_FME], [LrnFAM_HNS], [LrnFAM_LDA], [LrnFAM_LSR1], [LrnFAM_LSR2], [LrnFAM_LSR3], [LrnFAM_LSR4], [LrnFAM_MCF], [LrnFAM_NLM1], [LrnFAM_NLM2], [LrnFAM_PPE1], [LrnFAM_PPE2], [LrnFAM_SEN], [ProvSpecMon_A], [ProvSpecMon_B]) VALUES (N'1', NULL, NULL, 1000000019, N'FamilyName', N'GivenNames', CAST(N'1984-10-12' AS Date), 98, N'M', 2, N'AB123456C', 3, NULL, NULL, NULL, NULL, NULL, NULL, N'BL4 0DH', N'BL4 0DH', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Valid].[Learner] ([LearnRefNumber], [PrevLearnRefNumber], [PrevUKPRN], [ULN], [FamilyName], [GivenNames], [DateOfBirth], [Ethnicity], [Sex], [LLDDHealthProb], [NINumber], [PriorAttain], [Accom], [ALSCost], [PlanLearnHours], [PlanEEPHours], [MathGrade], [EngGrade], [HomePostcode], [CurrentPostcode], [LrnFAM_DLA], [LrnFAM_ECF], [LrnFAM_EDF1], [LrnFAM_EDF2], [LrnFAM_EHC], [LrnFAM_FME], [LrnFAM_HNS], [LrnFAM_LDA], [LrnFAM_LSR1], [LrnFAM_LSR2], [LrnFAM_LSR3], [LrnFAM_LSR4], [LrnFAM_MCF], [LrnFAM_NLM1], [LrnFAM_NLM2], [LrnFAM_PPE1], [LrnFAM_PPE2], [LrnFAM_SEN], [ProvSpecMon_A], [ProvSpecMon_B]) VALUES (N'3', NULL, NULL, 1000000027, N'FamilyName', N'GivenNames', CAST(N'1983-11-21' AS Date), 98, N'F', 2, N'AB123456C', 2, NULL, NULL, NULL, NULL, NULL, NULL, N'OX17 1EZ', N'OX17 1EZ', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)

-- Learning Delivery
INSERT [Valid].[LearningDelivery] ([LearnRefNumber], [LearnAimRef], [AimType], [AimSeqNumber], [LearnStartDate], [OrigLearnStartDate], [LearnPlanEndDate], [FundModel], [ProgType], [FworkCode], [PwayCode], [StdCode], [PartnerUKPRN], [DelLocPostCode], [AddHours], [PriorLearnFundAdj], [OtherFundAdj], [ConRefNumber], [EmpOutcome], [CompStatus], [LearnActEndDate], [WithdrawReason], [Outcome], [AchDate], [OutGrade], [SWSupAimId], [LrnDelFAM_ADL], [LrnDelFAM_ASL], [LrnDelFAM_EEF], [LrnDelFAM_FFI], [LrnDelFAM_FLN], [LrnDelFAM_HEM1], [LrnDelFAM_HEM2], [LrnDelFAM_HEM3], [LrnDelFAM_HHS1], [LrnDelFAM_HHS2], [LrnDelFAM_LDM1], [LrnDelFAM_LDM2], [LrnDelFAM_LDM3], [LrnDelFAM_LDM4], [LrnDelFAM_NSA], [LrnDelFAM_POD], [LrnDelFAM_RES], [LrnDelFAM_SOF], [LrnDelFAM_SPP], [LrnDelFAM_WPP], [ProvSpecMon_A], [ProvSpecMon_B], [ProvSpecMon_C], [ProvSpecMon_D]) VALUES (N'1', N'ZPROG001', 1, 1, CAST(N'2016-07-17' AS Date), NULL, CAST(N'2017-10-19' AS Date), 36, NULL, NULL, NULL, 999, NULL, N'OX17 1EZ', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, N'0c230fbbdf109a4fa0578d7b2482628f', NULL, NULL, NULL, N'2', NULL, NULL, NULL, NULL, NULL, NULL, N'129', NULL, NULL, NULL, NULL, NULL, NULL, N'105', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [Valid].[LearningDelivery] ([LearnRefNumber], [LearnAimRef], [AimType], [AimSeqNumber], [LearnStartDate], [OrigLearnStartDate], [LearnPlanEndDate], [FundModel], [ProgType], [FworkCode], [PwayCode], [StdCode], [PartnerUKPRN], [DelLocPostCode], [AddHours], [PriorLearnFundAdj], [OtherFundAdj], [ConRefNumber], [EmpOutcome], [CompStatus], [LearnActEndDate], [WithdrawReason], [Outcome], [AchDate], [OutGrade], [SWSupAimId], [LrnDelFAM_ADL], [LrnDelFAM_ASL], [LrnDelFAM_EEF], [LrnDelFAM_FFI], [LrnDelFAM_FLN], [LrnDelFAM_HEM1], [LrnDelFAM_HEM2], [LrnDelFAM_HEM3], [LrnDelFAM_HHS1], [LrnDelFAM_HHS2], [LrnDelFAM_LDM1], [LrnDelFAM_LDM2], [LrnDelFAM_LDM3], [LrnDelFAM_LDM4], [LrnDelFAM_NSA], [LrnDelFAM_POD], [LrnDelFAM_RES], [LrnDelFAM_SOF], [LrnDelFAM_SPP], [LrnDelFAM_WPP], [ProvSpecMon_A], [ProvSpecMon_B], [ProvSpecMon_C], [ProvSpecMon_D]) VALUES (N'3', N'ZPROG001', 1, 1, CAST(N'2016-05-19' AS Date), NULL, CAST(N'2017-08-19' AS Date), 36, 20, 550, 6, NULL, NULL, N'OX17 1EZ', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, N'5def120a4e378040ac5d5ac2858700dd', NULL, NULL, NULL, N'2', NULL, NULL, NULL, NULL, NULL, NULL, N'129', NULL, NULL, NULL, NULL, NULL, NULL, N'105', NULL, NULL, NULL, NULL, NULL, NULL)

-- LearningDeliveryFAM
INSERT INTO [Valid].[LearningDeliveryFAM] ([LearnRefNumber], [AimSeqNumber], [LearnDelFAMType], [LearnDelFAMCode], [LearnDelFAMDateFrom], [LearnDelFAMDateTo]) VALUES ('1', 1, 'ACT', '1', CAST(N'2016-07-17' AS Date), CAST(N'2017-10-19' AS Date))
INSERT INTO [Valid].[LearningDeliveryFAM] ([LearnRefNumber], [AimSeqNumber], [LearnDelFAMType], [LearnDelFAMCode], [LearnDelFAMDateFrom], [LearnDelFAMDateTo]) VALUES ('3', 1, 'ACT', '1', CAST(N'2016-05-19' AS Date), CAST(N'2017-08-19' AS Date))

-- Earnings price episodes
INSERT [Rulebase].[AEC_ApprenticeshipPriceEpisode] ([LearnRefNumber], [PriceEpisodeAimSeqNumber], [EpisodeStartDate], [PriceEpisodeIdentifier], [PriceEpisodeActualEndDate], [PriceEpisodeActualInstalments], [EpisodeEffectiveTNPStartDate], [PriceEpisodeCappedRemainingTNPAmount], [PriceEpisodeCompleted], [PriceEpisodeCompletionElement], [PriceEpisodeExpectedTotalMonthlyValue], [PriceEpisodeInstalmentValue], [PriceEpisodePlannedEndDate], [PriceEpisodePlannedInstalments], [PriceEpisodePreviousEarnings], [PriceEpisodeRemainingAmountWithinUpperLimit], [PriceEpisodeRemainingTNPAmount], [PriceEpisodeTotalEarnings], [PriceEpisodeTotalTNPPrice], [PriceEpisodeUpperBandLimit], [PriceEpisodeUpperLimitAdjustment], [TNP1], [TNP2], [TNP3], [TNP4] ,[PriceEpisodeContractType]) VALUES (N'1', 1, CAST(N'2016-07-17' AS Date), N'27-25-2016-07-17', NULL, NULL, CAST(N'2016-07-17' AS Date), NULL, NULL, CAST(300.00000 AS Decimal(10, 5)), NULL, CAST(100.00000 AS Decimal(10, 5)), CAST(N'2017-10-19' AS Date), NULL, NULL, NULL, NULL, NULL, CAST(15000.00000 AS Decimal(10, 5)), NULL, NULL, CAST(12000.00000 AS Decimal(10, 5)), CAST(3000.00000 AS Decimal(10, 5)), NULL, NULL,'Levy Contract')
INSERT [Rulebase].[AEC_ApprenticeshipPriceEpisode] ([LearnRefNumber], [PriceEpisodeAimSeqNumber], [EpisodeStartDate], [PriceEpisodeIdentifier], [PriceEpisodeActualEndDate], [PriceEpisodeActualInstalments], [EpisodeEffectiveTNPStartDate], [PriceEpisodeCappedRemainingTNPAmount], [PriceEpisodeCompleted], [PriceEpisodeCompletionElement], [PriceEpisodeExpectedTotalMonthlyValue], [PriceEpisodeInstalmentValue], [PriceEpisodePlannedEndDate], [PriceEpisodePlannedInstalments], [PriceEpisodePreviousEarnings], [PriceEpisodeRemainingAmountWithinUpperLimit], [PriceEpisodeRemainingTNPAmount], [PriceEpisodeTotalEarnings], [PriceEpisodeTotalTNPPrice], [PriceEpisodeUpperBandLimit], [PriceEpisodeUpperLimitAdjustment], [TNP1], [TNP2], [TNP3], [TNP4],[PriceEpisodeContractType]) VALUES (N'3', 1, CAST(N'2016-05-19' AS Date), N'27-25-2016-05-19', NULL, NULL, CAST(N'2016-05-19' AS Date), NULL, NULL, CAST(300.00000 AS Decimal(10, 5)), NULL, CAST(100.00000 AS Decimal(10, 5)), CAST(N'2017-08-19' AS Date), NULL, NULL, NULL, NULL, NULL, CAST(15000.00000 AS Decimal(10, 5)), NULL, NULL, CAST(12000.00000 AS Decimal(10, 5)), CAST(3000.00000 AS Decimal(10, 5)), NULL, NULL,'Levy Contract')
