﻿-- Learning provider
INSERT [Valid].[LearningProvider] ([UKPRN]) VALUES (10007459)

INSERT [Rulebase].[AEC_ApprenticeshipPriceEpisode] ([LearnRefNumber], [PriceEpisodeIdentifier], [EpisodeEffectiveTNPStartDate], [EpisodeStartDate], [PriceEpisodeActualEndDate], [PriceEpisodeActualInstalments], [PriceEpisodeAimSeqNumber], [PriceEpisodeCappedRemainingTNPAmount], [PriceEpisodeCompleted], [PriceEpisodeCompletionElement], [PriceEpisodeExpectedTotalMonthlyValue], [PriceEpisodeInstalmentValue], [PriceEpisodePlannedEndDate], [PriceEpisodePlannedInstalments], [PriceEpisodePreviousEarnings], [PriceEpisodeRemainingAmountWithinUpperLimit], [PriceEpisodeRemainingTNPAmount], [PriceEpisodeTotalEarnings], [PriceEpisodeTotalTNPPrice], [PriceEpisodeUpperBandLimit], [PriceEpisodeUpperLimitAdjustment], [TNP1], [TNP2], [TNP3], [TNP4], [PriceEpisodeFirstAdditionalPaymentThresholdDate], [PriceEpisodeSecondAdditionalPaymentThresholdDate], [PriceEpisodeContractType]) VALUES (N'2', N'25-98765-06/08/2017', CAST(N'2017-08-06' AS Date), CAST(N'2017-08-06' AS Date), NULL, 0, 1, CAST(7500.00000 AS Decimal(10, 5)), 0, CAST(1500.00000 AS Decimal(10, 5)), CAST(6000.00000 AS Decimal(10, 5)), CAST(500.00000 AS Decimal(10, 5)), CAST(N'2018-08-08' AS Date), 12, CAST(0.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(6000.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(7500.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(6000.00000 AS Decimal(10, 5)), CAST(1500.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(N'2017-11-04' AS Date), NULL, N'Levy Contract')

INSERT [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] ([LearnRefNumber], [PriceEpisodeIdentifier], [Period], [PriceEpisodeApplic1618FrameworkUpliftBalancing], [PriceEpisodeApplic1618FrameworkUpliftCompletionPayment], [PriceEpisodeApplic1618FrameworkUpliftOnProgPayment], [PriceEpisodeBalancePayment], [PriceEpisodeBalanceValue], [PriceEpisodeCompletionPayment], [PriceEpisodeFirstDisadvantagePayment], [PriceEpisodeFirstEmp1618Pay], [PriceEpisodeFirstProv1618Pay], [PriceEpisodeFundLineType], [PriceEpisodeInstalmentsThisPeriod], [PriceEpisodeLSFCash], [PriceEpisodeOnProgPayment], [PriceEpisodeSecondDisadvantagePayment], [PriceEpisodeSecondEmp1618Pay], [PriceEpisodeSecondProv1618Pay]) VALUES (N'2', N'25-98765-06/08/2017', 4, CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(500.00000 AS Decimal(10, 5)), CAST(500.00000 AS Decimal(10, 5)), N'16-18 Levy funding line', 12, CAST(0.00000 AS Decimal(10, 5)), CAST(1000.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)), CAST(0.00000 AS Decimal(10, 5)))
